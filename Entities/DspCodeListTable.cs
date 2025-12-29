using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class DspCodeListTable
{
    public int Id { get; set; }

    public string CodeListTableName { get; set; } = null!;

    public bool IsUserDefined { get; set; }

    public string Major { get; set; } = null!;

    public virtual ICollection<DspAttribute> DspAttributeCategories { get; set; } = new List<DspAttribute>();

    public virtual ICollection<DspAttribute> DspAttributeCodelistTables { get; set; } = new List<DspAttribute>();

    public virtual DspCodeListHierarchy? DspCodeListHierarchy { get; set; }

    public virtual ICollection<DspCodeListValue> DspCodeListValues { get; set; } = new List<DspCodeListValue>();
}
