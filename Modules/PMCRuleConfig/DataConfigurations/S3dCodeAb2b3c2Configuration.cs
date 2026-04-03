using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMCSystem_Backend.Modules.PMCRuleConfig.Entities;

namespace PMCSystem_Backend.Modules.PMCRuleConfig.DataConfigurations;

/// <summary>
/// S3D_Code_AB2B3C2 视图配置
/// </summary>
public class S3dCodeAb2b3c2Configuration : IEntityTypeConfiguration<S3dCodeAb2b3c2>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<S3dCodeAb2b3c2> entity)
    {
        entity.HasNoKey().ToView("S3D_Code_AB2B3C2", "dbo");
        entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
        entity.Property(e => e.Id).HasColumnName("ID");
        entity.Property(e => e.MaterialsGradeCl).HasColumnName("MaterialsGrade_CL");
        entity.Property(e => e.MaterialsGradeCode).HasMaxLength(100);
        entity.Property(e => e.PipingClassCl).HasColumnName("PipingClass_CL");
        entity.Property(e => e.PipingClassCode).HasMaxLength(100);
        entity.Property(e => e.PipingStandardCode).HasMaxLength(100);
        entity.Property(e => e.PressureRatingCl).HasColumnName("PressureRating_CL");
        entity.Property(e => e.PressureRatingCode).HasMaxLength(100);
        entity.Property(e => e.RuleName).HasMaxLength(255);
        entity.Property(e => e.Status);
    }
}
