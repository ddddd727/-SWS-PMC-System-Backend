namespace PMCSystem_Backend.Models
{
    public class DspSpmcDictPipingBendDto
    {
        public int Id { get; set; }

        public decimal? OutSideDiameter { get; set; }

        public string? OutSideDiameterUnit { get; set; }

        public decimal? HeaderClampLength { get; set; }

        public decimal? TailClampLength { get; set; }
    }

    public class CreateDspSpmcDictPipingBendDto
    {
        public decimal? OutSideDiameter { get; set; }

        public string? OutSideDiameterUnit { get; set; }

        public decimal? HeaderClampLength { get; set; }

        public decimal? TailClampLength { get; set; }
    }

    public class UpdateDspSpmcDictPipingBendDto : CreateDspSpmcDictPipingBendDto
    {
        public int Id { get; set; }
    }
}
