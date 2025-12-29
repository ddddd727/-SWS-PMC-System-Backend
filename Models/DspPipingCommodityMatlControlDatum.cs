using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Models;

public partial class DspPipingCommodityMatlControlDatum
{
    public int Id { get; set; }

    public string ContractorCommodityCode { get; set; } = null!;

    public decimal? FirstSizeFrom { get; set; }

    public decimal? FirstSizeTo { get; set; }

    public string? FirstSizeUnits { get; set; }

    public decimal? SecondSizeFrom { get; set; }

    public decimal? SecondSizeTo { get; set; }

    public string? SecondSizeUnits { get; set; }

    public int? MultisizeOption { get; set; }

    public string? IndustryCommodityCode { get; set; }

    public string? ClientCommodityCode { get; set; }

    public string? CimiscommodityCode { get; set; }

    public string? ShortMaterialDescription { get; set; }

    public string? LocalizedShortMaterialDesc { get; set; }

    public string? LongMaterialDescription { get; set; }

    public int? Vendor { get; set; }

    public int? Manufacturer { get; set; }

    public int? FabricationType { get; set; }

    public int? SupplyResponsibility { get; set; }

    public int? ReportingType { get; set; }

    public int? QuantityOfReportableParts { get; set; }

    public int? GasketRequirements { get; set; }

    public int? BoltingRequirements { get; set; }

    public int? ClampRequirement { get; set; }

    public int? WeldingRequirement { get; set; }

    public int? LooseMaterialRequirements { get; set; }

    public int? SubstCapScrewsQuantity { get; set; }

    public int? SubstCapScrewCntrCommodityCode { get; set; }

    public int? SubstCapScrewDiameter { get; set; }

    public int? TappedHoleDepth { get; set; }

    public int? TappedHoleDepth2 { get; set; }

    public int? CapScrewEngagementGap { get; set; }

    public int? MultiportValveOpReq { get; set; }

    public int? ValveOperatorType { get; set; }

    public int? ValveOperatorGeoIndStd { get; set; }

    public string? ValveOperatorCatalogPartNumber { get; set; }

    public int? ReportableCommodityCode { get; set; }

    public int? PartDataSource { get; set; }

    public int? AltOrientationCommodityCode { get; set; }

    public int? HyperlinkToElectronicVendor { get; set; }

    public int? HyperlinkToElectronicManuals { get; set; }

    public int? PipingNote1 { get; set; }

    public int? VendorPartNumber { get; set; }

    public int? ManufacturerPartNumber { get; set; }

    public int? AltReportableCommodityCode { get; set; }

    public int? QuantityOfAltReportableParts { get; set; }

    public int? EClasseProcurementCode { get; set; }

    public int? UnspsceProcurementCode { get; set; }

    public int? LegacyCommodityCode { get; set; }

    public virtual DspValveOperatorMatlControlDatum? ValveOperatorCatalogPartNumberNavigation { get; set; }
}
