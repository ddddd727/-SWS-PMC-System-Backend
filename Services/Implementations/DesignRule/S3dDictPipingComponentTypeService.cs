using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Core.Data;
using PMCSystem_Backend.Modules.DesignRules.Dtos;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Services.Implementations
{
    public class S3dDictPipingComponentTypeService : IS3dDictPipingComponentTypeService
    {
        private readonly PmcContextCky _context;
        private readonly IMapper _mapper;

        public S3dDictPipingComponentTypeService(PmcContextCky context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<S3dDictPipingComponentTypeDto>> GetAllAsync()
        {
            var entities = await _context.S3dDictPipingComponentTypes.ToListAsync();
            return _mapper.Map<IEnumerable<S3dDictPipingComponentTypeDto>>(entities);
        }
    }
}
