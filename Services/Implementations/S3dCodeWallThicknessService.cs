using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Services.Implementations
{
    public class S3dCodeWallThicknessService : IS3dCodeWallThicknessService
    {
        private readonly PmcContextCky _context;
        private readonly IMapper _mapper;

        public S3dCodeWallThicknessService(PmcContextCky context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<S3dCodeWallThicknessDto>> GetAllAsync()
        {
            var data = await _context.S3dCodeWallThicknesses.ToListAsync();
            return _mapper.Map<List<S3dCodeWallThicknessDto>>(data);
        }
    }
}
