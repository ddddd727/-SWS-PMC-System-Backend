using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Services.Implementations
{
    public class WallThicknessCodeConvertedService : IWallThicknessCodeConvertedService
    {
        private readonly PmcContext _context;
        private readonly IMapper _mapper;

        public WallThicknessCodeConvertedService(PmcContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<WallThicknessCodeConvertedDto>> GetAllAsync()
        {
            var data = await _context.S3dCodeWallThicknesses.ToListAsync();
            return _mapper.Map<List<WallThicknessCodeConvertedDto>>(data);
        }
    }
}
