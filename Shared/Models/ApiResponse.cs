using Microsoft.Identity.Client;
using System.Text.Json.Serialization;

namespace PMCSystem_Backend.Shared.Models
{
    /// <summary>
    /// API 统一响应模型
    /// </summary>
    /// <typeparam name="T"> 数据类型 </typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// 响应代码 （200 = 成功，其他 = 错误码）
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }

        /// <summary>
        /// 响应信息
        /// </summary>
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        /// <summary>
        ///  响应数据
        /// </summary>
        [JsonPropertyName("data")]
        public T? Data { get; set; }

        /// <summary>
        /// 响应时间戳
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 追踪ID（用于日志追踪）
        /// </summary>
        [JsonPropertyName("traceId")]
        public string? TraceId   { get; set; }

        // 成功响应快捷方法
        public static ApiResponse<T> Success(T data, string message = "操作成功")
        {
            return new ApiResponse<T>
            {
                Code = 200,
                Message = message,
                Data = data,
            };
        }

        // 失败响应快捷方法
        public static ApiResponse<T> Fail(int code, string message, T? data = default)
        {
            return new ApiResponse<T>
            {
                Code = code,
                Message = message,
                Data = data
            };
        }

        // 重载 + 运算符， 便于链式调用
        public static implicit operator ApiResponse<T>(T data) => Success(data);
    }

    /// <summary>
    /// API 统一响应模型（无数据）
    /// </summary>
    public class ApiResponse
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("traceId")]
        public string? TraceId { get; set; }

        public static ApiResponse Success(string message = "操作成功")
        {
            return new ApiResponse
            {
                Code = 200,
                Message = message
            };
        }

        public static ApiResponse Fail(int code, string message)
        {
            return new ApiResponse
            {
                Code = code,
                Message = message
            };
        }
    }
}
