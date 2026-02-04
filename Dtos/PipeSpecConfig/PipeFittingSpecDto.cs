namespace PMCSystem_Backend.Dtos.PipeSpecConfig
{
    /// <summary>
    /// 用于标准配置页面的选择的标准和材料信息
    /// </summary>
    public class PipeFittingSpecDto
    {
        // 标准名字
        public string StandardName { get; set; } = string.Empty;

        // 标准内材料
        public List<string> MaterialList { get; set; } = new List<string>();
    }
}
