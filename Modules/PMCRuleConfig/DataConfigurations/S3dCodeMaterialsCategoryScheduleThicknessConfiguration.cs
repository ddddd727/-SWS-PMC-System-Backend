using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMCSystem_Backend.Modules.PMCRuleConfig.Entities;

namespace PMCSystem_Backend.Modules.PMCRuleConfig.DataConfigurations;

/// <summary>
/// S3D_Code_MaterialsCategoryScheduleThickness 视图配置
/// </summary>
public class S3dCodeMaterialsCategoryScheduleThicknessConfiguration : IEntityTypeConfiguration<S3dCodeMaterialsCategoryScheduleThickness>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<S3dCodeMaterialsCategoryScheduleThickness> entity)
    {
        entity.HasNoKey().ToView("S3D_Code_MaterialsCategoryScheduleThickness", "dbo");
        entity.Property(e => e.MaterialsCategoryCl).HasColumnName("MaterialsCategory_CL");
        entity.Property(e => e.MaterialsCategoryCode).HasMaxLength(100);
        entity.Property(e => e.MaterialsCategoryDesc).HasMaxLength(255);
        entity.Property(e => e.ScheduleThicknessCl).HasColumnName("ScheduleThickness_CL");
        entity.Property(e => e.ScheduleThicknessCode).HasMaxLength(100);
        entity.Property(e => e.ScheduleThicknessDesc).HasMaxLength(255);
    }
}
