using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dRuleSimpleSelectionSeriesCompMap
{
    public int Id { get; set; }

    public int SimpleSelectionSeriesId { get; set; }

    public int ComponentTypeId { get; set; }

    public bool Status { get; set; }

    public virtual S3dDictPipingComponentType ComponentType { get; set; } = null!;

    public virtual S3dDictSimpleSelectionSeries SimpleSelectionSeries { get; set; } = null!;
}
