namespace PMCSystem_Backend.Modules.DesignRules.Dtos
{
    public class S3dDictPipingBendDataDto
    {
        public int Id { get; set; }
        public double? OutSideDiameter { get; set; }
        public string? OutSideDiameterUnit { get; set; }
        public double? HeaderClampLength { get; set; }
        public double? TailClampLength { get; set; }
        public int? MaterialsCategoryCl { get; set; }
        public double? BendRadius { get; set; }
        public double? MaxPipeLength { get; set; }
        public string? WallThicknessFrom { get; set; }
        public string? WallThicknessTo { get; set; }
        public int? MachineNum { get; set; }
        public bool? Status { get; set; }
    }

    public class CreateS3dDictPipingBendDataDto
    {
        public double? OutSideDiameter { get; set; }
        public string? OutSideDiameterUnit { get; set; }
        public double? HeaderClampLength { get; set; }
        public double? TailClampLength { get; set; }
        public int? MaterialsCategoryCl { get; set; }
        public double? BendRadius { get; set; }
        public double? MaxPipeLength { get; set; }
        public double? WallThicknessFrom { get; set; }
        public double? WallThicknessTo { get; set; }
        public int? MachineNum { get; set; }
        public bool? Status { get; set; }
    }

    public class UpdateS3dDictPipingBendDataDto : CreateS3dDictPipingBendDataDto
    {
        public int Id { get; set; }
    }
}
