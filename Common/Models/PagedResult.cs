using System.ComponentModel.DataAnnotations;

namespace PMCSystem_Backend.Common.Models
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        // 转换为ApiResponse
        public ApiResponse<PagedResult<T>> ToApiResponse(string message = "查询成功")
        {
            return ApiResponse<PagedResult<T>>.Success(this, message);
        }
    }

    public class PagedRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "页码必须大于0")]
        public int PageIndex { get; set; } = 1;

        [Range(1, 100, ErrorMessage ="每页数量必须在1-100之间")]
        public int PageSize { get; set; } = 20;

        public string SortBy { get; set; }

        public bool IsDescending { get; set; } = true;

        public string Keyword { get; set; }
    }
}
