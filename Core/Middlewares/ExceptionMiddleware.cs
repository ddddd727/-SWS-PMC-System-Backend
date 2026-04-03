using PMCSystem_Backend.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace PMCSystem_Backend.Core.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _environment;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // ... 异常处理逻辑 ...

            var response = new ApiResponse<object>
            {
                Code = 500,
                Message = _environment.IsDevelopment()
                    ? exception.Message
                    : "系统内部错误",
                TraceId = context.TraceIdentifier
            };

            // 开发环境返回详细错误信息
            if (_environment.IsDevelopment())
            {
                response.Data = new
                {
                    exception.StackTrace,
                    exception.Source,
                    InnerException = exception.InnerException?.Message
                };
            }

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = _environment.IsDevelopment()
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);

            // 日志记录 - 生产环境记录更详细
            if (_environment.IsProduction())
            {
                _logger.LogError(exception,
                    "Unhandled exception: {Message}, TraceId: {TraceId}",
                    exception.Message, context.TraceIdentifier);
            }
        }
    }
}
