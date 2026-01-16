namespace PMCSystem_Backend.Models
{
    public class PmcCodeGenerateRequest
    {
        public List<string> PmcCodes { get; set; } = new List<string>();
    }

    public class PmcCodeGenerateResponseItem
    {
        public string Pmc { get; set; } = string.Empty;

        public string A { get; set; } = string.Empty;

        public string B1 { get; set; } = string.Empty;

        public string B2 { get; set; } = string.Empty;

        public string B3 { get; set; } = string.Empty;

        public string C1 { get; set; } = string.Empty;

        public string C2 { get; set; } = string.Empty;

        public string D { get; set; } = string.Empty;

        public string? ADesc { get; set; }

        public string? B1Desc { get; set; }

        public string? B2Desc { get; set; }

        public string? B3Desc { get; set; }

        public string? C1Desc { get; set; }

        public string? C2Desc { get; set; }

        public string? DDesc { get; set; }
    }

    public class PmcCodeQueryItem
    {
        public string PmcCode { get; set; } = string.Empty;
        public string PipingClassName { get; set; } = string.Empty;
        public string MaterialsCategoryName { get; set; } = string.Empty;
        public string PipingStandardName { get; set; } = string.Empty;
        public string MaterialsGradeName { get; set; } = string.Empty;
        public string FlangeStandardName { get; set; } = string.Empty;
        public string PressureRatingName { get; set; } = string.Empty;
        public string ScheduleThicknessName { get; set; } = string.Empty;
    }

    public class PmcOptionDto
    {
        public string Label { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty; // Code
    }

    public class PmcCodeSaveItem
    {
        public string PmcCode { get; set; } = string.Empty;

        public string PipingClassName { get; set; } = string.Empty;

        public string MaterialsCategoryName { get; set; } = string.Empty;

        public string PipingStandardName { get; set; } = string.Empty;

        public string MaterialsGradeName { get; set; } = string.Empty;

        public string FlangeStandardName { get; set; } = string.Empty;

        public string PressureRatingName { get; set; } = string.Empty;

        public string ScheduleThicknessName { get; set; } = string.Empty;
    }

    public class PmcCodeSaveRequest
    {
        public string ShipType { get; set; } = string.Empty;

        public string ShipNo { get; set; } = string.Empty;

        public List<PmcCodeSaveItem> Items { get; set; } = new List<PmcCodeSaveItem>();
    }

    public class ShipInfo
    {
        public string ShipType { get; set; } = string.Empty;
        public string ShipNumber { get; set; } = string.Empty;
    }

    public class CopyRuleRequest
    {
        public string SourceShipType { get; set; } = string.Empty;
        public string SourceShipNo { get; set; } = string.Empty;
        public string TargetShipType { get; set; } = string.Empty;
        public string TargetShipNo { get; set; } = string.Empty;
    }
}

