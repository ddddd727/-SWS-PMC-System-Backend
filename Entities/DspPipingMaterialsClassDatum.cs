using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class DspPipingMaterialsClassDatum
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
}
