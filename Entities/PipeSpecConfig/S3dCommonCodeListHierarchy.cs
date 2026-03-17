using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dCommonCodeListHierarchy
{
    public int Id { get; set; }

    public int CodeListTableId { get; set; }

    public int? ParentCodeListTableId { get; set; }
}
