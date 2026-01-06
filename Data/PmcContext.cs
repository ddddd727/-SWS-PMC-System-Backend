using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Entities;

namespace PMCSystem_Backend.Data;

public partial class PmcContext : DbContext
{
    public PmcContext()
    {
    }

    public PmcContext(DbContextOptions<PmcContext> options)
        : base(options)
    {
    }

    public virtual DbSet<S3dCdbPipeComponent> S3dCdbPipeComponents { get; set; }

    public virtual DbSet<S3dCdbPipeStock> S3dCdbPipeStocks { get; set; }

    public virtual DbSet<S3dCommonPlainPipingGenericData> S3dCommonPlainPipingGenericData { get; set; }

    public virtual DbSet<S3dRulePmcData> S3dRulePmcdata { get; set; }

    public virtual DbSet<S3dRuleShortCodeMap> S3dRuleShortCodeMaps { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=PMC;Trusted_Connection=True;TrustServerCertificate=true");

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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
