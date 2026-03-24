using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using PMCSystem_Backend.Modules.DesignRules.Entities;
using PMCSystem_Backend.Modules.PipingSpecifications.Dtos;
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
        ConfigureS3dCdbPipeComponent(modelBuilder);
        ConfigureS3dCdbPipeStock(modelBuilder);
        ConfigureS3dRulePmcData(modelBuilder);
        ConfigureS3dRuleShortCodeMap(modelBuilder);
        ConfigureS3dRuleShortCodeHierarchyRule(modelBuilder);
        ConfigureS3dRulePipingBendParameter(modelBuilder);
        ConfigureS3dRulePipingCompStandard(modelBuilder);
        ConfigureS3dCommonCodeList(modelBuilder);
        ConfigureS3dDictPipingComponentType(modelBuilder);
        ConfigureS3dRuleComponentTypeHierarchyRule(modelBuilder);
        ConfigureS3dWallThicknessInfo(modelBuilder);
        ConfigurePipeSpecVersion(modelBuilder);
        ConfigureDesignRulesViews(modelBuilder);
        ConfigureDesignRulesPlainPipingGenericData(modelBuilder);
        ConfigurePmcRuleConfigTables(modelBuilder);
        ConfigurePmcRuleConfigViews(modelBuilder);

        OnModelCreatingPartial(modelBuilder);
    }

    private static void ConfigureS3dCdbPipeComponent(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<S3dCdbPipeComponent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_CDB___3214EC27D0D17D28");
            entity.ToTable("S3D_CDB_PipeComponent");
            entity.HasIndex(e => e.IndustryCommodityCode, "UQ__S3D_CDB___782B02A1FEF5916B").IsUnique();
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BendAngle).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.CommodityType).HasMaxLength(255);
            entity.Property(e => e.GeometricIndustryStandard).HasMaxLength(255);
            entity.Property(e => e.GeometryType).HasMaxLength(255);
            entity.Property(e => e.IndustryCommodityCode).HasMaxLength(255);
            entity.Property(e => e.MaterialGrade).HasMaxLength(255);
            entity.Property(e => e.PartClassName).HasMaxLength(255);
            entity.Property(e => e.PartDataBasis).HasMaxLength(100);
        });
    }

    private static void ConfigureS3dCdbPipeStock(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<S3dCdbPipeStock>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_CDB___3214EC275F1C56C7");
            entity.ToTable("S3D_CDB_PipeStock");
            entity.HasIndex(e => e.IndustryCommodityCode, "UQ__S3D_CDB___782B02A18C6083DE").IsUnique();
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CommodityType).HasMaxLength(100);
            entity.Property(e => e.Density).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.GeometricIndustryStandard).HasMaxLength(255);
            entity.Property(e => e.IndustryCommodityCode).HasMaxLength(100);
            entity.Property(e => e.MaterialGrade).HasMaxLength(100);
            entity.Property(e => e.MaximumPipeLength).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.MinimumPipeLength).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.PurchaseLength).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.WeightPerUnitLength).HasColumnType("decimal(10, 3)");
        });
    }

    private static void ConfigureS3dRulePmcData(ModelBuilder modelBuilder)
    {
        var jsonOptions = new JsonSerializerOptions();
        var listPmcStandardInfoComparer = new ValueComparer<List<PmcStandardInfo>>(
            (c1, c2) => JsonSerializer.Serialize(c1, jsonOptions) == JsonSerializer.Serialize(c2, jsonOptions),
            c => c == null ? 0 : JsonSerializer.Serialize(c, jsonOptions).GetHashCode(),
            c => JsonSerializer.Deserialize<List<PmcStandardInfo>>(JsonSerializer.Serialize(c, jsonOptions), jsonOptions)!
        );

        modelBuilder.Entity<S3dRulePmcData>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27203D42D1");
            entity.ToTable("S3D_Rule_PMCData");
            entity.HasIndex(e => new { e.Pmccode, e.ShipType, e.ShipNo }, "UQ_SPMC_PMCCode").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AccessoriesStandard)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, jsonOptions),
                    v => JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                    listPmcStandardInfoComparer)
                .HasMaxLength(500).HasColumnName("AccessoriesStandard");
            entity.Property(e => e.BlindFlangeStandard)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, jsonOptions),
                    v => JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                    listPmcStandardInfoComparer)
                .HasMaxLength(500).HasColumnName("BlindFlangeStandard");
            entity.Property(e => e.BoltStandard)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, jsonOptions),
                    v => JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                    listPmcStandardInfoComparer)
                .HasMaxLength(500).HasColumnName("BoltStandard");
            entity.Property(e => e.BossesStandard)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, jsonOptions),
                    v => JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                    listPmcStandardInfoComparer)
                .HasMaxLength(500).HasColumnName("BossesStandard");
            entity.Property(e => e.CapsStandard)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, jsonOptions),
                    v => JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                    listPmcStandardInfoComparer)
                .HasMaxLength(500).HasColumnName("CapsStandard");
            entity.Property(e => e.ElbowStandard)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, jsonOptions),
                    v => JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                    listPmcStandardInfoComparer)
                .HasMaxLength(500).HasColumnName("ElbowStandard");
            entity.Property(e => e.FlangeStandard)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, jsonOptions),
                    v => JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                    listPmcStandardInfoComparer)
                .HasMaxLength(500).HasColumnName("FlangeStandard");
            entity.Property(e => e.FlangeStandardName).HasMaxLength(255).HasColumnName("FlangeStandardName");
            entity.Property(e => e.GasketStandard)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, jsonOptions),
                    v => JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                    listPmcStandardInfoComparer)
                .HasMaxLength(500).HasColumnName("GasketStandard");
            entity.Property(e => e.MaterialsCategoryName).HasMaxLength(255);
            entity.Property(e => e.MaterialsGradeName).HasMaxLength(255);
            entity.Property(e => e.NutStandard)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, jsonOptions),
                    v => JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                    listPmcStandardInfoComparer)
                .HasMaxLength(500).HasColumnName("NutStandard");
            entity.Property(e => e.OverpassStandard)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, jsonOptions),
                    v => JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                    listPmcStandardInfoComparer)
                .HasMaxLength(500).HasColumnName("OverpassStandard");
            entity.Property(e => e.PipeStandard)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, jsonOptions),
                    v => JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                    listPmcStandardInfoComparer)
                .HasMaxLength(500).HasColumnName("PipeStandard");
            entity.Property(e => e.PipingClassName).HasMaxLength(255);
            entity.Property(e => e.PipingStandardName).HasMaxLength(255).HasColumnName("PipingStandardName");
            entity.Property(e => e.Pmccode).HasMaxLength(255).HasColumnName("PMCCode");
            entity.Property(e => e.PressureRatingName).HasMaxLength(255).HasColumnName("PressureRatingName");
            entity.Property(e => e.RedStandard)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, jsonOptions),
                    v => JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                    listPmcStandardInfoComparer)
                .HasMaxLength(500).HasColumnName("RedStandard");
            entity.Property(e => e.SaddlesStandard)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, jsonOptions),
                    v => JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                    listPmcStandardInfoComparer)
                .HasMaxLength(500).HasColumnName("SaddlesStandard");
            entity.Property(e => e.ScheduleThicknessName).HasMaxLength(255).HasColumnName("ScheduleThicknessName");
            entity.Property(e => e.ShipNo).HasMaxLength(255).HasColumnName("ShipNo");
            entity.Property(e => e.ShipType).HasMaxLength(255).HasColumnName("ShipType");
            entity.Property(e => e.SleeveStandard)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, jsonOptions),
                    v => JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                    listPmcStandardInfoComparer)
                .HasMaxLength(500).HasColumnName("SleeveStandard");
            entity.Property(e => e.Status).HasMaxLength(100).HasColumnName("Status");
            entity.Property(e => e.TeeStandard)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, jsonOptions),
                    v => JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                    listPmcStandardInfoComparer)
                .HasMaxLength(500).HasColumnName("TeeStandard");
            entity.Property(e => e.WasherStandard)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, jsonOptions),
                    v => JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                    listPmcStandardInfoComparer)
                .HasMaxLength(500).HasColumnName("WasherStandard");
            entity.Property(e => e.IsByRule).HasDefaultValue(true);
            entity.Property(e => e.VersionNum).HasDefaultValue(1);
        });
    }

    private static void ConfigureS3dRuleShortCodeMap(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<S3dRuleShortCodeMap>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("S3D_Rule_ShortCodeMap");
            entity.HasIndex(e => new { e.ComponentTypeId, e.ShortCode }, "UQ_ShortCodeMap_ID_Code").IsUnique();
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ComponentTypeId).HasColumnName("ComponentTypeID");
            entity.Property(e => e.ShortCode).HasMaxLength(50);
        });
    }

    private static void ConfigureS3dRuleShortCodeHierarchyRule(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<S3dRuleShortCodeHierarchyRule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DSP_ShortCodeHierarchyRule_PK");
            entity.ToTable("S3D_Rule_ShortCodeHierarchyRule");
            entity.HasIndex(e => new { e.ShortCodeHierarchyType, e.ShortCode }, "DSP_ShortCodeHierarchyRule_UNIQUE").IsUnique();
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ShortCode).HasMaxLength(50);
            entity.Property(e => e.ShortCodeHierarchyType).HasMaxLength(100);
        });
    }

    private static void ConfigureS3dRulePipingBendParameter(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<S3dRulePipingBendParameter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27DEA01B36");
            entity.ToTable("S3D_Rule_PipingBendParameter");
            entity.HasIndex(e => new { e.MaterialsCategoryCl, e.GeometricIndustryStandardCl, e.MaterialsGradeCl, e.NormalDiameter, e.UnitType }, "UQ_PipingBendParameter_MaterialsCategory_Standard_Grade_Diameter_Unit").IsUnique();
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BendRadiusMultiplier).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.MaterialsCategoryCl).HasColumnName("MaterialsCategory_CL");
            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.MaterialsGradeCl).HasColumnName("MaterialsGrade_CL");
            entity.Property(e => e.NormalDiameter);
            entity.Property(e => e.UnitType).HasMaxLength(50);
            entity.Property(e => e.WallThicknessFrom).HasMaxLength(50);
            entity.Property(e => e.WallThicknessTo).HasMaxLength(50);
            entity.Property(e => e.Status).HasDefaultValue(true);
        });
    }

    private static void ConfigureS3dRulePipingCompStandard(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<S3dRulePipingCompStandard>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("S3D_Rule_PipingCompStandard");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.ComponentTypeId).HasColumnName("ComponentTypeID");
            entity.Property(e => e.MaterialsCategoryCl).HasColumnName("MaterialsCategory_CL");
            entity.Property(e => e.JsonData).HasColumnName("JsonData");
        });
    }

    private static void ConfigureS3dCommonCodeList(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<S3dCommonCodeListHierarchy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DSP_CodeListHierarchy_PK");
            entity.ToTable("S3D_Common_CodeListHierarchy");
            entity.HasIndex(e => e.CodeListTableId, "DSP_CodeListHierarchy_UNIQUE").IsUnique();
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CodeListTableId).HasColumnName("CodeListTableID");
            entity.Property(e => e.ParentCodeListTableId).HasColumnName("ParentCodeListTableID");
        });

        modelBuilder.Entity<S3dCommonCodeListTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UD_CodeL__3214EC27484123D6");
            entity.ToTable("S3D_Common_CodeListTable");
            entity.HasIndex(e => e.Id, "UQ__UD_CodeL__3214EC26E7A97EC1").IsUnique();
            entity.HasIndex(e => e.CodeListTableName, "UQ__UD_CodeL__87753419918C0611").IsUnique();
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CodeListTableName).HasMaxLength(255);
            entity.Property(e => e.Major).HasMaxLength(10).HasDefaultValue("C");
        });

        modelBuilder.Entity<S3dCommonCodeListValue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UD_CodeL__0382013A28D8123E");
            entity.ToTable("S3D_Common_CodeListValue");
            entity.HasIndex(e => new { e.CodeListTableId, e.CodeListNumber }, "UQ__UD_CodeL__31A75F5EA32D472E").IsUnique();
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CodeListTableId).HasColumnName("CodeListTableID");
            entity.Property(e => e.IsUserDefine).HasDefaultValue(true);
            entity.Property(e => e.LongStringValue).HasMaxLength(255);
            entity.Property(e => e.ShortStringValue).HasMaxLength(255);
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.HasOne(d => d.CodeListTable).WithMany(p => p.S3dCommonCodeListValues)
                .HasForeignKey(d => d.CodeListTableId)
                .HasConstraintName("FK__UD_CodeLi__CodeL__3B75D760");
        });
    }

    private static void ConfigureS3dDictPipingComponentType(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<S3dDictPipingComponentType>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("S3D_Dict_PipingComponentType");
            entity.HasIndex(e => e.ComponentTypeName, "UQ_PipingComponentType_Name").IsUnique();
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ComponentTypeName).HasMaxLength(255);
            entity.Property(e => e.ComponentTypeDescription).HasMaxLength(255);
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.CreatedBy).HasMaxLength(100).HasDefaultValueSql("(suser_sname())");
            entity.Property(e => e.ModifiedBy).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ModifiedDate).HasDefaultValueSql("(getdate())");
        });
    }

    private static void ConfigureS3dRuleComponentTypeHierarchyRule(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<S3dRuleComponentTypeHierarchyRule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC2773507ACE");
            entity.ToTable("S3D_Rule_ComponentTypeHierarchyRule");
            entity.HasIndex(e => new { e.ComponentTypeId, e.PipingCommoditySubClassCl }, "UQ_PipingComponentType_ID_SubClass").IsUnique();
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ComponentTypeId).HasColumnName("ComponentTypeID");
            entity.Property(e => e.PipingCommoditySubClassCl).HasColumnName("PipingCommoditySubClass_CL");
            entity.Property(e => e.Status).HasDefaultValue(true);
        });
    }

    private static void ConfigureS3dWallThicknessInfo(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<S3dWallThicknessInfo>(entity =>
        {
            entity.HasNoKey().ToView("S3D_WallThickness_Info");
            entity.Property(e => e.GeometricIndustryStandard).HasMaxLength(255);
            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.NormalDiameter).HasColumnType("float");
            entity.Property(e => e.PipingOutsideDiameter).HasColumnType("float");
            entity.Property(e => e.ScheduleThickness).HasMaxLength(255);
            entity.Property(e => e.ScheduleThicknessCl).HasColumnName("ScheduleThickness_CL");
            entity.Property(e => e.UnitType).HasMaxLength(100);
            entity.Property(e => e.Version).HasMaxLength(100);
            entity.Property(e => e.WallThickness).HasColumnType("float");
        });
    }

    private static void ConfigurePipeSpecVersion(ModelBuilder modelBuilder)
    {
        var jsonOptions = new JsonSerializerOptions();
        var listPmcStandardInfoComparer = new ValueComparer<List<PmcStandardInfo>>(
            (c1, c2) => JsonSerializer.Serialize(c1, jsonOptions) == JsonSerializer.Serialize(c2, jsonOptions),
            c => c == null ? 0 : JsonSerializer.Serialize(c, jsonOptions).GetHashCode(),
            c => JsonSerializer.Deserialize<List<PmcStandardInfo>>(JsonSerializer.Serialize(c, jsonOptions), jsonOptions)!
        );

        modelBuilder.Entity<PipeSpecVersion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_PipeSpecVersion");
            entity.ToTable("S3D_Rule_PipeSpecVersion");
            entity.HasIndex(e => new { e.PmcCode, e.ShipType, e.ShipNo }, "IX_PipeSpecVersion_PmcCode_ShipType_ShipNo");
            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.PmcCode).HasMaxLength(255).HasColumnName("PmcCode");
            entity.Property(e => e.ShipType).HasMaxLength(255).HasColumnName("ShipType");
            entity.Property(e => e.ShipNo).HasMaxLength(255).HasColumnName("ShipNo");
            entity.Property(e => e.Version).HasColumnName("Version");
            entity.Property(e => e.PipingClassName).HasMaxLength(255).HasColumnName("PipingClassName");
            entity.Property(e => e.MaterialsCategoryName).HasMaxLength(255).HasColumnName("MaterialsCategoryName");
            entity.Property(e => e.PipingStandardName).HasMaxLength(255).HasColumnName("PipingStandardName");
            entity.Property(e => e.MaterialsGradeName).HasMaxLength(255).HasColumnName("MaterialsGradeName");
            entity.Property(e => e.FlangeStandardName).HasMaxLength(255).HasColumnName("FlangeStandardName");
            entity.Property(e => e.PressureRatingName).HasMaxLength(255).HasColumnName("PressureRatingName");
            entity.Property(e => e.ScheduleThicknessName).HasMaxLength(255).HasColumnName("ScheduleThicknessName");
            entity.Property(e => e.Status).HasMaxLength(100).HasColumnName("Status");
            entity.Property(e => e.CreatedAt).HasColumnName("CreatedAt");
            entity.Property(e => e.CreatedBy).HasMaxLength(255).HasColumnName("CreatedBy");
            entity.Property(e => e.Comment).HasMaxLength(500).HasColumnName("Comment");

            entity.Property(e => e.PipeStandard)
                .HasConversion(
                    v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                    v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                    listPmcStandardInfoComparer)
                .HasColumnName("PipeStandard");
            entity.Property(e => e.ElbowStandard)
                .HasConversion(
                    v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                    v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                    listPmcStandardInfoComparer)
                .HasColumnName("ElbowStandard");
            entity.Property(e => e.RedStandard)
                .HasConversion(
                    v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                    v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                    listPmcStandardInfoComparer)
                .HasColumnName("RedStandard");
            entity.Property(e => e.TeeStandard)
                .HasConversion(
                    v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                    v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                    listPmcStandardInfoComparer)
                .HasColumnName("TeeStandard");
            entity.Property(e => e.SleeveStandard)
                .HasConversion(
                    v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                    v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                    listPmcStandardInfoComparer)
                .HasColumnName("SleeveStandard");
            entity.Property(e => e.BossesStandard)
                .HasConversion(
                    v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                    v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                    listPmcStandardInfoComparer)
                .HasColumnName("BossesStandard");
            entity.Property(e => e.SaddlesStandard)
                .HasConversion(
                    v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                    v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                    listPmcStandardInfoComparer)
                .HasColumnName("SaddlesStandard");
            entity.Property(e => e.CapsStandard)
                .HasConversion(
                    v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                    v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                    listPmcStandardInfoComparer)
                .HasColumnName("CapsStandard");
            entity.Property(e => e.OverpassStandard)
                .HasConversion(
                    v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                    v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                    listPmcStandardInfoComparer)
                .HasColumnName("OverpassStandard");
            entity.Property(e => e.AccessoriesStandard)
                .HasConversion(
                    v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                    v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                    listPmcStandardInfoComparer)
                .HasColumnName("AccessoriesStandard");
            entity.Property(e => e.FlangeStandard)
                .HasConversion(
                    v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                    v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                    listPmcStandardInfoComparer)
                .HasColumnName("FlangeStandard");
            entity.Property(e => e.BlindFlangeStandard)
                .HasConversion(
                    v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                    v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                    listPmcStandardInfoComparer)
                .HasColumnName("BlindFlangeStandard");
            entity.Property(e => e.GasketStandard)
                .HasConversion(
                    v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                    v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                    listPmcStandardInfoComparer)
                .HasColumnName("GasketStandard");
            entity.Property(e => e.BoltStandard)
                .HasConversion(
                    v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                    v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                    listPmcStandardInfoComparer)
                .HasColumnName("BoltStandard");
            entity.Property(e => e.NutStandard)
                .HasConversion(
                    v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                    v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                    listPmcStandardInfoComparer)
                .HasColumnName("NutStandard");
            entity.Property(e => e.WasherStandard)
                .HasConversion(
                    v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                    v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                    listPmcStandardInfoComparer)
                .HasColumnName("WasherStandard");
        });
    }

    private static void ConfigureDesignRulesViews(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<S3dCodePipingBendParameter>(entity =>
        {
            entity.HasNoKey().ToView("S3D_Code_PipingBendParameter");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaterialsCategoryCl).HasColumnName("MaterialsCategory_CL");
            entity.Property(e => e.MaterialsCategory).HasMaxLength(255);
            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.GeometricIndustryStandard).HasMaxLength(255);
            entity.Property(e => e.MaterialsGradeCl).HasColumnName("MaterialsGrade_CL");
            entity.Property(e => e.MaterialsGrade).HasMaxLength(255);
            entity.Property(e => e.NormalDiameter);
            entity.Property(e => e.UnitType).HasMaxLength(50);
            entity.Property(e => e.WallThicknessFrom).HasMaxLength(50);
            entity.Property(e => e.WallThicknessTo).HasMaxLength(50);
            entity.Property(e => e.BendRadiusMultiplier).HasColumnType("decimal(10, 3)");
        });

        modelBuilder.Entity<S3dCodePlainPipingGenericData>(entity =>
        {
            entity.HasNoKey().ToView("S3D_Code_PlainPipingGenericData");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.NominalPipingDiameter).HasColumnType("float");
            entity.Property(e => e.NominalDiameterUnits).HasMaxLength(10);
            entity.Property(e => e.EndStandardCl).HasColumnName("EndStandard_CL");
            entity.Property(e => e.EndStandard).HasMaxLength(255);
            entity.Property(e => e.ScheduleThicknessCl).HasColumnName("ScheduleThickness_CL");
            entity.Property(e => e.ScheduleThickness).HasMaxLength(255);
            entity.Property(e => e.PipingOutsideDiameter).HasMaxLength(50);
            entity.Property(e => e.WallThickness).HasMaxLength(50);
        });

        modelBuilder.Entity<S3dCodeShortCodeMap>(entity =>
        {
            entity.HasNoKey().ToView("S3D_Code_ShortCodeMap");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ComponentTypeId).HasColumnName("ComponentTypeID");
            entity.Property(e => e.ComponentTypeName).HasMaxLength(255);
            entity.Property(e => e.ShortCode).HasMaxLength(50);
        });

        modelBuilder.Entity<S3dClMaterialsGrade>(entity =>
        {
            entity.HasNoKey().ToView("S3D_CL_MaterialsGrade");
            entity.Property(e => e.CodeListNumber);
            entity.Property(e => e.ShortStringValue).HasMaxLength(255);
            entity.Property(e => e.LongStringValue).HasMaxLength(255);
        });

        modelBuilder.Entity<S3dDictPipingBendData>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Dict__3214EC27EA0A1461");
            entity.ToTable("S3D_Dict_PipingBendData");
            entity.HasIndex(e => new { e.OutSideDiameter, e.OutSideDiameterUnit, e.MachineNum }, "UQ_PipingBend_OutSideDiameter_Unit_MachineNum").IsUnique();
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.HeaderClampLength);
            entity.Property(e => e.MaterialsCategoryCl).HasColumnName("MaterialsCategory_CL");
            entity.Property(e => e.MachineNum).HasDefaultValue(1);
            entity.Property(e => e.OutSideDiameter);
            entity.Property(e => e.OutSideDiameterUnit).HasMaxLength(100);
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.TailClampLength);
        });
    }

    private static void ConfigureDesignRulesPlainPipingGenericData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<S3dCommonPlainPipingGenericData>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DSP_PlainPipingGenericData_PK");
            entity.ToTable("S3D_Common_PlainPipingGenericData");
            entity.HasIndex(
                e => new
                {
                    e.NominalPipingDiameter,
                    e.NominalDiameterUnits,
                    e.EndStandardCl,
                    e.ScheduleThicknessCl,
                    e.PressureRatingCl
                },
                "DSP_PlainPipingGenericData_UNIQUE")
                .IsUnique();
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.NominalPipingDiameter).HasColumnType("float");
            entity.Property(e => e.NominalDiameterUnits).HasMaxLength(5);
            entity.Property(e => e.EndStandardCl).HasColumnName("EndStandard_CL");
            entity.Property(e => e.ScheduleThicknessCl).HasColumnName("ScheduleThickness_CL");
            entity.Property(e => e.PressureRatingCl).HasColumnName("PressureRating_CL");
            entity.Property(e => e.PipingOutsideDiameter).HasMaxLength(50);
            entity.Property(e => e.WallThickness).HasMaxLength(50);
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.CreatedBy).HasMaxLength(100).HasDefaultValueSql("(suser_sname())");
            entity.Property(e => e.ModifiedBy).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime2(3)");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime2(3)");
        });
    }

    private static void ConfigurePmcRuleConfigTables(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<S3dRuleAb2b3c2>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27605C7BC0");
            entity.ToTable("S3D_Rule_AB2B3C2");
            entity.HasIndex(e => new { e.PipingClassCl, e.GeometricIndustryStandardCl, e.MaterialsGradeCl, e.PressureRatingCl, e.RuleName }, "UQ_SPMC_AB2B3C2").IsUnique();
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.MaterialsGradeCl).HasColumnName("MaterialsGrade_CL");
            entity.Property(e => e.PipingClassCl).HasColumnName("PipingClass_CL");
            entity.Property(e => e.PressureRatingCl).HasColumnName("PressureRating_CL");
            entity.Property(e => e.RuleName).HasMaxLength(255);
            entity.Property(e => e.Status).HasDefaultValue(true);
        });

        modelBuilder.Entity<S3dRuleB1b2b3d>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC2775C8CBEA");
            entity.ToTable("S3D_Rule_B1B2B3D");
            entity.HasIndex(e => new { e.MaterialsCategoryCl, e.GeometricIndustryStandardCl, e.MaterialsGradeCl, e.ScheduleThicknessCl, e.RuleName }, "UQ_SPMC_B1B2B3D").IsUnique();
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.MaterialsCategoryCl).HasColumnName("MaterialsCategory_CL");
            entity.Property(e => e.MaterialsGradeCl).HasColumnName("MaterialsGrade_CL");
            entity.Property(e => e.RuleName).HasMaxLength(255);
            entity.Property(e => e.ScheduleThicknessCl).HasColumnName("ScheduleThickness_CL");
            entity.Property(e => e.Status).HasDefaultValue(true);
        });

        modelBuilder.Entity<S3dRuleC1c2>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27C1758066");
            entity.ToTable("S3D_Rule_C1C2");
            entity.HasIndex(e => new { e.GeometricIndustryStandardCl, e.PressureRatingCl, e.RuleName }, "UQ_SPMC_C1C2").IsUnique();
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.PressureRatingCl).HasColumnName("PressureRating_CL");
            entity.Property(e => e.RuleName).HasMaxLength(255);
        });
    }

    private static void ConfigurePmcRuleConfigViews(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<S3dCodeAb2b3c2>(entity =>
        {
            entity.HasNoKey().ToView("S3D_Code_AB2B3C2", "dbo");
            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaterialsGradeCl).HasColumnName("MaterialsGrade_CL");
            entity.Property(e => e.MaterialsGradeCode).HasMaxLength(100);
            entity.Property(e => e.PipingClassCl).HasColumnName("PipingClass_CL");
            entity.Property(e => e.PipingClassCode).HasMaxLength(100);
            entity.Property(e => e.PipingStandardCode).HasMaxLength(100);
            entity.Property(e => e.PressureRatingCl).HasColumnName("PressureRating_CL");
            entity.Property(e => e.PressureRatingCode).HasMaxLength(100);
            entity.Property(e => e.RuleName).HasMaxLength(255);
            entity.Property(e => e.Status);
        });

        modelBuilder.Entity<S3dCodeB1b2b3d>(entity =>
        {
            entity.HasNoKey().ToView("S3D_Code_B1B2B3D", "dbo");
            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaterialsCategoryCl).HasColumnName("MaterialsCategory_CL");
            entity.Property(e => e.MaterialsCategoryCode).HasMaxLength(100);
            entity.Property(e => e.MaterialsGradeCl).HasColumnName("MaterialsGrade_CL");
            entity.Property(e => e.MaterialsGradeCode).HasMaxLength(100);
            entity.Property(e => e.PipingStandardCode).HasMaxLength(100);
            entity.Property(e => e.RuleName).HasMaxLength(255);
            entity.Property(e => e.ScheduleThicknessCl).HasColumnName("ScheduleThickness_CL");
            entity.Property(e => e.ScheduleThicknessCode).HasMaxLength(100);
            entity.Property(e => e.Status);
        });

        modelBuilder.Entity<S3dCodeC1c2>(entity =>
        {
            entity.HasNoKey().ToView("S3D_Code_C1C2", "dbo");
            entity.Property(e => e.FlangeStandardCode).HasMaxLength(100);
            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PressureRatingCl).HasColumnName("PressureRating_CL");
            entity.Property(e => e.PressureRatingCode).HasMaxLength(100);
            entity.Property(e => e.RuleName).HasMaxLength(255);
            entity.Property(e => e.Status);
        });

        modelBuilder.Entity<S3dCodePipingClass>(entity =>
        {
            entity.HasNoKey().ToView("S3D_Code_PipingClass", "dbo");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PipingClassCode).HasMaxLength(100);
            entity.Property(e => e.ShortStringValue).HasMaxLength(255);
        });

        modelBuilder.Entity<S3dCodeFlangeStandPressureRating>(entity =>
        {
            entity.HasNoKey().ToView("S3D_Code_FlangeStandPressureRating", "dbo");
            entity.Property(e => e.FlangeStandDesc).HasMaxLength(255);
            entity.Property(e => e.FlangeStandardCode).HasMaxLength(100);
            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.PressureRatingCl).HasColumnName("PressureRating_CL");
            entity.Property(e => e.PressureRatingCode).HasMaxLength(100);
            entity.Property(e => e.PressureRatingDesc).HasMaxLength(255);
        });

        modelBuilder.Entity<S3dCodeMaterialsCategoryPipingStandard>(entity =>
        {
            entity.HasNoKey().ToView("S3D_Code_MaterialsCategoryPipingStandard", "dbo");
            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.MaterialsCategoryCl).HasColumnName("MaterialsCategory_CL");
            entity.Property(e => e.MaterialsCategoryCode).HasMaxLength(100);
            entity.Property(e => e.MaterialsCategoryDesc).HasMaxLength(255);
            entity.Property(e => e.PipeStandDesc).HasMaxLength(255);
            entity.Property(e => e.PipingStandardCode).HasMaxLength(100);
        });

        modelBuilder.Entity<S3dCodeMaterialsCategoryScheduleThickness>(entity =>
        {
            entity.HasNoKey().ToView("S3D_Code_MaterialsCategoryScheduleThickness", "dbo");
            entity.Property(e => e.MaterialsCategoryCl).HasColumnName("MaterialsCategory_CL");
            entity.Property(e => e.MaterialsCategoryCode).HasMaxLength(100);
            entity.Property(e => e.MaterialsCategoryDesc).HasMaxLength(255);
            entity.Property(e => e.ScheduleThicknessCl).HasColumnName("ScheduleThickness_CL");
            entity.Property(e => e.ScheduleThicknessCode).HasMaxLength(100);
            entity.Property(e => e.ScheduleThicknessDesc).HasMaxLength(255);
        });

        modelBuilder.Entity<S3dCodePipingStandardMaterialsGrade>(entity =>
        {
            entity.HasNoKey().ToView("S3D_Code_PipingStandardMaterialsGrade", "dbo");
            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.MaterialsGradeCl).HasColumnName("MaterialsGrade_CL");
            entity.Property(e => e.MaterialsGradeCode).HasMaxLength(100);
            entity.Property(e => e.MaterialsGradeDesc).HasMaxLength(255);
            entity.Property(e => e.PipeStandDesc).HasMaxLength(255);
            entity.Property(e => e.PipingStandardCode).HasMaxLength(100);
        });

        modelBuilder.Entity<S3dCodePipingStandardPressureRating>(entity =>
        {
            entity.HasNoKey().ToView("S3D_Code_PipingStandardPressureRating", "dbo");
            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.PipingStandardCode).HasMaxLength(100);
            entity.Property(e => e.PipingStandardDesc).HasMaxLength(255);
            entity.Property(e => e.PressureRatingCl).HasColumnName("PressureRating_CL");
            entity.Property(e => e.PressureRatingCode).HasMaxLength(100);
            entity.Property(e => e.PressureRatingDesc).HasMaxLength(255);
        });
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
