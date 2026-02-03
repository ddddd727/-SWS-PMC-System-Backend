namespace PMCSystem_Backend.Dtos.PipeSpecConfig
{
    /// <summary>
    /// 部件类型信息，用于规格书配置时的部件类型选择
    /// </summary>
    public class ComponentTypeInfoDto
    {
        public string ComponentTypeName { get; set; } = string.Empty;

        public string ComponentTypeDescription { get; set; } = string.Empty;
    }
}
