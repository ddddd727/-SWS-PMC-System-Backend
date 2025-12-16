using PMCSystem_Backend.Services.Impletation;
using PMCSystem_Backend.Services.Interface;

var builder = WebApplication.CreateBuilder(args);


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


// Swagger配置（API文档）
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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






