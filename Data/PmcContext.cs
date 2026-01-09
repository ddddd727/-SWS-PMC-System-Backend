using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PMCSystem_Backend.Entities;
using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Data;

public partial class PmcContext : DbContext
{
    private readonly IConfiguration _configuration;

    public PmcContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public PmcContext(DbContextOptions<PmcContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<S3dCdbPipeComponent> S3dCdbPipeComponents { get; set; }

    public virtual DbSet<S3dCdbPipeStock> S3dCdbPipeStocks { get; set; }

    public virtual DbSet<S3dCommonPlainPipingGenericData> S3dCommonPlainPipingGenericData { get; set; }

    public virtual DbSet<S3dRulePmcData> S3dRulePmcdata { get; set; }

    public virtual DbSet<S3dRuleShortCodeMap> S3dRuleShortCodeMaps { get; set; }

    public virtual DbSet<S3dRulePipingCompStandard> S3dRulePipingCompStandards { get; set; }

    public virtual DbSet<S3dCommonCodeListHierarchy> S3dCommonCodeListHierarchies { get; set; }

    public virtual DbSet<S3dCommonCodeListTable> S3dCommonCodeListTables { get; set; }

    public virtual DbSet<S3dCommonCodeListValue> S3dCommonCodeListValues { get; set; }

    public virtual DbSet<S3dDictPipingComponentType> S3dDictPipingComponentTypes { get; set; }

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

        modelBuilder.Entity<S3dCommonPlainPipingGenericData>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DSP_PlainPipingGenericData_PK");

            entity.ToTable("S3D_Common_PlainPipingGenericData");

            entity.HasIndex(e => new { e.NominalPipingDiameter, e.NominalDiameterUnits, e.EndStandardCl, e.ScheduleCl, e.PressureRatingCl }, "DSP_PlainPipingGenericData_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EndStandardCl).HasColumnName("EndStandard_CL");
            entity.Property(e => e.NominalDiameterUnits).HasMaxLength(5);
            entity.Property(e => e.NominalPipingDiameter).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.PipingOutsideDiameter).HasColumnType("decimal(5, 3)");
            entity.Property(e => e.PressureRatingCl).HasColumnName("PressureRating_CL");
            entity.Property(e => e.ScheduleCl).HasColumnName("Schedule_CL");
            entity.Property(e => e.WallThickness).HasColumnType("decimal(5, 3)");
        });

        modelBuilder.Entity<S3dRulePmcData>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27203D42D1");

            entity.ToTable("S3D_Rule_PMCData");

            entity.HasIndex(e => new { e.Pmccode, e.ShipType, e.ShipNo }, "UQ_SPMC_PMCCode").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AccessoriesStandard).HasMaxLength(500);
            entity.Property(e => e.BlindFlangeStandard).HasMaxLength(500);
            entity.Property(e => e.BoltStandard).HasMaxLength(500);
            entity.Property(e => e.BossesStandard).HasMaxLength(500);
            entity.Property(e => e.CapsStandard).HasMaxLength(500);
            entity.Property(e => e.ElbowStandard).HasMaxLength(500);
            entity.Property(e => e.FlangeStandard).HasMaxLength(500);
            entity.Property(e => e.FlangeStandardName).HasMaxLength(255);
            entity.Property(e => e.GasketStandard).HasMaxLength(500);
            entity.Property(e => e.MaterialsCategoryName).HasMaxLength(255);
            entity.Property(e => e.MaterialsGradeName).HasMaxLength(255);
            entity.Property(e => e.NutStandard).HasMaxLength(500);
            entity.Property(e => e.OverpassStandard).HasMaxLength(500);
            entity.Property(e => e.PipeStandard).HasMaxLength(500);
            entity.Property(e => e.PipingClassName).HasMaxLength(255);
            entity.Property(e => e.PipingStandardName).HasMaxLength(255);
            entity.Property(e => e.Pmccode)
                .HasMaxLength(255)
                .HasColumnName("PMCCode");
            entity.Property(e => e.PressureRatingName).HasMaxLength(255);
            entity.Property(e => e.RedStandard).HasMaxLength(500);
            entity.Property(e => e.SaddlesStandard).HasMaxLength(500);
            entity.Property(e => e.ScheduleThicknessName).HasMaxLength(255);
            entity.Property(e => e.ShipNo).HasMaxLength(255);
            entity.Property(e => e.ShipType).HasMaxLength(255);
            entity.Property(e => e.SleeveStandard).HasMaxLength(500);
            entity.Property(e => e.Status).HasMaxLength(100);
            entity.Property(e => e.TeeStandard).HasMaxLength(500);
            entity.Property(e => e.WasherStandard).HasMaxLength(500);
        });

        modelBuilder.Entity<S3dRuleShortCodeMap>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC2738D19ED1");

            entity.ToTable("S3D_Rule_ShortCodeMap");

            entity.HasIndex(e => new { e.ComponentTypeId, e.ShortCode }, "UQ_ShortCodeMap_ID_Code").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ComponentTypeId).HasColumnName("ComponentTypeID");
            entity.Property(e => e.ShortCode).HasMaxLength(50);
        });

        modelBuilder.Entity<S3dRulePipingCompStandard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27723621A9");

            entity.ToTable("S3D_Rule_PipingCompStandard");

            entity.HasIndex(e => new { e.GeometricIndustryStandardCl, e.ComponentTypeId, e.MaterialsCategoryCl }, "UQ_PipingCompStandard_GeometricIndustryStandard_ComponentType_MaterialsCategory").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ComponentTypeId).HasColumnName("ComponentTypeID");
            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.MaterialsCategoryCl).HasColumnName("MaterialsCategory_CL");
            entity.Property(e => e.Status).HasDefaultValue(true);
        });


        modelBuilder.Entity<S3dCommonCodeListHierarchy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DSP_CodeListHierarchy_PK");

            entity.ToTable("S3D_Common_CodeListHierarchy");

            entity.HasIndex(e => e.CodeListTableId, "DSP_CodeListHierarchy_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CodeListTableId).HasColumnName("CodeListTableID");
            entity.Property(e => e.ParentCodeListTableId).HasColumnName("ParentCodeListTableID");

            entity.HasOne(d => d.CodeListTable).WithOne(p => p.S3dCommonCodeListHierarchy)
                .HasForeignKey<S3dCommonCodeListHierarchy>(d => d.CodeListTableId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DSP_CodeListHierarchy_DSP_CodeListTable_FK");
        });

        modelBuilder.Entity<S3dCommonCodeListTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UD_CodeL__3214EC27484123D6");

            entity.ToTable("S3D_Common_CodeListTable");

            entity.HasIndex(e => e.Id, "UQ__UD_CodeL__3214EC26E7A97EC1").IsUnique();

            entity.HasIndex(e => e.CodeListTableName, "UQ__UD_CodeL__87753419918C0611").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CodeListTableName).HasMaxLength(255);
            entity.Property(e => e.Major)
                .HasMaxLength(10)
                .HasDefaultValue("C");
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

        modelBuilder.Entity<S3dDictPipingComponentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Dict__3214EC2756D3AB25");

            entity.ToTable("S3D_Dict_PipingComponentType");

            entity.HasIndex(e => e.ComponentTypeName, "UQ_PipingComponentType_Name").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ComponentTypeDescription).HasMaxLength(255);
            entity.Property(e => e.ComponentTypeName).HasMaxLength(255);
            entity.Property(e => e.Status).HasDefaultValue(true);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
