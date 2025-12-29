using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Models;

public partial class DspSpmcDictPipingComponentTypeHierarchy
{
    public int Id { get; set; }

    public int? ComponentTypeId { get; set; }

    public int? PipingCommoditySubClassCl { get; set; }

    public virtual DspSpmcDictPipingComponentType? ComponentType { get; set; }
}
