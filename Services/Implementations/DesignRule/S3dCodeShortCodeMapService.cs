using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Core.Data;
using PMCSystem_Backend.Modules.DesignRules.Dtos;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Implementations
{
    public class S3dCodeShortCodeMapService : IS3dCodeShortCodeMapService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public S3dCodeShortCodeMapService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<S3dCodeShortCodeMapDto>> GetAllAsync()
        {
            var entities = await _context.S3dCodeShortCodeMaps.ToListAsync();
            return _mapper.Map<List<S3dCodeShortCodeMapDto>>(entities);
        }
    }
}
