namespace PMCSystem_Backend.Dtos.DesignRule
{
    public class CreateCodeListTableCatelogDto
    {
        public string CodeListTableName { get; set; } = null!;

        public bool IsUserDefined { get; set; }

        public string Major { get; set; } = null!;
    }
}
