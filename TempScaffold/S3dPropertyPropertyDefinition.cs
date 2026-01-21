using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.TempScaffold;

public partial class S3dPropertyPropertyDefinition
{
    public long Id { get; set; }

    public long InterfaceId { get; set; }

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

    public bool? IsDeleted { get; set; }

    public virtual S3dPropertyInterfaceConfig Interface { get; set; } = null!;

    public virtual ICollection<S3dPropertyVersionHistory> S3dPropertyVersionHistories { get; set; } = new List<S3dPropertyVersionHistory>();
}
