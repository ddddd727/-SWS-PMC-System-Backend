using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMCSystem_Backend.Modules.PMCRuleConfig.Entities;

namespace PMCSystem_Backend.Modules.PMCRuleConfig.DataConfigurations;

/// <summary>
/// S3D_Code_MaterialsCategoryPipingStandard 视图配置
/// </summary>
public class S3dCodeMaterialsCategoryPipingStandardConfiguration : IEntityTypeConfiguration<S3dCodeMaterialsCategoryPipingStandard>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<S3dCodeMaterialsCategoryPipingStandard> entity)
    {
        entity.HasNoKey().ToView("S3D_Code_MaterialsCategoryPipingStandard", "dbo");
        entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
        entity.Property(e => e.MaterialsCategoryCl).HasColumnName("MaterialsCategory_CL");
        entity.Property(e => e.MaterialsCategoryCode).HasMaxLength(100);
        entity.Property(e => e.MaterialsCategoryDesc).HasMaxLength(255);
        entity.Property(e => e.PipeStandDesc).HasMaxLength(255);
        entity.Property(e => e.PipingStandardCode).HasMaxLength(100);
    }
}
