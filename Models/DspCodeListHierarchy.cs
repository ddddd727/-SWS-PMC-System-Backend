using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Models;

public partial class DspCodeListHierarchy
{
    public int Id { get; set; }

    public int CodeListTableId { get; set; }

    public int? ParentCodeListTableId { get; set; }

    public virtual DspCodeListTable CodeListTable { get; set; } = null!;
}
