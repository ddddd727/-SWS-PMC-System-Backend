using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMCSystem_Backend.Modules.PMCRuleConfig.Entities;

namespace PMCSystem_Backend.Modules.PMCRuleConfig.DataConfigurations;

/// <summary>
/// S3D_Code_PipingStandardMaterialsGrade 视图配置
/// </summary>
public class S3dCodePipingStandardMaterialsGradeConfiguration : IEntityTypeConfiguration<S3dCodePipingStandardMaterialsGrade>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<S3dCodePipingStandardMaterialsGrade> entity)
    {
        entity.HasNoKey().ToView("S3D_Code_PipingStandardMaterialsGrade", "dbo");
        entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
        entity.Property(e => e.MaterialsGradeCl).HasColumnName("MaterialsGrade_CL");
        entity.Property(e => e.MaterialsGradeCode).HasMaxLength(100);
        entity.Property(e => e.MaterialsGradeDesc).HasMaxLength(255);
        entity.Property(e => e.PipeStandDesc).HasMaxLength(255);
        entity.Property(e => e.PipingStandardCode).HasMaxLength(100);
    }
}
