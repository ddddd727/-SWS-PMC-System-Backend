using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PMCSystem_Backend.Modules.DesignRules.Entities;
using PMCSystem_Backend.Modules.PipingSpecifications.Entities;
using PMCSystem_Backend.Modules.PMCRuleConfig.Entities;
using PMCSystem_Backend.Shared.Entities;

namespace PMCSystem_Backend.Core.Data;

/// <summary>
/// 统一的 DbContext，合并原 PmcContext、PmcContextCky、PmcContextLr、SpecContext 的实体和配置。
/// </summary>
public partial class AppDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public AppDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<S3dCdbPipeComponent> S3dCdbPipeComponents { get; set; }
    public virtual DbSet<S3dCdbPipeStock> S3dCdbPipeStocks { get; set; }
    public virtual DbSet<S3dRulePmcData> S3dRulePmcdata { get; set; }
    public virtual DbSet<S3dRuleShortCodeMap> S3dRuleShortCodeMaps { get; set; }
    public virtual DbSet<S3dRuleShortCodeHierarchyRule> S3dRuleShortCodeHierarchyRules { get; set; }
    public virtual DbSet<S3dRulePipingBendParameter> S3dRulePipingBendParameters { get; set; }
    public virtual DbSet<S3dRulePipingCompStandard> S3dRulePipingCompStandards { get; set; }
    public virtual DbSet<S3dCommonCodeListHierarchy> S3dCommonCodeListHierarchies { get; set; }
    public virtual DbSet<S3dCommonCodeListTable> S3dCommonCodeListTables { get; set; }
    public virtual DbSet<S3dCommonCodeListValue> S3dCommonCodeListValues { get; set; }
    public virtual DbSet<S3dDictPipingComponentType> S3dDictPipingComponentTypes { get; set; }
    public virtual DbSet<S3dRuleComponentTypeHierarchyRule> S3dRuleComponentTypeHierarchyRules { get; set; }
    public virtual DbSet<S3dDictPipingBendData> S3dDictPipingBendData { get; set; }
    public virtual DbSet<S3dWallThicknessInfo> S3dWallThicknessInfo { get; set; }
    public virtual DbSet<S3dCodePipingBendParameter> S3dCodePipingBendParameters { get; set; }
    public virtual DbSet<S3dCodePlainPipingGenericData> S3dCodePlainPipingGenericData { get; set; }
    public virtual DbSet<S3dCodeShortCodeMap> S3dCodeShortCodeMaps { get; set; }
    public virtual DbSet<S3dClMaterialsGrade> S3dClMaterialsGrades { get; set; }
    public virtual DbSet<S3dCommonPlainPipingGenericData> S3dCommonPlainPipingGenericData { get; set; }
    public virtual DbSet<PipeSpecVersion> PipeSpecVersions { get; set; }
    public virtual DbSet<S3dRuleAb2b3c2> S3dRuleAb2b3c2s { get; set; }
    public virtual DbSet<S3dRuleB1b2b3d> S3dRuleB1b2b3ds { get; set; }
    public virtual DbSet<S3dRuleC1c2> S3dRuleC1c2s { get; set; }
    public virtual DbSet<S3dCodeAb2b3c2> S3dCodeAb2b3c2s { get; set; }
    public virtual DbSet<S3dCodeB1b2b3d> S3dCodeB1b2b3ds { get; set; }
    public virtual DbSet<S3dCodeC1c2> S3dCodeC1c2s { get; set; }
    public virtual DbSet<S3dCodePipingClass> S3dCodePipingClasses { get; set; }
    public virtual DbSet<S3dCodeFlangeStandPressureRating> S3dCodeFlangeStandPressureRatings { get; set; }
    public virtual DbSet<S3dCodeMaterialsCategoryPipingStandard> S3dCodeMaterialsCategoryPipingStandards { get; set; }
    public virtual DbSet<S3dCodeMaterialsCategoryScheduleThickness> S3dCodeMaterialsCategoryScheduleThicknesses { get; set; }
    public virtual DbSet<S3dCodePipingStandardMaterialsGrade> S3dCodePipingStandardMaterialsGrades { get; set; }
    public virtual DbSet<S3dCodePipingStandardPressureRating> S3dCodePipingStandardPressureRatings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
