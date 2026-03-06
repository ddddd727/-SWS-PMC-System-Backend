using Dapper;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Dtos.Dict;
using PMCSystem_Backend.Services.Interfaces;
using System.Data;

namespace PMCSystem_Backend.Services.Implementations.DictStrategies
{
    public class FlangeDictStrategy : IDictStrategy
    {
        private readonly PmcContext _context;

        public FlangeDictStrategy(PmcContext context)
        {
            _context = context;
        }

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
                    v1.LongStringValue as standard, 
                    t.ComponentTypeName as type, 
                    t.ComponentTypeDescription as description, 
                    v2.LongStringValue as mainMaterial 
                FROM S3D_Dict_PipingComponentType t 
                LEFT JOIN S3D_Rule_PipingCompStandard t1 ON t.id = t1.ComponentTypeID 
                LEFT JOIN S3D_CL_GeometricIndustryStandard v1 ON t1.GeometricIndustryStandard_CL = v1.CodeListNumber 
                LEFT JOIN S3D_CL_MaterialsCategory v2 ON t1.MaterialsCategory_CL = v2.CodeListNumber 
                WHERE t.ID = @ComponentTypeId AND t1.Status = 1";

            return await conn.QueryAsync(sql, new { ComponentTypeId = componentTypeId });
        }

        public Task<int> AddAsync(string type, DictItemConfig config, DictInputDto data) => throw new NotImplementedException("法兰连接复杂逻辑开发中");
        public Task<int> UpdateAsync(string type, int id, DictItemConfig config, DictInputDto data) => throw new NotImplementedException();
        public Task<int> DeleteAsync(string type, int id, DictItemConfig config) => throw new NotImplementedException();
    }
}