using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMCSystem_Backend.Shared.Entities;

namespace PMCSystem_Backend.Shared.DataConfigurations;

/// <summary>
/// S3D_Common_PlainPipingGenericData 表配置
/// </summary>
public class S3dCommonPlainPipingGenericDataConfiguration : IEntityTypeConfiguration<S3dCommonPlainPipingGenericData>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<S3dCommonPlainPipingGenericData> entity)
    {
        entity.HasKey(e => e.Id).HasName("DSP_PlainPipingGenericData_PK");
        entity.ToTable("S3D_Common_PlainPipingGenericData");
        entity.HasIndex(
            e => new
            {
                e.NominalPipingDiameter,
                e.NominalDiameterUnits,
                e.EndStandardCl,
                e.ScheduleThicknessCl,
                e.PressureRatingCl
            },
            "DSP_PlainPipingGenericData_UNIQUE")
            .IsUnique();
        entity.Property(e => e.Id).HasColumnName("ID");
        entity.Property(e => e.NominalPipingDiameter).HasColumnType("float");
        entity.Property(e => e.NominalDiameterUnits).HasMaxLength(5);
        entity.Property(e => e.EndStandardCl).HasColumnName("EndStandard_CL");
        entity.Property(e => e.ScheduleThicknessCl).HasColumnName("ScheduleThickness_CL");
        entity.Property(e => e.PressureRatingCl).HasColumnName("PressureRating_CL");
        entity.Property(e => e.PipingOutsideDiameter).HasMaxLength(50);
        entity.Property(e => e.WallThickness).HasMaxLength(50);
        entity.Property(e => e.Status).HasDefaultValue(true);
        entity.Property(e => e.CreatedBy).HasMaxLength(100).HasDefaultValueSql("(suser_sname())");
        entity.Property(e => e.ModifiedBy).HasMaxLength(100);
        entity.Property(e => e.CreatedDate).HasColumnType("datetime2(3)");
        entity.Property(e => e.ModifiedDate).HasColumnType("datetime2(3)");
    }
}
