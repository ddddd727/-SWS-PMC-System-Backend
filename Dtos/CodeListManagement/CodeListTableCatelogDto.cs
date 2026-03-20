namespace PMCSystem_Backend.Dtos.CodeListManagement
{
    public class CodeListTableCatelogDto
    {
        public int Id { get; set; }

        public string CodeListTableName { get; set; } = null!;

        public bool IsUserDefined { get; set; }

        public string Major { get; set; } = null!;
    }
}
