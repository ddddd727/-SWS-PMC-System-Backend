using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Core.Data;
using PMCSystem_Backend.Modules.StandardComponents.Dtos;
using PMCSystem_Backend.Services.Interfaces.CodeListManagement;
using PMCSystem_Backend.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Implementations.CodeListManagement
{
    public class CodeListTableCatelogService : ICodeListTableCatelogService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CodeListTableCatelogService(AppDbContext context, IMapper mapper)
        {
            _context = context;
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





        public async Task<CodeListCombinedResponseDto> GetCombinedCodeListAsync(string codeListTableName)
        {
            if (string.IsNullOrWhiteSpace(codeListTableName))
                throw new ArgumentException("codeListTableName cannot be empty", nameof(codeListTableName));

            // 获取层级信息
            var start = await _context.S3dCommonCodeListTables
                .AsNoTracking()
                .Where(t => t.CodeListTableName == codeListTableName)
                .Select(t => new { t.Id, t.CodeListTableName })
                .FirstOrDefaultAsync();

            var result = new List<string>();
            if (start != null)
            {
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

                    var hierarchy = await _context.S3dCommonCodeListHierarchies
                        .AsNoTracking()
                        .Where(h => h.CodeListTableId == currentId)
                        .Select(h => h.ParentCodeListTableId)
                        .FirstOrDefaultAsync();

                    if (hierarchy == null)
                        break;

                    currentId = hierarchy.Value;
                }

                result.Reverse();
            }

            var combinedResponse = new CodeListCombinedResponseDto { Count = result.Count };
            if (result.Count > 0) combinedResponse.Level1 = result[0];
            if (result.Count > 1) combinedResponse.Level2 = result[1];
            if (result.Count > 2) combinedResponse.Level3 = result[2];
            if (result.Count > 3) combinedResponse.Level4 = result[3];
            if (result.Count > 4) combinedResponse.Level5 = result[4];

            // 构建层级名称列表
            var levels = new List<string?>();
            if (!string.IsNullOrWhiteSpace(combinedResponse.Level1)) levels.Add(combinedResponse.Level1);
            if (!string.IsNullOrWhiteSpace(combinedResponse.Level2)) levels.Add(combinedResponse.Level2);
            if (!string.IsNullOrWhiteSpace(combinedResponse.Level3)) levels.Add(combinedResponse.Level3);
            if (!string.IsNullOrWhiteSpace(combinedResponse.Level4)) levels.Add(combinedResponse.Level4);
            if (!string.IsNullOrWhiteSpace(combinedResponse.Level5)) levels.Add(combinedResponse.Level5);

            if (levels.Count == 0)
                return combinedResponse;

            // 获取各层级数据
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
                return combinedResponse;

            // 按层级添加数据
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
                    combinedResponse.LevelData.Add(data);
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
                    combinedResponse.LevelData.Add(data);
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
                    combinedResponse.LevelData.Add(data);
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
                    combinedResponse.LevelData.Add(data);
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
                    combinedResponse.LevelData.Add(data);
                }
            }

            return combinedResponse;
        }

        public async Task<List<CodeListValueDto>> GetCodeListValuesByParentShortStringValueAsync(string shortStringValue)
        {
            if (string.IsNullOrWhiteSpace(shortStringValue))
                throw new ArgumentException("shortStringValue cannot be empty", nameof(shortStringValue));

            // 首先根据 ShortStringValue 找到对应的 CodeListTableID 和 CodeListNumber
            var codeListValue = await _context.S3dCommonCodeListValues
                .AsNoTracking()
                .Where(v => v.ShortStringValue == shortStringValue)
                .FirstOrDefaultAsync();

            if (codeListValue == null)
                return new List<CodeListValueDto>();

            // 使用 CodeListTableID 去 S3D_Common_CodeListHierarchy 作为 ParentCodeListTableID 查找对应行的 CodeListTableID
            var childCodeListTable = await _context.S3dCommonCodeListHierarchies
                .AsNoTracking()
                .Where(h => h.ParentCodeListTableId == codeListValue.CodeListTableId)
                .FirstOrDefaultAsync();

            if (childCodeListTable == null)
                return new List<CodeListValueDto>();

            // 使用找到的 CodeListNumber 作为 ParentCodeListNumber，并且 CodeListTableID 为找到的子表ID，查询相关数据
            var parentCodeListNumber = codeListValue.CodeListNumber;
            var relatedValues = await _context.S3dCommonCodeListValues
                .AsNoTracking()
                .Where(v => v.ParentCodeListNumber == parentCodeListNumber && v.CodeListTableId == childCodeListTable.CodeListTableId)
                .Select(v => new CodeListValueDto
                {
                    ShortStringValue = v.ShortStringValue,
                    LongStringValue = v.LongStringValue,
                    CodeListNumber = v.CodeListNumber,
                    Status = v.Status ? 1 : 0
                })
                .ToListAsync();

            return relatedValues;
        }

        public async Task<CodeListValueDto> CreateCodeListValueAsync(CreateCodeListValueDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // 1. 根据 CodeListTableName 查找对应的 CodeListTableID
            var codeListTable = await _context.S3dCommonCodeListTables
                .AsNoTracking()
                .Where(t => t.CodeListTableName == dto.CodeListTableName)
                .FirstOrDefaultAsync();

            if (codeListTable == null)
                throw new ArgumentException($"CodeListTable with name '{dto.CodeListTableName}' not found");

            // 2. 根据 ParentShortStringValue 查找对应的 CodeListNumber 作为 ParentCodeListNumber
            int? parentCodeListNumber = null;
            if (!string.IsNullOrWhiteSpace(dto.ParentShortStringValue))
            {
                var parentCodeListValue = await _context.S3dCommonCodeListValues
                    .AsNoTracking()
                    .Where(v => v.ShortStringValue == dto.ParentShortStringValue)
                    .FirstOrDefaultAsync();

                if (parentCodeListValue != null)
                {
                    parentCodeListNumber = parentCodeListValue.CodeListNumber;
                }
            }

            // 3. 创建新的 CodeListValue 实体
            var newCodeListValue = new S3dCommonCodeListValue
            {
                CodeListTableId = codeListTable.Id,
                ParentCodeListNumber = parentCodeListNumber,
                ShortStringValue = dto.ShortStringValue,
                LongStringValue = dto.LongStringValue,
                CodeListNumber = dto.CodeListNumber,
                IsUserDefine = true, // 默认值
                Status = dto.Status
            };

            // 4. 保存到数据库
            _context.S3dCommonCodeListValues.Add(newCodeListValue);
            await _context.SaveChangesAsync();

            // 5. 映射并返回结果
            return new CodeListValueDto
            {
                ShortStringValue = newCodeListValue.ShortStringValue,
                LongStringValue = newCodeListValue.LongStringValue,
                CodeListNumber = newCodeListValue.CodeListNumber,
                Status = newCodeListValue.Status ? 1 : 0
            };
        }
    }
}
