using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMCSystem_Backend.Modules.PMCRuleConfig.Entities;

namespace PMCSystem_Backend.Modules.PMCRuleConfig.DataConfigurations;

/// <summary>
/// S3D_Rule_C1C2 表配置
/// </summary>
public class S3dRuleC1c2Configuration : IEntityTypeConfiguration<S3dRuleC1c2>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<S3dRuleC1c2> entity)
    {
        entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27C1758066");
        entity.ToTable("S3D_Rule_C1C2");
        entity.HasIndex(e => new { e.GeometricIndustryStandardCl, e.PressureRatingCl, e.RuleName }, "UQ_SPMC_C1C2").IsUnique();
        entity.Property(e => e.Id).HasColumnName("ID");
        entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
        entity.Property(e => e.PressureRatingCl).HasColumnName("PressureRating_CL");
        entity.Property(e => e.RuleName).HasMaxLength(255);
    }
}
