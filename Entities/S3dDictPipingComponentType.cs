using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dDictPipingComponentType
{
    public int Id { get; set; }

    public string? ComponentTypeName { get; set; }

    public string? ComponentTypeDescription { get; set; }

    public virtual ICollection<S3dDictPipingComponentTypeHierarchy> S3dDictPipingComponentTypeHierarchies { get; set; } = new List<S3dDictPipingComponentTypeHierarchy>();
}
