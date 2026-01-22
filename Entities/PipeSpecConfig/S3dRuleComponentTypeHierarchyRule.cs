using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities.PipeSpecConfig;

public partial class S3dRuleComponentTypeHierarchyRule
{
    public int Id { get; set; }

    public int ComponentTypeId { get; set; }

    public int? PipingCommoditySubClassCl { get; set; }

    public bool Status { get; set; }

    public string? JsonData { get; set; }
}
