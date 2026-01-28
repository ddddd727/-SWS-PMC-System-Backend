namespace PMCSystem_Backend.Dtos.PmcSpecRuleConfig
{
    public class PmcSpecInfoDto
    {
        public string PmcCode { get; set; }

        public List<PmcStandardInfo>? ElbowStandard { get; set; }

        public List<PmcStandardInfo>? RedStandard { get; set; }

        public List<PmcStandardInfo>? TeeStandard { get; set; }

        public List<PmcStandardInfo>? SleeveStandard { get; set; }

        public List<PmcStandardInfo>? BossesStandard { get; set; }

        public List<PmcStandardInfo>? SaddlesStandard { get; set; }

        public List<PmcStandardInfo>? CapsStandard { get; set; }

        public List<PmcStandardInfo>? OverpassStandard { get; set; }

        public List<PmcStandardInfo>? AccessoriesStandard { get; set; }

        public List<PmcStandardInfo>? FlangeStandard { get; set; }

        public List<PmcStandardInfo>? BlindFlangeStandard { get; set; }

        public List<PmcStandardInfo>? GasketStandard { get; set; }

        public List<PmcStandardInfo>? BoltStandard { get; set; }

        public List<PmcStandardInfo>? NutStandard { get; set; }

        public List<PmcStandardInfo>? WasherStandard { get; set; }
    }
}
