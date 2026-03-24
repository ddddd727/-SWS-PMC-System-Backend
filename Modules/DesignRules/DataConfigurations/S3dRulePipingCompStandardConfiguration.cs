using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMCSystem_Backend.Modules.PipingSpecifications.Entities;

namespace PMCSystem_Backend.Modules.DesignRules.DataConfigurations;

/// <summary>
/// S3D_Rule_PipingCompStandard 表配置
/// </summary>
public class S3dRulePipingCompStandardConfiguration : IEntityTypeConfiguration<S3dRulePipingCompStandard>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<S3dRulePipingCompStandard> entity)
    {
        entity.HasKey(e => e.Id);
        entity.ToTable("S3D_Rule_PipingCompStandard");
        entity.Property(e => e.Id).HasColumnName("ID");
        entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
        entity.Property(e => e.ComponentTypeId).HasColumnName("ComponentTypeID");
        entity.Property(e => e.MaterialsCategoryCl).HasColumnName("MaterialsCategory_CL");
        entity.Property(e => e.JsonData).HasColumnName("JsonData");
    }
}
