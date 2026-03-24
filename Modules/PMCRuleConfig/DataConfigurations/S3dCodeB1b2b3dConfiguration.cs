using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMCSystem_Backend.Modules.PMCRuleConfig.Entities;

namespace PMCSystem_Backend.Modules.PMCRuleConfig.DataConfigurations;

/// <summary>
/// S3D_Code_B1B2B3D 视图配置
/// </summary>
public class S3dCodeB1b2b3dConfiguration : IEntityTypeConfiguration<S3dCodeB1b2b3d>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<S3dCodeB1b2b3d> entity)
    {
        entity.HasNoKey().ToView("S3D_Code_B1B2B3D", "dbo");
        entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
        entity.Property(e => e.Id).HasColumnName("ID");
        entity.Property(e => e.MaterialsCategoryCl).HasColumnName("MaterialsCategory_CL");
        entity.Property(e => e.MaterialsCategoryCode).HasMaxLength(100);
        entity.Property(e => e.MaterialsGradeCl).HasColumnName("MaterialsGrade_CL");
        entity.Property(e => e.MaterialsGradeCode).HasMaxLength(100);
        entity.Property(e => e.PipingStandardCode).HasMaxLength(100);
        entity.Property(e => e.RuleName).HasMaxLength(255);
        entity.Property(e => e.ScheduleThicknessCl).HasColumnName("ScheduleThickness_CL");
        entity.Property(e => e.ScheduleThicknessCode).HasMaxLength(100);
        entity.Property(e => e.Status);
    }
}
