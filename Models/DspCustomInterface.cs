using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Models;

public partial class DspCustomInterface
{
    public int Id { get; set; }

    public string InterfaceName { get; set; } = null!;

    public virtual ICollection<DspAttribute> DspAttributes { get; set; } = new List<DspAttribute>();
}
