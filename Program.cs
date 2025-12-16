var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueFronted", policy =>
    {
        policy.WithOrigins("http://localhost:3000")     // Vue默认端口，生产时替换为实际前端URL
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

app.UseCors("AllowVueFrontend");


// 依赖注入配置


// Swagger配置（API文档）
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
app.UseSwagger();
app.UseSwaggerUI();
