namespace PMCSystem_Backend.Dtos.CodeListManagement
{
    public class CodeListValueDto
    {
        public string ShortStringValue { get; set; } = null!;

        public string LongStringValue { get; set; } = null!;

        public int CodeListNumber { get; set; }

        public int Status { get; set; }
    }
}
