using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMCSystem_Backend.Modules.PMCRuleConfig.Entities;

namespace PMCSystem_Backend.Modules.PMCRuleConfig.DataConfigurations;

/// <summary>
/// S3D_Rule_B1B2B3D 表配置
/// </summary>
public class S3dRuleB1b2b3dConfiguration : IEntityTypeConfiguration<S3dRuleB1b2b3d>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<S3dRuleB1b2b3d> entity)
    {
        entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC2775C8CBEA");
        entity.ToTable("S3D_Rule_B1B2B3D");
        entity.HasIndex(e => new { e.MaterialsCategoryCl, e.GeometricIndustryStandardCl, e.MaterialsGradeCl, e.ScheduleThicknessCl, e.RuleName }, "UQ_SPMC_B1B2B3D").IsUnique();
        entity.Property(e => e.Id).HasColumnName("ID");
        entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
        entity.Property(e => e.MaterialsCategoryCl).HasColumnName("MaterialsCategory_CL");
        entity.Property(e => e.MaterialsGradeCl).HasColumnName("MaterialsGrade_CL");
        entity.Property(e => e.RuleName).HasMaxLength(255);
        entity.Property(e => e.ScheduleThicknessCl).HasColumnName("ScheduleThickness_CL");
        entity.Property(e => e.Status).HasDefaultValue(true);
    }
}
