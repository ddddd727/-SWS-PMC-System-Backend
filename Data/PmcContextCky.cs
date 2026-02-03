using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Entities;
using PMCSystem_Backend.Entities.PipeSpecConfig;

namespace PMCSystem_Backend.Data;

public partial class PmcContextCky : DbContext
{
    public PmcContextCky()
    {
    }

    public PmcContextCky(DbContextOptions<PmcContextCky> options)
        : base(options)
    {
    }

    public virtual DbSet<S3dCodePipingBendParameter> S3dCodePipingBendParameters { get; set; }

    public virtual DbSet<S3dCodeWallThickness> S3dCodeWallThicknesses { get; set; }

    public virtual DbSet<S3dCommonCodeListTable> S3dCommonCodeListTables { get; set; }

    public virtual DbSet<S3dCommonCodeListValue> S3dCommonCodeListValues { get; set; }

    public virtual DbSet<S3dDictPipingBendData> S3dDictPipingBendData { get; set; }

    public virtual DbSet<S3dDictWallThickness> S3dDictWallThicknesses { get; set; }

    public virtual DbSet<S3dRulePipingBendParameter> S3dRulePipingBendParameters { get; set; }

    public virtual DbSet<S3dRuleShortCodeHierarchyRule> S3dRuleShortCodeHierarchyRules { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<S3dCodePipingBendParameter>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("S3D_Code_PipingBendParameter");

            entity.Property(e => e.BendRadiusMultiplier).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaterialsCategory).HasMaxLength(255);
            entity.Property(e => e.MaterialsCategoryCl).HasColumnName("MaterialsCategory_CL");
            entity.Property(e => e.NormalDiameter).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.ScheduleThickness).HasMaxLength(255);
            entity.Property(e => e.ScheduleThicknessCl).HasColumnName("ScheduleThickness_CL");
            entity.Property(e => e.UnitType).HasMaxLength(100);
        });

        modelBuilder.Entity<S3dCodeWallThickness>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("S3D_Code_WallThickness");

            entity.Property(e => e.EndStandard).HasMaxLength(255);
            entity.Property(e => e.EndStandardCl).HasColumnName("EndStandard_CL");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Ndpunit)
                .HasMaxLength(100)
                .HasColumnName("NDPUnit");
            entity.Property(e => e.Npd)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("NPD");
            entity.Property(e => e.PipingOutsideDiameter).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.ScheduleThickness).HasMaxLength(255);
            entity.Property(e => e.ScheduleThicknessCl).HasColumnName("ScheduleThickness_CL");
            entity.Property(e => e.WallThickness).HasColumnType("decimal(10, 3)");
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

        modelBuilder.Entity<S3dDictWallThickness>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Dict__3214EC27B13DA1CB");

            entity.ToTable("S3D_Dict_WallThickness");

            entity.HasIndex(e => new { e.Npd, e.Ndpunit, e.ScheduleThicknessCl, e.EndStandardCl }, "UQ_WallThickness_NPD_Unit_Sche_Standard").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EndStandardCl).HasColumnName("EndStandard_CL");
            entity.Property(e => e.Ndpunit)
                .HasMaxLength(100)
                .HasColumnName("NDPUnit");
            entity.Property(e => e.Npd)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("NPD");
            entity.Property(e => e.PipingOutsideDiameter).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.ScheduleThicknessCl).HasColumnName("ScheduleThickness_CL");
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.WallThickness).HasColumnType("decimal(10, 3)");
        });

        modelBuilder.Entity<S3dRulePipingBendParameter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27DEA01B36");

            entity.ToTable("S3D_Rule_PipingBendParameter");

            entity.HasIndex(e => new { e.MaterialsCategoryCl, e.NormalDiameter, e.UnitType, e.ScheduleThicknessCl }, "UQ_PipingBendParameter_MaterialsCategory_Diameter_Unit_ScheduleThickness").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BendRadiusMultiplier).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.MaterialsCategoryCl).HasColumnName("MaterialsCategory_CL");
            entity.Property(e => e.NormalDiameter).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.ScheduleThicknessCl).HasColumnName("ScheduleThickness_CL");
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.UnitType).HasMaxLength(100);
        });

        modelBuilder.Entity<S3dRuleShortCodeHierarchyRule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DSP_ShortCodeHierarchyRule_PK");

            entity.ToTable("S3D_Rule_ShortCodeHierarchyRule");

            entity.HasIndex(e => new { e.ShortCodeHierarchyType, e.ShortCode }, "DSP_ShortCodeHierarchyRule_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ShortCode).HasMaxLength(50);
            entity.Property(e => e.ShortCodeHierarchyType).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
