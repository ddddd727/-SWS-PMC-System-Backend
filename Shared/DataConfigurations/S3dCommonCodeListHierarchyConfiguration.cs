using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMCSystem_Backend.Shared.Entities;

namespace PMCSystem_Backend.Shared.DataConfigurations;

/// <summary>
/// S3D_Common_CodeListHierarchy 表配置
/// </summary>
public class S3dCommonCodeListHierarchyConfiguration : IEntityTypeConfiguration<S3dCommonCodeListHierarchy>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<S3dCommonCodeListHierarchy> entity)
    {
        entity.HasKey(e => e.Id).HasName("DSP_CodeListHierarchy_PK");
        entity.ToTable("S3D_Common_CodeListHierarchy");
        entity.HasIndex(e => e.CodeListTableId, "DSP_CodeListHierarchy_UNIQUE").IsUnique();
        entity.Property(e => e.Id).HasColumnName("ID");
        entity.Property(e => e.CodeListTableId).HasColumnName("CodeListTableID");
        entity.Property(e => e.ParentCodeListTableId).HasColumnName("ParentCodeListTableID");
    }
}
