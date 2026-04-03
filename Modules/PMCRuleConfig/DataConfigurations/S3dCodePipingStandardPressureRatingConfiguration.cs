using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMCSystem_Backend.Modules.PMCRuleConfig.Entities;

namespace PMCSystem_Backend.Modules.PMCRuleConfig.DataConfigurations;

/// <summary>
/// S3D_Code_PipingStandardPressureRating 视图配置
/// </summary>
public class S3dCodePipingStandardPressureRatingConfiguration : IEntityTypeConfiguration<S3dCodePipingStandardPressureRating>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<S3dCodePipingStandardPressureRating> entity)
    {
        entity.HasNoKey().ToView("S3D_Code_PipingStandardPressureRating", "dbo");
        entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
        entity.Property(e => e.PipingStandardCode).HasMaxLength(100);
        entity.Property(e => e.PipingStandardDesc).HasMaxLength(255);
        entity.Property(e => e.PressureRatingCl).HasColumnName("PressureRating_CL");
        entity.Property(e => e.PressureRatingCode).HasMaxLength(100);
        entity.Property(e => e.PressureRatingDesc).HasMaxLength(255);
    }
}
