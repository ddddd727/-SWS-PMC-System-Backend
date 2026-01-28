namespace PMCSystem_Backend.Models
{
    public class S3dDictPipingBendDataDto
    {
        public int Id { get; set; }
        public decimal? OutSideDiameter { get; set; }
        public string? OutSideDiameterUnit { get; set; }
        public decimal? HeaderClampLength { get; set; }
        public decimal? TailClampLength { get; set; }
        public int? MachineNum { get; set; }
        public bool? Status { get; set; }
    }

    public class CreateS3dDictPipingBendDataDto
    {
        public decimal? OutSideDiameter { get; set; }
        public string? OutSideDiameterUnit { get; set; }
        public decimal? HeaderClampLength { get; set; }
        public decimal? TailClampLength { get; set; }
        public int? MachineNum { get; set; }
        public bool? Status { get; set; }
    }

    public class UpdateS3dDictPipingBendDataDto : CreateS3dDictPipingBendDataDto
    {
        public int Id { get; set; }
    }
}
