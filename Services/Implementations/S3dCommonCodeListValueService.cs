using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Services.Implementations
{
    public class S3dCommonCodeListValueService : IS3dCommonCodeListValueService
    {
        private readonly PmcContextCky _context;

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

        private async Task<IEnumerable<S3dCommonCodeListValueDto>> GetByTableNameAsync(string tableName)
        {
            // 1. Find the ID from S3D_Common_CodeListTable where CodeListTableName matches
            var tableEntity = await _context.S3dCommonCodeListTables
                .FirstOrDefaultAsync(t => t.CodeListTableName == tableName);

            if (tableEntity == null)
            {
                return Enumerable.Empty<S3dCommonCodeListValueDto>();
            }

            // 2. Find all values from S3D_Common_CodeListValue where CodeListTableID matches
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
