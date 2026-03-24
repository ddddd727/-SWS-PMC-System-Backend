using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMCSystem_Backend.Modules.DesignRules.Entities;

namespace PMCSystem_Backend.Modules.DesignRules.DataConfigurations;

/// <summary>
/// S3D_Code_PlainPipingGenericData 视图配置
/// </summary>
public class S3dCodePlainPipingGenericDataConfiguration : IEntityTypeConfiguration<S3dCodePlainPipingGenericData>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<S3dCodePlainPipingGenericData> entity)
    {
        entity.HasNoKey().ToView("S3D_Code_PlainPipingGenericData");
        entity.Property(e => e.Id).HasColumnName("ID");
        entity.Property(e => e.NominalPipingDiameter).HasColumnType("float");
        entity.Property(e => e.NominalDiameterUnits).HasMaxLength(10);
        entity.Property(e => e.EndStandardCl).HasColumnName("EndStandard_CL");
        entity.Property(e => e.EndStandard).HasMaxLength(255);
        entity.Property(e => e.ScheduleThicknessCl).HasColumnName("ScheduleThickness_CL");
        entity.Property(e => e.ScheduleThickness).HasMaxLength(255);
        entity.Property(e => e.PipingOutsideDiameter).HasMaxLength(50);
        entity.Property(e => e.WallThickness).HasMaxLength(50);
    }
}
