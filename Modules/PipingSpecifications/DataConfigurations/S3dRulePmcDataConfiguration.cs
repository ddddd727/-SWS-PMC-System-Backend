using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMCSystem_Backend.Modules.PipingSpecifications.Dtos;
using PMCSystem_Backend.Modules.PipingSpecifications.Entities;

namespace PMCSystem_Backend.Modules.PipingSpecifications.DataConfigurations;

/// <summary>
/// S3D_Rule_PMCData 表配置
/// </summary>
public class S3dRulePmcDataConfiguration : IEntityTypeConfiguration<S3dRulePmcData>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<S3dRulePmcData> entity)
    {
        var jsonOptions = new JsonSerializerOptions();
        var listPmcStandardInfoComparer = new ValueComparer<List<PmcStandardInfo>>(
            (c1, c2) => JsonSerializer.Serialize(c1, jsonOptions) == JsonSerializer.Serialize(c2, jsonOptions),
            c => c == null ? 0 : JsonSerializer.Serialize(c, jsonOptions).GetHashCode(),
            c => JsonSerializer.Deserialize<List<PmcStandardInfo>>(JsonSerializer.Serialize(c, jsonOptions), jsonOptions)!
        );

        entity.HasKey(e => e.Id).HasName("PK__S3D_Rule__3214EC27203D42D1");
        entity.ToTable("S3D_Rule_PMCData");
        entity.HasIndex(e => new { e.Pmccode, e.ShipType, e.ShipNo }, "UQ_SPMC_PMCCode").IsUnique();

        entity.Property(e => e.Id).HasColumnName("ID");
        entity.Property(e => e.AccessoriesStandard)
            .HasConversion(
                v => JsonSerializer.Serialize(v, jsonOptions),
                v => JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                listPmcStandardInfoComparer)
            .HasMaxLength(500).HasColumnName("AccessoriesStandard");
        entity.Property(e => e.BlindFlangeStandard)
            .HasConversion(
                v => JsonSerializer.Serialize(v, jsonOptions),
                v => JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                listPmcStandardInfoComparer)
            .HasMaxLength(500).HasColumnName("BlindFlangeStandard");
        entity.Property(e => e.BoltStandard)
            .HasConversion(
                v => JsonSerializer.Serialize(v, jsonOptions),
                v => JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                listPmcStandardInfoComparer)
            .HasMaxLength(500).HasColumnName("BoltStandard");
        entity.Property(e => e.BossesStandard)
            .HasConversion(
                v => JsonSerializer.Serialize(v, jsonOptions),
                v => JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                listPmcStandardInfoComparer)
            .HasMaxLength(500).HasColumnName("BossesStandard");
        entity.Property(e => e.CapsStandard)
            .HasConversion(
                v => JsonSerializer.Serialize(v, jsonOptions),
                v => JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                listPmcStandardInfoComparer)
            .HasMaxLength(500).HasColumnName("CapsStandard");
        entity.Property(e => e.ElbowStandard)
            .HasConversion(
                v => JsonSerializer.Serialize(v, jsonOptions),
                v => JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                listPmcStandardInfoComparer)
            .HasMaxLength(500).HasColumnName("ElbowStandard");
        entity.Property(e => e.FlangeStandard)
            .HasConversion(
                v => JsonSerializer.Serialize(v, jsonOptions),
                v => JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                listPmcStandardInfoComparer)
            .HasMaxLength(500).HasColumnName("FlangeStandard");
        entity.Property(e => e.FlangeStandardName).HasMaxLength(255).HasColumnName("FlangeStandardName");
        entity.Property(e => e.GasketStandard)
            .HasConversion(
                v => JsonSerializer.Serialize(v, jsonOptions),
                v => JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                listPmcStandardInfoComparer)
            .HasMaxLength(500).HasColumnName("GasketStandard");
        entity.Property(e => e.MaterialsCategoryName).HasMaxLength(255);
        entity.Property(e => e.MaterialsGradeName).HasMaxLength(255);
        entity.Property(e => e.NutStandard)
            .HasConversion(
                v => JsonSerializer.Serialize(v, jsonOptions),
                v => JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                listPmcStandardInfoComparer)
            .HasMaxLength(500).HasColumnName("NutStandard");
        entity.Property(e => e.OverpassStandard)
            .HasConversion(
                v => JsonSerializer.Serialize(v, jsonOptions),
                v => JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                listPmcStandardInfoComparer)
            .HasMaxLength(500).HasColumnName("OverpassStandard");
        entity.Property(e => e.PipeStandard)
            .HasConversion(
                v => JsonSerializer.Serialize(v, jsonOptions),
                v => JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                listPmcStandardInfoComparer)
            .HasMaxLength(500).HasColumnName("PipeStandard");
        entity.Property(e => e.PipingClassName).HasMaxLength(255);
        entity.Property(e => e.PipingStandardName).HasMaxLength(255).HasColumnName("PipingStandardName");
        entity.Property(e => e.Pmccode).HasMaxLength(255).HasColumnName("PMCCode");
        entity.Property(e => e.PressureRatingName).HasMaxLength(255).HasColumnName("PressureRatingName");
        entity.Property(e => e.RedStandard)
            .HasConversion(
                v => JsonSerializer.Serialize(v, jsonOptions),
                v => JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                listPmcStandardInfoComparer)
            .HasMaxLength(500).HasColumnName("RedStandard");
        entity.Property(e => e.SaddlesStandard)
            .HasConversion(
                v => JsonSerializer.Serialize(v, jsonOptions),
                v => JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                listPmcStandardInfoComparer)
            .HasMaxLength(500).HasColumnName("SaddlesStandard");
        entity.Property(e => e.ScheduleThicknessName).HasMaxLength(255).HasColumnName("ScheduleThicknessName");
        entity.Property(e => e.ShipNo).HasMaxLength(255).HasColumnName("ShipNo");
        entity.Property(e => e.ShipType).HasMaxLength(255).HasColumnName("ShipType");
        entity.Property(e => e.SleeveStandard)
            .HasConversion(
                v => JsonSerializer.Serialize(v, jsonOptions),
                v => JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                listPmcStandardInfoComparer)
            .HasMaxLength(500).HasColumnName("SleeveStandard");
        entity.Property(e => e.Status).HasMaxLength(100).HasColumnName("Status");
        entity.Property(e => e.TeeStandard)
            .HasConversion(
                v => JsonSerializer.Serialize(v, jsonOptions),
                v => JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                listPmcStandardInfoComparer)
            .HasMaxLength(500).HasColumnName("TeeStandard");
        entity.Property(e => e.WasherStandard)
            .HasConversion(
                v => JsonSerializer.Serialize(v, jsonOptions),
                v => JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                listPmcStandardInfoComparer)
            .HasMaxLength(500).HasColumnName("WasherStandard");
        entity.Property(e => e.IsByRule).HasDefaultValue(true);
        entity.Property(e => e.VersionNum).HasDefaultValue(1);
    }
}
