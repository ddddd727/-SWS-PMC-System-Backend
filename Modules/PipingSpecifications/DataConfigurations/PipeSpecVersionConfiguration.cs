using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMCSystem_Backend.Modules.PipingSpecifications.Dtos;
using PMCSystem_Backend.Modules.PipingSpecifications.Entities;

namespace PMCSystem_Backend.Modules.PipingSpecifications.DataConfigurations;

/// <summary>
/// S3D_Rule_PipeSpecVersion 表配置
/// </summary>
public class PipeSpecVersionConfiguration : IEntityTypeConfiguration<PipeSpecVersion>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<PipeSpecVersion> entity)
    {
        var jsonOptions = new JsonSerializerOptions();
        var listPmcStandardInfoComparer = new ValueComparer<List<PmcStandardInfo>>(
            (c1, c2) => JsonSerializer.Serialize(c1, jsonOptions) == JsonSerializer.Serialize(c2, jsonOptions),
            c => c == null ? 0 : JsonSerializer.Serialize(c, jsonOptions).GetHashCode(),
            c => JsonSerializer.Deserialize<List<PmcStandardInfo>>(JsonSerializer.Serialize(c, jsonOptions), jsonOptions)!
        );

        entity.HasKey(e => e.Id).HasName("PK_PipeSpecVersion");
        entity.ToTable("S3D_Rule_PipeSpecVersion");
        entity.HasIndex(e => new { e.PmcCode, e.ShipType, e.ShipNo }, "IX_PipeSpecVersion_PmcCode_ShipType_ShipNo");
        entity.Property(e => e.Id).HasColumnName("Id");
        entity.Property(e => e.PmcCode).HasMaxLength(255).HasColumnName("PmcCode");
        entity.Property(e => e.ShipType).HasMaxLength(255).HasColumnName("ShipType");
        entity.Property(e => e.ShipNo).HasMaxLength(255).HasColumnName("ShipNo");
        entity.Property(e => e.Version).HasColumnName("Version");
        entity.Property(e => e.PipingClassName).HasMaxLength(255).HasColumnName("PipingClassName");
        entity.Property(e => e.MaterialsCategoryName).HasMaxLength(255).HasColumnName("MaterialsCategoryName");
        entity.Property(e => e.PipingStandardName).HasMaxLength(255).HasColumnName("PipingStandardName");
        entity.Property(e => e.MaterialsGradeName).HasMaxLength(255).HasColumnName("MaterialsGradeName");
        entity.Property(e => e.FlangeStandardName).HasMaxLength(255).HasColumnName("FlangeStandardName");
        entity.Property(e => e.PressureRatingName).HasMaxLength(255).HasColumnName("PressureRatingName");
        entity.Property(e => e.ScheduleThicknessName).HasMaxLength(255).HasColumnName("ScheduleThicknessName");
        entity.Property(e => e.Status).HasMaxLength(100).HasColumnName("Status");
        entity.Property(e => e.CreatedAt).HasColumnName("CreatedAt");
        entity.Property(e => e.CreatedBy).HasMaxLength(255).HasColumnName("CreatedBy");
        entity.Property(e => e.Comment).HasMaxLength(500).HasColumnName("Comment");

        entity.Property(e => e.PipeStandard)
            .HasConversion(
                v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                listPmcStandardInfoComparer)
            .HasColumnName("PipeStandard");
        entity.Property(e => e.ElbowStandard)
            .HasConversion(
                v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                listPmcStandardInfoComparer)
            .HasColumnName("ElbowStandard");
        entity.Property(e => e.RedStandard)
            .HasConversion(
                v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                listPmcStandardInfoComparer)
            .HasColumnName("RedStandard");
        entity.Property(e => e.TeeStandard)
            .HasConversion(
                v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                listPmcStandardInfoComparer)
            .HasColumnName("TeeStandard");
        entity.Property(e => e.SleeveStandard)
            .HasConversion(
                v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                listPmcStandardInfoComparer)
            .HasColumnName("SleeveStandard");
        entity.Property(e => e.BossesStandard)
            .HasConversion(
                v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                listPmcStandardInfoComparer)
            .HasColumnName("BossesStandard");
        entity.Property(e => e.SaddlesStandard)
            .HasConversion(
                v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                listPmcStandardInfoComparer)
            .HasColumnName("SaddlesStandard");
        entity.Property(e => e.CapsStandard)
            .HasConversion(
                v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                listPmcStandardInfoComparer)
            .HasColumnName("CapsStandard");
        entity.Property(e => e.OverpassStandard)
            .HasConversion(
                v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                listPmcStandardInfoComparer)
            .HasColumnName("OverpassStandard");
        entity.Property(e => e.AccessoriesStandard)
            .HasConversion(
                v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                listPmcStandardInfoComparer)
            .HasColumnName("AccessoriesStandard");
        entity.Property(e => e.FlangeStandard)
            .HasConversion(
                v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                listPmcStandardInfoComparer)
            .HasColumnName("FlangeStandard");
        entity.Property(e => e.BlindFlangeStandard)
            .HasConversion(
                v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                listPmcStandardInfoComparer)
            .HasColumnName("BlindFlangeStandard");
        entity.Property(e => e.GasketStandard)
            .HasConversion(
                v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                listPmcStandardInfoComparer)
            .HasColumnName("GasketStandard");
        entity.Property(e => e.BoltStandard)
            .HasConversion(
                v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                listPmcStandardInfoComparer)
            .HasColumnName("BoltStandard");
        entity.Property(e => e.NutStandard)
            .HasConversion(
                v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                listPmcStandardInfoComparer)
            .HasColumnName("NutStandard");
        entity.Property(e => e.WasherStandard)
            .HasConversion(
                v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                v => string.IsNullOrEmpty(v) ? null : JsonSerializer.Deserialize<List<PmcStandardInfo>>(v, jsonOptions),
                listPmcStandardInfoComparer)
            .HasColumnName("WasherStandard");
    }
}
