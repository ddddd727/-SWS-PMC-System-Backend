using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Modules.DesignRules.Entities;

public partial class S3dRuleShortCodeHierarchyRule
{
    public int Id { get; set; }

    public string ShortCodeHierarchyType { get; set; } = null!;

    public string ShortCode { get; set; } = null!;
}
