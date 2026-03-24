using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMCSystem_Backend.Modules.PMCRuleConfig.Entities;

namespace PMCSystem_Backend.Modules.PMCRuleConfig.DataConfigurations;

/// <summary>
/// S3D_Rule_AB2B3C2 表配置
/// </summary>
public class S3dRuleAb2b3c2Configuration : IEntityTypeConfiguration<S3dRuleAb2b3c2>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<S3dRuleAb2b3c2> entity)
    {
        entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27605C7BC0");
        entity.ToTable("S3D_Rule_AB2B3C2");
        entity.HasIndex(e => new { e.PipingClassCl, e.GeometricIndustryStandardCl, e.MaterialsGradeCl, e.PressureRatingCl, e.RuleName }, "UQ_SPMC_AB2B3C2").IsUnique();
        entity.Property(e => e.Id).HasColumnName("ID");
        entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
        entity.Property(e => e.MaterialsGradeCl).HasColumnName("MaterialsGrade_CL");
        entity.Property(e => e.PipingClassCl).HasColumnName("PipingClass_CL");
        entity.Property(e => e.PressureRatingCl).HasColumnName("PressureRating_CL");
        entity.Property(e => e.RuleName).HasMaxLength(255);
        entity.Property(e => e.Status).HasDefaultValue(true);
    }
}
