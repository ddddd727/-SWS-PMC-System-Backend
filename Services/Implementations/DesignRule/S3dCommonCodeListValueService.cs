using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Core.Data;
using PMCSystem_Backend.Modules.DesignRules.Dtos;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Services.Implementations
{
    public class S3dCommonCodeListValueService : IS3dCommonCodeListValueService
    {
        private readonly AppDbContext _context;
        public async Task<IEnumerable<dynamic>> GetOptionsAsync(string tableName)
        {

            var tableEntity = await _context.S3dCommonCodeListTables
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.CodeListTableName == tableName);

            if (tableEntity == null)
            {

                return Enumerable.Empty<dynamic>();
            }

            // 2. 查询该表的所有选项
            var values = await _context.S3dCommonCodeListValues
                .AsNoTracking()
                .Where(v => v.CodeListTableId == tableEntity.Id)
                .OrderBy(v => v.CodeListNumber)
                .Select(v => new
                {
                    Value = v.CodeListNumber,
                    Short = v.ShortStringValue,
                    Long = v.LongStringValue,
                })
                .ToListAsync();

            return values;
        }
        public S3dCommonCodeListValueService(AppDbContext context)
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
