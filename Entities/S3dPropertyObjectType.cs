using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dPropertyObjectType
{
    public long Id { get; set; }

    public string ObjectTypeName { get; set; } = null!;

    public string? Description { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<S3dPropertyInterfaceConfig> S3dPropertyInterfaceConfigs { get; set; } = new List<S3dPropertyInterfaceConfig>();
}
