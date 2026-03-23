namespace PMCSystem_Backend.Modules.StandardComponents.Dtos
{
    public class CreateCodeListTableCatelogDto
    {
        public string CodeListTableName { get; set; } = null!;

        public bool IsUserDefined { get; set; }

        public string Major { get; set; } = null!;
    }
}
