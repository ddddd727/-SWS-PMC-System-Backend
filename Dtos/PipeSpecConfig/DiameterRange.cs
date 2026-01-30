namespace PMCSystem_Backend.Dtos.PipeSpecConfig
{
    /// <summary>
    /// 通径范围内容
    /// </summary>
    public class DiameterRange
    {
        double DiameterMin { get; set; }

        double DiameterMax { get; set; }

        string DiameterUnit { get; set; }
    }
}
