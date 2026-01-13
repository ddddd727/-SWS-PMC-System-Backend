namespace PMCSystem_Backend.Services.Interfaces
{
    /// <summary>
    /// 该服务主要用于对建库数据表的自生成
    /// </summary>
    public interface ICatalogTableService
    {
        /// <summary>
        /// 生成对应的管系部件过滤表
        /// </summary>
        public void GeneratePipeCommodityFilterTable();


        /// <summary>
        /// 生成Part的对应模板表
        /// </summary>
        /// <param name="partType"> 部件类型 </param>
        public void GeneratePartCatalogTable(string partType);


    }
}
