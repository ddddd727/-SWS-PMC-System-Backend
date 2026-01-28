namespace PMCSystem_Backend.Models
{
    public class S3dCommonCodeListValueDto
    {
        public int CodeListNumber { get; set; }
        public string ShortStringValue { get; set; } = null!;
        public string LongStringValue { get; set; } = null!;
    }
}
