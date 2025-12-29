using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Models;

public partial class DspCodeListValue
{
    public int Id { get; set; }

    public int CodeListTableId { get; set; }

    public int? ParentNumberId { get; set; }

    public string ShortStringValue { get; set; } = null!;

    public string LongStringValue { get; set; } = null!;

    public int CodeListNumber { get; set; }

    public bool IsUserDefine { get; set; }

    public bool Status { get; set; }

    public virtual DspCodeListTable CodeListTable { get; set; } = null!;
}
