namespace PMCSystem_Backend.Common.Constants
{
    /// <summary>
    /// 管系规格书配置状态常量
    /// 前端字段对应：英文状态码 + 中文描述
    /// </summary>
    public static class SpecConfigStatus
    {
        /// <summary>待配置 - 用户未对PMC编码对应的规格书进行配置保存或生成时</summary>
        public const string Pending = "pending";

        /// <summary>待审核 - 用户保存或生成规格书后</summary>
        public const string Review = "review";

        /// <summary>已审核 - 接受审核流程通过后</summary>
        public const string Approved = "approved";

        /// <summary>
        /// 规范化为有效的状态码，空或未知时返回 pending
        /// </summary>
        public static string Normalize(string? status)
        {
            if (string.IsNullOrWhiteSpace(status)) return Pending;
            var s = status.Trim().ToLowerInvariant();
            if (s == Pending || s == Review || s == Approved) return s;
            // 兼容旧的中文值
            if (status.Contains("待配置") || status.Contains("pending")) return Pending;
            if (status.Contains("待审核") || status.Contains("review")) return Review;
            if (status.Contains("已审核") || status.Contains("approved")) return Approved;
            return Pending;
        }
    }
}
