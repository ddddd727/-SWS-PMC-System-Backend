using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dDictPipingComponentType
{
    public int Id { get; set; }

    public string ComponentTypeName { get; set; } = null!;

    public string ComponentTypeDescription { get; set; } = null!;

    public bool Status { get; set; }

    public virtual ICollection<S3dDictGeometricIndustryStandard> S3dDictGeometricIndustryStandards { get; set; } = new List<S3dDictGeometricIndustryStandard>();

    public virtual ICollection<S3dRuleComponentTypeHierarchyRule> S3dRuleComponentTypeHierarchyRules { get; set; } = new List<S3dRuleComponentTypeHierarchyRule>();

    public virtual ICollection<S3dRulePipingCompStandard> S3dRulePipingCompStandards { get; set; } = new List<S3dRulePipingCompStandard>();

    public virtual ICollection<S3dRuleShortCodeMap> S3dRuleShortCodeMaps { get; set; } = new List<S3dRuleShortCodeMap>();

    public virtual ICollection<S3dRuleSimpleSelectionSeriesCompMap> S3dRuleSimpleSelectionSeriesCompMaps { get; set; } = new List<S3dRuleSimpleSelectionSeriesCompMap>();
}
