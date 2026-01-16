using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using PMCSystem_Backend.Data;
using PMCSystem_Backend.MappingProfiles;
using PMCSystem_Backend.Services.Impletation;
using PMCSystem_Backend.Services.Interface;
using PMCSystem_Backend.Common.Middelswares;
using Serilog;
using System.Text.Json;
using PMCSystem_Backend.Services.Interfaces;
using PMCSystem_Backend.Services.Implementations;

// 初始化Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateBootstrapLogger();

try
{
    Log.Information("正在启动 Web 应用程序...");

    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
        .ReadFrom.Configuration(context.Configuration) // 从配置文件读取
        .ReadFrom.Services(services) // 从DI容器注入配置的Sink和Enricher
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day));
    //builder.Services.AddSerilog();

    // Add services to the container.
    // 注册业务服务已移动到下方

    // 注册自定义服务为Scoped生命周期，每个请求创建一个新实例
    builder.Services.AddScoped<IExampleService, ExampleService>();
    builder.Services.AddScoped<IDspSpmcDictPipingBendService, DspSpmcDictPipingBendService>();
    builder.Services.AddScoped<IDspSpmcDictPipingBendDataService, DspSpmcDictPipingBendDataService>();
    builder.Services.AddScoped<IPmcSpecService, PmcSpecService>();
    builder.Services.AddScoped<IWallThicknessCodeConvertedService, WallThicknessCodeConvertedService>();
    builder.Services.AddScoped<IS3dDictWallThicknessService, S3dDictWallThicknessService>();
    builder.Services.AddScoped<IS3dRuleShortCodeHierarchyRuleService, S3dRuleShortCodeHierarchyRuleService>();

    builder.Services.AddControllers();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowVueFrontend", policy =>
        {
            policy.WithOrigins(
                "http://localhost:5173",   // Vite 默认端口
                "http://localhost:3000",   // 一些前端工具默认端口
                "http://localhost:8080"    // Vue CLI 默认端口
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
    });

    // 注册 DbContext (使用 PmcTestContext 作为主上下文)
    builder.Services.AddDbContext<PmcTestContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    // 注册 MyDbContext
    builder.Services.AddDbContext<MyDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


    // Swagger配置，API文档
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // 注册 AutoMapper
    builder.Services.AddAutoMapper(typeof(ExampleProfile));
    // builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    builder.Services.AddControllers(options =>
    {
        options.Filters.Add(new ProducesAttribute("application/json"));
    });

    // 配置JSON序列化
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            // 统一使用小驼峰命名
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            // 空值属性可选
            options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            // 时间格式
            options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        });

    // 注册日志服务
    builder.Services.AddLogging();


    var app = builder.Build();



    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            // 设置Swagger UI 的根路径为 /swagger
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            options.RoutePrefix = "swagger";        // 访问http://localhost:xxxx/swagger 即可打开UI
        });
    }
    else
    {
        // 生产环境也可以开启，根据需求关闭或限制访问
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.RoutePrefix = "swagger";
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        });
    }
    // 配置中间件

    // Configure the HTTP request pipeline.

    // 注册异常处理中间件（应放在管道最前面，以捕获所有异常）
    app.UseMiddleware<ExceptionMiddleware>();

    app.UseHttpsRedirection();

    app.UseCors("AllowVueFrontend");

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "应用程序启动失败");
}
finally
{
    Log.CloseAndFlush();
}
