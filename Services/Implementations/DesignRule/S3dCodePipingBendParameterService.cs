using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Dtos.PmcSpecRuleConfig;
using PMCSystem_Backend.Entities;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Implementations
{
    public class S3dCodePipingBendParameterService : IS3dCodePipingBendParameterService
    {
        private readonly PmcContextCky _context;
        private readonly IMapper _mapper;

        public S3dCodePipingBendParameterService(PmcContextCky context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<S3dCodePipingBendParameterDto>> GetAllAsync()
        {
            var entities = await _context.S3dCodePipingBendParameters.ToListAsync();
            return _mapper.Map<List<S3dCodePipingBendParameterDto>>(entities);
        }
    }
}
