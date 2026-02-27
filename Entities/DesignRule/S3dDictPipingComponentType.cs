using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities.PipeSpecConfig;

public partial class S3dDictPipingComponentType
{
    public int Id { get; set; }

    public string ComponentTypeName { get; set; } = null!;

    public string ComponentTypeDescription { get; set; } = null!;

    public bool Status { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime ModifiedDate { get; set; }
}
