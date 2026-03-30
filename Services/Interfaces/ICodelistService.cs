using PMCSystem_Backend.Modules.PipingSpecifications.Dtos.CodelistTable;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface ICodelistService
    {
        /// <summary>
        /// 根据Codelist表名和Codelist值获取对应的短描述
        /// </summary>
        /// <param name="codelistTableName"></param>
        /// <param name="codelistValue"></param>
        /// <returns></returns>
        Task<string> GetShortDesciptionByCodelistValue(string codelistTableName, string codelistValue);


        /// <summary>
        /// 根据列名和Codelist值（整数类型）获取对应的描述
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="codelistValue"></param>
        /// <returns></returns>
        Task<string> GetCodelistDescriptionAsync(string columnName, int codelistValue);

        /// <summary>
        /// 根据 Codelist 表名和短描述反查对应的 Codelist 值
        /// </summary>
        /// <param name="codelistTableName">Codelist 表名</param>
        /// <param name="shortDescription">短描述（ShortStringValue）</param>
        /// <returns>匹配到的 CodeListNumber；未命中或歧义时返回 null</returns>
        Task<int?> GetCodeListNumberByShortDescriptionAsync(string codelistTableName, string shortDescription);


        /// <summary>
        /// 批量获取Codelist描述
        /// </summary>
        /// <param name="codelistTableName"></param>
        /// <param name="codelistNumbers"></param>
        /// <returns></returns>
        Task<Dictionary<int, string>> GetCodelistDescriptionsAsync(string codelistTableName, IEnumerable<int> codelistNumbers);

        /// <summary>
        /// 根据父表名和父表代码值获取对应子表中所有的值
        /// </summary>
        /// <param name="parentTableName"></param>
        /// <param name="parentCodeNumber"></param>
        /// <returns></returns>
        Task<Dictionary<string, List<CodeListItem>>> GetChildCodeListsByParentValueAsync(string parentTableName, int parentCodeNumber);

        /// <summary>
        /// 获取父表的所有直接子表
        /// </summary>
        /// <param name="parentTableName"></param>
        /// <returns></returns>
        Task<List<CodeListTableInfo>> GetDirectChildTablesAsync(string parentTableName);

        /// <summary>
        /// 获取指定表的所有父表
        /// </summary>
        /// <param name="childTableName"></param>
        /// <returns></returns>
        Task<List<CodeListTableInfo>> GetParentTablesAsync(string childTableName);
    }
}
