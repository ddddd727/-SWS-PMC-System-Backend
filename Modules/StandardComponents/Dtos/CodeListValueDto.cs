namespace PMCSystem_Backend.Modules.StandardComponents.Dtos
{
    public class CodeListValueDto
    {
        public string ShortStringValue { get; set; } = null!;

        public string LongStringValue { get; set; } = null!;

        public int CodeListNumber { get; set; }

        public int Status { get; set; }
    }
}
