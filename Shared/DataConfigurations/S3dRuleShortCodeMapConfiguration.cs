using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMCSystem_Backend.Shared.Entities;

namespace PMCSystem_Backend.Shared.DataConfigurations;

/// <summary>
/// S3D_Rule_ShortCodeMap 表配置
/// </summary>
public class S3dRuleShortCodeMapConfiguration : IEntityTypeConfiguration<S3dRuleShortCodeMap>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<S3dRuleShortCodeMap> entity)
    {
        entity.HasKey(e => e.Id);
        entity.ToTable("S3D_Rule_ShortCodeMap");
        entity.HasIndex(e => new { e.ComponentTypeId, e.ShortCode }, "UQ_ShortCodeMap_ID_Code").IsUnique();
        entity.Property(e => e.Id).HasColumnName("ID");
        entity.Property(e => e.ComponentTypeId).HasColumnName("ComponentTypeID");
        entity.Property(e => e.ShortCode).HasMaxLength(50);
    }
}
