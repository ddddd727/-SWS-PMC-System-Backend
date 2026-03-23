using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Core.Data;
using PMCSystem_Backend.Modules.DesignRules.Dtos;
using PMCSystem_Backend.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Services.Implementations
{
    public class S3dCodePipingClassViewService : IS3dCodePipingClassViewService
    {
        private readonly SpecContext _context;

        public S3dCodePipingClassViewService(SpecContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<S3dCodePipingClassDto>> GetAllAsync()
        {
            return await _context.S3dCodePipingClasses
                .AsNoTracking()
                .Select(x => new S3dCodePipingClassDto
                {
                    PipingClassCode = x.PipingClassCode,
                    ShortStringValue = x.ShortStringValue,
                    CodeListNumber = x.CodeListNumber
                })
                .ToListAsync();
        }
    }
}

