using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Entities;

namespace PMCSystem_Backend.Data;

public partial class PmcContextLr : DbContext
{
    public PmcContextLr()
    {
    }

    public PmcContextLr(DbContextOptions<PmcContextLr> options)
        : base(options)
    {
    }

    public virtual DbSet<S3dCodeAb2b3c2> S3dCodeAb2b3c2s { get; set; }

    public virtual DbSet<S3dCodeB1b2b3d> S3dCodeB1b2b3ds { get; set; }

    public virtual DbSet<S3dCodeC1c2> S3dCodeC1c2s { get; set; }

    public virtual DbSet<S3dCodeFlangeStandPressureRating> S3dCodeFlangeStandPressureRatings { get; set; }

    public virtual DbSet<S3dCodeMaterialsCategoryPipingStandard> S3dCodeMaterialsCategoryPipingStandards { get; set; }

    public virtual DbSet<S3dCodeMaterialsCategoryScheduleThickness> S3dCodeMaterialsCategoryScheduleThicknesses { get; set; }

    public virtual DbSet<S3dCodePipingClass> S3dCodePipingClasses { get; set; }

    public virtual DbSet<S3dCodePipingStandardMaterialsGrade> S3dCodePipingStandardMaterialsGrades { get; set; }

    public virtual DbSet<S3dRulePmcdatum> S3dRulePmcdata { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<S3dCodeAb2b3c2>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("S3D_Code_AB2B3C2");

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
        });

        modelBuilder.Entity<S3dCodeB1b2b3d>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("S3D_Code_B1B2B3D");

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
        });

        modelBuilder.Entity<S3dCodeC1c2>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("S3D_Code_C1C2");

            entity.Property(e => e.FlangeStandardCode).HasMaxLength(100);
            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PressureRatingCl).HasColumnName("PressureRating_CL");
            entity.Property(e => e.PressureRatingCode).HasMaxLength(100);
            entity.Property(e => e.RuleName).HasMaxLength(255);
        });

        modelBuilder.Entity<S3dCodeFlangeStandPressureRating>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("S3D_Code_FlangeStandPressureRating");

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
                .ToView("S3D_Code_MaterialsCategoryPipingStandard");

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
                .ToView("S3D_Code_MaterialsCategoryScheduleThickness");

            entity.Property(e => e.MaterialsCategoryCl).HasColumnName("MaterialsCategory_CL");
            entity.Property(e => e.MaterialsCategoryCode).HasMaxLength(100);
            entity.Property(e => e.MaterialsCategoryDesc).HasMaxLength(255);
            entity.Property(e => e.ScheduleThicknessCl).HasColumnName("ScheduleThickness_CL");
            entity.Property(e => e.ScheduleThicknessCode).HasMaxLength(100);
            entity.Property(e => e.ScheduleThicknessDesc).HasMaxLength(255);
        });

        modelBuilder.Entity<S3dCodePipingClass>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("S3D_Code_PipingClass");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PipingClassCode).HasMaxLength(100);
            entity.Property(e => e.ShortStringValue).HasMaxLength(255);
        });

        modelBuilder.Entity<S3dCodePipingStandardMaterialsGrade>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("S3D_Code_PipingStandardMaterialsGrade");

            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.MaterialsGradeCl).HasColumnName("MaterialsGrade_CL");
            entity.Property(e => e.MaterialsGradeCode).HasMaxLength(100);
            entity.Property(e => e.MaterialsGradeDesc).HasMaxLength(255);
            entity.Property(e => e.PipeStandDesc).HasMaxLength(255);
            entity.Property(e => e.PipingStandardCode).HasMaxLength(100);
        });

        modelBuilder.Entity<S3dRulePmcdatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27056CD15E");

            entity.ToTable("S3D_Rule_PMCData");

            entity.HasIndex(e => new { e.Pmccode, e.ShipType, e.ShipNo }, "UQ_SPMC_PMCCode").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FlangeStandardName).HasMaxLength(255);
            entity.Property(e => e.MaterialsCategoryName).HasMaxLength(255);
            entity.Property(e => e.MaterialsGradeName).HasMaxLength(255);
            entity.Property(e => e.PipingClassName).HasMaxLength(255);
            entity.Property(e => e.PipingStandardName).HasMaxLength(255);
            entity.Property(e => e.Pmccode)
                .HasMaxLength(255)
                .HasColumnName("PMCCode");
            entity.Property(e => e.PressureRatingName).HasMaxLength(255);
            entity.Property(e => e.ScheduleThicknessName).HasMaxLength(255);
            entity.Property(e => e.ShipNo).HasMaxLength(255);
            entity.Property(e => e.ShipType).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
