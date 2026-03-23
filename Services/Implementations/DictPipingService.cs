using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using System.Text.Json;
using Dapper;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Services.Implementations.DictStrategies;
using PMCSystem_Backend.Services.Interfaces;
using PMCSystem_Backend.Core.Data;
using PMCSystem_Backend.Modules.StandardComponents.Dtos;

namespace PMCSystem_Backend.Services.Implementations
{
    // 列配置项类
    public class ColumnItem
    {
        public string? DbField { get; set; }
        public string? Title { get; set; }
        public string? UiType { get; set; }
        public bool? IsHidden { get; set; }
        public bool? IsRequired { get; set; }
        public bool? IsReadOnly { get; set; }
        public bool? Show { get; set; }
        public bool? IsNew { get; set; }
    }

    /// <summary>
    /// 管道字典服务实现类
    /// 提供管道组件相关的数据访问和业务逻辑处理
    /// </summary>
    public class DictPipingService(
        DictStrategyFactory strategyFactory,
        DictConfigManager configManager,
        PmcContext context,
        ILogger<DictPipingService> logger) : IDictPipingService
    {
        // 缓存 JsonSerializerOptions 实例，避免每次序列化/反序列化时重复创建
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        // 静态锁对象，用于进程内线程同步
        private static readonly SemaphoreSlim _configFileSemaphore = new(1, 1);

        // 常量定义
        private const string PART_PREFIX = "part-";
        private const string CONNECT_TYPE_FLANGE = "法兰";
        private const string STRATEGY_FLANGE = "Flange";
        private const string STRATEGY_FITTING = "Fitting";
        private const int STATUS_ACTIVE = 1;
        private const int STATUS_INACTIVE = 0;

        private readonly DictStrategyFactory _strategyFactory = strategyFactory;
        private readonly DictConfigManager _configManager = configManager;
        private readonly PmcContext _context = context;
        private readonly ILogger<DictPipingService> _logger = logger;

        #region 1. 查询 (GetTableData)

        /// <summary>
        /// 获取表格数据
        /// </summary>
        /// <param name="type">组件类型</param>
        /// <param name="keyword">搜索关键字</param>
        /// <returns>表格数据DTO</returns>
        public async Task<DictTableDto> GetTableDataAsync(string type, string? keyword = null)
        {
            // 1. 重新加载配置，确保获取到最新的列配置
            _configManager.LoadAllConfigs();

            // 2. 获取配置
            var config = _configManager.GetConfig(type);

            // 使用对象初始化器和集合表达式简化代码
            var result = new DictTableDto
            {
                Columns =
                [
                    .. config.Columns
                        // .Where(c => !c.DbField.Equals("JsonData", StringComparison.OrdinalIgnoreCase))
                        .Select(c => new DictColumnDto
                        {
                            Prop = c.DbField,
                            Label = c.Title,
                            Show = !c.IsHidden,
                            UiType = c.UiType ?? "Input",
                            Required = c.IsRequired,
                            IsPrimaryKey = c.IsPrimaryKey,
                            IsReadOnly = c.IsReadOnly,
                            
                            // 注意：这里删除了 Options = c.Options，把它移到了 DataSource 内部处理
                            DataSource = c.DataSource is null ? null : new DictDataSourceDto
                            {
                                Url = c.DataSource.Url,
                                LabelField = c.DataSource.LabelField,
                                ValueField = c.DataSource.ValueField,
                                
                                // 正确的 Options 映射方式：
                                Options = c.DataSource.Options?.Select(o => new DictStaticOptionDto
                                {
                                    Label = o.Label,
                                    Value = o.Value,
                                    Disabled = o.Disabled
                                }).ToList(),

                                ValueMapping = c.DataSource.ValueMapping,

                                FilterUsed = c.DataSource.FilterUsed
                            }
                        })
                ]
            };

            // 先查询 S3D_Dict_PipingComponentType 表，获取 componentTypeName = 'elbow' 的 ID 和 ConnectType
            using var conn = _context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open) await conn.OpenAsync();

            var componentTypeName = type.Replace(PART_PREFIX, "");
            var componentTypeResult = await conn.QueryFirstOrDefaultAsync<dynamic>(
                "SELECT ID, ConnectType FROM S3D_Dict_PipingComponentType WHERE ComponentTypeName = @ComponentTypeName",
                new { ComponentTypeName = componentTypeName });

            if (componentTypeResult is not null)
            {
                var componentTypeId = componentTypeResult.ID;
                var connectType = componentTypeResult.ConnectType;

                // 根据 ConnectType 决定使用哪种策略
                string strategyType = connectType == CONNECT_TYPE_FLANGE ? STRATEGY_FLANGE : STRATEGY_FITTING;
                var pipingStrategy = _strategyFactory.GetStrategy(strategyType);

                if (pipingStrategy is not null)
                {
                    // 根据策略类型调用相应的方法
                    IEnumerable<dynamic> pipingData;
                    if (strategyType == STRATEGY_FITTING && pipingStrategy is FittingDictStrategy fittingStrategy)
                    {
                        pipingData = await fittingStrategy.GetPipingComponentTypeDataAsync(componentTypeId);
                    }
                    else if (strategyType == STRATEGY_FLANGE && pipingStrategy is FlangeDictStrategy flangeStrategy)
                    {
                        // 这里需要实现 FlangeDictStrategy 的相应方法
                        // 暂时使用相同的方法名，后续可以根据需要修改
                        pipingData = await flangeStrategy.GetPipingComponentTypeDataAsync(componentTypeId);
                    }
                    else
                    {
                        // 如果没有对应的策略，返回空数据
                        return result;
                    }

                    result.Rows =
                    [
                        .. pipingData
                            .Select(row => (IDictionary<string, object>)row)
                            .Select(d =>
                            {
                                var dict = new Dictionary<string, object>(d);

                                // 处理 JsonData 字段，将其中的键值对提取为顶层属性
                                if (dict.TryGetValue("JsonData", out var jsonDataValue) && jsonDataValue is not null)
                                {
                                    try
                                    {
                                        string jsonDataString = jsonDataValue.ToString() ?? string.Empty;
                                        if (!string.IsNullOrEmpty(jsonDataString))
                                        {
                                            var jsonDataArray = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(jsonDataString);
                                            if (jsonDataArray is not null)
                                            {
                                                foreach (var item in jsonDataArray)
                                                {
                                                    if (item.TryGetValue("DbField", out var dbFieldValue) &&
                                                        item.TryGetValue("value", out var value))
                                                    {
                                                        string dbField = dbFieldValue.ToString() ?? string.Empty;
                                                        if (!string.IsNullOrEmpty(dbField))
                                                        {
                                                            dict[dbField] = value;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        // 记录日志，包含原始数据，方便后续回溯和手动修复
                                        _logger?.LogWarning(ex, "解析 JsonData 字段时发生错误，原始数据: {JsonData}", jsonDataValue);
                                    }
                                }

                                return dict;
                            })
                    ];
                }
                return result;
            }

            return result;
        }

        #endregion

        #region 3. 新增 (Add)

        /// <summary>
        /// 新增管道组件标准记录
        /// </summary>
        /// <param name="type">组件类型</param>
        /// <param name="data">组件数据</param>
        /// <returns>新记录ID</returns>
        public async Task<int> AddAsync(string type, DictInputDto data)
        {
            // 1. 输入验证
            if (data is null || data.Count == 0)
                throw new ArgumentNullException(nameof(data), "提交数据不能为空");

            // 处理 columns 字段，更新配置文件
            await ProcessColumnsAsync(type, data, "新增");

            // 验证必填字段
            ValidateRequiredFields(type, data, "新增");

            // 2. 提取标准字段值
            var (jsonData, geometricIndustryStandardCl, componentTypeId, materialsCategoryCl) = ExtractStandardValues(data);

            // 3. 检查数据是否存在
            try
            {
                return await ExecuteInTransactionAsync(async (conn, transaction) =>
                {
                    // 先查询是否存在相同的记录，处理 NULL 值的情况
                    string checkSql = @"
                        SELECT ID FROM S3D_Rule_PipingCompStandard 
                        WHERE ComponentTypeID = @ComponentTypeID 
                        AND GeometricIndustryStandard_CL = @GeometricIndustryStandard_CL 
                        AND (MaterialsCategory_CL = @MaterialsCategory_CL OR (MaterialsCategory_CL IS NULL AND @MaterialsCategory_CL IS NULL))
                    ";

                    var existingId = await conn.QueryFirstOrDefaultAsync<int?>(checkSql, new
                    {
                        ComponentTypeID = componentTypeId,
                        GeometricIndustryStandard_CL = geometricIndustryStandardCl,
                        MaterialsCategory_CL = materialsCategoryCl
                    }, transaction);

                    if (existingId.HasValue)
                    {
                        // 记录存在，更新 status 为 1
                        string updateSql = @"
                            UPDATE S3D_Rule_PipingCompStandard 
                            SET Status = 1, 
                                JsonData = @JsonData, 
                                ModifiedDate = GETDATE()
                            WHERE ID = @Id
                        ";
                        await conn.ExecuteAsync(updateSql, new
                        {
                            Id = existingId.Value,
                            JsonData = jsonData
                        }, transaction);
                        return existingId.Value;
                    }
                    else
                    {
                        // 记录不存在，执行插入操作
                        string insertSql = @"
                            INSERT INTO S3D_Rule_PipingCompStandard 
                            (GeometricIndustryStandard_CL, ComponentTypeID, MaterialsCategory_CL, JsonData, CreatedDate, ModifiedDate, Status)
                            VALUES (@GeometricIndustryStandard_CL, @ComponentTypeID, @MaterialsCategory_CL, @JsonData, GETDATE(), GETDATE(), 1);
                            SELECT CAST(SCOPE_IDENTITY() AS INT);
                        ";

                        return await conn.ExecuteScalarAsync<int>(insertSql, new
                        {
                            GeometricIndustryStandard_CL = geometricIndustryStandardCl,
                            ComponentTypeID = componentTypeId,
                            MaterialsCategory_CL = materialsCategoryCl,
                            JsonData = jsonData
                        }, transaction);
                    }
                }, "新增或更新管道组件标准记录");
            }
            catch (Exception ex)
            {
                // 记录日志，包含原始数据，方便后续回溯和手动修复
                _logger?.LogError(ex, "新增或更新管道组件标准记录时数据库操作失败，类型: {Type}, 数据: {Data}", type, data);
                throw new Exception($"数据库操作失败: {ex.Message}");
            }
        }

        #endregion

        #region 4. 修改 (Update)

        /// <summary>
        /// 更新管道组件标准记录
        /// </summary>
        /// <param name="type">组件类型</param>
        /// <param name="id">记录ID</param>
        /// <param name="data">更新的数据</param>
        /// <returns>受影响的行数</returns>
        public async Task<int> UpdateAsync(string type, int id, DictInputDto data)
        {
            // 1. 输入验证
            if (data is null || data.Count == 0)
                throw new ArgumentNullException(nameof(data), "提交数据不能为空");

            if (string.IsNullOrEmpty(type))
                throw new ArgumentNullException(nameof(type), "类型参数不能为空");

            if (id <= 0)
                throw new ArgumentException("ID 必须为正整数", nameof(id));

            // 处理 columns 字段，更新配置文件
            await ProcessColumnsAsync(type, data, "更新");

            // 验证必填字段
            ValidateRequiredFields(type, data, "更新");

            // 2. 提取标准字段值（更新时不包含 componentTypeId）
            var (jsonData, geometricIndustryStandardCl, _, materialsCategoryCl) = ExtractStandardValues(data);

            // 3. 从 data 中获取 status 值（可选，默认为 false）
            int status = 0; // 默认值
            if (data.TryGetValue("status", out var statusObj) && statusObj is not null)
            {
                status = statusObj switch
                {
                    bool boolValue => boolValue ? 1 : 0,
                    JsonElement jsonElement when jsonElement.ValueKind == JsonValueKind.True => 1,
                    JsonElement jsonElement when jsonElement.ValueKind == JsonValueKind.False => 0,
                    _ => 0 // 格式不正确时默认为 false
                };
            }

            // 4. 执行更新操作
            try
            {
                return await ExecuteInTransactionAsync(async (conn, transaction) =>
                {
                    string sql = @"
                        UPDATE S3D_Rule_PipingCompStandard 
                        SET 
                            GeometricIndustryStandard_CL = @GeometricIndustryStandard_CL,
                            MaterialsCategory_CL = @MaterialsCategory_CL,
                            JsonData = @JsonData,
                            Status = @Status,
                            ModifiedDate = GETDATE()
                        WHERE ID = @Id
                    ";

                    return await conn.ExecuteAsync(sql, new
                    {
                        GeometricIndustryStandard_CL = geometricIndustryStandardCl,
                        MaterialsCategory_CL = materialsCategoryCl,
                        JsonData = jsonData,
                        Status = status,
                        Id = id
                    }, transaction);
                }, "更新管道组件标准记录");
            }
            catch (Exception ex)
            {
                // 记录日志，包含原始数据，方便后续回溯和手动修复
                _logger?.LogError(ex, "更新管道组件标准记录时数据库操作失败，类型: {Type}, ID: {Id}, 数据: {Data}", type, id, data);
                throw new Exception($"数据库操作失败: {ex.Message}");
            }
        }

        #endregion

        #region 5. 删除 (Delete)

        /// <summary>
        /// 删除管道组件标准记录
        /// </summary>
        /// <param name="type">组件类型</param>
        /// <param name="id">记录ID</param>
        /// <returns>受影响的行数</returns>
        public async Task<int> DeleteAsync(string type, int id)
        {
            var config = _configManager.GetConfig(type);
            var strategy = _strategyFactory.GetStrategy(config.HandlerType);
            return await strategy.DeleteAsync(type, id, config);
        }

        #endregion

        #region 6. 批量删除 (Batch Delete)

        /// <summary>
        /// 批量删除管道组件标准记录
        /// </summary>
        /// <param name="type">组件类型</param>
        /// <param name="ids">记录ID列表</param>
        /// <returns>受影响的行数</returns>
        public async Task<int> BatchDeleteAsync(string type, List<int> ids)
        {
            // 1. 输入验证
            if (ids is null || ids.Count == 0)
                throw new ArgumentNullException(nameof(ids), "ID 列表不能为空");

            if (string.IsNullOrEmpty(type))
                throw new ArgumentNullException(nameof(type), "类型参数不能为空");

            // 验证 ids 列表中的值是否为有效整数
            if (ids.Any(id => id <= 0))
                throw new ArgumentException("ID 必须为正整数", nameof(ids));

            // 2. 批量删除记录
            return await ExecuteInTransactionAsync(async (conn, transaction) =>
            {
                string sql = "DELETE FROM S3D_Rule_PipingCompStandard WHERE ID IN @Ids";
                return await conn.ExecuteAsync(sql, new { Ids = ids }, transaction);
            }, "批量删除管道组件标准记录");
        }

        #endregion

        #region 7. 获取ComponentType列表

        /// <summary>
        /// 获取管道组件类型列表
        /// </summary>
        /// <returns>组件类型列表</returns>
        public async Task<List<dynamic>> GetComponentTypeListAsync()
        {
            using var conn = _context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open) await conn.OpenAsync();

            string sql = "SELECT id, ComponentTypeName, ComponentTypeDescription FROM S3D_Dict_PipingComponentType WHERE status = 1";
            var result = await conn.QueryAsync<dynamic>(sql);
            return [.. result];
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 验证输入数据
        /// </summary>
        /// <param name="config">字典配置</param>
        /// <param name="data">输入数据</param>
        private void ValidateInput(DictItemConfig config, DictInputDto data)
        {
            if (data is null)
                throw new ArgumentNullException(nameof(data), "提交数据不能为空");

            var errors = new List<string>();

            foreach (var column in config.Columns)
            {
                if (column.IsRequired && !column.IsPrimaryKey)
                {
                    // 首先检查顶层data中是否有该字段
                    if (!data.TryGetValue(column.DbField, out var value) || IsNullOrEmpty(value))
                    {
                        // 如果顶层没有，检查JsonData中是否有该字段
                        if (data.TryGetValue("JsonData", out var jsonDataValue) && jsonDataValue is not null)
                        {
                            var jsonDataArray = ParseJsonData(jsonDataValue, "验证输入数据");
                            if (jsonDataArray is not null)
                            {
                                var jsonField = jsonDataArray.FirstOrDefault(item =>
                                    item.TryGetValue("DbField", out var dbFieldValue) &&
                                    dbFieldValue.ToString() == column.DbField
                                );
                                if (jsonField is not null && jsonField.TryGetValue("value", out var jsonValue) && !IsNullOrEmpty(jsonValue))
                                {
                                    // JsonData中找到该字段且值不为空，跳过错误
                                    continue;
                                }
                            }
                        }
                        errors.Add($"字段 '{column.Title}' ({column.DbField}) 不能为空");
                    }
                }
            }

            if (errors.Count > 0)
            {
                throw new Exception($"数据校验失败: {string.Join("; ", errors)}");
            }
        }

        /// <summary>
        /// 检查值是否为空
        /// </summary>
        /// <param name="value">要检查的值</param>
        /// <returns>是否为空</returns>
        private static bool IsNullOrEmpty(object? value)
        {
            if (value is null) return true;
            if (value is string str && string.IsNullOrWhiteSpace(str)) return true;
            // 布尔值false是有效值，不应该被视为空
            if (value is bool) return false;
            // 数值类型0是有效值，不应该被视为空
            if (value is int || value is long || value is float || value is double) return false;
            return false;
        }

        /// <summary>
        /// 转换 JsonElement 为 Dapper 可识别的类型
        /// </summary>
        /// <param name="value">要转换的值</param>
        /// <returns>转换后的值</returns>
        private static object? ConvertJsonElement(object? value)
        {
            if (value is null)
                return null;

            // 检查是否为 JsonElement 类型
            if (value.GetType().FullName == typeof(JsonElement).FullName)
            {
                // 获取 ValueKind 属性
                var valueKindProperty = value.GetType().GetProperty("ValueKind");
                if (valueKindProperty is null)
                    return value.ToString();

                var valueKind = valueKindProperty?.GetValue(value, null);
                if (valueKind is null)
                    return value.ToString();

                var valueKindValue = Convert.ToInt32(valueKind);

                // 根据 ValueKind 决定调用哪个方法
                string methodName = valueKindValue switch
                {
                    1 => "GetString",
                    2 => "GetInt32",
                    3 => "GetInt32",
                    4 => "GetInt32",
                    5 => "GetInt32",
                    6 => "GetInt32",
                    7 => "GetInt64",
                    8 => "GetDouble",
                    9 => "GetBoolean",
                    _ => "GetString"
                };

                var getValueMethod = value.GetType().GetMethod(methodName);
                if (getValueMethod is not null)
                {
                    try
                    {
                        return getValueMethod.Invoke(value, null);
                    }
                    catch
                    {
                        return value.ToString();
                    }
                }

                return value.ToString();
            }

            // 如果不是 JsonElement，直接返回
            return value;
        }

        /// <summary>
        /// 解析 JsonData 字符串为字典列表
        /// </summary>
        /// <param name="jsonDataValue">JsonData 值</param>
        /// <param name="context">上下文信息，用于日志记录</param>
        /// <returns>解析后的字典列表，解析失败返回 null</returns>
        private List<Dictionary<string, object>>? ParseJsonData(object? jsonDataValue, string context)
        {
            if (jsonDataValue is null)
                return null;

            try
            {
                // 处理 JsonElement 类型的值
                var convertedValue = ConvertJsonElement(jsonDataValue);
                string jsonDataString = convertedValue?.ToString() ?? string.Empty;

                if (string.IsNullOrEmpty(jsonDataString))
                    return null;

                var jsonDataArray = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(jsonDataString, _jsonSerializerOptions);
                return jsonDataArray;
            }
            catch (Exception ex)
            {
                // 记录日志，包含原始数据，方便后续回溯和手动修复
                _logger?.LogWarning(ex, "解析 JsonData 字段时发生错误，上下文: {Context}, 原始数据: {JsonData}", context, jsonDataValue);
                return null;
            }
        }

        /// <summary>
        /// 处理 columns 字段并更新配置
        /// </summary>
        /// <param name="type">组件类型</param>
        /// <param name="data">输入数据</param>
        /// <param name="context">上下文信息</param>
        private async Task ProcessColumnsAsync(string type, DictInputDto data, string context)
        {
            if (!data.TryGetValue("columns", out var columnsValue) || columnsValue is null)
                return;

            try
            {
                string columnsJson = columnsValue.ToString() ?? string.Empty;
                if (string.IsNullOrEmpty(columnsJson))
                    return;

                var columns = JsonSerializer.Deserialize<List<ColumnItem>>(columnsJson, _jsonSerializerOptions);
                if (columns is not null)
                {
                    await UpdateConfig(type, columns);
                }
            }
            catch (Exception ex)
            {
                // 记录日志，包含原始数据，方便后续回溯和手动修复
                _logger?.LogWarning(ex, "{Context}时反序列化 columns 字段发生错误，原始数据: {ColumnsValue}", context, columnsValue);
            }
        }

        /// <summary>
        /// 验证必填字段
        /// </summary>
        /// <param name="type">组件类型</param>
        /// <param name="data">输入数据</param>
        /// <param name="context">上下文信息</param>
        private void ValidateRequiredFields(string type, DictInputDto data, string context)
        {
            var config = _configManager.GetConfig(type);
            var errors = new List<string>();

            foreach (var column in config.Columns)
            {
                if (column.IsRequired && !column.IsPrimaryKey)
                {
                    // 首先检查顶层data中是否有该字段
                    if (!data.TryGetValue(column.DbField, out var value) || IsNullOrEmpty(value))
                    {
                        // 如果顶层没有，检查JsonData中是否有该字段
                        if (data.TryGetValue("JsonData", out var jsonDataValue) && jsonDataValue is not null)
                        {
                            var jsonDataArray = ParseJsonData(jsonDataValue, $"{context}必填验证");
                            if (jsonDataArray is not null)
                            {
                                var jsonField = jsonDataArray.FirstOrDefault(item =>
                                    item.TryGetValue("DbField", out var dbFieldValue) &&
                                    dbFieldValue.ToString() == column.DbField
                                );
                                if (jsonField is not null && jsonField.TryGetValue("value", out var jsonValue) && !IsNullOrEmpty(jsonValue))
                                {
                                    // JsonData中找到该字段且值不为空，跳过错误
                                    continue;
                                }
                            }
                        }
                        errors.Add($"字段 '{column.Title}' ({column.DbField}) 不能为空");
                    }
                }
            }

            if (errors.Count > 0)
            {
                throw new Exception($"数据校验失败: {string.Join("; ", errors)}");
            }
        }

        /// <summary>
        /// 提取标准字段值
        /// </summary>
        /// <param name="data">输入数据</param>
        /// <returns>标准字段值元组</returns>
        private (string? JsonData, object? GeometricIndustryStandardCl, object? ComponentTypeId, object? MaterialsCategoryCl) ExtractStandardValues(DictInputDto data)
        {
            // 处理 JsonData 字段
            var jsonData = data.TryGetValue("JsonData", out var jsonDataValue) && !IsNullOrEmpty(jsonDataValue)
                ? jsonDataValue.ToString()
                : null;

            // 转换 JsonElement 类型为 Dapper 可识别的类型
            var geometricIndustryStandardCl = data.TryGetValue("geometricIndustryStandardCL", out var standardValue) && !IsNullOrEmpty(standardValue)
                ? ConvertJsonElement(standardValue)
                : null;
            var componentTypeId = data.TryGetValue("componentTypeId", out var componentTypeIdValue) && !IsNullOrEmpty(componentTypeIdValue)
                ? ConvertJsonElement(componentTypeIdValue)
                : null;
            var materialsCategoryCl = data.TryGetValue("materialsCategoryCL", out var mainMaterialValue) && !IsNullOrEmpty(mainMaterialValue)
                ? ConvertJsonElement(mainMaterialValue)
                : null;

            return (jsonData, geometricIndustryStandardCl, componentTypeId, materialsCategoryCl);
        }

        /// <summary>
        /// 更新配置文件
        /// </summary>
        /// <param name="type">组件类型</param>
        /// <param name="columns">列配置列表</param>
        /// <returns></returns>
        private async Task UpdateConfig(string type, List<ColumnItem> columns)
        {
            try
            {
                // 如果 columns 为空或不包含 IsNew 为 true 的列，则不需要处理
                if (columns is null || !columns.Any(c => c.IsNew == true))
                {
                    return;
                }

                // 确定配置文件路径
                string configFile = "fitting.json"; // 默认配置文件

                // 从配置管理器获取配置，以确定使用哪种策略
                var configInfo = _configManager.GetConfig(type);
                if (!string.IsNullOrEmpty(configInfo.HandlerType))
                {
                    // 根据 HandlerType 决定配置文件，与 DictStrategyFactory 保持一致
                    configFile = configInfo.HandlerType.ToLower() + ".json";
                }

                string configPath = Path.Combine(Directory.GetCurrentDirectory(), "Configs", "DictConfigs", configFile);

                // 检查配置文件是否存在
                if (!File.Exists(configPath))
                {
                    throw new FileNotFoundException($"配置文件不存在: {configPath}");
                }

                // 使用文件锁和重试机制处理并发写入
                await ExecuteWithFileLockAsync(configPath, async () =>
                {
                    string jsonContent = await File.ReadAllTextAsync(configPath);

                    // 使用缓存的 JsonSerializerOptions 实例进行反序列化，避免重复创建
                    var config = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(jsonContent, _jsonSerializerOptions);

                    if (config is not null && config.TryGetValue(type, out var componentConfig))
                    {
                        if (componentConfig.TryGetValue("Columns", out var columnsObj))
                        {
                            // 处理不同类型的 columnsObj
                            List<Dictionary<string, object>> existingColumns;

                            if (columnsObj is JsonElement jsonElement)
                            {
                                // 处理 JsonElement 类型
                                existingColumns = jsonElement.Deserialize<List<Dictionary<string, object>>>() ?? [];
                            }
                            else if (columnsObj is List<object> listObj)
                            {
                                // 处理 List<object> 类型
                                existingColumns = [.. listObj.Select(c => c as Dictionary<string, object>).Where(c => c != null).Select(c => c!)];
                            }
                            else if (columnsObj is List<Dictionary<string, object>> dictList)
                            {
                                // 处理 List<Dictionary<string, object>> 类型
                                existingColumns = dictList;
                            }
                            else
                            {
                                existingColumns = [];
                            }

                            bool needUpdate = false;

                            // 遍历新列，添加 IsNew 为 true 的列
                            foreach (var column in columns)
                            {
                                if (column.IsNew == true && !string.IsNullOrEmpty(column.DbField))
                                {
                                    // 检查是否已存在相同的 DbField
                                    bool exists = existingColumns.Any(c =>
                                        c.ContainsKey("DbField") && c["DbField"]?.ToString() == column.DbField
                                    );

                                    if (!exists)
                                    {
                                        // 创建新列配置
                                        var newColumn = new Dictionary<string, object>
                                        {
                                            ["DbField"] = column.DbField,
                                            ["Title"] = column.Title ?? string.Empty,
                                            ["UiType"] = column.UiType ?? string.Empty,
                                            ["IsHidden"] = column.IsHidden ?? false,
                                            ["IsRequired"] = column.IsRequired ?? false,
                                            ["IsReadOnly"] = column.IsReadOnly ?? false
                                        };

                                        existingColumns.Add(newColumn);
                                        needUpdate = true;
                                    }
                                }
                            }

                            // 只有当需要更新时才保存配置文件
                            if (needUpdate)
                            {
                                // 保存更新后的配置
                                componentConfig["Columns"] = existingColumns;
                                config[type] = componentConfig;

                                string updatedJson = JsonSerializer.Serialize(config, _jsonSerializerOptions);

                                await File.WriteAllTextAsync(configPath, updatedJson);
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                // 记录日志，包含类型和列信息，方便后续回溯和手动修复
                _logger?.LogError(ex, "更新配置文件失败，类型: {Type}, 列信息: {Columns}", type, columns);
                throw new Exception($"更新配置文件失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 使用信号量和重试机制执行文件操作，处理并发访问
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="action">要执行的操作</param>
        /// <returns>任务</returns>
        private async Task ExecuteWithFileLockAsync(string filePath, Func<Task> action)
        {
            // 等待信号量（进程内线程同步）
            await _configFileSemaphore.WaitAsync();

            try
            {
                const int maxRetries = 10;
                const int delayMilliseconds = 100;

                for (int i = 0; i < maxRetries; i++)
                {
                    try
                    {
                        // 执行操作
                        await action();
                        return;
                    }
                    catch (IOException ex)
                    {
                        // 文件被锁定，等待后重试
                        if (i < maxRetries - 1)
                        {
                            _logger?.LogWarning("文件被占用，等待 {Delay}ms 后重试 ({Attempt}/{MaxRetries}): {FilePath}",
                                delayMilliseconds, i + 1, maxRetries, filePath);
                            await Task.Delay(delayMilliseconds * (i + 1)); // 递增延迟
                        }
                        else
                        {
                            throw new IOException($"无法访问文件，已达到最大重试次数: {filePath}", ex);
                        }
                    }
                }
            }
            finally
            {
                // 释放信号量
                _configFileSemaphore.Release();
            }
        }

        /// <summary>
        /// 在事务中执行数据库操作
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="action">数据库操作</param>
        /// <param name="errorMessage">错误消息</param>
        /// <returns>操作结果</returns>
        private async Task<T> ExecuteInTransactionAsync<T>(Func<IDbConnection, IDbTransaction, Task<T>> action, string errorMessage)
        {
            using var conn = _context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open) await conn.OpenAsync();
            using var transaction = conn.BeginTransaction();
            try
            {
                var result = await action(conn, transaction);
                transaction.Commit();
                return result;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        /// <summary>
        /// 在事务中执行数据库操作（无返回值）
        /// </summary>
        /// <param name="action">数据库操作</param>
        /// <param name="errorMessage">错误消息</param>
        private async Task ExecuteInTransactionAsync(Func<IDbConnection, IDbTransaction, Task> action, string errorMessage)
        {
            using var conn = _context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open) await conn.OpenAsync();
            using var transaction = conn.BeginTransaction();
            try
            {
                await action(conn, transaction);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        #endregion
    }
}