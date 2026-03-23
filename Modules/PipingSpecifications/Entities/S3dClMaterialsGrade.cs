using System;

namespace PMCSystem_Backend.Modules.PipingSpecifications.Entities
{
    /// <summary>
    /// 视图 S3D_CL_MaterialsGrade 对应的实体，仅用于只读查询材料牌号。
    /// </summary>
    public partial class S3dClMaterialsGrade
    {
        public int CodeListNumber { get; set; }

        public string? ShortStringValue { get; set; }

        public string? LongStringValue { get; set; }
    }
}

