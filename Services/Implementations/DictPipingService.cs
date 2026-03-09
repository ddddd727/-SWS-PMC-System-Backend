using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Dtos.Dict;
using PMCSystem_Backend.Services.Implementations.DictStrategies;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Services.Implementations
{
    public class DictPipingService(DictStrategyFactory strategyFactory, DictConfigManager configManager, PmcContext context) : IDictPipingService
    {
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

        #region 1. 查询 (GetTableData)

        public async Task<DictTableDto> GetTableDataAsync(string type, string? keyword = null)
        {
            // 1. 获取配置
            var config = _configManager.GetConfig(type);
            var result = new DictTableDto();

            // 2. 填充列配置
            result.Columns = config.Columns
                .Where(c => !c.DbField.Equals("JsonData", StringComparison.OrdinalIgnoreCase))
                .Select(c => new DictColumnDto
                {
                    Prop = c.DbField,
                    Label = c.Title,
                    Show = !c.IsHidden,
                    UiType = c.UiType ?? "Input",
                    Required = c.IsRequired,
                    IsPrimaryKey = c.IsPrimaryKey,
                    IsReadOnly = c.IsReadOnly,

                    // 映射 Options：把配置里的选项传给前端
                    Options = c.Options,

                    DataSource = c.DataSource == null ? null : new DictDataSourceDto
                    {
                        Url = c.DataSource.Url,
                        LabelField = c.DataSource.LabelField,
                        ValueField = c.DataSource.ValueField,
                        ValueMapping = c.DataSource.ValueMapping
                    }
                }).ToList();

            // 先查询 S3D_Dict_PipingComponentType 表，获取 componentTypeName = 'elbow' 的 ID 和 ConnectType
            using var conn = _context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open) await conn.OpenAsync();

            var componentTypeName = type.Replace(PART_PREFIX, "");
            var componentTypeResult = await conn.QueryFirstOrDefaultAsync<dynamic>(
                "SELECT ID, ConnectType FROM S3D_Dict_PipingComponentType WHERE ComponentTypeName = @ComponentTypeName AND Status = @Status",
                new { ComponentTypeName = componentTypeName, Status = STATUS_ACTIVE });

            if (componentTypeResult != null)
            {
                var componentTypeId = componentTypeResult.ID;
                var connectType = componentTypeResult.ConnectType;

                // 根据 ConnectType 决定使用哪种策略
                string strategyType = connectType == CONNECT_TYPE_FLANGE ? STRATEGY_FLANGE : STRATEGY_FITTING;
                var pipingStrategy = _strategyFactory.GetStrategy(strategyType);

                if (pipingStrategy != null)
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

                    result.Rows = pipingData
                        .Select(row => (IDictionary<string, object>)row)
                        .Select(d => new Dictionary<string, object>(d))
                        .ToList();
                }
                return result;
            }

            return result;
        }

        #endregion

        #region 2. 获取弯头数据 (GetElbowData)

        public async Task<DictTableDto> GetElbowDataAsync()
        {
            return await GetTableDataAsync("part-elbow");
        }

        #endregion

        #region 3. 新增 (Add)

        public async Task<int> AddAsync(string type, DictInputDto data)
        {
            var config = _configManager.GetConfig(type);
            ValidateInput(config, data);
            var strategy = _strategyFactory.GetStrategy(config.HandlerType);
            return await strategy.AddAsync(type, config, data);
        }

        #endregion

        #region 4. 修改 (Update)

        public async Task<int> UpdateAsync(string type, int id, DictInputDto data)
        {
            // 1. 输入验证
            if (data == null || data.Count == 0)
                throw new ArgumentNullException(nameof(data), "提交数据不能为空");

            if (string.IsNullOrEmpty(type))
                throw new ArgumentNullException(nameof(type), "类型参数不能为空");

            if (id <= 0)
                throw new ArgumentException("ID 必须为正整数", nameof(id));

            // 2. 从数据中提取需要更新的字段
            if (!data.TryGetValue("geometricIndustryStandardLong", out var standardValue) || IsNullOrEmpty(standardValue))
                throw new ArgumentException("标准字段不能为空", nameof(data));

            if (!data.TryGetValue("materialsCategoryLong", out var mainMaterialValue) || IsNullOrEmpty(mainMaterialValue))
                throw new ArgumentException("主材料字段不能为空", nameof(data));

            // 3. 处理 JsonData 字段
            var jsonDataValue = data.TryGetValue("JsonData", out var jsonData) && !IsNullOrEmpty(jsonData)
                ? jsonData.ToString()
                : null;

            // 4. 转换 JsonElement 类型为 Dapper 可识别的类型
            var geometricIndustryStandardCl = ConvertJsonElement(standardValue);
            var materialsCategoryCl = ConvertJsonElement(mainMaterialValue);

            // 5. 执行更新操作
            using var conn = _context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open) await conn.OpenAsync();
            using var transaction = conn.BeginTransaction();
            try
            {
                // 使用参数化查询更新字段
                string sql = @"
                    UPDATE S3D_Rule_PipingCompStandard 
                    SET 
                        GeometricIndustryStandard_CL = @GeometricIndustryStandard_CL,
                        MaterialsCategory_CL = @MaterialsCategory_CL,
                        JsonData = @JsonData,
                        ModifiedDate = GETDATE()
                    WHERE ID = @Id
                ";

                int affected = await conn.ExecuteAsync(sql, new 
                {
                    GeometricIndustryStandard_CL = geometricIndustryStandardCl,
                    MaterialsCategory_CL = materialsCategoryCl,
                    JsonData = jsonDataValue,
                    Id = id
                }, transaction);

                transaction.Commit();
                return affected;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        #endregion

        #region 5. 删除 (Delete)

        public async Task<int> DeleteAsync(string type, int id)
        {
            var config = _configManager.GetConfig(type);
            var strategy = _strategyFactory.GetStrategy(config.HandlerType);
            return await strategy.DeleteAsync(type, id, config);
        }

        #endregion

        #region 6. 批量删除 (Batch Delete)

        public async Task<int> BatchDeleteAsync(string type, List<int> ids)
        {
            // 1. 输入验证
            if (ids == null || ids.Count == 0)
                throw new ArgumentNullException(nameof(ids), "ID 列表不能为空");

            if (string.IsNullOrEmpty(type))
                throw new ArgumentNullException(nameof(type), "类型参数不能为空");

            // 验证 ids 列表中的值是否为有效整数
            if (ids.Any(id => id <= 0))
                throw new ArgumentException("ID 必须为正整数", nameof(ids));

            // 2. 批量更新 status 为 0
            using var conn = _context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open) await conn.OpenAsync();
            using var transaction = conn.BeginTransaction();
            try
            {
                // 使用参数化查询批量更新 status 为 0 和 ModifiedDate 为当前时间
                string sql = "UPDATE S3D_Rule_PipingCompStandard SET Status = @Status, ModifiedDate = GETDATE() WHERE ID IN @Ids";

                int affected = await conn.ExecuteAsync(sql, new { Status = STATUS_INACTIVE, Ids = ids }, transaction);

                transaction.Commit();
                return affected;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        #endregion

        #region 辅助方法

        private void ValidateInput(DictItemConfig config, DictInputDto data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "提交数据不能为空");

            var errors = new List<string>();

            foreach (var column in config.Columns)
            {
                if (column.IsRequired && !column.IsPrimaryKey)
                {
                    if (!data.TryGetValue(column.DbField, out var value) || IsNullOrEmpty(value))
                    {
                        errors.Add($"字段 '{column.Title}' ({column.DbField}) 不能为空");
                    }
                }
            }

            if (errors.Any())
            {
                throw new Exception($"数据校验失败: {string.Join("; ", errors)}");
            }
        }

        private bool IsNullOrEmpty(object? value)
        {
            if (value == null) return true;
            if (value is string str && string.IsNullOrWhiteSpace(str)) return true;
            return false;
        }

        /// <summary>
        /// 转换 JsonElement 为 Dapper 可识别的类型
        /// </summary>
        private static object ConvertJsonElement(object value)
        {
            // 检查是否为 JsonElement 类型
            if (value.GetType().FullName == "System.Text.Json.JsonElement")
            {
                // 获取 ValueKind 属性
                var valueKindProperty = value.GetType().GetProperty("ValueKind");
                if (valueKindProperty == null)
                    return value.ToString();

                var valueKind = valueKindProperty.GetValue(value, null);
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
                if (getValueMethod != null)
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

        #endregion
    }
}