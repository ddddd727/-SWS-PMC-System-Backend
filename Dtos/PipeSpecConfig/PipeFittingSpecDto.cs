namespace PMCSystem_Backend.Dtos.PipeSpecConfig
{
    /// <summary>
    /// 用于标准配置页面的选择的标准和材料信息
    /// </summary>
    public class PipeFittingSpecDto
    {
        /// <summary>
        /// 标准名称
        /// </summary>
        public string StandardName { get; set; } = string.Empty;

        /// <summary>
        /// 标准内材料列表
        /// </summary>
        public List<string> MaterialList { get; set; } = new List<string>();
    }
}
