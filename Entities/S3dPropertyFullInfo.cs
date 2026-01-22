using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dPropertyFullInfo
{
    public long Id { get; set; }

    public long ObjectTypeId { get; set; }

    public string ObjectTypeName { get; set; } = null!;

    public long InterfaceId { get; set; }

    public string InterfaceName { get; set; } = null!;

    public string CategoryName { get; set; } = null!;

    public string AttributeName { get; set; } = null!;

    public string? AttributeUserName { get; set; }

    public string DataType { get; set; } = null!;

    public string? UnitsType { get; set; }

    public string? PrimaryUnits { get; set; }

    public string? CodelistName { get; set; }

    public string? CodelistNamespace { get; set; }

    public bool? OnPropertyPage { get; set; }

    public bool? IsReadOnly { get; set; }

    public bool? IsSymbolParameter { get; set; }

    public int? SortOrder { get; set; }

    public string? Description { get; set; }

    public int? Version { get; set; }

    public string? Modifier { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
