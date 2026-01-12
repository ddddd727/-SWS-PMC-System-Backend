using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities;

public partial class S3dRuleValveOperatorMatlControlDatum
{
    public int Id { get; set; }

    public string OperatorPartNumber { get; set; } = null!;

    public string? ShortMatlDescription { get; set; }

    public string? LocalizedShortMaterialDescription { get; set; }

    public string? LongMaterialDescription { get; set; }

    public int? Vendor { get; set; }

    public int? Manufacturer { get; set; }

    public int? ValveOperatorType { get; set; }

    public int? ReportableCommodityCode { get; set; }

    public int? QuantityOfReportableParts { get; set; }

    public int? AltReportableCommodityCode { get; set; }

    public int? QuantityOfAltReportableParts { get; set; }

    public int? HyperlinkToElectronicVendor { get; set; }

    public int? HyperlinkToElectronicManuals { get; set; }

    public virtual ICollection<S3dRulePipingCommodityMatlControlDatum> S3dRulePipingCommodityMatlControlData { get; set; } = new List<S3dRulePipingCommodityMatlControlDatum>();
}
