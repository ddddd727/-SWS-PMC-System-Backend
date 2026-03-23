namespace PMCSystem_Backend.Modules.DesignRules.Dtos
{
    public class S3dCommonCodeListValueDto
    {
        public int CodeListNumber { get; set; }
        public string ShortStringValue { get; set; } = null!;
        public string LongStringValue { get; set; } = null!;
    }
}
