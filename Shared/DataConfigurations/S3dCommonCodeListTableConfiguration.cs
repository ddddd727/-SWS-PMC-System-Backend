using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMCSystem_Backend.Shared.Entities;

namespace PMCSystem_Backend.Shared.DataConfigurations;

/// <summary>
/// S3D_Common_CodeListTable 表配置
/// </summary>
public class S3dCommonCodeListTableConfiguration : IEntityTypeConfiguration<S3dCommonCodeListTable>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<S3dCommonCodeListTable> entity)
    {
        entity.HasKey(e => e.Id).HasName("PK__UD_CodeL__3214EC27484123D6");
        entity.ToTable("S3D_Common_CodeListTable");
        entity.HasIndex(e => e.Id, "UQ__UD_CodeL__3214EC26E7A97EC1").IsUnique();
        entity.HasIndex(e => e.CodeListTableName, "UQ__UD_CodeL__87753419918C0611").IsUnique();
        entity.Property(e => e.Id).HasColumnName("ID");
        entity.Property(e => e.CodeListTableName).HasMaxLength(255);
        entity.Property(e => e.Major).HasMaxLength(10).HasDefaultValue("C");
    }
}
