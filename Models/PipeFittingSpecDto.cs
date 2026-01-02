namespace PMCSystem_Backend.Models
{
    public class PipeFittingSpecDto
    {
        // 标准名字
        public string StandardName { get; set; }

        // 标准描述
        public string StandardDescription { get; set; }

        // 标准内材料
        public List<string> MaterialList { get; set; }
    }
}
