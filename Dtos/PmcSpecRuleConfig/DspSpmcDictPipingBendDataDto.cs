namespace PMCSystem_Backend.Models
{
    public class DspSpmcDictPipingBendDataDto
    {
        public int Id { get; set; }
        public decimal? OutSideDiameter { get; set; }
        public string? OutSideDiameterUnit { get; set; }
        public decimal? HeaderClampLength { get; set; }
        public decimal? TailClampLength { get; set; }
        public int? MachineNum { get; set; }
        public bool? Status { get; set; }
    }

    public class CreateDspSpmcDictPipingBendDataDto
    {
        public decimal? OutSideDiameter { get; set; }
        public string? OutSideDiameterUnit { get; set; }
        public decimal? HeaderClampLength { get; set; }
        public decimal? TailClampLength { get; set; }
        public int? MachineNum { get; set; }
        public bool? Status { get; set; }
    }

    public class UpdateDspSpmcDictPipingBendDataDto : CreateDspSpmcDictPipingBendDataDto
    {
        public int Id { get; set; }
    }
}
