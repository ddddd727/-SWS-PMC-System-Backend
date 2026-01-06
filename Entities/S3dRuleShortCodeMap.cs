using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dRuleShortCodeMap
{
    public int Id { get; set; }

    public int ComponentTypeId { get; set; }

    public string ShortCode { get; set; } = null!;
}
