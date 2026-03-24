using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMCSystem_Backend.Modules.PMCRuleConfig.Entities;

namespace PMCSystem_Backend.Modules.PMCRuleConfig.DataConfigurations;

/// <summary>
/// S3D_Code_PipingClass 视图配置
/// </summary>
public class S3dCodePipingClassConfiguration : IEntityTypeConfiguration<S3dCodePipingClass>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<S3dCodePipingClass> entity)
    {
        entity.HasNoKey().ToView("S3D_Code_PipingClass", "dbo");
        entity.Property(e => e.Id).HasColumnName("ID");
        entity.Property(e => e.PipingClassCode).HasMaxLength(100);
        entity.Property(e => e.ShortStringValue).HasMaxLength(255);
    }
}
