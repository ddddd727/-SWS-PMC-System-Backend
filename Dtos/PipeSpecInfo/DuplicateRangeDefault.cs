namespace PMCSystem_Backend.Dtos.PipeSpecInfo
{
    /// <summary>
    /// 重复NPD范围的默认配置
    /// </summary>
    public class DuplicateRangeDefault
    {
        public double OverlapMin { get; set; }

        public double OverlapMax { get; set; }

        public object DefaultStandardFileId { get; set; }

        public string  DefaultStandardFileName { get; set; }

        public List<NPDRange> Ranges { get; set; } = new List<NPDRange>();

        public List<Object> StandardFiles { get; set; } = new List<Object>();

        public string RangeKey { get; set; }
    }
}
