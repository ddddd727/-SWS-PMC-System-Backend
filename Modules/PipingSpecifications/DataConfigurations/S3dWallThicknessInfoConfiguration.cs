using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMCSystem_Backend.Modules.PipingSpecifications.Entities;

namespace PMCSystem_Backend.Modules.PipingSpecifications.DataConfigurations;

/// <summary>
/// S3D_WallThickness_Info 视图配置
/// </summary>
public class S3dWallThicknessInfoConfiguration : IEntityTypeConfiguration<S3dWallThicknessInfo>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<S3dWallThicknessInfo> entity)
    {
        entity.HasNoKey().ToView("S3D_WallThickness_Info");
        entity.Property(e => e.GeometricIndustryStandard).HasMaxLength(255);
        entity.Property(e => e.GeometricIndustryStandardCl).HasColumnName("GeometricIndustryStandard_CL");
        entity.Property(e => e.Id).HasColumnName("ID");
        entity.Property(e => e.NormalDiameter).HasColumnType("float");
        entity.Property(e => e.PipingOutsideDiameter).HasColumnType("float");
        entity.Property(e => e.ScheduleThickness).HasMaxLength(255);
        entity.Property(e => e.ScheduleThicknessCl).HasColumnName("ScheduleThickness_CL");
        entity.Property(e => e.UnitType).HasMaxLength(100);
        entity.Property(e => e.Version).HasMaxLength(100);
        entity.Property(e => e.WallThickness).HasColumnType("float");
    }
}
