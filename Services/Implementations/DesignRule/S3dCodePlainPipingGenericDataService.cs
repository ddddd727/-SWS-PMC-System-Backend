using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Core.Data;
using PMCSystem_Backend.Modules.DesignRules.Dtos;
using PMCSystem_Backend.Services.Interfaces;

namespace PMCSystem_Backend.Services.Implementations
{
    public class S3dCodePlainPipingGenericDataService : IS3dCodePlainPipingGenericDataService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public S3dCodePlainPipingGenericDataService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<S3dCodePlainPipingGenericDataDto>> GetAllAsync()
        {
            var entities = await _context.S3dCodePlainPipingGenericData
                .FromSqlRaw(
                    """
                    SELECT
                        ID,
                        COALESCE(
                            TRY_CAST(NominalPipingDiameter AS float),
                            TRY_CONVERT(float, REPLACE(CONVERT(nvarchar(100), NominalPipingDiameter), ',', '.'))
                        ) AS NominalPipingDiameter,
                        NominalDiameterUnits,
                        EndStandard_CL,
                        EndStandard,
                        ScheduleThickness_CL,
                        ScheduleThickness,
                        PipingOutsideDiameter,
                        WallThickness,
                        Status
                    FROM dbo.S3D_Code_PlainPipingGenericData
                    """)
                .AsNoTracking()
                .ToListAsync();
            return _mapper.Map<List<S3dCodePlainPipingGenericDataDto>>(entities);
        }
    }
}
