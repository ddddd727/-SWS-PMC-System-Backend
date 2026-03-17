using Dapper;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Dtos.Dict;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Services.Implementations
{
    public class S3dCommonCodeListValueService : IS3dCommonCodeListValueService
    {
        private readonly PmcContextCky _context;

        public async Task<IEnumerable<dynamic>> GetOptionsAsync(
           string tableName,
           DataSourceRelationConfig? relation = null)
        {
            // 第一步：找子表ID
            var childTable = await _context.S3dCommonCodeListTables
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.CodeListTableName == tableName);

            if (childTable == null) return Enumerable.Empty<dynamic>();

            // 第二步：查子表全量数据
            var childValues = await _context.S3dCommonCodeListValues
                .AsNoTracking()
                .Where(v => v.CodeListTableId == childTable.Id)
                .OrderBy(v => v.CodeListNumber)
                .ToListAsync();

            // 没配 LoadRelation → 直接返回，字段名用表名前缀
            if (relation == null)
            {
                return childValues.Select(v => (dynamic)new Dictionary<string, object?>
                {
                    [$"{tableName}_CL"] = v.CodeListNumber,
                    [$"{tableName}_Short"] = v.ShortStringValue,
                    [$"{tableName}_Long"] = v.LongStringValue,
                }).ToList();
            }

            // 配了 LoadRelation → 渐进查父表

            // 第三步：用 SQL 查 S3D_Common_CodeListHierarchy 找父表ID
            using var conn = _context.Database.GetDbConnection();
            if (conn.State != System.Data.ConnectionState.Open)
                await conn.OpenAsync();

            var parentTableId = await conn.ExecuteScalarAsync<int?>(
                "SELECT ParentCodeListTableID FROM S3D_Common_CodeListHierarchy WHERE CodeListTableID = @id",
                new { id = childTable.Id }
            );

            if (parentTableId == null)
            {
                // 找不到父表 → 降级返回，不报错
                return childValues.Select(v => (dynamic)new Dictionary<string, object?>
                {
                    [$"{tableName}_CL"] = v.CodeListNumber,
                    [$"{tableName}_Short"] = v.ShortStringValue,
                    [$"{tableName}_Long"] = v.LongStringValue,
                }).ToList();
            }

            // 第四步：只查子节点实际用到的父节点
            // CodeListTableID + CodeListNumber 联合定位，避免跨表 Number 重复
            var neededParentNumbers = childValues
                .Where(c => c.ParentCodeListNumber != null)
                .Select(c => c.ParentCodeListNumber!.Value)
                .Distinct()
                .ToList();

            var parentValues = await _context.S3dCommonCodeListValues
                .AsNoTracking()
                .Where(v => v.CodeListTableId == parentTableId
                         && neededParentNumbers.Contains(v.CodeListNumber))
                .ToListAsync();

            // 第五步：内存关联组装，字段名全部用表名前缀
            return childValues.Select(v =>
            {
                var parent = v.ParentCodeListNumber == null ? null
                    : parentValues.FirstOrDefault(
                        p => p.CodeListTableId == parentTableId
                          && p.CodeListNumber == v.ParentCodeListNumber
                    );

                return (dynamic)new Dictionary<string, object?>
                {
                    [$"{tableName}_CL"] = v.CodeListNumber,
                    [$"{tableName}_Short"] = v.ShortStringValue,
                    [$"{tableName}_Long"] = v.LongStringValue,
                    [$"{tableName}_Parent_Long"] = parent?.LongStringValue
                };
            }).ToList();
        }

        public S3dCommonCodeListValueService(PmcContextCky context)
        {
            _context = context;
        }

        public async Task<IEnumerable<S3dCommonCodeListValueDto>> GetMaterialsCategoryAsync()
        {
            return await GetByTableNameAsync("MaterialsCategory");
        }

        public async Task<IEnumerable<S3dCommonCodeListValueDto>> GetScheduleThicknessAsync()
        {
            return await GetByTableNameAsync("ScheduleThickness");
        }

        public async Task<IEnumerable<S3dCommonCodeListValueDto>> GetEndStandardAsync()
        {
            return await GetByTableNameAsync("EndStandard");
        }

        public async Task<IEnumerable<S3dCommonCodeListValueDto>> GetGeometricIndustryStandardAsync()
        {
            return await GetByTableNameAsync("GeometricIndustryStandard");
        }

        public async Task<IEnumerable<S3dCommonCodeListValueDto>> GetMaterialsGradeAsync()
        {
            return await GetByTableNameAsync("MaterialsGrade");
        }

        public async Task<IEnumerable<S3dCommonCodeListValueDto>> GetShortCodeHierarchyClassAsync()
        {
            return await GetByTableNameAsync("ShortCodeHierarchyClass");
        }

        private async Task<IEnumerable<S3dCommonCodeListValueDto>> GetByTableNameAsync(string tableName)
        {
            var tableEntity = await _context.S3dCommonCodeListTables
                .FirstOrDefaultAsync(t => t.CodeListTableName == tableName);

            if (tableEntity == null)
            {
                return Enumerable.Empty<S3dCommonCodeListValueDto>();
            }

            var values = await _context.S3dCommonCodeListValues
                .Where(v => v.CodeListTableId == tableEntity.Id)
                .Select(v => new S3dCommonCodeListValueDto
                {
                    CodeListNumber = v.CodeListNumber,
                    ShortStringValue = v.ShortStringValue,
                    LongStringValue = v.LongStringValue
                })
                .ToListAsync();

            return values;
        }
    }
}
