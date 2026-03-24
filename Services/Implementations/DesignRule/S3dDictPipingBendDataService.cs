using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Core.Data;
using PMCSystem_Backend.Modules.DesignRules.Dtos;
using PMCSystem_Backend.Modules.DesignRules.Entities;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Services.Implementations
{
    public class S3dDictPipingBendDataService : IS3dDictPipingBendDataService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IS3dCommonCodeListValueService _codeListService;

        public S3dDictPipingBendDataService(AppDbContext context, IMapper mapper, IS3dCommonCodeListValueService codeListService)
        {
            _context = context;
            _mapper = mapper;
            _codeListService = codeListService;
        }

        public async Task<S3dDictPipingBendDataDto> CreateAsync(CreateS3dDictPipingBendDataDto dto)
        {
            var entity = _mapper.Map<S3dDictPipingBendData>(dto);
            entity.Status = dto.Status ?? true;

            _context.S3dDictPipingBendData.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<S3dDictPipingBendDataDto>(entity);
        }

        public async Task<List<S3dDictPipingBendDataDto>> GetAllListAsync()
        {
            var data = await _context.S3dDictPipingBendData.ToListAsync();
            return _mapper.Map<List<S3dDictPipingBendDataDto>>(data);
        }

        public async Task<bool> UpdateAsync(UpdateS3dDictPipingBendDataDto dto)
        {
            var entity = await _context.S3dDictPipingBendData.FindAsync(dto.Id);
            if (entity == null) return false;

            _mapper.Map(dto, entity);
            if (dto.Status.HasValue)
            {
                entity.Status = dto.Status.Value;
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.S3dDictPipingBendData.FindAsync(id);
            if (entity == null) return false;

            _context.S3dDictPipingBendData.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
