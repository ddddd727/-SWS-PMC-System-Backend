using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMCSystem_Backend.Modules.DesignRules.Entities;

namespace PMCSystem_Backend.Modules.DesignRules.DataConfigurations;

/// <summary>
/// S3D_Rule_PipingBendParameter 表配置
/// </summary>
public class S3dRulePipingBendParameterConfiguration : IEntityTypeConfiguration<S3dRulePipingBendParameter>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<S3dRulePipingBendParameter> entity)
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
    }
}
