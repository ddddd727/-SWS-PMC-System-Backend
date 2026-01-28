using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Common.Enums;
using PMCSystem_Backend.Common.Models;

namespace PMCSystem_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ApiControllerBase : ControllerBase
    {
        /// <summary>
        /// 获取当前请求的TraceId
        /// </summary>
        protected string TraceId => HttpContext.TraceIdentifier;

        /// <summary>
        /// 成功响应（有数据）
        /// </summary>
        protected IActionResult Success<T>(T data, string message = "操作成功")
        {
            var response = ApiResponse<T>.Success(data, message);
            response.TraceId = TraceId;
            return Ok(response);
        }

        /// <summary>
        /// 成功响应（无数据）
        /// </summary>
        protected IActionResult Success(string message = "操作成功")
        {
            var response = ApiResponse.Success(message);
            response.TraceId = TraceId;
            return Ok(response);
        }

        /// <summary>
        /// 创建成功响应（201状态码）
        /// </summary>
        protected IActionResult Created<T>(T data, string message = "创建成功")
        {
            var response = ApiResponse<T>.Success(data, message);
            response.TraceId = TraceId;
            return StatusCode(StatusCodes.Status201Created, response);
        }

        /// <summary>
        /// 分页数据响应
        /// </summary>
        protected IActionResult Paged<T>(PagedResult<T> result, string message = "查询成功")
        {
            var response = result.ToApiResponse(message);
            response.TraceId = TraceId;
            return Ok(response);
        }

        /// <summary>
        /// 失败响应
        /// </summary>
        protected IActionResult Fail(ApiErrorCode errorCode, string? message = null)
        {
            var errorMessage = message ?? GetDefaultErrorMessage(errorCode);
            var response = ApiResponse.Fail((int)errorCode, errorMessage);
            response.TraceId = TraceId;

            return errorCode switch
            {
                ApiErrorCode.Unauthorized => Unauthorized(response),
                ApiErrorCode.Forbidden => Forbid(),
                ApiErrorCode.ResourceNotFound => NotFound(response),
                ApiErrorCode.ValidationError => BadRequest(response),
                _ => BadRequest(response)
            };
        }

        /// <summary>
        /// 验证错误响应（用于ModelState验证失败）
        /// </summary>
        protected IActionResult ValidationFailed()
        {
            var errors = ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .Select(e => new ApiError
                {
                    Field = e.Key,
                    Message = e.Value?.Errors.First().ErrorMessage,
                    ErrorCode = "VALIDATION_ERROR"
                }).ToList();

            var response = ApiResponse<List<ApiError>>.Fail(
                (int)ApiErrorCode.ValidationError,
                "请求参数验证失败",
                errors
            );
            response.TraceId = TraceId;

            return BadRequest(response);
        }

        /// <summary>
        /// 自定义错误响应
        /// </summary>
        protected IActionResult Error<T>(ApiErrorCode errorCode, T data, string? message = null)
        {
            var errorMessage = message ?? GetDefaultErrorMessage(errorCode);
            var response = ApiResponse<T>.Fail((int)errorCode, errorMessage, data);
            response.TraceId = TraceId;

            return BadRequest(response);
        }

        private static string GetDefaultErrorMessage(ApiErrorCode errorCode)
        {
            return errorCode switch
            {
                ApiErrorCode.Unauthorized => "未授权访问",
                ApiErrorCode.Forbidden => "禁止访问",
                ApiErrorCode.ResourceNotFound => "资源不存在",
                ApiErrorCode.ValidationError => "参数验证失败",
                ApiErrorCode.BusinessRuleViolation => "业务规则校验失败",
                ApiErrorCode.DuplicateResource => "资源已存在",
                _ => "操作失败"
            };
        }

        /// <summary>
        /// 自动处理ModelState验证
        /// </summary>
        protected IActionResult? AutoValidate()
        {
            if (!ModelState.IsValid)
            {
                return ValidationFailed();
            }
            return null;
        }
    }
}
