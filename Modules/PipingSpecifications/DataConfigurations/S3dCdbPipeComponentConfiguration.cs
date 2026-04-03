using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMCSystem_Backend.Modules.PipingSpecifications.Entities;

namespace PMCSystem_Backend.Modules.PipingSpecifications.DataConfigurations;

/// <summary>
/// S3D_CDB_PipeComponent 表配置
/// </summary>
public class S3dCdbPipeComponentConfiguration : IEntityTypeConfiguration<S3dCdbPipeComponent>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<S3dCdbPipeComponent> entity)
    {
        entity.HasKey(e => e.Id).HasName("PK__S3D_CDB___3214EC27D0D17D28");
        entity.ToTable("S3D_CDB_PipeComponent");
        entity.HasIndex(e => e.IndustryCommodityCode, "UQ__S3D_CDB___782B02A1FEF5916B").IsUnique();
        entity.Property(e => e.Id).HasColumnName("ID");
        entity.Property(e => e.BendAngle).HasColumnType("decimal(10, 3)");
        entity.Property(e => e.CommodityType).HasMaxLength(255);
        entity.Property(e => e.GeometricIndustryStandard).HasMaxLength(255);
        entity.Property(e => e.GeometryType).HasMaxLength(255);
        entity.Property(e => e.IndustryCommodityCode).HasMaxLength(255);
        entity.Property(e => e.MaterialGrade).HasMaxLength(255);
        entity.Property(e => e.PartClassName).HasMaxLength(255);
        entity.Property(e => e.PartDataBasis).HasMaxLength(100);
    }
}
