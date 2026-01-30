namespace PMCSystem_Backend.Dtos.PipeSpecInfo
{
    public class FullConfiguration
    {
        public List<Object> StandardFileIds { get; set; } = new List<object>();

        public List<StandardFileConfig> StandardFileConfigs { get; set; } = new List<StandardFileConfig>();

        public List<StandardFileConfiguration> Configurations { get; set; } = new List<StandardFileConfiguration>();

        public List<DuplicateRangeDefault> DuplicateRangeDefaults { get; set; } = new List<DuplicateRangeDefault>();
    }
}
