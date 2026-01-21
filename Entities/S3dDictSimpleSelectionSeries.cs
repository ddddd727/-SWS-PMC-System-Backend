using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dDictSimpleSelectionSeries
{
    public int Id { get; set; }

    public string SimpleSelectionSeriesName { get; set; } = null!;

    public bool Status { get; set; }

    public virtual ICollection<S3dRuleSimpleSelectionSeriesCompMap> S3dRuleSimpleSelectionSeriesCompMaps { get; set; } = new List<S3dRuleSimpleSelectionSeriesCompMap>();

    public virtual ICollection<S3dRuleSimpleSelectionSeriesStandsMap> S3dRuleSimpleSelectionSeriesStandsMaps { get; set; } = new List<S3dRuleSimpleSelectionSeriesStandsMap>();
}
