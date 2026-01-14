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
}

