using System.Threading.Tasks;
using System.Collections.Generic;
using PMCSystem_Backend.Modules.StandardComponents.Dtos;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface IDictService
    {
        Task<DictTableDto> GetTableDataAsync(string type, string? keyword = null);
        Task<int> AddAsync(string type, DictInputDto data);
        Task<int> UpdateAsync(string type, int id, DictInputDto data);
        Task<int> DeleteAsync(string type, int id);

        /// <summary>
        /// 远程字段校验（内置规则 + IsUnique + UniqueConstraints）
        /// 供 CustomRules.Type=Url 的前端调用
        /// </summary>
        Task<DictValidateResponse> ValidateFieldAsync(string type, DictValidateRequest request);
        Task<IEnumerable<dynamic>> GetCodeListOptionsAsync(string tableName, DataSourceRelationConfig? relation = null);

        /// <summary>
        /// 按列级/表格级 OptionsSource 策略解析下拉选项。field 对应列 DbField，多 Select 时需传。
        /// optionsSource 为查询参数覆盖：view / codelist（与同字典内列配置冲突时以查询为准）。
        /// </summary>
        Task<IEnumerable<dynamic>> GetDropdownOptionsAsync(string dictType, string? field = null, string? optionsSource = null);

        /// <summary>从视图 DISTINCT 查询选项行。</summary>
        Task<IEnumerable<dynamic>> GetDictViewOptionsAsync(string viewName, IReadOnlyList<string> columns);
    }
}