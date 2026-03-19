using System.Threading.Tasks;
using System.Collections.Generic;
using PMCSystem_Backend.Dtos.Dict;

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
    }
}