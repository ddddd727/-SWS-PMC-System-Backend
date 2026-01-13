using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Entities;

namespace PMCSystem_Backend.Data;

public partial class PmcTestContext : DbContext
{
    public PmcTestContext(DbContextOptions<PmcTestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<S3dCdbBoltPart> S3dCdbBoltParts { get; set; }

    public virtual DbSet<S3dCdbGasketPart> S3dCdbGasketParts { get; set; }

    public virtual DbSet<S3dCdbGeometryDatum> S3dCdbGeometryData { get; set; }

    public virtual DbSet<S3dCdbNutPart> S3dCdbNutParts { get; set; }

    public virtual DbSet<S3dCdbPipeComponent> S3dCdbPipeComponents { get; set; }

    public virtual DbSet<S3dCdbPipeStock> S3dCdbPipeStocks { get; set; }

    public virtual DbSet<S3dCdbPortDatum> S3dCdbPortData { get; set; }

    public virtual DbSet<S3dCdbWasherPart> S3dCdbWasherParts { get; set; }

    public virtual DbSet<S3dClGeometricIndustryPractice> S3dClGeometricIndustryPractices { get; set; }

    public virtual DbSet<S3dClGeometricIndustryStandard> S3dClGeometricIndustryStandards { get; set; }

    public virtual DbSet<S3dClMaterialsCategory> S3dClMaterialsCategories { get; set; }

    public virtual DbSet<S3dClMaterialsGrade> S3dClMaterialsGrades { get; set; }

    public virtual DbSet<S3dClPipingClass> S3dClPipingClasses { get; set; }

    public virtual DbSet<S3dClPressureRating> S3dClPressureRatings { get; set; }

    public virtual DbSet<S3dClRatingPractice> S3dClRatingPractices { get; set; }

    public virtual DbSet<S3dClScheduleThickness> S3dClScheduleThicknesses { get; set; }

    public virtual DbSet<S3dCommonAttribute> S3dCommonAttributes { get; set; }

    public virtual DbSet<S3dCommonCodeListHierarchy> S3dCommonCodeListHierarchies { get; set; }

    public virtual DbSet<S3dCommonCodeListTable> S3dCommonCodeListTables { get; set; }

    public virtual DbSet<S3dCommonCodeListValue> S3dCommonCodeListValues { get; set; }

    public virtual DbSet<S3dCommonCustomInterface> S3dCommonCustomInterfaces { get; set; }

    public virtual DbSet<S3dCommonPipingGenericDataBolted> S3dCommonPipingGenericDataBolteds { get; set; }

    public virtual DbSet<S3dCommonPipingGenericDataFemale> S3dCommonPipingGenericDataFemales { get; set; }

    public virtual DbSet<S3dCommonPlainPipingGenericDatum> S3dCommonPlainPipingGenericData { get; set; }

    public virtual DbSet<S3dDictFlangeStandard> S3dDictFlangeStandards { get; set; }

    public virtual DbSet<S3dDictGeometricIndustryPractice> S3dDictGeometricIndustryPractices { get; set; }

    public virtual DbSet<S3dDictGeometricIndustryStandard> S3dDictGeometricIndustryStandards { get; set; }

    public virtual DbSet<S3dDictMaterialsCategory> S3dDictMaterialsCategories { get; set; }

    public virtual DbSet<S3dDictMaterialsGrade> S3dDictMaterialsGrades { get; set; }

    public virtual DbSet<S3dDictPipingBend> S3dDictPipingBends { get; set; }

    public virtual DbSet<S3dDictPipingBendDatum> S3dDictPipingBendData { get; set; }

    public virtual DbSet<S3dDictPipingClass> S3dDictPipingClasses { get; set; }

    public virtual DbSet<S3dDictPipingComponentType> S3dDictPipingComponentTypes { get; set; }

    public virtual DbSet<S3dDictPipingStandard> S3dDictPipingStandards { get; set; }

    public virtual DbSet<S3dDictPressureRating> S3dDictPressureRatings { get; set; }

    public virtual DbSet<S3dDictScheduleSeries> S3dDictScheduleSeries { get; set; }

    public virtual DbSet<S3dDictScheduleThickness> S3dDictScheduleThicknesses { get; set; }

    public virtual DbSet<S3dDictWallThickness> S3dDictWallThicknesses { get; set; }

    public virtual DbSet<S3dPropertyInterfaceConfig> S3dPropertyInterfaceConfigs { get; set; }

    public virtual DbSet<S3dPropertyObjectType> S3dPropertyObjectTypes { get; set; }

    public virtual DbSet<S3dPropertyPropertyDefinition> S3dPropertyPropertyDefinitions { get; set; }

    public virtual DbSet<S3dPropertyVersionHistory> S3dPropertyVersionHistories { get; set; }

    public virtual DbSet<S3dRuleAb2b3c2> S3dRuleAb2b3c2s { get; set; }

    public virtual DbSet<S3dRuleB1b2b3d> S3dRuleB1b2b3ds { get; set; }

    public virtual DbSet<S3dRuleBendAngle> S3dRuleBendAngles { get; set; }

    public virtual DbSet<S3dRuleBoltExtension> S3dRuleBoltExtensions { get; set; }

    public virtual DbSet<S3dRuleBoltSelectionFilter> S3dRuleBoltSelectionFilters { get; set; }

    public virtual DbSet<S3dRuleC1c2> S3dRuleC1c2s { get; set; }

    public virtual DbSet<S3dRuleCapScrewLenCalTolerance> S3dRuleCapScrewLenCalTolerances { get; set; }

    public virtual DbSet<S3dRuleComponentTypeHierarchyRule> S3dRuleComponentTypeHierarchyRules { get; set; }

    public virtual DbSet<S3dRuleDefaultChangeOfDirectionPerSpec> S3dRuleDefaultChangeOfDirectionPerSpecs { get; set; }

    public virtual DbSet<S3dRuleGasketSelectionFilter> S3dRuleGasketSelectionFilters { get; set; }

    public virtual DbSet<S3dRuleMachBoltLenCalTolerance> S3dRuleMachBoltLenCalTolerances { get; set; }

    public virtual DbSet<S3dRuleMaterialsGrade> S3dRuleMaterialsGrades { get; set; }

    public virtual DbSet<S3dRuleMatingPort> S3dRuleMatingPorts { get; set; }

    public virtual DbSet<S3dRuleMinPipeLengthPurchasePerSpec> S3dRuleMinPipeLengthPurchasePerSpecs { get; set; }

    public virtual DbSet<S3dRuleMinimumPipeLengthRulePerSpec> S3dRuleMinimumPipeLengthRulePerSpecs { get; set; }

    public virtual DbSet<S3dRuleNutSelectionFilter> S3dRuleNutSelectionFilters { get; set; }

    public virtual DbSet<S3dRulePipeBranch> S3dRulePipeBranches { get; set; }

    public virtual DbSet<S3dRulePipeNominalDiameter> S3dRulePipeNominalDiameters { get; set; }

    public virtual DbSet<S3dRulePipeTakedownPart> S3dRulePipeTakedownParts { get; set; }

    public virtual DbSet<S3dRulePipingBendParameter> S3dRulePipingBendParameters { get; set; }

    public virtual DbSet<S3dRulePipingCommodityFilter> S3dRulePipingCommodityFilters { get; set; }

    public virtual DbSet<S3dRulePipingCommodityMatlControlDatum> S3dRulePipingCommodityMatlControlData { get; set; }

    public virtual DbSet<S3dRulePipingCompStandard> S3dRulePipingCompStandards { get; set; }

    public virtual DbSet<S3dRulePipingMaterialsClassDatum> S3dRulePipingMaterialsClassData { get; set; }

    public virtual DbSet<S3dRulePmcdatum> S3dRulePmcdata { get; set; }

    public virtual DbSet<S3dRulePreferredCapScrewLength> S3dRulePreferredCapScrewLengths { get; set; }

    public virtual DbSet<S3dRulePreferredMachBoltLength> S3dRulePreferredMachBoltLengths { get; set; }

    public virtual DbSet<S3dRulePreferredStudBoltLength> S3dRulePreferredStudBoltLengths { get; set; }

    public virtual DbSet<S3dRulePreferredTapEndStudBoltLength> S3dRulePreferredTapEndStudBoltLengths { get; set; }

    public virtual DbSet<S3dRulePressureRating> S3dRulePressureRatings { get; set; }

    public virtual DbSet<S3dRuleReinforcingWeldDatum> S3dRuleReinforcingWeldData { get; set; }

    public virtual DbSet<S3dRuleShortCodeHierarchyRule> S3dRuleShortCodeHierarchyRules { get; set; }

    public virtual DbSet<S3dRuleShortCodeMap> S3dRuleShortCodeMaps { get; set; }

    public virtual DbSet<S3dRuleSlipOnFlangeSetbackDistance> S3dRuleSlipOnFlangeSetbackDistances { get; set; }

    public virtual DbSet<S3dRuleStudBoltLenCalTolerance> S3dRuleStudBoltLenCalTolerances { get; set; }

    public virtual DbSet<S3dRuleTapEndStudBoltLenCalTol> S3dRuleTapEndStudBoltLenCalTols { get; set; }

    public virtual DbSet<S3dRuleValveOperatorMatlControlDatum> S3dRuleValveOperatorMatlControlData { get; set; }

    public virtual DbSet<S3dRuleWasherSelectionFilter> S3dRuleWasherSelectionFilters { get; set; }

    public virtual DbSet<S3dRuleWeldClearanceRule> S3dRuleWeldClearanceRules { get; set; }

    public virtual DbSet<S3dRuleWeldModelRepresentationRule> S3dRuleWeldModelRepresentationRules { get; set; }

    public virtual DbSet<S3dRuleWeldTypeRule> S3dRuleWeldTypeRules { get; set; }

    public virtual DbSet<VwFlangeStandPressureRating> VwFlangeStandPressureRatings { get; set; }

    public virtual DbSet<VwMaterialsCategoryPipingStandard> VwMaterialsCategoryPipingStandards { get; set; }

    public virtual DbSet<VwPipingStandardMaterialsGrade> VwPipingStandardMaterialsGrades { get; set; }

    public virtual DbSet<VwPipingStandardPressureRating> VwPipingStandardPressureRatings { get; set; }

    public virtual DbSet<VwPipingStandardScheduleThickness> VwPipingStandardScheduleThicknesses { get; set; }

    public virtual DbSet<VwS3dRuleAb2b3c2WithCode> VwS3dRuleAb2b3c2WithCodes { get; set; }

    public virtual DbSet<VwS3dRuleB1b2b3dWithCode> VwS3dRuleB1b2b3dWithCodes { get; set; }

    public virtual DbSet<VwS3dRuleC1c2WithCode> VwS3dRuleC1c2WithCodes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<S3dCdbBoltPart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_CDB___3214EC27C388D377");

            entity.ToTable("S3D_CDB_BoltPart");

            entity.HasIndex(e => e.IndustryCommodityCode, "UQ__S3D_CDB___782B02A1CD12FAB1").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BoltType).HasMaxLength(100);
            entity.Property(e => e.GeometricIndustryStandard).HasMaxLength(255);
            entity.Property(e => e.IndustryCommodityCode).HasMaxLength(255);
            entity.Property(e => e.MaterialGrade).HasMaxLength(255);
        });

        modelBuilder.Entity<S3dCdbGasketPart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_CDB___3214EC2781363C49");

            entity.ToTable("S3D_CDB_GasketPart");

            entity.HasIndex(e => new { e.IndustryCommodityCode, e.NominalDiameterFrom, e.NominalDiameterTo, e.NominalDiameter, e.NpdUnitType, e.GeometricIndustryStandard }, "UQ_S3D_CDB_GasketPart_CompositeUnique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FlangeFacing).HasMaxLength(100);
            entity.Property(e => e.GasketInsideDiameter).HasMaxLength(100);
            entity.Property(e => e.GasketOutsideDiameter).HasMaxLength(100);
            entity.Property(e => e.GasketType).HasMaxLength(100);
            entity.Property(e => e.GeometricIndustryStandard).HasMaxLength(255);
            entity.Property(e => e.IndustryCommodityCode).HasMaxLength(255);
            entity.Property(e => e.MaterialGrade).HasMaxLength(100);
            entity.Property(e => e.NominalDiameter).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.NominalDiameterFrom).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.NominalDiameterTo).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.NpdUnitType).HasMaxLength(20);
            entity.Property(e => e.ProcurementThickness).HasMaxLength(100);
            entity.Property(e => e.ThicknessFor3Dmodel)
                .HasMaxLength(100)
                .HasColumnName("ThicknessFor3DModel");
        });

        modelBuilder.Entity<S3dCdbGeometryDatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_CDB___3214EC276B0D0184");

            entity.ToTable("S3D_CDB_GeometryData");

            entity.HasIndex(e => new { e.Npd1, e.NpdUnitType1, e.EndPreparation1, e.ScheduleThickness1, e.Npd2, e.NpdUnitType2, e.EndPreparation2, e.ScheduleThickness2, e.GeometricIndustryStandard }, "UQ_S3D_CDB_GeometryData_CompositeUnique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BendRadius).HasMaxLength(100);
            entity.Property(e => e.DryCogX).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.DryCogY).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.DryCogZ).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.DryWeight).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.EndPreparation1).HasMaxLength(100);
            entity.Property(e => e.EndPreparation2).HasMaxLength(100);
            entity.Property(e => e.GeometricIndustryStandard).HasMaxLength(255);
            entity.Property(e => e.IndustryCommodityCode).HasMaxLength(255);
            entity.Property(e => e.MaterialsMgmtIdent).HasMaxLength(255);
            entity.Property(e => e.Npd1)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("NPD1");
            entity.Property(e => e.Npd2)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("NPD2");
            entity.Property(e => e.NpdUnitType1).HasMaxLength(20);
            entity.Property(e => e.NpdUnitType2).HasMaxLength(20);
            entity.Property(e => e.PartDescription).HasMaxLength(500);
            entity.Property(e => e.ScheduleThickness1).HasMaxLength(50);
            entity.Property(e => e.ScheduleThickness2).HasMaxLength(50);
        });

        modelBuilder.Entity<S3dCdbNutPart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_CDB___3214EC2794445D53");

            entity.ToTable("S3D_CDB_NutPart");

            entity.HasIndex(e => e.IndustryCommodityCode, "UQ__S3D_CDB___782B02A1BF70807B").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.GeometricIndustryStandard).HasMaxLength(255);
            entity.Property(e => e.IndustryCommodityCode).HasMaxLength(255);
            entity.Property(e => e.MaterialGrade).HasMaxLength(100);
            entity.Property(e => e.NutHeight).HasMaxLength(100);
            entity.Property(e => e.NutType).HasMaxLength(100);
        });

        modelBuilder.Entity<S3dCdbPipeComponent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_CDB___3214EC27E87DE18B");

            entity.ToTable("S3D_CDB_PipeComponent");

            entity.HasIndex(e => e.IndustryCommodityCode, "UQ__S3D_CDB___782B02A1C230F695").IsUnique();

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
            entity.HasKey(e => e.Id).HasName("PK__S3D_CDB___3214EC2720138F5C");

            entity.ToTable("S3D_CDB_PipeStock");

            entity.HasIndex(e => e.IndustryCommodityCode, "UQ__S3D_CDB___782B02A1A40F0DB3").IsUnique();

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

        modelBuilder.Entity<S3dCdbPortDatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_CDB___3214EC27011179BF");

            entity.ToTable("S3D_CDB_PortData");

            entity.HasIndex(e => new { e.Npd1, e.NpdUnitType1, e.PressureRating1, e.EndPreparation1, e.EndStandard1, e.SchduleThickness1, e.FlowDirection1, e.Npd2, e.NpdUnitType2, e.PressureRating2, e.EndPreparation2, e.EndStandard2, e.SchduleThickness2, e.FlowDirection2 }, "UQ_S3D_CDB_PortData_CompositeUnique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EndPreparation1).HasMaxLength(100);
            entity.Property(e => e.EndPreparation2).HasMaxLength(100);
            entity.Property(e => e.EndStandard1).HasMaxLength(255);
            entity.Property(e => e.EndStandard2).HasMaxLength(255);
            entity.Property(e => e.FlowDirection1).HasMaxLength(50);
            entity.Property(e => e.FlowDirection2).HasMaxLength(50);
            entity.Property(e => e.Npd1)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("NPD1");
            entity.Property(e => e.Npd2)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("NPD2");
            entity.Property(e => e.NpdUnitType1).HasMaxLength(20);
            entity.Property(e => e.NpdUnitType2).HasMaxLength(20);
            entity.Property(e => e.PressureRating1).HasMaxLength(50);
            entity.Property(e => e.PressureRating2).HasMaxLength(50);
            entity.Property(e => e.SchduleThickness1).HasMaxLength(50);
            entity.Property(e => e.SchduleThickness2).HasMaxLength(50);
        });

        modelBuilder.Entity<S3dCdbWasherPart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_CDB___3214EC27E4F84F37");

            entity.ToTable("S3D_CDB_WasherPart");

            entity.HasIndex(e => e.IndustryCommodityCode, "UQ__S3D_CDB___782B02A1AC047644").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.GeometricIndustryStandard).HasMaxLength(255);
            entity.Property(e => e.IndustryCommodityCode).HasMaxLength(255);
            entity.Property(e => e.MaterialGrade).HasMaxLength(100);
            entity.Property(e => e.WasherThickness).HasMaxLength(100);
            entity.Property(e => e.WasherType).HasMaxLength(100);
        });

        modelBuilder.Entity<S3dClGeometricIndustryPractice>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("S3D_CL_GeometricIndustryPractice");

            entity.Property(e => e.LongStringValue).HasMaxLength(255);
            entity.Property(e => e.ShortStringValue).HasMaxLength(255);
        });

        modelBuilder.Entity<S3dClGeometricIndustryStandard>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("S3D_CL_GeometricIndustryStandard");

            entity.Property(e => e.LongStringValue).HasMaxLength(255);
            entity.Property(e => e.ShortStringValue).HasMaxLength(255);
        });

        modelBuilder.Entity<S3dClMaterialsCategory>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("S3D_CL_MaterialsCategory");

            entity.Property(e => e.LongStringValue).HasMaxLength(255);
            entity.Property(e => e.ShortStringValue).HasMaxLength(255);
        });

        modelBuilder.Entity<S3dClMaterialsGrade>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("S3D_CL_MaterialsGrade");

            entity.Property(e => e.LongStringValue).HasMaxLength(255);
            entity.Property(e => e.ShortStringValue).HasMaxLength(255);
        });

        modelBuilder.Entity<S3dClPipingClass>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("S3D_CL_PipingClass");

            entity.Property(e => e.LongStringValue).HasMaxLength(255);
            entity.Property(e => e.ShortStringValue).HasMaxLength(255);
        });

        modelBuilder.Entity<S3dClPressureRating>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("S3D_CL_PressureRating");

            entity.Property(e => e.LongStringValue).HasMaxLength(255);
            entity.Property(e => e.ShortStringValue).HasMaxLength(255);
        });

        modelBuilder.Entity<S3dClRatingPractice>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("S3D_CL_RatingPractice");

            entity.Property(e => e.LongStringValue).HasMaxLength(255);
            entity.Property(e => e.ShortStringValue).HasMaxLength(255);
        });

        modelBuilder.Entity<S3dClScheduleThickness>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("S3D_CL_ScheduleThickness");

            entity.Property(e => e.LongStringValue).HasMaxLength(255);
            entity.Property(e => e.ShortStringValue).HasMaxLength(255);
        });

        modelBuilder.Entity<S3dCommonAttribute>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UD_Custo__C189298A34FA4233");

            entity.ToTable("S3D_Common_Attributes");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AttributeName).HasMaxLength(255);
            entity.Property(e => e.AttributeUserName).HasMaxLength(255);
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CodelistTableId).HasColumnName("CodelistTableID");
            entity.Property(e => e.InterfaceId).HasColumnName("InterfaceID");
            entity.Property(e => e.OnPropertyPage).HasDefaultValue(true);
            entity.Property(e => e.PrimaryUnits).HasMaxLength(50);
            entity.Property(e => e.SymbolParameter).HasMaxLength(255);
            entity.Property(e => e.Type).HasMaxLength(100);
            entity.Property(e => e.UnitsType).HasMaxLength(100);

            entity.HasOne(d => d.Category).WithMany(p => p.S3dCommonAttributeCategories)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__UD_Attrib__Categ__45F365D3");

            entity.HasOne(d => d.CodelistTable).WithMany(p => p.S3dCommonAttributeCodelistTables)
                .HasForeignKey(d => d.CodelistTableId)
                .HasConstraintName("FK__UD_Attrib__Codel__48CFD27E");

            entity.HasOne(d => d.Interface).WithMany(p => p.S3dCommonAttributes)
                .HasForeignKey(d => d.InterfaceId)
                .HasConstraintName("FK__UD_Attrib__Inter__44FF419A");
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

        modelBuilder.Entity<S3dCommonCustomInterface>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UD_Custo__3214EC272AA66006");

            entity.ToTable("S3D_Common_CustomInterfaces");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.InterfaceName).HasMaxLength(255);
        });

        modelBuilder.Entity<S3dCommonPipingGenericDataBolted>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Comm__3214EC27288C32BB");

            entity.ToTable("S3D_Common_PipingGenericDataBolted");

            entity.HasIndex(e => new { e.NominalPipingDiameter, e.NominalDiameterUnits, e.PressureRatingCl, e.EndPreparationCl, e.EndStandardCl }, "DSP_PipingGenericDataBolted_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BodyOutsideDiameter).HasColumnType("decimal(5, 3)");
            entity.Property(e => e.BoltCircleDiameter).HasColumnType("decimal(5, 3)");
            entity.Property(e => e.BoltDiameter).HasColumnType("decimal(5, 3)");
            entity.Property(e => e.DrillingTemplatePattern).HasMaxLength(100);
            entity.Property(e => e.EndPreparationCl).HasColumnName("EndPreparation_CL");
            entity.Property(e => e.EndStandardCl).HasColumnName("EndStandard_CL");
            entity.Property(e => e.FlangeFaceProjection).HasColumnType("decimal(5, 3)");
            entity.Property(e => e.FlangeGrooveWidth).HasColumnType("decimal(5, 3)");
            entity.Property(e => e.FlangeOutsideDiameter).HasColumnType("decimal(5, 3)");
            entity.Property(e => e.FlangeThickness).HasColumnType("decimal(5, 3)");
            entity.Property(e => e.FlangeThicknessTolerance).HasColumnType("decimal(5, 3)");
            entity.Property(e => e.NominalDiameterUnits).HasMaxLength(5);
            entity.Property(e => e.NominalPipingDiameter).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.PressureRatingCl).HasColumnName("PressureRating_CL");
            entity.Property(e => e.RaisedFaceDiameter).HasColumnType("decimal(5, 3)");
            entity.Property(e => e.SeatingDepth).HasColumnType("decimal(5, 3)");
        });

        modelBuilder.Entity<S3dCommonPipingGenericDataFemale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DSP_PipingGenericDataFemale_PK");

            entity.ToTable("S3D_Common_PipingGenericDataFemale");

            entity.HasIndex(e => new { e.NominalPipingDiameter, e.NominalDiameterUnits, e.PressureRatingCl, e.EndPreparationCl, e.EndStandardCl, e.ScheduleThicknessCl }, "DSP_PipingGenericDataFemale_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BodyOutsideDiameter).HasColumnType("decimal(5, 3)");
            entity.Property(e => e.EndPreparationCl).HasColumnName("EndPreparation_CL");
            entity.Property(e => e.EndStandardCl).HasColumnName("EndStandard_CL");
            entity.Property(e => e.HubOutsideDiameter).HasColumnType("decimal(5, 3)");
            entity.Property(e => e.HubThickness).HasColumnType("decimal(5, 3)");
            entity.Property(e => e.NominalDiameterUnits).HasMaxLength(5);
            entity.Property(e => e.NominalPipingDiameter).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.PressureRatingCl).HasColumnName("PressureRating_CL");
            entity.Property(e => e.ScheduleThicknessCl).HasColumnName("ScheduleThickness_CL");
            entity.Property(e => e.SocketDepth).HasColumnType("decimal(5, 3)");
            entity.Property(e => e.SocketDiameter).HasColumnType("decimal(5, 3)");
            entity.Property(e => e.SocketOffset).HasColumnType("decimal(5, 3)");
            entity.Property(e => e.ThreadDepth).HasColumnType("decimal(5, 3)");
        });

        modelBuilder.Entity<S3dCommonPlainPipingGenericDatum>(entity =>
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

        modelBuilder.Entity<S3dDictFlangeStandard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Dict__3214EC271559B6CC");

            entity.ToTable("S3D_Dict_FlangeStandard");

            entity.HasIndex(e => e.GeometricIndustryStandardCl, "UQ_FlangeStandard_GeometricIndustryStandard").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FlangeStandardCode).HasMaxLength(100);
            entity.Property(e => e.GeometricIndustryPracticeCl).HasColumnName("GeometricIndustryPractice_CL");
            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.Status).HasDefaultValue(true);
        });

        modelBuilder.Entity<S3dDictGeometricIndustryPractice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Dict__3214EC274CB870E3");

            entity.ToTable("S3D_Dict_GeometricIndustryPractice");

            entity.HasIndex(e => e.GeometricIndustryPracticeCl, "UQ_GeometricIndustryPractice_GeometricIndustryPractice").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.GeometricIndustryPracticeCl).HasColumnName("GeometricIndustryPractice_CL");
            entity.Property(e => e.Status).HasDefaultValue(true);
        });

        modelBuilder.Entity<S3dDictGeometricIndustryStandard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Dict__3214EC275E29B8EB");

            entity.ToTable("S3D_Dict_GeometricIndustryStandard");

            entity.HasIndex(e => e.GeometricIndustryStandardCl, "UQ_GeometricIndustryStandard_GeometricIndustryStandard").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ComponentTypeId).HasColumnName("ComponentTypeID");
            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.Status).HasDefaultValue(true);

            entity.HasOne(d => d.ComponentType).WithMany(p => p.S3dDictGeometricIndustryStandards)
                .HasForeignKey(d => d.ComponentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GeometricIndustryStandard_PipingComponentType");
        });

        modelBuilder.Entity<S3dDictMaterialsCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Dict__3214EC27F84C8810");

            entity.ToTable("S3D_Dict_MaterialsCategory");

            entity.HasIndex(e => e.MaterialsCategoryCl, "UQ_MaterialsCategory_MaterialsCategory").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaterialsCategoryCl).HasColumnName("MaterialsCategory_CL");
            entity.Property(e => e.MaterialsCategoryCode).HasMaxLength(100);
            entity.Property(e => e.Status).HasDefaultValue(true);
        });

        modelBuilder.Entity<S3dDictMaterialsGrade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Dict__3214EC27A685A653");

            entity.ToTable("S3D_Dict_MaterialsGrade");

            entity.HasIndex(e => e.MaterialsGradeCl, "UQ_MaterialsGrade_MaterialsGrade").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaterialsGradeCl).HasColumnName("MaterialsGrade_CL");
            entity.Property(e => e.MaterialsGradeCode).HasMaxLength(100);
            entity.Property(e => e.Status).HasDefaultValue(true);
        });

        modelBuilder.Entity<S3dDictPipingBend>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Dict__3214EC2735624A8D");

            entity.ToTable("S3D_Dict_PipingBend");

            entity.HasIndex(e => new { e.OutSideDiameter, e.OutSideDiameterUnit }, "UQ_PipingBend_OutSideDiameter_Unit").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.HeaderClampLength).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.OutSideDiameter).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.OutSideDiameterUnit).HasMaxLength(100);
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.TailClampLength).HasColumnType("decimal(10, 3)");
        });

        modelBuilder.Entity<S3dDictPipingBendDatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Dict__3214EC27258AC095");

            entity.ToTable("S3D_Dict_PipingBendData");

            entity.HasIndex(e => new { e.OutSideDiameter, e.OutSideDiameterUnit, e.MachineNum }, "UQ_PipingBend_OutSideDiameter_Unit_MachineNum").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.HeaderClampLength).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.MachineNum).HasDefaultValue(1);
            entity.Property(e => e.OutSideDiameter).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.OutSideDiameterUnit).HasMaxLength(100);
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.TailClampLength).HasColumnType("decimal(10, 3)");
        });

        modelBuilder.Entity<S3dDictPipingClass>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Dict__3214EC27D320CD47");

            entity.ToTable("S3D_Dict_PipingClass");

            entity.HasIndex(e => e.PipingClassCl, "UQ_PipingClass_PipingClass").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PipingClassCl).HasColumnName("PipingClass_CL");
            entity.Property(e => e.PipingClassCode).HasMaxLength(100);
            entity.Property(e => e.Status).HasDefaultValue(true);
        });

        modelBuilder.Entity<S3dDictPipingComponentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Dict__3214EC27633071B7");

            entity.ToTable("S3D_Dict_PipingComponentType");

            entity.HasIndex(e => e.ComponentTypeName, "UQ_PipingComponentType_Name").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ComponentTypeDescription).HasMaxLength(255);
            entity.Property(e => e.ComponentTypeName).HasMaxLength(255);
            entity.Property(e => e.Status).HasDefaultValue(true);
        });

        modelBuilder.Entity<S3dDictPipingStandard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Dict__3214EC27CE59BB28");

            entity.ToTable("S3D_Dict_PipingStandard");

            entity.HasIndex(e => new { e.GeometricIndustryStandardCl, e.PipingStandardCode, e.MaterialsCategoryCl, e.ScheduleSeriesId }, "UQ_PipingStandard_GeometricIndustryStandard_Code_MaterialsCategory_ScheduleSeriesID").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.GeometricIndustryPracticeCl).HasColumnName("GeometricIndustryPractice_CL");
            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.MaterialsCategoryCl).HasColumnName("MaterialsCategory_CL");
            entity.Property(e => e.PipingStandardCode).HasMaxLength(100);
            entity.Property(e => e.ScheduleSeriesId).HasColumnName("ScheduleSeriesID");
            entity.Property(e => e.Status).HasDefaultValue(true);
        });

        modelBuilder.Entity<S3dDictPressureRating>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Dict__3214EC2744DD424C");

            entity.ToTable("S3D_Dict_PressureRating");

            entity.HasIndex(e => e.PressureRatingCl, "UQ_PressureRating_PressureRating").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PressureRatingCl).HasColumnName("PressureRating_CL");
            entity.Property(e => e.PressureRatingCode).HasMaxLength(100);
            entity.Property(e => e.Status).HasDefaultValue(true);
        });

        modelBuilder.Entity<S3dDictScheduleSeries>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Dict__3214EC27AD84D77B");

            entity.ToTable("S3D_Dict_ScheduleSeries");

            entity.HasIndex(e => e.ScheduleSeriesName, "UQ_ScheduleSeries_Name").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ScheduleSeriesName).HasMaxLength(255);
            entity.Property(e => e.Status).HasDefaultValue(true);
        });

        modelBuilder.Entity<S3dDictScheduleThickness>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Dict__3214EC2737921018");

            entity.ToTable("S3D_Dict_ScheduleThickness");

            entity.HasIndex(e => new { e.ScheduleThicknessCl, e.MaterialsCategoryCl, e.ScheduleSeriesId }, "UQ_ScheduleThickness_ScheduleThickness_MaterialsCategory_ScheduleSeriesID").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaterialsCategoryCl).HasColumnName("MaterialsCategory_CL");
            entity.Property(e => e.ScheduleSeriesId).HasColumnName("ScheduleSeriesID");
            entity.Property(e => e.ScheduleThicknessCl).HasColumnName("ScheduleThickness_CL");
            entity.Property(e => e.ScheduleThicknessCode).HasMaxLength(100);
        });

        modelBuilder.Entity<S3dDictWallThickness>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Dict__3214EC27B7D4D8EE");

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

        modelBuilder.Entity<S3dPropertyInterfaceConfig>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Prop__3213E83FC95335E4");

            entity.ToTable("S3D_Property_InterfaceConfig");

            entity.HasIndex(e => new { e.ObjectTypeId, e.InterfaceName }, "UQ_interface_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .HasColumnName("created_by");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.InterfaceName)
                .HasMaxLength(100)
                .HasColumnName("interface_name");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.ObjectTypeId).HasColumnName("object_type_id");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.ObjectType).WithMany(p => p.S3dPropertyInterfaceConfigs)
                .HasForeignKey(d => d.ObjectTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_interface_object_type");
        });

        modelBuilder.Entity<S3dPropertyObjectType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Prop__3213E83FFF57C1FB");

            entity.ToTable("S3D_Property_ObjectType");

            entity.HasIndex(e => e.ObjectTypeName, "UQ_object_type_name").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .HasColumnName("created_by");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.ObjectTypeName)
                .HasMaxLength(100)
                .HasColumnName("object_type_name");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<S3dPropertyPropertyDefinition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Prop__3213E83F6333BA14");

            entity.ToTable("S3D_Property_PropertyDefinition");

            entity.HasIndex(e => new { e.InterfaceId, e.AttributeName }, "UQ_interface_attribute").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AttributeName)
                .HasMaxLength(100)
                .HasColumnName("attribute_name");
            entity.Property(e => e.AttributeUserName)
                .HasMaxLength(255)
                .HasColumnName("attribute_user_name");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .HasColumnName("category_name");
            entity.Property(e => e.CodelistName)
                .HasMaxLength(255)
                .HasColumnName("codelist_name");
            entity.Property(e => e.CodelistNamespace)
                .HasMaxLength(255)
                .HasColumnName("codelist_namespace");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .HasColumnName("created_by");
            entity.Property(e => e.DataType)
                .HasMaxLength(50)
                .HasColumnName("data_type");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.InterfaceId).HasColumnName("interface_id");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.IsReadOnly)
                .HasDefaultValue(false)
                .HasColumnName("is_read_only");
            entity.Property(e => e.IsSymbolParameter)
                .HasDefaultValue(false)
                .HasColumnName("is_symbol_parameter");
            entity.Property(e => e.Modifier)
                .HasMaxLength(100)
                .HasColumnName("modifier");
            entity.Property(e => e.OnPropertyPage)
                .HasDefaultValue(true)
                .HasColumnName("on_property_page");
            entity.Property(e => e.PrimaryUnits)
                .HasMaxLength(50)
                .HasColumnName("primary_units");
            entity.Property(e => e.SortOrder)
                .HasDefaultValue(0)
                .HasColumnName("sort_order");
            entity.Property(e => e.UnitsType)
                .HasMaxLength(50)
                .HasColumnName("units_type");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("updated_at");
            entity.Property(e => e.Version)
                .HasDefaultValue(1)
                .HasColumnName("version");

            entity.HasOne(d => d.Interface).WithMany(p => p.S3dPropertyPropertyDefinitions)
                .HasForeignKey(d => d.InterfaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_property_interface");
        });

        modelBuilder.Entity<S3dPropertyVersionHistory>(entity =>
        {
            entity.ToTable("S3D_Property_VersionHistory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AttributeUserName)
                .HasMaxLength(255)
                .HasColumnName("attribute_user_name");
            entity.Property(e => e.ChangeDescription)
                .HasMaxLength(500)
                .HasColumnName("change_description");
            entity.Property(e => e.CodelistName)
                .HasMaxLength(255)
                .HasColumnName("codelist_name");
            entity.Property(e => e.CodelistNamespace)
                .HasMaxLength(255)
                .HasColumnName("codelist_namespace");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.DataType)
                .HasMaxLength(50)
                .HasColumnName("data_type");
            entity.Property(e => e.IsReadOnly).HasColumnName("is_read_only");
            entity.Property(e => e.IsSymbolParameter).HasColumnName("is_symbol_parameter");
            entity.Property(e => e.Modifier)
                .HasMaxLength(100)
                .HasColumnName("modifier");
            entity.Property(e => e.OnPropertyPage).HasColumnName("on_property_page");
            entity.Property(e => e.PrimaryUnits)
                .HasMaxLength(50)
                .HasColumnName("primary_units");
            entity.Property(e => e.PropertyId).HasColumnName("property_id");
            entity.Property(e => e.UnitsType)
                .HasMaxLength(50)
                .HasColumnName("units_type");
            entity.Property(e => e.Version).HasColumnName("version");

            entity.HasOne(d => d.Property).WithMany(p => p.S3dPropertyVersionHistories)
                .HasForeignKey(d => d.PropertyId)
                .HasConstraintName("FK_history_property");
        });

        modelBuilder.Entity<S3dRuleAb2b3c2>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27014122C8");

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
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27D59B94F2");

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

        modelBuilder.Entity<S3dRuleBendAngle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27A123FD9F");

            entity.ToTable("S3D_Rule_BendAngles");

            entity.HasIndex(e => new { e.SpecId, e.Npd, e.NpdUnitType, e.BendAngle }, "DSP_BendAngles_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BendAngle).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Npd).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.NpdUnitType).HasMaxLength(5);
            entity.Property(e => e.SpecId).HasColumnName("SpecID");

            entity.HasOne(d => d.Spec).WithMany(p => p.S3dRuleBendAngles)
                .HasForeignKey(d => d.SpecId)
                .HasConstraintName("FK_BendSpecification_Spec");
        });

        modelBuilder.Entity<S3dRuleBoltExtension>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27EB1B4F42");

            entity.ToTable("S3D_Rule_BoltExtension");

            entity.HasIndex(e => new { e.SpecId, e.NominalPipingDiameter, e.NominalPipingDiameterUnits, e.PressureRating, e.EndPreparation, e.EndStandard }, "DSP_BoltExtension_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AltBoltExtensionForMachBolts2).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.AltBoltExtensionForMachBolts3).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.AltBoltExtensionForMachBolts4).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.AltBoltExtensionForMachBolts5).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.AltBoltExtensionForMachBolts6).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.AltBoltExtensionForStuds2).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.AltBoltExtensionForStuds3).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.AltBoltExtensionForStuds4).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.AltBoltExtensionForStuds5).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.AltBoltExtensionForStuds6).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.EndPreparation).HasMaxLength(50);
            entity.Property(e => e.EndStandard).HasMaxLength(50);
            entity.Property(e => e.NominalPipingDiameter).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.NominalPipingDiameterUnits).HasMaxLength(5);
            entity.Property(e => e.PressureRating).HasMaxLength(50);
            entity.Property(e => e.SpecId).HasColumnName("SpecID");
            entity.Property(e => e.StandardBoltExtForMachBolts).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.StandardBoltExtensionForStuds).HasColumnType("decimal(8, 3)");

            entity.HasOne(d => d.Spec).WithMany(p => p.S3dRuleBoltExtensions)
                .HasForeignKey(d => d.SpecId)
                .HasConstraintName("FK_BoltExtensionData_Spec");
        });

        modelBuilder.Entity<S3dRuleBoltSelectionFilter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC276D5092A0");

            entity.ToTable("S3D_Rule_BoltSelectionFilter");

            entity.HasIndex(e => new { e.SpecId, e.NominalDiameterFrom, e.NominalDiameterTo, e.NpdUnitType, e.BoltOption, e.MaximumTemperature, e.EndPreparation, e.PressureRating, e.AlternateEndPreparation, e.EndStandard, e.AlternatePressureRating, e.AlternateEndStandard }, "DSP_BoltSelectionFilter_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AlternateEndPreparation).HasMaxLength(50);
            entity.Property(e => e.AlternateEndStandard).HasMaxLength(50);
            entity.Property(e => e.AlternatePressureRating).HasMaxLength(50);
            entity.Property(e => e.BoltExtensionOption).HasMaxLength(50);
            entity.Property(e => e.BoltOption).HasMaxLength(50);
            entity.Property(e => e.Comments).HasMaxLength(500);
            entity.Property(e => e.ContractorCommodityCode).HasMaxLength(50);
            entity.Property(e => e.EndPreparation).HasMaxLength(50);
            entity.Property(e => e.EndStandard).HasMaxLength(50);
            entity.Property(e => e.FabricationCategoryOverride).HasMaxLength(100);
            entity.Property(e => e.LubricationRequirements).HasMaxLength(255);
            entity.Property(e => e.MaximumTemperature).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.NominalDiameterFrom).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.NominalDiameterTo).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.NpdUnitType).HasMaxLength(5);
            entity.Property(e => e.PipingNote1).HasMaxLength(255);
            entity.Property(e => e.PressureRating).HasMaxLength(50);
            entity.Property(e => e.SpecId).HasColumnName("SpecID");
            entity.Property(e => e.SupplyResponsibilityOverride).HasMaxLength(100);

            entity.HasOne(d => d.Spec).WithMany(p => p.S3dRuleBoltSelectionFilters)
                .HasForeignKey(d => d.SpecId)
                .HasConstraintName("FK_ComponentSpecRange_Spec");
        });

        modelBuilder.Entity<S3dRuleC1c2>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC277C0CBE54");

            entity.ToTable("S3D_Rule_C1C2");

            entity.HasIndex(e => new { e.GeometricIndustryStandardCl, e.PressureRatingCl, e.RuleName }, "UQ_SPMC_C1C2").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.PressureRatingCl).HasColumnName("PressureRating_CL");
            entity.Property(e => e.RuleName).HasMaxLength(255);
        });

        modelBuilder.Entity<S3dRuleCapScrewLenCalTolerance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC2776B6C3FB");

            entity.ToTable("S3D_Rule_CapScrewLenCalTolerance");

            entity.HasIndex(e => new { e.BoltLengthFrom, e.BoltLengthTo, e.BoltDiameterFrom }, "DSP_CapScrewLenCalTolerance_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BoltDiameterFrom).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.BoltDiameterTo).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.BoltLengthFrom).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.BoltLengthTo).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.BoltLengthTolerance).HasColumnType("decimal(6, 3)");
        });

        modelBuilder.Entity<S3dRuleComponentTypeHierarchyRule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC272691E900");

            entity.ToTable("S3D_Rule_ComponentTypeHierarchyRule");

            entity.HasIndex(e => new { e.ComponentTypeId, e.PipingCommoditySubClassCl }, "UQ_PipingComponentType_ID_SubClass").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ComponentTypeId).HasColumnName("ComponentTypeID");
            entity.Property(e => e.PipingCommoditySubClassCl).HasColumnName("PipingCommoditySubClass_CL");
            entity.Property(e => e.Status).HasDefaultValue(true);

            entity.HasOne(d => d.ComponentType).WithMany(p => p.S3dRuleComponentTypeHierarchyRules)
                .HasForeignKey(d => d.ComponentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ComponentTypeHierarchyRule_ComponentType");
        });

        modelBuilder.Entity<S3dRuleDefaultChangeOfDirectionPerSpec>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC2702E03EBF");

            entity.ToTable("S3D_Rule_DefaultChangeOfDirectionPerSpec");

            entity.HasIndex(e => new { e.SpecId, e.BendAngleFrom, e.BendAngleTo }, "DSP_DefaultChangeOfDirectionPerSpec_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BendAngleFrom).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.BendAngleTo).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.FunctionalShortCode).HasMaxLength(50);
            entity.Property(e => e.SpecId).HasColumnName("SpecID");

            entity.HasOne(d => d.Spec).WithMany(p => p.S3dRuleDefaultChangeOfDirectionPerSpecs)
                .HasForeignKey(d => d.SpecId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DSP_DefaultChangeOfDirectionPerSpec_DSP_PipingMaterialsClassData_FK");
        });

        modelBuilder.Entity<S3dRuleGasketSelectionFilter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27318D54BF");

            entity.ToTable("S3D_Rule_GasketSelectionFilter");

            entity.HasIndex(e => new { e.SpecId, e.NominalDiameterFrom, e.NominalDiameterTo, e.NpdUnitType, e.GasketOption, e.MaximumTemperature, e.MinimumTemperature, e.EndPreparation, e.PressureRating, e.EndStandard, e.AlternateEndPreparation, e.AlternatePressureRating, e.AlternateEndStandard, e.FluidCode, e.ScheduleThickness }, "DSP_GasketSelectionFilter_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AltReportableCommodityCode).HasMaxLength(50);
            entity.Property(e => e.AlternateEndPreparation).HasMaxLength(50);
            entity.Property(e => e.AlternateEndStandard).HasMaxLength(50);
            entity.Property(e => e.AlternatePressureRating).HasMaxLength(50);
            entity.Property(e => e.Comments).HasMaxLength(500);
            entity.Property(e => e.ContractorCommodityCode).HasMaxLength(50);
            entity.Property(e => e.EndPreparation).HasMaxLength(50);
            entity.Property(e => e.EndStandard).HasMaxLength(50);
            entity.Property(e => e.FabricationCategoryOverride).HasMaxLength(100);
            entity.Property(e => e.FluidCode).HasMaxLength(50);
            entity.Property(e => e.GasketOption).HasMaxLength(50);
            entity.Property(e => e.MaximumTemperature).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MinimumTemperature).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.NominalDiameterFrom).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.NominalDiameterTo).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.NpdUnitType).HasMaxLength(5);
            entity.Property(e => e.PipingNote1).HasMaxLength(255);
            entity.Property(e => e.PressureRating).HasMaxLength(50);
            entity.Property(e => e.ReportableCommodityCode).HasMaxLength(50);
            entity.Property(e => e.RingNumber).HasMaxLength(20);
            entity.Property(e => e.ScheduleThickness).HasMaxLength(50);
            entity.Property(e => e.SpecId).HasColumnName("SpecID");
            entity.Property(e => e.SupplyResponsibilityOverride).HasMaxLength(100);

            entity.HasOne(d => d.Spec).WithMany(p => p.S3dRuleGasketSelectionFilters)
                .HasForeignKey(d => d.SpecId)
                .HasConstraintName("FK_GasketSpecification_Spec");
        });

        modelBuilder.Entity<S3dRuleMachBoltLenCalTolerance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27CD9A0B9C");

            entity.ToTable("S3D_Rule_MachBoltLenCalTolerance");

            entity.HasIndex(e => new { e.BoltLengthFrom, e.BoltLengthTo, e.BoltDiameterFrom, e.BoltDiameterTo }, "DSP_MachBoltLenCalTolerance_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BoltDiameterFrom).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.BoltDiameterTo).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.BoltLengthFrom).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.BoltLengthTo).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.BoltLengthTolerance).HasColumnType("decimal(6, 3)");
        });

        modelBuilder.Entity<S3dRuleMaterialsGrade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27BA6A5C11");

            entity.ToTable("S3D_Rule_MaterialsGrade");

            entity.HasIndex(e => new { e.MaterialsGradeCl, e.MaterialsCategoryCl, e.GeometricIndustryStandardCl }, "UQ_MaterialsGrade_MaterialsCategory_GeometricIndustryStandard").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.Manufacturer).HasMaxLength(255);
            entity.Property(e => e.MaterialsCategoryCl).HasColumnName("MaterialsCategory_CL");
            entity.Property(e => e.MaterialsGradeCl).HasColumnName("MaterialsGrade_CL");
            entity.Property(e => e.Status).HasDefaultValue(true);
        });

        modelBuilder.Entity<S3dRuleMatingPort>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27A3749D56");

            entity.ToTable("S3D_Rule_MatingPorts");

            entity.HasIndex(e => new { e.EndPrep1, e.EndPrep2 }, "DSP_MatingPorts_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EndPrep1).HasMaxLength(20);
            entity.Property(e => e.EndPrep2).HasMaxLength(20);
        });

        modelBuilder.Entity<S3dRuleMinPipeLengthPurchasePerSpec>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC2705EF5FD0");

            entity.ToTable("S3D_Rule_MinPipeLengthPurchasePerSpec");

            entity.HasIndex(e => new { e.SpecId, e.NominalPipingDiameter, e.NominalPipingDiameterUnits, e.PurchaseLength }, "DSP_MinPipeLengthPurchasePerSpec_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MinimumPipeLength).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.NominalPipingDiameter).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.NominalPipingDiameterUnits).HasMaxLength(5);
            entity.Property(e => e.PreferredMinimumPipeLength).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.PurchaseLength).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.SpecId).HasColumnName("SpecID");

            entity.HasOne(d => d.Spec).WithMany(p => p.S3dRuleMinPipeLengthPurchasePerSpecs)
                .HasForeignKey(d => d.SpecId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PipeLengthData_PipingMaterialsClassData");
        });

        modelBuilder.Entity<S3dRuleMinimumPipeLengthRulePerSpec>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27AFCC8BEB");

            entity.ToTable("S3D_Rule_MinimumPipeLengthRulePerSpec");

            entity.HasIndex(e => new { e.SpecId, e.Npd, e.NpdUnitType }, "DSP_MinimumPipeLengthRulePerSpec_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MinimumPipeLength).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.Npd).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.NpdUnitType).HasMaxLength(5);
            entity.Property(e => e.PreferredMinimumPipeLength).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.SpecId).HasColumnName("SpecID");

            entity.HasOne(d => d.Spec).WithMany(p => p.S3dRuleMinimumPipeLengthRulePerSpecs)
                .HasForeignKey(d => d.SpecId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MinimumPipeLength_Specification");
        });

        modelBuilder.Entity<S3dRuleNutSelectionFilter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC2760B095F0");

            entity.ToTable("S3D_Rule_NutSelectionFilter");

            entity.HasIndex(e => new { e.BoltDiameter, e.SpecId, e.MaximumTemperature, e.PressureRating, e.NutOption, e.BoltType }, "DSP_NutSelectionFilter_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BoltDiameter).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.BoltType).HasMaxLength(50);
            entity.Property(e => e.Comments).HasMaxLength(500);
            entity.Property(e => e.ContractorCommodityCode).HasMaxLength(50);
            entity.Property(e => e.FabricationCategoryOverride).HasMaxLength(100);
            entity.Property(e => e.MaximumTemperature).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.NutOption).HasMaxLength(50);
            entity.Property(e => e.PipingNote1).HasMaxLength(255);
            entity.Property(e => e.PressureRating).HasMaxLength(50);
            entity.Property(e => e.SpecId).HasColumnName("SpecID");
            entity.Property(e => e.SupplNutCntrCommodityCode).HasMaxLength(50);
            entity.Property(e => e.SupplementaryNutOption).HasMaxLength(50);
            entity.Property(e => e.SupplyResponsibilityOverride).HasMaxLength(100);

            entity.HasOne(d => d.Spec).WithMany(p => p.S3dRuleNutSelectionFilters)
                .HasForeignKey(d => d.SpecId)
                .HasConstraintName("FK_BoltingSpecification_Spec");
        });

        modelBuilder.Entity<S3dRulePipeBranch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC273C8168DA");

            entity.ToTable("S3D_Rule_PipeBranch");

            entity.HasIndex(e => new { e.SpecId, e.HeaderSize, e.BranchSize, e.AngleLow, e.AngleHigh, e.HdrSizeNpdunitType, e.BrSizeNpdunitType }, "DSP_PipeBranch_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AngleHigh).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.AngleLow).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.BrSizeNpdunitType)
                .HasMaxLength(5)
                .HasColumnName("BrSizeNPDUnitType");
            entity.Property(e => e.BranchSize).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.HdrSizeNpdunitType)
                .HasMaxLength(5)
                .HasColumnName("HdrSizeNPDUnitType");
            entity.Property(e => e.HeaderSize).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.SecondaryShortCode).HasMaxLength(50);
            entity.Property(e => e.ShortCode).HasMaxLength(50);
            entity.Property(e => e.SpecId).HasColumnName("SpecID");
            entity.Property(e => e.TertiaryShortCode).HasMaxLength(50);

            entity.HasOne(d => d.Spec).WithMany(p => p.S3dRulePipeBranches)
                .HasForeignKey(d => d.SpecId)
                .HasConstraintName("DSP_PipeBranch_DSP_PipingMaterialsClassData_FK");
        });

        modelBuilder.Entity<S3dRulePipeNominalDiameter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27A4B66849");

            entity.ToTable("S3D_Rule_PipeNominalDiameters");

            entity.HasIndex(e => new { e.SpecId, e.Npd, e.NpdUnitType }, "DSP_PipeNominalDiameters_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Npd).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.NpdUnitType).HasMaxLength(5);
            entity.Property(e => e.SpecId).HasColumnName("SpecID");

            entity.HasOne(d => d.Spec).WithMany(p => p.S3dRulePipeNominalDiameters)
                .HasForeignKey(d => d.SpecId)
                .HasConstraintName("DSP_PipeNominalDiameters_DSP_PipingMaterialsClassData_FK");
        });

        modelBuilder.Entity<S3dRulePipeTakedownPart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC277C5E9EEC");

            entity.ToTable("S3D_Rule_PipeTakedownParts");

            entity.HasIndex(e => new { e.SpecId, e.TakeDownShortCode, e.WeldShortCode, e.IsPairRequired, e.Npd, e.NpdUnitType, e.IsWeld }, "DSP_PipeTakedownParts_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Npd).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.NpdUnitType).HasMaxLength(5);
            entity.Property(e => e.SpecId).HasColumnName("SpecID");
            entity.Property(e => e.TakeDownShortCode).HasMaxLength(20);
            entity.Property(e => e.WeldShortCode).HasMaxLength(20);

            entity.HasOne(d => d.Spec).WithMany(p => p.S3dRulePipeTakedownParts)
                .HasForeignKey(d => d.SpecId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ComponentCodeData_PipingMaterialsClassData");
        });

        modelBuilder.Entity<S3dRulePipingBendParameter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27835BEAC8");

            entity.ToTable("S3D_Rule_PipingBendParameter");

            entity.HasIndex(e => new { e.MaterialsCategoryCl, e.NormalDiameter, e.UnitType, e.ScheduleThicknessCl }, "UQ_PipingBendParameter_MaterialsCategory_Diameter_Unit_ScheduleThickness").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BendRadiusMultiplier).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.MaterialsCategoryCl).HasColumnName("MaterialsCategory_CL");
            entity.Property(e => e.NormalDiameter).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.ScheduleThicknessCl).HasColumnName("ScheduleThickness_CL");
            entity.Property(e => e.UnitType).HasMaxLength(100);
        });

        modelBuilder.Entity<S3dRulePipingCommodityFilter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27FB9C9221");

            entity.ToTable("S3D_Rule_PipingCommodityFilter");

            entity.HasIndex(e => new { e.SpecId, e.ShortCode, e.OptionCode, e.FirstSizeFrom, e.FirstSizeTo, e.FirstSizeUnits, e.FirstSizeSchedule, e.SecondSizeFrom, e.SecondSizeTo, e.SecondSizeUnits, e.SecondSizeSchedule, e.MultisizeOption }, "DSP_PipingCommodityFilter_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BendRadius).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.BendRadiusMultiplier).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.CommodityCode).HasMaxLength(100);
            entity.Property(e => e.EngineeringTag).HasMaxLength(100);
            entity.Property(e => e.FirstSizeFrom).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.FirstSizeSchedule).HasMaxLength(20);
            entity.Property(e => e.FirstSizeTo).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.FirstSizeUnits).HasMaxLength(5);
            entity.Property(e => e.MultisizeOption).HasMaxLength(50);
            entity.Property(e => e.OptionCode).HasMaxLength(20);
            entity.Property(e => e.PipingNote1).HasMaxLength(255);
            entity.Property(e => e.SecondSizeFrom).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.SecondSizeSchedule).HasMaxLength(20);
            entity.Property(e => e.SecondSizeTo).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.SecondSizeUnits).HasMaxLength(5);
            entity.Property(e => e.SelectionBasis).HasMaxLength(50);
            entity.Property(e => e.ShortCode).HasMaxLength(20);
            entity.Property(e => e.SpecId).HasColumnName("SpecID");

            entity.HasOne(d => d.Spec).WithMany(p => p.S3dRulePipingCommodityFilters)
                .HasForeignKey(d => d.SpecId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SpecComponent_SpecData");
        });

        modelBuilder.Entity<S3dRulePipingCommodityMatlControlDatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC2776631CC6");

            entity.ToTable("S3D_Rule_PipingCommodityMatlControlData");

            entity.HasIndex(e => new { e.ContractorCommodityCode, e.FirstSizeFrom, e.FirstSizeTo, e.FirstSizeUnits, e.SecondSizeFrom, e.SecondSizeTo, e.SecondSizeUnits, e.MultisizeOption }, "DSP_PipingCommodityMatlControlData_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CimiscommodityCode)
                .HasMaxLength(255)
                .HasColumnName("CIMISCommodityCode");
            entity.Property(e => e.ClientCommodityCode).HasMaxLength(255);
            entity.Property(e => e.ContractorCommodityCode).HasMaxLength(255);
            entity.Property(e => e.EClasseProcurementCode).HasColumnName("eClasseProcurementCode");
            entity.Property(e => e.FirstSizeFrom).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.FirstSizeTo).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.FirstSizeUnits).HasMaxLength(5);
            entity.Property(e => e.IndustryCommodityCode).HasMaxLength(255);
            entity.Property(e => e.LocalizedShortMaterialDesc).HasMaxLength(255);
            entity.Property(e => e.LongMaterialDescription).HasMaxLength(255);
            entity.Property(e => e.SecondSizeFrom).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.SecondSizeTo).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.SecondSizeUnits).HasMaxLength(5);
            entity.Property(e => e.ShortMaterialDescription).HasMaxLength(255);
            entity.Property(e => e.UnspsceProcurementCode).HasColumnName("UNSPSCeProcurementCode");
            entity.Property(e => e.ValveOperatorCatalogPartNumber).HasMaxLength(255);

            entity.HasOne(d => d.ValveOperatorCatalogPartNumberNavigation).WithMany(p => p.S3dRulePipingCommodityMatlControlData)
                .HasPrincipalKey(p => p.OperatorPartNumber)
                .HasForeignKey(d => d.ValveOperatorCatalogPartNumber)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("DSP_PipingCommodityMatlControlData_DSP_ValveOperatorMatlControlData_FK");
        });

        modelBuilder.Entity<S3dRulePipingCompStandard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27562E5BF9");

            entity.ToTable("S3D_Rule_PipingCompStandard");

            entity.HasIndex(e => new { e.GeometricIndustryStandardCl, e.ComponentTypeId, e.MaterialsCategoryCl }, "UQ_PipingCompStandard_GeometricIndustryStandard_ComponentType_MaterialsCategory").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ComponentTypeId).HasColumnName("ComponentTypeID");
            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.MaterialsCategoryCl).HasColumnName("MaterialsCategory_CL");
            entity.Property(e => e.Status).HasDefaultValue(true);

            entity.HasOne(d => d.ComponentType).WithMany(p => p.S3dRulePipingCompStandards)
                .HasForeignKey(d => d.ComponentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PipingComponentStandard_PipingComponentType");
        });

        modelBuilder.Entity<S3dRulePipingMaterialsClassDatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27E7705502");

            entity.ToTable("S3D_Rule_PipingMaterialsClassData");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ApprovalDate).HasMaxLength(20);
            entity.Property(e => e.ApprovedBy).HasMaxLength(20);
            entity.Property(e => e.Comments).HasMaxLength(255);
            entity.Property(e => e.FluidService).HasMaxLength(255);
            entity.Property(e => e.JacketMaterialsDescription).HasMaxLength(255);
            entity.Property(e => e.JumperMaterialsDescription).HasMaxLength(255);
            entity.Property(e => e.LastModifiedOn).HasMaxLength(20);
            entity.Property(e => e.MaterialsDescription).HasMaxLength(1000);
            entity.Property(e => e.MaterialsOfConstructionClass).HasMaxLength(255);
            entity.Property(e => e.RevisionNumber).HasMaxLength(20);
            entity.Property(e => e.SpecName).HasMaxLength(20);
        });

        modelBuilder.Entity<S3dRulePmcdatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27672C025E");

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

        modelBuilder.Entity<S3dRulePreferredCapScrewLength>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC275C016FC5");

            entity.ToTable("S3D_Rule_PreferredCapScrewLength");

            entity.HasIndex(e => new { e.BoltDiameterTo, e.BoltDiameterFrom, e.BoltDiameterIncrement, e.MaterialsGrade }, "DSP_PreferredCapScrewLength_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BoltDiameterFrom).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.BoltDiameterIncrement).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.BoltDiameterTo).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.MaterialsGrade).HasMaxLength(50);
            entity.Property(e => e.PreferredBoltLengthFrom).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.PreferredBoltLengthIncrement).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.PreferredBoltLengthTo).HasColumnType("decimal(8, 3)");
        });

        modelBuilder.Entity<S3dRulePreferredMachBoltLength>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27089E2A51");

            entity.ToTable("S3D_Rule_PreferredMachBoltLength");

            entity.HasIndex(e => new { e.BoltDiameterFrom, e.BoltDiameterTo, e.BoltDiameterIncrement, e.MaterialsGrade }, "DSP_PreferredMachBoltLength_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BoltDiameterFrom).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.BoltDiameterIncrement).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.BoltDiameterTo).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.MaterialsGrade).HasMaxLength(50);
            entity.Property(e => e.PreferredBoltLengthFrom).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.PreferredBoltLengthIncrement).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.PreferredBoltLengthTo).HasColumnType("decimal(8, 3)");
        });

        modelBuilder.Entity<S3dRulePreferredStudBoltLength>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC275EB392AA");

            entity.ToTable("S3D_Rule_PreferredStudBoltLength");

            entity.HasIndex(e => new { e.BoltDiameterFrom, e.BoltDiameterTo, e.BoltDiameterIncrement, e.MaterialsGrade }, "DSP_PreferredStudBoltLength_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BoltDiameterFrom).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.BoltDiameterIncrement).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.BoltDiameterTo).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.MaterialsGrade).HasMaxLength(50);
            entity.Property(e => e.PreferredBoltLengthFrom).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.PreferredBoltLengthIncrement).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.PreferredBoltLengthTo).HasColumnType("decimal(8, 3)");
        });

        modelBuilder.Entity<S3dRulePreferredTapEndStudBoltLength>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27A3006AF1");

            entity.ToTable("S3D_Rule_PreferredTapEndStudBoltLength");

            entity.HasIndex(e => new { e.BoltDiameterFrom, e.BoltDiameterTo, e.BoltDiameterIncrement, e.MaterialsGrade }, "DSP_PreferredTapEndStudBoltLength_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BoltDiameterFrom).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.BoltDiameterIncrement).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.BoltDiameterTo).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.MaterialsGrade).HasMaxLength(50);
            entity.Property(e => e.PreferredBoltLengthFrom).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.PreferredBoltLengthIncrement).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.PreferredBoltLengthTo).HasColumnType("decimal(8, 3)");
        });

        modelBuilder.Entity<S3dRulePressureRating>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27DEDBB0E5");

            entity.ToTable("S3D_Rule_PressureRating");

            entity.HasIndex(e => new { e.PressureRatingCl, e.GeometricIndustryStandardCl }, "UQ_PressureRating_PressureRating_GeometricIndustryStandard").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.PressureRatingCl).HasColumnName("PressureRating_CL");
            entity.Property(e => e.Status).HasDefaultValue(true);
        });

        modelBuilder.Entity<S3dRuleReinforcingWeldDatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC278F9642D6");

            entity.ToTable("S3D_Rule_ReinforcingWeldData");

            entity.HasIndex(e => new { e.SpecId, e.HeaderSize, e.BranchSize, e.AcuteBranchAngleFrom, e.AcuteBranchAngleTo, e.BranchSizeUnitsOfMeasure, e.HeaderSizeUnitsOfMeasure }, "DSP_ReinforcingWeldData_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AcuteBranchAngleFrom).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.AcuteBranchAngleTo).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.BranchSize).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.BranchSizeUnitsOfMeasure).HasMaxLength(5);
            entity.Property(e => e.HeaderSize).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.HeaderSizeUnitsOfMeasure).HasMaxLength(5);
            entity.Property(e => e.MinimumReinforcingWeldSize).HasColumnType("decimal(10, 5)");
            entity.Property(e => e.SpecId).HasColumnName("SpecID");

            entity.HasOne(d => d.Spec).WithMany(p => p.S3dRuleReinforcingWeldData)
                .HasForeignKey(d => d.SpecId)
                .HasConstraintName("DSP_ReinforcingWeldData_DSP_PipingMaterialsClassData_FK");
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

        modelBuilder.Entity<S3dRuleShortCodeMap>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27BA82FA1D");

            entity.ToTable("S3D_Rule_ShortCodeMap");

            entity.HasIndex(e => new { e.ComponentTypeId, e.ShortCode }, "UQ_ShortCodeMap_ID_Code").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ComponentTypeId).HasColumnName("ComponentTypeID");
            entity.Property(e => e.ShortCode).HasMaxLength(50);

            entity.HasOne(d => d.ComponentType).WithMany(p => p.S3dRuleShortCodeMaps)
                .HasForeignKey(d => d.ComponentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShortCodeMap_PipingComponentType");
        });

        modelBuilder.Entity<S3dRuleSlipOnFlangeSetbackDistance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27AFFC2846");

            entity.ToTable("S3D_Rule_SlipOnFlangeSetbackDistance");

            entity.HasIndex(e => new { e.NominalPipingDiameterFrom, e.NominalPipingDiameterTo, e.NominalPipingDiameterUnits, e.EndStandard }, "DSP_SlipOnFlangeSetbackDistance_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CompanyPracticeGap).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.CompanyPracticeRoundOffFactor).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.EndStandard).HasMaxLength(50);
            entity.Property(e => e.MaximumWeldThickness).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.NominalPipingDiameterFrom).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.NominalPipingDiameterTo).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.NominalPipingDiameterUnits).HasMaxLength(5);
        });

        modelBuilder.Entity<S3dRuleStudBoltLenCalTolerance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC279928AE97");

            entity.ToTable("S3D_Rule_StudBoltLenCalTolerance");

            entity.HasIndex(e => new { e.BoltLengthFrom, e.BoltLengthTo, e.BoltDiameterFrom, e.BoltDiameterTo }, "DSP_StudBoltLenCalTolerance_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BoltDiameterFrom).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.BoltDiameterTo).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.BoltLengthFrom).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.BoltLengthTo).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.BoltLengthTolerance).HasColumnType("decimal(8, 3)");
        });

        modelBuilder.Entity<S3dRuleTapEndStudBoltLenCalTol>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27B806BAAC");

            entity.ToTable("S3D_Rule_TapEndStudBoltLenCalTol");

            entity.HasIndex(e => new { e.BoltLengthFrom, e.BoltLengthTo, e.BoltDiameterFrom, e.BoltDiameterTo }, "DSP_TapEndStudBoltLenCalTol_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BoltDiameterFrom).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.BoltDiameterTo).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.BoltLengthFrom).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.BoltLengthTo).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.BoltLengthTolerance).HasColumnType("decimal(8, 3)");
        });

        modelBuilder.Entity<S3dRuleValveOperatorMatlControlDatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27D9949D4D");

            entity.ToTable("S3D_Rule_ValveOperatorMatlControlData");

            entity.HasIndex(e => e.OperatorPartNumber, "DSP_ValveOperatorMatlControlData_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.LocalizedShortMaterialDescription).HasMaxLength(500);
            entity.Property(e => e.LongMaterialDescription).HasMaxLength(500);
            entity.Property(e => e.OperatorPartNumber).HasMaxLength(255);
            entity.Property(e => e.ShortMatlDescription).HasMaxLength(500);
        });

        modelBuilder.Entity<S3dRuleWasherSelectionFilter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC275DCE2305");

            entity.ToTable("S3D_Rule_WasherSelectionFilter");

            entity.HasIndex(e => new { e.SpecId, e.WasherOption, e.MaximumTemperature, e.BoltDiameter, e.PressureRating }, "DSP_WasherSelectionFilter_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BoltDiameter).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.Comments).HasMaxLength(500);
            entity.Property(e => e.ContractorCommodityCode).HasMaxLength(50);
            entity.Property(e => e.FabricationCategoryOverride).HasMaxLength(100);
            entity.Property(e => e.MaximumTemperature).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PipingNote1).HasMaxLength(255);
            entity.Property(e => e.PressureRating).HasMaxLength(50);
            entity.Property(e => e.SpecId).HasColumnName("SpecID");
            entity.Property(e => e.SupplWasherCntrCommodityCode).HasMaxLength(50);
            entity.Property(e => e.SupplementaryWasherReqmt).HasMaxLength(100);
            entity.Property(e => e.SupplyResponsibilityOverride).HasMaxLength(100);
            entity.Property(e => e.WasherOption).HasMaxLength(50);

            entity.HasOne(d => d.Spec).WithMany(p => p.S3dRuleWasherSelectionFilters)
                .HasForeignKey(d => d.SpecId)
                .HasConstraintName("FK_WasherSpecification_Spec");
        });

        modelBuilder.Entity<S3dRuleWeldClearanceRule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC2707624681");

            entity.ToTable("S3D_Rule_WeldClearanceRule");

            entity.HasIndex(e => new { e.WeldClass, e.NominalPipingDiameterUnits, e.NominalPipingDiameterTo, e.NominalPipingDiameterFrom, e.SpecId }, "DSP_WeldClearanceRule_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.NominalPipingDiameterFrom).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.NominalPipingDiameterTo).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.NominalPipingDiameterUnits).HasMaxLength(5);
            entity.Property(e => e.SpecId).HasColumnName("SpecID");
            entity.Property(e => e.WeldClass).HasMaxLength(50);
            entity.Property(e => e.WeldClearanceLength).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.WeldClearanceRadiusIncrease).HasColumnType("decimal(8, 3)");

            entity.HasOne(d => d.Spec).WithMany(p => p.S3dRuleWeldClearanceRules)
                .HasForeignKey(d => d.SpecId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WeldClearance_SpecData");
        });

        modelBuilder.Entity<S3dRuleWeldModelRepresentationRule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC273D0155B0");

            entity.ToTable("S3D_Rule_WeldModelRepresentationRule");

            entity.HasIndex(e => new { e.NominalPipingDiameterFrom, e.NominalPipingDiameterTo, e.NominalPipingDiameterUnits, e.WeldClass }, "DSP_WeldModelRepresentationRule_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaterialsGrade).HasMaxLength(50);
            entity.Property(e => e.NominalPipingDiameterFrom).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.NominalPipingDiameterTo).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.NominalPipingDiameterUnits).HasMaxLength(5);
            entity.Property(e => e.WeldClass).HasMaxLength(50);
            entity.Property(e => e.WeldRadiusIncrease).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.WeldThickness).HasColumnType("decimal(8, 3)");
        });

        modelBuilder.Entity<S3dRuleWeldTypeRule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27A386B10B");

            entity.ToTable("S3D_Rule_WeldTypeRule");

            entity.HasIndex(e => new { e.FabricationTypeOfEnd1, e.ConstructionRequirementOfEnd1, e.FabricationTypeOfEnd2, e.ConstructionRequirementOfEnd2 }, "DSP_WeldTypeRule_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ConstructionRequirementOfEnd1).HasMaxLength(50);
            entity.Property(e => e.ConstructionRequirementOfEnd2).HasMaxLength(50);
            entity.Property(e => e.FabricationTypeOfEnd1).HasMaxLength(50);
            entity.Property(e => e.FabricationTypeOfEnd2).HasMaxLength(50);
            entity.Property(e => e.WeldType).HasMaxLength(50);
        });

        modelBuilder.Entity<VwFlangeStandPressureRating>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VW_FlangeStand_PressureRating");

            entity.Property(e => e.FlangeStandDesc).HasMaxLength(255);
            entity.Property(e => e.FlangeStandardCode).HasMaxLength(100);
            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.PressureRatingCl).HasColumnName("PressureRating_CL");
            entity.Property(e => e.PressureRatingCode).HasMaxLength(100);
            entity.Property(e => e.PressureRatingDesc).HasMaxLength(255);
        });

        modelBuilder.Entity<VwMaterialsCategoryPipingStandard>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VW_MaterialsCategory_PipingStandard");

            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.MaterialsCategoryCl).HasColumnName("MaterialsCategory_CL");
            entity.Property(e => e.MaterialsCategoryCode).HasMaxLength(100);
            entity.Property(e => e.MaterialsCategoryDesc).HasMaxLength(255);
            entity.Property(e => e.PipeStandDesc).HasMaxLength(255);
            entity.Property(e => e.PipingStandardCode).HasMaxLength(100);
        });

        modelBuilder.Entity<VwPipingStandardMaterialsGrade>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VW_PipingStandard_MaterialsGrade");

            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.MaterialsGradeCl).HasColumnName("MaterialsGrade_CL");
            entity.Property(e => e.MaterialsGradeCode).HasMaxLength(100);
            entity.Property(e => e.MaterialsGradeDesc).HasMaxLength(255);
            entity.Property(e => e.PipeStandDesc).HasMaxLength(255);
            entity.Property(e => e.PipingStandardCode).HasMaxLength(100);
        });

        modelBuilder.Entity<VwPipingStandardPressureRating>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VW_PipingStandard_PressureRating");

            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.PipingStandardCode).HasMaxLength(100);
            entity.Property(e => e.PipingStandardDesc).HasMaxLength(255);
            entity.Property(e => e.PressureRatingCl).HasColumnName("PressureRating_CL");
            entity.Property(e => e.PressureRatingCode).HasMaxLength(100);
            entity.Property(e => e.PressureRatingDesc).HasMaxLength(255);
        });

        modelBuilder.Entity<VwPipingStandardScheduleThickness>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VW_PipingStandard_ScheduleThickness");

            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.PipeStandDesc).HasMaxLength(255);
            entity.Property(e => e.PipingStandardCode).HasMaxLength(100);
            entity.Property(e => e.ScheduleThicknessCl).HasColumnName("ScheduleThickness_CL");
            entity.Property(e => e.ScheduleThicknessCode).HasMaxLength(100);
            entity.Property(e => e.ScheduleThicknessDesc).HasMaxLength(255);
        });

        modelBuilder.Entity<VwS3dRuleAb2b3c2WithCode>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VW_S3D_Rule_AB2B3C2_WithCodes");

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

        modelBuilder.Entity<VwS3dRuleB1b2b3dWithCode>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VW_S3D_Rule_B1B2B3D_WithCodes");

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

        modelBuilder.Entity<VwS3dRuleC1c2WithCode>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VW_S3D_Rule_C1C2_WithCodes");

            entity.Property(e => e.FlangeStandardCode).HasMaxLength(100);
            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PressureRatingCl).HasColumnName("PressureRating_CL");
            entity.Property(e => e.PressureRatingCode).HasMaxLength(100);
            entity.Property(e => e.RuleName).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
