using System;
using System.Collections.Generic;

namespace PMCSystem_Backend.Entities
{
    public partial class S3dCodeShortCodeMap
    {
        public int Id { get; set; }

        public int ComponentTypeId { get; set; }

        public string ComponentTypeName { get; set; } = null!;

        public string ShortCode { get; set; } = null!;
    }
}
