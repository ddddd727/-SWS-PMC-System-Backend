using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Dtos.DesignRule;
using PMCSystem_Backend.Entities.PipeSpecConfig;
using PMCSystem_Backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Implementations
{
    public class CodeListTableCatelogService : ICodeListTableCatelogService
    {
        private readonly PmcContextCky _context;
        private readonly PmcContext _hierarchyContext;
        private readonly IMapper _mapper;

        public CodeListTableCatelogService(PmcContextCky context, PmcContext hierarchyContext, IMapper mapper)
        {
            _context = context;
            _hierarchyContext = hierarchyContext;
            _mapper = mapper;
        }

        public async Task<List<CodeListTableCatelogDto>> GetAllAsync()
        {
            var entities = await _context.S3dCommonCodeListTables
                .AsNoTracking()
                .ToListAsync();
            return _mapper.Map<List<CodeListTableCatelogDto>>(entities);
        }

        public async Task<CodeListTableCatelogDto> CreateAsync(CreateCodeListTableCatelogDto dto)
        {
            var entity = _mapper.Map<S3dCommonCodeListTable>(dto);
            
            // ID is identity, so we don't set it manually
            entity.Id = 0; 
            
            _context.S3dCommonCodeListTables.Add(entity);
            await _context.SaveChangesAsync();
            
            return _mapper.Map<CodeListTableCatelogDto>(entity);
        }

        public async Task<CodeListHierarchyDto> GetHierarchyNamesAsync(string codeListTableName)
        {
            if (string.IsNullOrWhiteSpace(codeListTableName))
                throw new ArgumentException("codeListTableName cannot be empty", nameof(codeListTableName));

            var start = await _context.S3dCommonCodeListTables
                .AsNoTracking()
                .Where(t => t.CodeListTableName == codeListTableName)
                .Select(t => new { t.Id, t.CodeListTableName })
                .FirstOrDefaultAsync();

            if (start == null)
                return new CodeListHierarchyDto { Count = 0 };

            var result = new List<string>();
            var visited = new HashSet<int>();

            var currentId = start.Id;
            while (visited.Add(currentId))
            {
                var currentName = await _context.S3dCommonCodeListTables
                    .AsNoTracking()
                    .Where(t => t.Id == currentId)
                    .Select(t => t.CodeListTableName)
                    .FirstOrDefaultAsync();

                if (string.IsNullOrWhiteSpace(currentName))
                    break;

                result.Add(currentName);

                var hierarchy = await _hierarchyContext.S3dCommonCodeListHierarchies
                    .AsNoTracking()
                    .Where(h => h.CodeListTableId == currentId)
                    .Select(h => h.ParentCodeListTableId)
                    .FirstOrDefaultAsync();

                if (hierarchy == null)
                    break;

                currentId = hierarchy.Value;
            }

            result.Reverse();

            var hierarchyDto = new CodeListHierarchyDto { Count = result.Count };
            if (result.Count > 0) hierarchyDto.Level1 = result[0];
            if (result.Count > 1) hierarchyDto.Level2 = result[1];
            if (result.Count > 2) hierarchyDto.Level3 = result[2];
            if (result.Count > 3) hierarchyDto.Level4 = result[3];
            if (result.Count > 4) hierarchyDto.Level5 = result[4];

            return hierarchyDto;
        }

        public async Task<MultiLevelCodeListResponseDto> GetMultiLevelCodeListValuesAsync(MultiLevelCodeListRequestDto request)
        {
            var response = new MultiLevelCodeListResponseDto();
            var levels = new List<string?>();
            
            if (!string.IsNullOrWhiteSpace(request.Level1)) levels.Add(request.Level1);
            if (!string.IsNullOrWhiteSpace(request.Level2)) levels.Add(request.Level2);
            if (!string.IsNullOrWhiteSpace(request.Level3)) levels.Add(request.Level3);
            if (!string.IsNullOrWhiteSpace(request.Level4)) levels.Add(request.Level4);
            if (!string.IsNullOrWhiteSpace(request.Level5)) levels.Add(request.Level5);

            if (levels.Count == 0)
                return response;

            var valuesList = new List<List<S3dCommonCodeListValue>>();

            foreach (var levelName in levels)
            {
                var table = await _context.S3dCommonCodeListTables
                    .AsNoTracking()
                    .Where(t => t.CodeListTableName == levelName)
                    .FirstOrDefaultAsync();

                if (table == null)
                    continue;

                var values = await _context.S3dCommonCodeListValues
                    .AsNoTracking()
                    .Where(v => v.CodeListTableId == table.Id)
                    .ToListAsync();

                valuesList.Add(values);
            }

            if (valuesList.Count == 0)
                return response;

            // 先添加 level1 的数据
            if (valuesList.Count > 0)
            {
                foreach (var v in valuesList[0])
                {
                    var data = new CodeListLevelData();
                    data.Level1ShortDesc = v.ShortStringValue;
                    data.Level1LongDesc = v.LongStringValue;
                    data.Level1CodeNum = v.CodeListNumber;
                    data.Level1Status = v.Status ? 1 : 0;
                    response.LevelData.Add(data);
                }
            }

            // 再添加 level2 的数据
            if (valuesList.Count > 1)
            {
                foreach (var v in valuesList[1])
                {
                    var data = new CodeListLevelData();
                    data.Level2ShortDesc = v.ShortStringValue;
                    data.Level2LongDesc = v.LongStringValue;
                    data.Level2CodeNum = v.CodeListNumber;
                    data.Level2Status = v.Status ? 1 : 0;
                    response.LevelData.Add(data);
                }
            }

            // 再添加 level3 的数据
            if (valuesList.Count > 2)
            {
                foreach (var v in valuesList[2])
                {
                    var data = new CodeListLevelData();
                    data.Level3ShortDesc = v.ShortStringValue;
                    data.Level3LongDesc = v.LongStringValue;
                    data.Level3CodeNum = v.CodeListNumber;
                    data.Level3Status = v.Status ? 1 : 0;
                    response.LevelData.Add(data);
                }
            }

            // 再添加 level4 的数据
            if (valuesList.Count > 3)
            {
                foreach (var v in valuesList[3])
                {
                    var data = new CodeListLevelData();
                    data.Level4ShortDesc = v.ShortStringValue;
                    data.Level4LongDesc = v.LongStringValue;
                    data.Level4CodeNum = v.CodeListNumber;
                    data.Level4Status = v.Status ? 1 : 0;
                    response.LevelData.Add(data);
                }
            }

            // 再添加 level5 的数据
            if (valuesList.Count > 4)
            {
                foreach (var v in valuesList[4])
                {
                    var data = new CodeListLevelData();
                    data.Level5ShortDesc = v.ShortStringValue;
                    data.Level5LongDesc = v.LongStringValue;
                    data.Level5CodeNum = v.CodeListNumber;
                    data.Level5Status = v.Status ? 1 : 0;
                    response.LevelData.Add(data);
                }
            }

            return response;
        }
    }
}
