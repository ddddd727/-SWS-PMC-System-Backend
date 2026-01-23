using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PMCSystem_Backend.Entities.TempEntities;

public partial class PmcContext : DbContext
{
    public PmcContext()
    {
    }

    public PmcContext(DbContextOptions<PmcContext> options)
        : base(options)
    {
    }

    public virtual DbSet<S3dRulePipingStandardSchMap> S3dRulePipingStandardSchMaps { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=PMC;Trusted_Connection=True;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<S3dRulePipingStandardSchMap>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27D120D525");

            entity.ToTable("S3D_Rule_PipingStandardSchMap");

            entity.HasIndex(e => new { e.GeometricIndustryStandardCl, e.ScheduleThicknessCl, e.NormalDiameter, e.WallThickness, e.Version }, "UQ_PipingStandardSchMap_ID_Sch_NPD_Thick_Version").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.NormalDiameter).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.PipingOutsideDiameter).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.ScheduleThicknessCl).HasColumnName("ScheduleThickness_CL");
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.UnitType).HasMaxLength(100);
            entity.Property(e => e.Version).HasMaxLength(100);
            entity.Property(e => e.WallThickness).HasColumnType("decimal(10, 3)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
