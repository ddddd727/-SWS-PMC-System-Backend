using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using PMCSystem_Backend.Data;
using PMCSystem_Backend.MappingProfiles;
using PMCSystem_Backend.Services.Impletation;
using PMCSystem_Backend.Services.Interface;
using Serilog;
using System.Text.Json;

// 配置Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateBootstrapLogger();

try
{
    Log.Information("正在启动 Web 应用程序主机...");

    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
        .ReadFrom.Configuration(context.Configuration) // 从配置文件读取
        .ReadFrom.Services(services) // 允许从DI容器注入服务到Sink或Enricher
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day));
    //builder.Services.AddSerilog();

    // Add services to the container.

    // 注册自定义服务（Scoped生命周期： 每个请求创建一个新实例）
    builder.Services.AddScoped<IExampleService, ExampleService>();

    builder.Services.AddControllers();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowVueFronted", policy =>
        {
            policy.WithOrigins("http://localhost:3000")     // Vue默认端口，生产时替换为实际前端URL
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
    });

    // 添加 DbContext注册
    builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


    // Swagger配置（API文档）
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
            // 忽略空值（可选）
            options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            // 时间格式
            options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        });

    // 注册异常中间件所需的服务
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
        // 生产环境可关闭或者限制访问
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.RoutePrefix = "swagger";
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        });
    }
    // 依赖注入配置

    // Configure the HTTP request pipeline.

    app.UseHttpsRedirection();

    app.UseCors("AllowVueFrontend");

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch(Exception ex)
{
    Log.Fatal(ex, "应用程序启动失败");
}
finally
{
    Log.CloseAndFlush();
}












