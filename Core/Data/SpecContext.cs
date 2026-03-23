using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Modules.PMCRuleConfig.Entities;

namespace PMCSystem_Backend.Core.Data;

public partial class SpecContext : DbContext
{
    public SpecContext()
    {
    }

    public SpecContext(DbContextOptions<SpecContext> options)
        : base(options)
    {
    }

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
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

        modelBuilder.Entity<S3dCodeAb2b3c2>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("S3D_Code_AB2B3C2", "dbo");

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

        modelBuilder.Entity<S3dCodeB1b2b3d>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("S3D_Code_B1B2B3D", "dbo");

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

        modelBuilder.Entity<S3dCodeC1c2>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("S3D_Code_C1C2", "dbo");

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
            entity
                .HasNoKey()
                .ToView("S3D_Code_PipingClass", "dbo");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PipingClassCode).HasMaxLength(100);
            entity.Property(e => e.ShortStringValue).HasMaxLength(255);
        });

        modelBuilder.Entity<S3dCodeFlangeStandPressureRating>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("S3D_Code_FlangeStandPressureRating", "dbo");

            entity.Property(e => e.FlangeStandDesc).HasMaxLength(255);
            entity.Property(e => e.FlangeStandardCode).HasMaxLength(100);
            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.PressureRatingCl).HasColumnName("PressureRating_CL");
            entity.Property(e => e.PressureRatingCode).HasMaxLength(100);
            entity.Property(e => e.PressureRatingDesc).HasMaxLength(255);
        });

        modelBuilder.Entity<S3dCodeMaterialsCategoryPipingStandard>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("S3D_Code_MaterialsCategoryPipingStandard", "dbo");

            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.MaterialsCategoryCl).HasColumnName("MaterialsCategory_CL");
            entity.Property(e => e.MaterialsCategoryCode).HasMaxLength(100);
            entity.Property(e => e.MaterialsCategoryDesc).HasMaxLength(255);
            entity.Property(e => e.PipeStandDesc).HasMaxLength(255);
            entity.Property(e => e.PipingStandardCode).HasMaxLength(100);
        });

        modelBuilder.Entity<S3dCodeMaterialsCategoryScheduleThickness>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("S3D_Code_MaterialsCategoryScheduleThickness", "dbo");

            entity.Property(e => e.MaterialsCategoryCl).HasColumnName("MaterialsCategory_CL");
            entity.Property(e => e.ScheduleThicknessCl).HasColumnName("ScheduleThickness_CL");
            entity.Property(e => e.ScheduleThicknessCode).HasMaxLength(100);
            entity.Property(e => e.ScheduleThicknessDesc).HasMaxLength(255);
        });

        modelBuilder.Entity<S3dCodePipingStandardMaterialsGrade>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("S3D_Code_PipingStandardMaterialsGrade", "dbo");

            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.MaterialsGradeCl).HasColumnName("MaterialsGrade_CL");
            entity.Property(e => e.MaterialsGradeCode).HasMaxLength(100);
            entity.Property(e => e.MaterialsGradeDesc).HasMaxLength(255);
            entity.Property(e => e.PipeStandDesc).HasMaxLength(255);
            entity.Property(e => e.PipingStandardCode).HasMaxLength(100);
        });

        modelBuilder.Entity<S3dCodePipingStandardPressureRating>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("S3D_Code_PipingStandardPressureRating", "dbo");

            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.PipingStandardCode).HasMaxLength(100);
            entity.Property(e => e.PipingStandardDesc).HasMaxLength(255);
            entity.Property(e => e.PressureRatingCl).HasColumnName("PressureRating_CL");
            entity.Property(e => e.PressureRatingCode).HasMaxLength(100);
            entity.Property(e => e.PressureRatingDesc).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
