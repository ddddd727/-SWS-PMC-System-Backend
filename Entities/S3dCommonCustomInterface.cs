using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dCommonCustomInterface
{
    public int Id { get; set; }

    public string InterfaceName { get; set; } = null!;

    public virtual ICollection<S3dCommonAttribute> S3dCommonAttributes { get; set; } = new List<S3dCommonAttribute>();
}
