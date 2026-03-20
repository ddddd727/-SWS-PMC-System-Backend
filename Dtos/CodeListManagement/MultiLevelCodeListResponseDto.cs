namespace PMCSystem_Backend.Dtos.CodeListManagement
{
    public class MultiLevelCodeListResponseDto
    {
        public List<CodeListLevelData> LevelData { get; set; } = new();
    }

    public class CodeListLevelData
    {
        public string? Level1ShortDesc { get; set; }

        public string? Level1LongDesc { get; set; }

        public int? Level1CodeNum { get; set; }

        public int? Level1Status { get; set; }

        public string? Level2ShortDesc { get; set; }

        public string? Level2LongDesc { get; set; }

        public int? Level2CodeNum { get; set; }

        public int? Level2Status { get; set; }

        public string? Level3ShortDesc { get; set; }

        public string? Level3LongDesc { get; set; }

        public int? Level3CodeNum { get; set; }

        public int? Level3Status { get; set; }

        public string? Level4ShortDesc { get; set; }

        public string? Level4LongDesc { get; set; }

        public int? Level4CodeNum { get; set; }

        public int? Level4Status { get; set; }

        public string? Level5ShortDesc { get; set; }

        public string? Level5LongDesc { get; set; }

        public int? Level5CodeNum { get; set; }

        public int? Level5Status { get; set; }
    }
}
