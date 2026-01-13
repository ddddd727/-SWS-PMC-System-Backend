namespace PMCSystem_Backend.Models
{
    public class PmcStandardInfoDto
    {
        public string StandardName { get; set; }

        public string StandardType { get; set; }

        public string NPD_min { get; set; }

        public string NPD_max { get; set; }

        public string NPD_unit { get; set; }

        public List<string> Materials { get; set; }
    }
}
