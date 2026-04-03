using Dapper;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Core.Data;
using PMCSystem_Backend.Modules.StandardComponents.Dtos;
using PMCSystem_Backend.Services.Interfaces;
using System.Data;

namespace PMCSystem_Backend.Services.Implementations.DictStrategies
{
    public class FlangeDictStrategy(AppDbContext context) : IDictStrategy
    {
        private readonly AppDbContext _context = context;

        /// <summary>
        /// 查询 Piping Component Type 数据
        /// </summary>
        public async Task<IEnumerable<dynamic>> GetPipingComponentTypeDataAsync(int componentTypeId)
        {
            using var conn = _context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open) await conn.OpenAsync();

            string sql = @"
    SELECT 
        t1.ID as id,
        t1.GeometricIndustryStandard_CL as geometricIndustryStandardCL,
        v1.LongStringValue as geometricIndustryStandardLong, 
        t.ComponentTypeName as componentTypeName, 
        t.ComponentTypeDescription as componentTypeDescription,
        t1.MaterialsCategory_CL as materialsCategoryCL,
        v2.LongStringValue as materialsCategoryLong,
        t1.status,
        t1.JsonData
    FROM S3D_Dict_ComponentType t 
    LEFT JOIN S3D_Rule_PipingCompStandard t1 ON t.id = t1.ComponentTypeID 
    LEFT JOIN S3D_Common_CodeListTable ct1 ON ct1.CodeListTableName = 'GeometricIndustryStandard'
    LEFT JOIN S3D_Common_CodeListValue v1 ON t1.GeometricIndustryStandard_CL = v1.CodeListNumber 
        AND v1.CodeListTableID = ct1.ID
    LEFT JOIN S3D_Common_CodeListTable ct2 ON ct2.CodeListTableName = 'MaterialsCategory'
    LEFT JOIN S3D_Common_CodeListValue v2 ON t1.MaterialsCategory_CL = v2.CodeListNumber 
        AND v2.CodeListTableID = ct2.ID
    WHERE t.ID = @ComponentTypeId
      AND t1.ID IS NOT NULL";

            return await conn.QueryAsync(sql, new { ComponentTypeId = componentTypeId });
        }

        public Task<int> AddAsync(string type, DictItemConfig config, DictInputDto data) => throw new NotImplementedException("法兰连接复杂逻辑开发中");
        public Task<int> UpdateAsync(string type, int id, DictItemConfig config, DictInputDto data) => throw new NotImplementedException();
        public Task<int> DeleteAsync(string type, int id, DictItemConfig config) => throw new NotImplementedException();
    }
}