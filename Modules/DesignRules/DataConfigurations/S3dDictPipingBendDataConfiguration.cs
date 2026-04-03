using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMCSystem_Backend.Modules.DesignRules.Entities;

namespace PMCSystem_Backend.Modules.DesignRules.DataConfigurations;

/// <summary>
/// S3D_Dict_PipingBendData 表配置
/// </summary>
public class S3dDictPipingBendDataConfiguration : IEntityTypeConfiguration<S3dDictPipingBendData>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<S3dDictPipingBendData> entity)
    {
        entity.HasKey(e => e.Id).HasName("PK__S3D_Dict__3214EC27EA0A1461");
        entity.ToTable("S3D_Dict_PipingBendData");
        entity.HasIndex(e => new { e.OutSideDiameter, e.OutSideDiameterUnit, e.MachineNum }, "UQ_PipingBend_OutSideDiameter_Unit_MachineNum").IsUnique();
        entity.Property(e => e.Id).HasColumnName("ID");
        entity.Property(e => e.HeaderClampLength);
        entity.Property(e => e.MaterialsCategoryCl).HasColumnName("MaterialsCategory_CL");
        entity.Property(e => e.MachineNum).HasDefaultValue(1);
        entity.Property(e => e.OutSideDiameter);
        entity.Property(e => e.OutSideDiameterUnit).HasMaxLength(100);
        entity.Property(e => e.Status).HasDefaultValue(true);
        entity.Property(e => e.TailClampLength);
    }
}
