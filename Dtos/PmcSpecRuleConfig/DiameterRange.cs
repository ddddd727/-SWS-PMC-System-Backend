namespace PMCSystem_Backend.Dtos.PmcSpecRuleConfig
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
