using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.Models;
using PMCSystem_Backend.Services.Interface;

namespace PMCSystem_Backend.Services.Impletation
{
    public class RuleAb2b3c2Service : IRuleAb2b3c2Service
    {
        private readonly PmcTestContext _context;
        private readonly IMapper _mapper;

        public RuleAb2b3c2Service(PmcTestContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<S3dRuleAb2b3c2Dto> GetAll()
        {
            return _context.S3dRuleAb2b3c2s
                .AsNoTracking()
                .ProjectTo<S3dRuleAb2b3c2Dto>(_mapper.ConfigurationProvider)
                .ToList();
        }

        public S3dRuleAb2b3c2Dto? GetById(int id)
        {
            var entity = _context.S3dRuleAb2b3c2s
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id);
            return entity == null ? null : _mapper.Map<S3dRuleAb2b3c2Dto>(entity);
        }
    }
}
