using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMCSystem_Backend.Shared.Entities;

namespace PMCSystem_Backend.Shared.DataConfigurations;

/// <summary>
/// S3D_Common_CodeListValue 表配置
/// </summary>
public class S3dCommonCodeListValueConfiguration : IEntityTypeConfiguration<S3dCommonCodeListValue>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<S3dCommonCodeListValue> entity)
    {
        entity.HasKey(e => e.Id).HasName("PK__UD_CodeL__0382013A28D8123E");
        entity.ToTable("S3D_Common_CodeListValue");
        entity.HasIndex(e => new { e.CodeListTableId, e.CodeListNumber }, "UQ__UD_CodeL__31A75F5EA32D472E").IsUnique();
        entity.Property(e => e.Id).HasColumnName("ID");
        entity.Property(e => e.CodeListTableId).HasColumnName("CodeListTableID");
        entity.Property(e => e.IsUserDefine).HasDefaultValue(true);
        entity.Property(e => e.LongStringValue).HasMaxLength(255);
        entity.Property(e => e.ShortStringValue).HasMaxLength(255);
        entity.Property(e => e.Status).HasDefaultValue(true);
        entity.HasOne(d => d.CodeListTable).WithMany(p => p.S3dCommonCodeListValues)
            .HasForeignKey(d => d.CodeListTableId)
            .HasConstraintName("FK__UD_CodeLi__CodeL__3B75D760");
    }
}
