namespace PMCSystem_Backend.Models
{
    /// <summary>
    /// Pmc标准配置信息
    /// </summary>
    public class PmcStandardInfo
    {
        // 标准名称
        public string StandardName { get; set; }

        // 标准类型（ComponentType）
        public string StandardType { get; set; }

        // 通径范围
        public DiameterRange DiameterRange { get; set; }

        // 部件材料
        public string Material { get; set; }

        // 是否是默认匹配
        public bool IsDefault { get; set; }

        // 默认匹配的重叠通径范围
        public List<DiameterRange>? OverlapRange { get; set; }
    }
}
