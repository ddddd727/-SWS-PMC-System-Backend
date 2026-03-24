using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMCSystem_Backend.Modules.DesignRules.Entities;

namespace PMCSystem_Backend.Modules.DesignRules.DataConfigurations;

/// <summary>
/// S3D_Code_ShortCodeMap 视图配置
/// </summary>
public class S3dCodeShortCodeMapConfiguration : IEntityTypeConfiguration<S3dCodeShortCodeMap>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<S3dCodeShortCodeMap> entity)
    {
        entity.HasNoKey().ToView("S3D_Code_ShortCodeMap");
        entity.Property(e => e.Id).HasColumnName("ID");
        entity.Property(e => e.ComponentTypeId).HasColumnName("ComponentTypeID");
        entity.Property(e => e.ComponentTypeName).HasMaxLength(255);
        entity.Property(e => e.ShortCode).HasMaxLength(50);
    }
}
