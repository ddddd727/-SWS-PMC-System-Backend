using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Data;
using PMCSystem_Backend.MappingProfiles;
using PMCSystem_Backend.MappingProfiles.PipeSpecMappers;
using PMCSystem_Backend.Common.Middelswares;
using Serilog;
using System.Text.Json;
using PMCSystem_Backend.Services.Interfaces;
using PMCSystem_Backend.Services.Implementations;
using PMCSystem_Backend.Services.Impletation;
using PMCSystem_Backend.Services.Interface;
using OfficeOpenXml;

// 注册编码提供程序，确保 EPPlus 处理 ZIP/xlsx 时正确解析编码（修复导出 Excel 无法打开问题）
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
// 设置 EPPlus 许可证上下文（必须在创建任何 ExcelPackage 之前设置）
ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 非商业用途，如果是商业用途请使用 LicenseContext.Commercial

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

    builder.Services.AddScoped<IDictService, DictService>();

    // Add services to the container.
    // 注册业务服务已移动到下方


    builder.Services.AddScoped<IPipeLimitRuleService, PipeLimitRuleService>();
    builder.Services.AddScoped<IMainMaterialRuleService, MainMaterialRuleService>();
    builder.Services.AddScoped<IFlangeRuleService, FlangeRuleService>();
    builder.Services.AddScoped<IPmcCodeService, PmcCodeService>();
    // 注册自定义服务为Scoped生命周期，每个请求创建一个新实例
    builder.Services.AddScoped<IDspSpmcDictPipingBendDataService, DspSpmcDictPipingBendDataService>();
    builder.Services.AddScoped<IWallThicknessCodeConvertedService, WallThicknessCodeConvertedService>();
    builder.Services.AddScoped<IPipingBendParameterCodeConvertedService, PipingBendParameterCodeConvertedService>();
    builder.Services.AddScoped<IS3dDictWallThicknessService, S3dDictWallThicknessService>();
    builder.Services.AddScoped<IS3dRuleShortCodeHierarchyRuleService, S3dRuleShortCodeHierarchyRuleService>();
    builder.Services.AddScoped<IS3dRulePipingBendParameterService, S3dRulePipingBendParameterService>();
    builder.Services.AddScoped<IS3dCommonCodeListValueService, S3dCommonCodeListValueService>();

    // 补全缺失的服务注册
    builder.Services.AddScoped<IS3dCodeAb2b3c2ViewService, S3dCodeAb2b3c2ViewService>();
    builder.Services.AddScoped<IS3dCodeB1b2b3dViewService, S3dCodeB1b2b3dViewService>();
    builder.Services.AddScoped<IS3dCodeC1c2ViewService, S3dCodeC1c2ViewService>();
    builder.Services.AddScoped<IS3dCodeFlangeStandPressureRatingService, S3dCodeFlangeStandPressureRatingService>();
    builder.Services.AddScoped<IS3dCodeMaterialsCategoryPipingStandardService, S3dCodeMaterialsCategoryPipingStandardService>();
    builder.Services.AddScoped<IS3dCodeMaterialsCategoryScheduleThicknessService, S3dCodeMaterialsCategoryScheduleThicknessService>();
    builder.Services.AddScoped<IS3dCodePipingClassViewService, S3dCodePipingClassViewService>();
    builder.Services.AddScoped<IS3dCodePipingStandardMaterialsGradeService, S3dCodePipingStandardMaterialsGradeService>();
    builder.Services.AddScoped<IS3dCodePipingStandardPressureRatingService, S3dCodePipingStandardPressureRatingService>();
    builder.Services.AddScoped<IS3dRuleAb2b3c2Service, S3dRuleAb2b3c2Service>();
    builder.Services.AddScoped<IS3dRuleB1b2b3dService, S3dRuleB1b2b3dService>();
    builder.Services.AddScoped<IS3dRuleC1c2Service, S3dRuleC1c2Service>();

    // 注册服务层的接口与实现
    // 注册自定义服务为Scoped生命周期，每个请求创建一个新实例
    builder.Services.AddScoped<IPmcSpecService, PmcSpecService>();
    builder.Services.AddScoped<ICodelistService, CodelistService>();
    builder.Services.AddScoped<IS3dRulePmcDataService, S3dRulePmcDataService>();
    builder.Services.AddScoped<ITemplatePreviewService>(provider =>
    {
        var templateBasePath = builder.Configuration.GetValue<string>("TemplateBasePath") ?? "Templates";
        var pmcSpecService = provider.GetRequiredService<IPmcSpecService>();
        var logger = provider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<TemplatePreviewService>>();
        return new TemplatePreviewService(templateBasePath, pmcSpecService, logger);
    });

    // 注册管系规格配置映射器
    builder.Services.AddScoped<IPipeSpecConfigMapper, PipeSpecConfigMapper>();

    builder.Services.AddControllers();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowVueFrontend", policy =>
        {
            policy.WithOrigins(
                "http://localhost:5173",   // Vite 默认端口
                "http://localhost:3000",   // 一些前端工具默认端口
                "http://localhost:8080"    // Vue 
                                           // CLI 默认端口
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
    });

    //  DbContext
    builder.Services.AddDbContext<SpecContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    // 注册多个 DbContext
    builder.Services.AddDbContext<PmcContextCky>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddDbContext<PmcContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddDbContext<PmcContextLr>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));




    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // ע AutoMapper
    builder.Services.AddAutoMapper(typeof(RuleProfiles));
    // builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    builder.Services.AddControllers(options =>
    {
        options.Filters.Add(new ProducesAttribute("application/json"));
    });

    builder.Configuration.AddJsonFile("Configs/dicts.json", optional: true, reloadOnChange: true);

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

    // 注册异常处理中间件（应放在管道最前面，以捕获所有异常）
    app.UseMiddleware<ExceptionMiddleware>();

    // app.UseHttpsRedirection();

    app.UseMiddleware<ExceptionMiddleware>();

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
