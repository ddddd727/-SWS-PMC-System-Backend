using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dCommonCodeListValue
{
    public int Id { get; set; }

    public int CodeListTableId { get; set; }

    public int? ParentCodeListNumber { get; set; }

    public string ShortStringValue { get; set; } = null!;

    public string LongStringValue { get; set; } = null!;

    public int CodeListNumber { get; set; }

    public bool IsUserDefine { get; set; }

    public bool Status { get; set; }

    public virtual S3dCommonCodeListTable CodeListTable { get; set; } = null!;
}
