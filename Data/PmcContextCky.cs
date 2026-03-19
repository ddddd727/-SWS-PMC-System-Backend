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

    public virtual DbSet<S3dCodePlainPipingGenericData> S3dCodePlainPipingGenericData { get; set; }

    public virtual DbSet<S3dCodeShortCodeMap> S3dCodeShortCodeMaps { get; set; }

        public virtual DbSet<PMCSystem_Backend.Entities.S3dRuleShortCodeMap> S3dRuleShortCodeMaps { get; set; }

        public virtual DbSet<S3dCommonCodeListTable> S3dCommonCodeListTables { get; set; }

    public virtual DbSet<S3dCommonCodeListValue> S3dCommonCodeListValues { get; set; }

    public virtual DbSet<S3dClMaterialsGrade> S3dClMaterialsGrades { get; set; }

    public virtual DbSet<S3dDictPipingBendData> S3dDictPipingBendData { get; set; }

    public virtual DbSet<S3dRulePipingBendParameter> S3dRulePipingBendParameters { get; set; }

    public virtual DbSet<S3dDictPipingComponentType> S3dDictPipingComponentTypes { get; set; }

    public virtual DbSet<S3dRuleShortCodeHierarchyRule> S3dRuleShortCodeHierarchyRules { get; set; }

    public virtual DbSet<PMCSystem_Backend.Entities.DesignRule.S3dCommonPlainPipingGenericData> S3dCommonPlainPipingGenericData { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<S3dCodePipingBendParameter>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("S3D_Code_PipingBendParameter");

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
            entity
                .HasNoKey()
                .ToView("S3D_Code_PlainPipingGenericData");

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

        modelBuilder.Entity<S3dClMaterialsGrade>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("S3D_CL_MaterialsGrade");

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

        modelBuilder.Entity<S3dRuleShortCodeHierarchyRule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DSP_ShortCodeHierarchyRule_PK");

            entity.ToTable("S3D_Rule_ShortCodeHierarchyRule");

            entity.HasIndex(e => new { e.ShortCodeHierarchyType, e.ShortCode }, "DSP_ShortCodeHierarchyRule_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ShortCode).HasMaxLength(50);
            entity.Property(e => e.ShortCodeHierarchyType).HasMaxLength(100);
        });

        modelBuilder.Entity<S3dCodeShortCodeMap>(entity =>
        {
            entity.HasNoKey().ToView("S3D_Code_ShortCodeMap");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ComponentTypeId).HasColumnName("ComponentTypeID");
            entity.Property(e => e.ComponentTypeName).HasMaxLength(255);
            entity.Property(e => e.ShortCode).HasMaxLength(50);
        });

        modelBuilder.Entity<PMCSystem_Backend.Entities.S3dRuleShortCodeMap>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("S3D_Rule_ShortCodeMap");

            entity.HasIndex(e => new { e.ComponentTypeId, e.ShortCode }, "UQ_ShortCodeMap_ID_Code").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ComponentTypeId).HasColumnName("ComponentTypeID");
            entity.Property(e => e.ShortCode).HasMaxLength(50);
        });

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

        modelBuilder.Entity<PMCSystem_Backend.Entities.DesignRule.S3dCommonPlainPipingGenericData>(entity =>
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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
