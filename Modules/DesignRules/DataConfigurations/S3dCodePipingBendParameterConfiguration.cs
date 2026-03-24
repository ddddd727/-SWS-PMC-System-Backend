using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMCSystem_Backend.Modules.DesignRules.Entities;

namespace PMCSystem_Backend.Modules.DesignRules.DataConfigurations;

/// <summary>
/// S3D_Code_PipingBendParameter 视图配置
/// </summary>
public class S3dCodePipingBendParameterConfiguration : IEntityTypeConfiguration<S3dCodePipingBendParameter>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<S3dCodePipingBendParameter> entity)
    {
        entity.HasNoKey().ToView("S3D_Code_PipingBendParameter");
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
    }
}
