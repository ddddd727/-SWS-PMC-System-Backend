using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.TempScaffold;

public partial class S3dPropertyInterfaceConfig
{
    public long Id { get; set; }

    public long ObjectTypeId { get; set; }

    public string InterfaceName { get; set; } = null!;

    public string? Description { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual S3dPropertyObjectType ObjectType { get; set; } = null!;

    public virtual ICollection<S3dPropertyPropertyDefinition> S3dPropertyPropertyDefinitions { get; set; } = new List<S3dPropertyPropertyDefinition>();
}
