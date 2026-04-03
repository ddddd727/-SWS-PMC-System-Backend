using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMCSystem_Backend.Modules.PipingSpecifications.Entities;

namespace PMCSystem_Backend.Modules.PipingSpecifications.DataConfigurations;

/// <summary>
/// S3D_CL_MaterialsGrade 视图配置
/// </summary>
public class S3dClMaterialsGradeConfiguration : IEntityTypeConfiguration<S3dClMaterialsGrade>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<S3dClMaterialsGrade> entity)
    {
        entity.HasNoKey().ToView("S3D_CL_MaterialsGrade");
        entity.Property(e => e.CodeListNumber);
        entity.Property(e => e.ShortStringValue).HasMaxLength(255);
        entity.Property(e => e.LongStringValue).HasMaxLength(255);
    }
}
