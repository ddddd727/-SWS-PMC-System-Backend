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
    public class DictPipingService : IDictPipingService
    {
        private readonly DictStrategyFactory _strategyFactory;
        private readonly DictConfigManager _configManager;
        private readonly PmcContext _context;

        public DictPipingService(DictStrategyFactory strategyFactory, DictConfigManager configManager, PmcContext context)
        {
            _strategyFactory = strategyFactory;
            _configManager = configManager;
            _context = context;
        }

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

            var componentTypeName = type.Replace("part-", "");
            var componentTypeResult = await conn.QueryFirstOrDefaultAsync<dynamic>(
                "SELECT ID, ConnectType FROM S3D_Dict_PipingComponentType WHERE ComponentTypeName = @ComponentTypeName and status = 1",
                new { ComponentTypeName = componentTypeName });

            if (componentTypeResult != null)
            {
                var componentTypeId = componentTypeResult.ID;
                var connectType = componentTypeResult.ConnectType;

                // 根据 ConnectType 决定使用哪种策略
                string strategyType = connectType == "法兰" ? "Flange" : "Fitting";
                var pipingStrategy = _strategyFactory.GetStrategy(strategyType);

                if (pipingStrategy != null)
                {
                    // 根据策略类型调用相应的方法
                    IEnumerable<dynamic> pipingData;
                    if (strategyType == "Fitting" && pipingStrategy is FittingDictStrategy fittingStrategy)
                    {
                        pipingData = await fittingStrategy.GetPipingComponentTypeDataAsync(componentTypeId);
                    }
                    else if (strategyType == "Flange" && pipingStrategy is FlangeDictStrategy flangeStrategy)
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
            var config = _configManager.GetConfig(type);
            ValidateInput(config, data);
            var strategy = _strategyFactory.GetStrategy(config.HandlerType);
            return await strategy.UpdateAsync(type, id, config, data);
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
            var config = _configManager.GetConfig(type);
            var strategy = _strategyFactory.GetStrategy(config.HandlerType);
            int totalAffected = 0;

            foreach (var id in ids)
            {
                int affected = await strategy.DeleteAsync(type, id, config);
                totalAffected += affected;
            }

            return totalAffected;
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

        #endregion
    }
}