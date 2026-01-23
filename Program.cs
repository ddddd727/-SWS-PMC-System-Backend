using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using PMCSystem_Backend.Data;
using PMCSystem_Backend.MappingProfiles;
using PMCSystem_Backend.Services.Implementations;
using PMCSystem_Backend.Services.Interfaces;
using PMCSystem_Backend.Common.Middelswares;
using Serilog;
using System.Text.Json;

// ����Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateBootstrapLogger();

try
{
    Log.Information("�������� Web Ӧ�ó�������...");

    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
        .ReadFrom.Configuration(context.Configuration) // �������ļ���ȡ
        .ReadFrom.Services(services) // ������DI����ע�����Sink��Enricher
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day));
    //builder.Services.AddSerilog();

    // Add services to the container.

    // 注册自定义服务（Scoped生命周期： 每次请求创建一个实例）
    builder.Services.AddScoped<IS3dRuleAb2b3c2Service, S3dRuleAb2b3c2Service>();
    builder.Services.AddScoped<IS3dRuleC1c2Service, S3dRuleC1c2Service>();
    builder.Services.AddScoped<IS3dRuleB1b2b3dService, S3dRuleB1b2b3dService>();
    builder.Services.AddScoped<IS3dCodeAb2b3c2ViewService, S3dCodeAb2b3c2ViewService>();
    builder.Services.AddScoped<IS3dCodeB1b2b3dViewService, S3dCodeB1b2b3dViewService>();
    builder.Services.AddScoped<IS3dCodeC1c2ViewService, S3dCodeC1c2ViewService>();
    builder.Services.AddScoped<IS3dCodePipingClassViewService, S3dCodePipingClassViewService>();
    builder.Services.AddScoped<IS3dCodeMaterialsCategoryPipingStandardService, S3dCodeMaterialsCategoryPipingStandardService>();
    builder.Services.AddScoped<IS3dCodeMaterialsCategoryScheduleThicknessService, S3dCodeMaterialsCategoryScheduleThicknessService>();
    builder.Services.AddScoped<IS3dCodeFlangeStandPressureRatingService, S3dCodeFlangeStandPressureRatingService>();
    builder.Services.AddScoped<IS3dCodePipingStandardMaterialsGradeService, S3dCodePipingStandardMaterialsGradeService>();
    builder.Services.AddScoped<IS3dCodePipingStandardPressureRatingService, S3dCodePipingStandardPressureRatingService>();
    // Dsp services removed

    builder.Services.AddControllers();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowVueFrontend", policy =>
        {
            policy.WithOrigins("http://localhost:3000")     // VueĬ϶˿ڣʱ滻ΪʵǰURL
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
    });

    //  DbContextע
    builder.Services.AddDbContext<SpecContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


    // Swagger���ã�API�ĵ���
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // ע AutoMapper
    builder.Services.AddAutoMapper(typeof(S3dMappingProfile));
    // builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    builder.Services.AddControllers(options =>
    {
        options.Filters.Add(new ProducesAttribute("application/json"));
    });

    // ����JSON���л�
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            // ͳһʹ��С�շ�����
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            // ���Կ�ֵ����ѡ��
            options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            // ʱ���ʽ
            options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        });

    // ע���쳣�м������ķ���
    builder.Services.AddLogging();


    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            // ����Swagger UI �ĸ�·��Ϊ /swagger
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            options.RoutePrefix = "swagger";        // ����http://localhost:xxxx/swagger ���ɴ�UI
        });
    }
    else
    {
        // ���������ɹرջ������Ʒ���
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.RoutePrefix = "swagger";
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        });
    }
    // ����ע������

    // Configure the HTTP request pipeline.

    app.UseHttpsRedirection();

    app.UseMiddleware<ExceptionMiddleware>();

    app.UseCors("AllowVueFrontend");

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch(Exception ex)
{
    Log.Fatal(ex, "Ӧ�ó�������ʧ��");
}
finally
{
    Log.CloseAndFlush();
}












