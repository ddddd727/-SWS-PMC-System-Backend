using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMCSystem_Backend.Modules.PMCRuleConfig.Entities;

namespace PMCSystem_Backend.Modules.PMCRuleConfig.DataConfigurations;

/// <summary>
/// S3D_Code_FlangeStandPressureRating 视图配置
/// </summary>
public class S3dCodeFlangeStandPressureRatingConfiguration : IEntityTypeConfiguration<S3dCodeFlangeStandPressureRating>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<S3dCodeFlangeStandPressureRating> entity)
    {
        entity.HasNoKey().ToView("S3D_Code_FlangeStandPressureRating", "dbo");
        entity.Property(e => e.FlangeStandDesc).HasMaxLength(255);
        entity.Property(e => e.FlangeStandardCode).HasMaxLength(100);
        entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
        entity.Property(e => e.PressureRatingCl).HasColumnName("PressureRating_CL");
        entity.Property(e => e.PressureRatingCode).HasMaxLength(100);
        entity.Property(e => e.PressureRatingDesc).HasMaxLength(255);
    }
}
