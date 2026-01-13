using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dPropertyVersionHistory
{
    public long Id { get; set; }

    public long PropertyId { get; set; }

    public int Version { get; set; }

    public string? AttributeUserName { get; set; }

    public string? DataType { get; set; }

    public string? UnitsType { get; set; }

    public string? PrimaryUnits { get; set; }

    public string? CodelistName { get; set; }

    public string? CodelistNamespace { get; set; }

    public bool? OnPropertyPage { get; set; }

    public bool? IsReadOnly { get; set; }

    public bool? IsSymbolParameter { get; set; }

    public string? ChangeDescription { get; set; }

    public string? Modifier { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual S3dPropertyPropertyDefinition Property { get; set; } = null!;
}
