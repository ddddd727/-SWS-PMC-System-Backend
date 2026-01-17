using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Services.Implementations
{
    public class PipingBendParameterCodeConvertedService : IPipingBendParameterCodeConvertedService
    {
        private readonly PmcTestContext _context;
        private readonly IMapper _mapper;

        public PipingBendParameterCodeConvertedService(PmcTestContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<PipingBendParameterCodeConvertedDto>> GetAllAsync()
        {
            var data = await _context.S3dRulePipingBendParameterCodeConverted.ToListAsync();
            return _mapper.Map<List<PipingBendParameterCodeConvertedDto>>(data);
        }
    }
}

