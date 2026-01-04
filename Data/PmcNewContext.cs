using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Entities;

namespace PMCSystem_Backend.Data;

public partial class PmcNewContext : DbContext
{
    public PmcNewContext()
    {
    }

    public PmcNewContext(DbContextOptions<PmcNewContext> options)
        : base(options)
    {
    }

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

    public virtual DbSet<S3dDictMaterialsCategory> S3dDictMaterialsCategories { get; set; }

    public virtual DbSet<S3dDictMaterialsGrade> S3dDictMaterialsGrades { get; set; }

    public virtual DbSet<S3dDictPipingBend> S3dDictPipingBends { get; set; }

    public virtual DbSet<S3dDictPipingClass> S3dDictPipingClasses { get; set; }

    public virtual DbSet<S3dDictPipingComponentType> S3dDictPipingComponentTypes { get; set; }

    public virtual DbSet<S3dDictPipingComponentTypeHierarchy> S3dDictPipingComponentTypeHierarchies { get; set; }

    public virtual DbSet<S3dDictPipingStandard> S3dDictPipingStandards { get; set; }

    public virtual DbSet<S3dDictPressureRating> S3dDictPressureRatings { get; set; }

    public virtual DbSet<S3dDictScheduleThickness> S3dDictScheduleThicknesses { get; set; }

    public virtual DbSet<S3dDictWallThickness> S3dDictWallThicknesses { get; set; }

    public virtual DbSet<S3dRuleAb2b3c2> S3dRuleAb2b3c2s { get; set; }

    public virtual DbSet<S3dRuleB1b2b3d> S3dRuleB1b2b3ds { get; set; }

    public virtual DbSet<S3dRuleC1c2> S3dRuleC1c2s { get; set; }

    public virtual DbSet<S3dRuleMaterialsGrade> S3dRuleMaterialsGrades { get; set; }

    public virtual DbSet<S3dRulePipingCommodityMatlControlDatum> S3dRulePipingCommodityMatlControlData { get; set; }

    public virtual DbSet<S3dRulePipingCompStandard> S3dRulePipingCompStandards { get; set; }

    public virtual DbSet<S3dRulePipingMaterialsClassDatum> S3dRulePipingMaterialsClassData { get; set; }

    public virtual DbSet<S3dRulePmcdata> S3dRulePmcdata { get; set; }

    public virtual DbSet<S3dRulePressureRating> S3dRulePressureRatings { get; set; }

    public virtual DbSet<S3dRuleShortCodeHierarchyRule> S3dRuleShortCodeHierarchyRules { get; set; }

    public virtual DbSet<S3dRuleValveOperatorMatlControlDatum> S3dRuleValveOperatorMatlControlData { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=PMC_NEW;Trusted_Connection=True;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
            entity.Property(e => e.ParentNumberId).HasColumnName("ParentNumberID");
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
            entity.HasKey(e => e.Id).HasName("PK__S3D_Comm__3214EC27F148ABFA");

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
            entity
                .HasNoKey()
                .ToTable("S3D_Dict_FlangeStandard");

            entity.Property(e => e.FlangeStandardCode).HasMaxLength(100);
            entity.Property(e => e.GeometricIndustryPracticeCl).HasColumnName("GeometricIndustryPractice_CL");
            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.Id).HasColumnName("ID");
        });

        modelBuilder.Entity<S3dDictGeometricIndustryPractice>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("S3D_Dict_GeometricIndustryPractice");

            entity.Property(e => e.GeometricIndustryPracticeCl).HasColumnName("GeometricIndustryPractice_CL");
            entity.Property(e => e.Id).HasColumnName("ID");
        });

        modelBuilder.Entity<S3dDictMaterialsCategory>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("S3D_Dict_MaterialsCategory");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaterialsCategoryCl).HasColumnName("MaterialsCategory_CL");
            entity.Property(e => e.MaterialsCategoryCode).HasMaxLength(100);
        });

        modelBuilder.Entity<S3dDictMaterialsGrade>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("S3D_Dict_MaterialsGrade");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaterialsGradeCl).HasColumnName("MaterialsGrade_CL");
            entity.Property(e => e.MaterialsGradeCode).HasMaxLength(100);
        });

        modelBuilder.Entity<S3dDictPipingBend>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("S3D_Dict_PipingBend");

            entity.Property(e => e.HeaderClampLength).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.OutSideDiameter).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.OutSideDiameterUnit).HasMaxLength(100);
            entity.Property(e => e.TailClampLength).HasColumnType("decimal(10, 3)");
        });

        modelBuilder.Entity<S3dDictPipingClass>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("S3D_Dict_PipingClass");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PipingClassCl).HasColumnName("PipingClass_CL");
            entity.Property(e => e.PipingClassCode).HasMaxLength(100);
        });

        modelBuilder.Entity<S3dDictPipingComponentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DSP_SPMC_DICT_PipingComponentType_PK");

            entity.ToTable("S3D_Dict_PipingComponentType");

            entity.HasIndex(e => e.ComponentTypeName, "DSP_SPMC_DICT_PipingComponentType_UNIQUE").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.ComponentTypeDescription).HasMaxLength(255);
            entity.Property(e => e.ComponentTypeName).HasMaxLength(255);
        });

        modelBuilder.Entity<S3dDictPipingComponentTypeHierarchy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DSP_SPMC_DICT_PipingComponentTypeHierarchy_PK");

            entity.ToTable("S3D_Dict_PipingComponentTypeHierarchy");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ComponentTypeId).HasColumnName("ComponentTypeID");
            entity.Property(e => e.PipingCommoditySubClassCl).HasColumnName("PipingCommoditySubClass_CL");

            entity.HasOne(d => d.ComponentType).WithMany(p => p.S3dDictPipingComponentTypeHierarchies)
                .HasForeignKey(d => d.ComponentTypeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("DSP_SPMC_DICT_PipingComponentTypeHierarchy_DSP_SPMC_DICT_PipingComponentType_FK");
        });

        modelBuilder.Entity<S3dDictPipingStandard>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("S3D_Dict_PipingStandard");

            entity.Property(e => e.GeometricIndustryPracticeCl).HasColumnName("GeometricIndustryPractice_CL");
            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaterialsCategoryCl).HasColumnName("MaterialsCategory_CL");
            entity.Property(e => e.PipingStandardCode).HasMaxLength(100);
        });

        modelBuilder.Entity<S3dDictPressureRating>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("S3D_Dict_PressureRating");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PressureRatingCl).HasColumnName("PressureRating_CL");
            entity.Property(e => e.PressureRatingCode).HasMaxLength(100);
        });

        modelBuilder.Entity<S3dDictScheduleThickness>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("S3D_Dict_ScheduleThickness");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaterialsCategoryCl).HasColumnName("MaterialsCategory_CL");
            entity.Property(e => e.ScheduleThicknessCl).HasColumnName("ScheduleThickness_CL");
            entity.Property(e => e.ScheduleThicknessCode).HasMaxLength(100);
        });

        modelBuilder.Entity<S3dDictWallThickness>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("S3D_Dict_WallThickness");

            entity.Property(e => e.EndStandardCl).HasColumnName("EndStandard_CL");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Ndpunit)
                .HasMaxLength(100)
                .HasColumnName("NDPUnit");
            entity.Property(e => e.Npd)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("NPD");
            entity.Property(e => e.PipingOutsideDiameter).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.ScheduleThicknessCl).HasColumnName("ScheduleThickness_CL");
            entity.Property(e => e.WallThickness).HasColumnType("decimal(10, 3)");
        });

        modelBuilder.Entity<S3dRuleAb2b3c2>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DSP_SPMC_RULE_AB2B3C2_PK");

            entity.ToTable("S3D_Rule_AB2B3C2");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.MaterialsGradeId).HasColumnName("MaterialsGradeID");
            entity.Property(e => e.PipingClassId).HasColumnName("PipingClassID");
            entity.Property(e => e.PipingStandardId).HasColumnName("PipingStandardID");
            entity.Property(e => e.PressureRatingId).HasColumnName("PressureRatingID");
            entity.Property(e => e.RuleName).HasMaxLength(255);
        });

        modelBuilder.Entity<S3dRuleB1b2b3d>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DSP_SPMC_RULE_B1B2B3D_PK");

            entity.ToTable("S3D_Rule_B1B2B3D");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.MaterialsCategoryId)
                .HasMaxLength(255)
                .HasColumnName("MaterialsCategoryID");
            entity.Property(e => e.MaterialsGradeId).HasColumnName("MaterialsGradeID");
            entity.Property(e => e.PipingStandardId).HasColumnName("PipingStandardID");
            entity.Property(e => e.RuleName).HasMaxLength(255);
            entity.Property(e => e.ScheduleThicknessId).HasColumnName("ScheduleThicknessID");
        });

        modelBuilder.Entity<S3dRuleC1c2>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DSP_SPMC_RULE_C1C2_PK");

            entity.ToTable("S3D_Rule_C1C2");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.FlangeStandardId).HasColumnName("FlangeStandardID");
            entity.Property(e => e.PressureRatingId).HasColumnName("PressureRatingID");
            entity.Property(e => e.RuleName).HasMaxLength(255);
        });

        modelBuilder.Entity<S3dRuleMaterialsGrade>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("S3D_Rule_MaterialsGrade");

            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Manufacturer).HasMaxLength(255);
            entity.Property(e => e.MaterialsCategoryCl).HasColumnName("MaterialsCategory_CL");
            entity.Property(e => e.MaterialsGradeCl).HasColumnName("MaterialsGrade_CL");
        });

        modelBuilder.Entity<S3dRulePipingCommodityMatlControlDatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC272403F88D");

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
            entity
                .HasNoKey()
                .ToTable("S3D_Rule_PipingCompStandard");

            entity.Property(e => e.ComponentType).HasMaxLength(100);
            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaterialsCategoryCl).HasColumnName("MaterialsCategory_CL");
        });

        modelBuilder.Entity<S3dRulePipingMaterialsClassDatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27F45B0F0D");

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

        modelBuilder.Entity<S3dRulePmcdata>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("S3D_Rule_PMCData");

            entity.Property(e => e.AccessoriesStandard).HasMaxLength(500);
            entity.Property(e => e.BlindFlangeStandard).HasMaxLength(500);
            entity.Property(e => e.BoltStandard).HasMaxLength(500);
            entity.Property(e => e.BossesStandard).HasMaxLength(500);
            entity.Property(e => e.CapsStandard).HasMaxLength(500);
            entity.Property(e => e.ElbowStandard).HasMaxLength(500);
            entity.Property(e => e.FlangeStandard).HasMaxLength(500);
            entity.Property(e => e.GasketStandard).HasMaxLength(500);
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Material).HasMaxLength(255);
            entity.Property(e => e.NutStandard).HasMaxLength(500);
            entity.Property(e => e.OverpassStandard).HasMaxLength(500);
            entity.Property(e => e.PipeStandard).HasMaxLength(500);
            entity.Property(e => e.Pmccode)
                .HasMaxLength(255)
                .HasColumnName("PMCCode");
            entity.Property(e => e.RedStandard).HasMaxLength(500);
            entity.Property(e => e.SaddlesStandard).HasMaxLength(500);
            entity.Property(e => e.ShipNo).HasMaxLength(255);
            entity.Property(e => e.SleeveStandard).HasMaxLength(500);
            entity.Property(e => e.Status).HasMaxLength(100);
            entity.Property(e => e.TeeStandard).HasMaxLength(500);
            entity.Property(e => e.WasherStandard).HasMaxLength(500);
        });

        modelBuilder.Entity<S3dRulePressureRating>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("S3D_Rule_PressureRating");

            entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PressureRatingCl).HasColumnName("PressureRating_CL");
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

        modelBuilder.Entity<S3dRuleValveOperatorMatlControlDatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC279216CE22");

            entity.ToTable("S3D_Rule_ValveOperatorMatlControlData");

            entity.HasIndex(e => e.OperatorPartNumber, "DSP_ValveOperatorMatlControlData_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.LocalizedShortMaterialDescription).HasMaxLength(500);
            entity.Property(e => e.LongMaterialDescription).HasMaxLength(500);
            entity.Property(e => e.OperatorPartNumber).HasMaxLength(255);
            entity.Property(e => e.ShortMatlDescription).HasMaxLength(500);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
