using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class DspAttribute
{
    public int Id { get; set; }

    public int InterfaceId { get; set; }

    public int? CategoryId { get; set; }

    public string AttributeName { get; set; } = null!;

    public string AttributeUserName { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? UnitsType { get; set; }

    public string? PrimaryUnits { get; set; }

    public int? CodelistTableId { get; set; }

    public bool OnPropertyPage { get; set; }

    public bool ReadOnly { get; set; }

    public string? SymbolParameter { get; set; }

    public virtual DspCodeListTable? Category { get; set; }

    public virtual DspCodeListTable? CodelistTable { get; set; }

    public virtual DspCustomInterface Interface { get; set; } = null!;
}
