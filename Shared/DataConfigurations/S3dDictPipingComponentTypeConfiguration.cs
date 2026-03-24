using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMCSystem_Backend.Shared.Entities;

namespace PMCSystem_Backend.Shared.DataConfigurations;

/// <summary>
/// S3D_Dict_ComponentType 表配置
/// </summary>
public class S3dDictPipingComponentTypeConfiguration : IEntityTypeConfiguration<S3dDictPipingComponentType>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<S3dDictPipingComponentType> entity)
    {
        entity.HasKey(e => e.Id);
        entity.ToTable("S3D_Dict_ComponentType");
        entity.HasIndex(e => e.ComponentTypeName, "UQ_PipingComponentType_Name").IsUnique();
        entity.Property(e => e.Id).HasColumnName("ID");
        entity.Property(e => e.ComponentTypeName).HasMaxLength(255);
        entity.Property(e => e.ComponentTypeDescription).HasMaxLength(255);
        entity.Property(e => e.Status).HasDefaultValue(true);
        entity.Property(e => e.CreatedBy).HasMaxLength(100).HasDefaultValueSql("(suser_sname())");
        entity.Property(e => e.ModifiedBy).HasMaxLength(100);
        entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
        entity.Property(e => e.ModifiedDate).HasDefaultValueSql("(getdate())");
    }
}
