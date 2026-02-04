namespace PMCSystem_Backend.Dtos.PipeSpecConfig
{
    /// <summary>
    /// Pmc标准配置信息
    /// </summary>
    public class PmcStandardInfo
    {
        /// <summary>标准名称</summary>
        public string? StandardName { get; set; }

        /// <summary>标准类型（ComponentType）</summary>
        public string? StandardType { get; set; }

        /// <summary>通径范围</summary>
        public DiameterRange? DiameterRange { get; set; }

        /// <summary>部件材料</summary>
        public string? Material { get; set; }

        /// <summary>是否是默认匹配</summary>
        public bool? IsDefault { get; set; }

        /// <summary>默认匹配的重叠通径范围</summary>
        public List<DiameterRange>? OverlapRange { get; set; }
    }
}
