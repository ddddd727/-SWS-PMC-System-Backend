using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Dtos.DesignRules;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Services.Implementations
{
    public class PipingBendParameterCodeConvertedService : IPipingBendParameterCodeConvertedService
    {
        private readonly PmcContextCky _context;
        private readonly IMapper _mapper;

        public PipingBendParameterCodeConvertedService(PmcContextCky context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<PipingBendParameterCodeConvertedDto>> GetAllAsync()
        {
            var data = await _context.S3dCodePipingBendParameters.ToListAsync();
            return _mapper.Map<List<PipingBendParameterCodeConvertedDto>>(data);
        }
    }
}

