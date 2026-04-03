using System.Text.Json.Serialization;

namespace PMCSystem_Backend.Modules.DesignRules.Dtos
{
    public class S3dCodePlainPipingGenericDataDto
    {
        public int Id { get; set; }
        public double? NominalPipingDiameter { get; set; }
        public string? NominalDiameterUnits { get; set; }
        public int? EndStandardCl { get; set; }
        public string? EndStandard { get; set; }
        public int? ScheduleThicknessCl { get; set; }
        public string? ScheduleThickness { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public string? PipingOutsideDiameter { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public string? WallThickness { get; set; }
        public bool? Status { get; set; }
    }
}
