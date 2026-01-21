using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using PMCSystem_Backend.Data;
using PMCSystem_Backend.MappingProfiles;
using PMCSystem_Backend.Services.Impletation;
using PMCSystem_Backend.Services.Interface;
using PMCSystem_Backend.Data;
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

    // ע���Զ������Scoped�������ڣ� ÿ�����󴴽�һ����ʵ����
    builder.Services.AddScoped<IExampleService, ExampleService>();
    builder.Services.AddScoped<IPipeLimitRuleService, PipeLimitRuleService>();
    builder.Services.AddScoped<IMainMaterialRuleService, MainMaterialRuleService>();
    builder.Services.AddScoped<IFlangeRuleService, FlangeRuleService>();
    builder.Services.AddScoped<IPmcCodeService, PmcCodeService>();

    builder.Services.AddControllers();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowVueFrontend", policy =>
        {
            policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
    });

    // ���� DbContextע��
    builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddDbContext<PmcTestContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PMC0120")));


    // Swagger���ã�API�ĵ���
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // ע�� AutoMapper
    builder.Services.AddAutoMapper(typeof(ExampleProfile), typeof(RuleProfiles));
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












