using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.TempScaffold;

public partial class S3dRulePipingMaterialsClassDatum
{
    public int Id { get; set; }

    public string? SpecName { get; set; }

    public string? MaterialsOfConstructionClass { get; set; }

    public string? MaterialsDescription { get; set; }

    public string? FluidService { get; set; }

    public int? DesignStandard { get; set; }

    public int? AutomatedFlangeSelectionOption { get; set; }

    public int? PipingCommodityOverrideOption { get; set; }

    public int? WasherCreationOption { get; set; }

    public int? GasketRequirementOverride { get; set; }

    public int? LiningMaterial { get; set; }

    public int? PipingNote1 { get; set; }

    public int? PipingSpecStatus { get; set; }

    public int? Responsibility { get; set; }

    public string? LastModifiedOn { get; set; }

    public string? Comments { get; set; }

    public string? RevisionNumber { get; set; }

    public string? ApprovedBy { get; set; }

    public string? ApprovalDate { get; set; }

    public int? JacketMatOfConstructionClass { get; set; }

    public int? JumperMatOfConstructionClass { get; set; }

    public string? JacketMaterialsDescription { get; set; }

    public string? JumperMaterialsDescription { get; set; }

    public int? JacketAndJumperFluidService { get; set; }

    public int? StressRelief { get; set; }

    public int? Examination { get; set; }

    public int? HyperlinkToHumanSpec { get; set; }

    public int? StressReliefRequirement { get; set; }

    public int? MaterialsGroup { get; set; }

    public int? WeldingProcedureSpecification { get; set; }

    public int? MaterialsType { get; set; }

    public virtual ICollection<S3dRuleBendAngle> S3dRuleBendAngles { get; set; } = new List<S3dRuleBendAngle>();

    public virtual ICollection<S3dRuleBoltExtension> S3dRuleBoltExtensions { get; set; } = new List<S3dRuleBoltExtension>();

    public virtual ICollection<S3dRuleBoltSelectionFilter> S3dRuleBoltSelectionFilters { get; set; } = new List<S3dRuleBoltSelectionFilter>();

    public virtual ICollection<S3dRuleDefaultChangeOfDirectionPerSpec> S3dRuleDefaultChangeOfDirectionPerSpecs { get; set; } = new List<S3dRuleDefaultChangeOfDirectionPerSpec>();

    public virtual ICollection<S3dRuleGasketSelectionFilter> S3dRuleGasketSelectionFilters { get; set; } = new List<S3dRuleGasketSelectionFilter>();

    public virtual ICollection<S3dRuleMinPipeLengthPurchasePerSpec> S3dRuleMinPipeLengthPurchasePerSpecs { get; set; } = new List<S3dRuleMinPipeLengthPurchasePerSpec>();

    public virtual ICollection<S3dRuleMinimumPipeLengthRulePerSpec> S3dRuleMinimumPipeLengthRulePerSpecs { get; set; } = new List<S3dRuleMinimumPipeLengthRulePerSpec>();

    public virtual ICollection<S3dRuleNutSelectionFilter> S3dRuleNutSelectionFilters { get; set; } = new List<S3dRuleNutSelectionFilter>();

    public virtual ICollection<S3dRulePipeBranch> S3dRulePipeBranches { get; set; } = new List<S3dRulePipeBranch>();

    public virtual ICollection<S3dRulePipeNominalDiameter> S3dRulePipeNominalDiameters { get; set; } = new List<S3dRulePipeNominalDiameter>();

    public virtual ICollection<S3dRulePipeTakedownPart> S3dRulePipeTakedownParts { get; set; } = new List<S3dRulePipeTakedownPart>();

    public virtual ICollection<S3dRulePipingCommodityFilter> S3dRulePipingCommodityFilters { get; set; } = new List<S3dRulePipingCommodityFilter>();

    public virtual ICollection<S3dRuleReinforcingWeldDatum> S3dRuleReinforcingWeldData { get; set; } = new List<S3dRuleReinforcingWeldDatum>();

    public virtual ICollection<S3dRuleWasherSelectionFilter> S3dRuleWasherSelectionFilters { get; set; } = new List<S3dRuleWasherSelectionFilter>();

    public virtual ICollection<S3dRuleWeldClearanceRule> S3dRuleWeldClearanceRules { get; set; } = new List<S3dRuleWeldClearanceRule>();
}
