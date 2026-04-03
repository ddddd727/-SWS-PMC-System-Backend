namespace PMCSystem_Backend.Modules.StandardComponents.Dtos
{
    public class CodeListCombinedResponseDto
    {
        public int Count { get; set; }

        public string? Level1 { get; set; }

        public string? Level2 { get; set; }

        public string? Level3 { get; set; }

        public string? Level4 { get; set; }

        public string? Level5 { get; set; }

        public List<CodeListLevelData> LevelData { get; set; } = new();
    }
}
