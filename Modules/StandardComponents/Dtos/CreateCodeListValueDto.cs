namespace PMCSystem_Backend.Modules.StandardComponents.Dtos
{
    public class CreateCodeListValueDto
    {
        public string CodeListTableName { get; set; } = null!;
        public string ParentShortStringValue { get; set; } = null!;
        public string ShortStringValue { get; set; } = null!;
        public string LongStringValue { get; set; } = null!;
        public int CodeListNumber { get; set; }
        public bool Status { get; set; } = true;
    }
}