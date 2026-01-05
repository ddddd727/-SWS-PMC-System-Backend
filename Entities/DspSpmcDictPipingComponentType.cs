using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class DspSpmcDictPipingComponentType
{
    public int Id { get; set; }

    public string? ComponentTypeName { get; set; }

    public string? ComponentTypeDescription { get; set; }

    public virtual ICollection<DspSpmcDictPipingComponentTypeHierarchy> DspSpmcDictPipingComponentTypeHierarchies { get; set; } = new List<DspSpmcDictPipingComponentTypeHierarchy>();
}
