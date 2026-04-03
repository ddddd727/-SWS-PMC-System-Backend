using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMCSystem_Backend.Modules.PipingSpecifications.Entities;

namespace PMCSystem_Backend.Modules.PipingSpecifications.DataConfigurations;

/// <summary>
/// S3D_Rule_ComponentTypeHierarchyRule 表配置
/// </summary>
public class S3dRuleComponentTypeHierarchyRuleConfiguration : IEntityTypeConfiguration<S3dRuleComponentTypeHierarchyRule>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<S3dRuleComponentTypeHierarchyRule> entity)
    {
        entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC2773507ACE");
        entity.ToTable("S3D_Rule_ComponentTypeHierarchyRule");
        entity.HasIndex(e => new { e.ComponentTypeId, e.PipingCommoditySubClassCl }, "UQ_PipingComponentType_ID_SubClass").IsUnique();
        entity.Property(e => e.Id).HasColumnName("ID");
        entity.Property(e => e.ComponentTypeId).HasColumnName("ComponentTypeID");
        entity.Property(e => e.PipingCommoditySubClassCl).HasColumnName("PipingCommoditySubClass_CL");
        entity.Property(e => e.Status).HasDefaultValue(true);
    }
}
