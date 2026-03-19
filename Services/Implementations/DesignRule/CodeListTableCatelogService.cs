using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Dtos.DesignRule;
using PMCSystem_Backend.Entities.PipeSpecConfig;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Implementations
{
    public class CodeListTableCatelogService : ICodeListTableCatelogService
    {
        private readonly PmcContextCky _context;
        private readonly IMapper _mapper;

        public CodeListTableCatelogService(PmcContextCky context, IMapper mapper)
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
    }
}
