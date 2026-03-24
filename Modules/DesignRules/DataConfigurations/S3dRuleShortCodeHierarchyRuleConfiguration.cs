using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMCSystem_Backend.Modules.DesignRules.Entities;

namespace PMCSystem_Backend.Modules.DesignRules.DataConfigurations;

/// <summary>
/// S3D_Rule_ShortCodeHierarchyRule 表配置
/// </summary>
public class S3dRuleShortCodeHierarchyRuleConfiguration : IEntityTypeConfiguration<S3dRuleShortCodeHierarchyRule>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<S3dRuleShortCodeHierarchyRule> entity)
    {
        entity.HasKey(e => e.Id).HasName("DSP_ShortCodeHierarchyRule_PK");
        entity.ToTable("S3D_Rule_ShortCodeHierarchyRule");
        entity.HasIndex(e => new { e.ShortCodeHierarchyType, e.ShortCode }, "DSP_ShortCodeHierarchyRule_UNIQUE").IsUnique();
        entity.Property(e => e.Id).HasColumnName("ID");
        entity.Property(e => e.ShortCode).HasMaxLength(50);
        entity.Property(e => e.ShortCodeHierarchyType).HasMaxLength(100);
    }
}
