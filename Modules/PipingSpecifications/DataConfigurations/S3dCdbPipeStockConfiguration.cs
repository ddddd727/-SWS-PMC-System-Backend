using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMCSystem_Backend.Modules.PipingSpecifications.Entities;

namespace PMCSystem_Backend.Modules.PipingSpecifications.DataConfigurations;

/// <summary>
/// S3D_CDB_PipeStock 表配置
/// </summary>
public class S3dCdbPipeStockConfiguration : IEntityTypeConfiguration<S3dCdbPipeStock>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<S3dCdbPipeStock> entity)
    {
        entity.HasKey(e => e.Id).HasName("PK__S3D_CDB___3214EC275F1C56C7");
        entity.ToTable("S3D_CDB_PipeStock");
        entity.HasIndex(e => e.IndustryCommodityCode, "UQ__S3D_CDB___782B02A18C6083DE").IsUnique();
        entity.Property(e => e.Id).HasColumnName("ID");
        entity.Property(e => e.CommodityType).HasMaxLength(100);
        entity.Property(e => e.Density).HasColumnType("decimal(10, 3)");
        entity.Property(e => e.GeometricIndustryStandard).HasMaxLength(255);
        entity.Property(e => e.IndustryCommodityCode).HasMaxLength(100);
        entity.Property(e => e.MaterialGrade).HasMaxLength(100);
        entity.Property(e => e.MaximumPipeLength).HasColumnType("decimal(10, 3)");
        entity.Property(e => e.MinimumPipeLength).HasColumnType("decimal(10, 3)");
        entity.Property(e => e.PurchaseLength).HasColumnType("decimal(10, 3)");
        entity.Property(e => e.WeightPerUnitLength).HasColumnType("decimal(10, 3)");
    }
}
