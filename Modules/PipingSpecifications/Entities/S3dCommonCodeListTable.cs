using PMCSystem_Backend.Modules.DesignRules.Entities;
using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Modules.PipingSpecifications.Entities;

public partial class S3dCommonCodeListTable
{
    public int Id { get; set; }

    public string CodeListTableName { get; set; } = null!;

    public bool IsUserDefined { get; set; }

    public string Major { get; set; } = null!;

    
    public virtual ICollection<S3dCommonCodeListValue> S3dCommonCodeListValues { get; set; } = new List<S3dCommonCodeListValue>();
}
