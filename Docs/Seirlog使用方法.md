#### 步骤4: 使用方法（日志写入）
Serilog 完全兼容 ASP.NET Core 的 ILogger<T>，无需额外代码。

+ **在控制器中**：C#

```plain
private readonly ILogger<ExampleController> _logger;

public ExampleController(ILogger<ExampleController> logger)
{
    _logger = logger;
}

[HttpGet]
public IActionResult Get()
{
    _logger.LogInformation("获取示例数据，开始执行");
    _logger.LogWarning("这是一个警告示例 {UserId}", 123);
    try
    {
        // 业务代码
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "获取数据失败");
    }
    return Ok("Success");
}
```

+ **结构化日志示例**（强大之处）：C#

```plain
_logger.LogInformation("用户 {UserId} 执行操作 {Action}，结果 {Result}", userId, "Login", success);
```

输出 JSON 如：{"UserId": 123, "Action": "Login", "Result": true}，便于查询。

+ **直接使用 Serilog 静态 Log**（全局）：C#

```plain
Serilog.Log.Information("全局日志示例");
```

#### 步骤5: 测试与查看日志
1. 运行项目（dotnet run）。
2. 访问接口，观察控制台输出结构化日志。
3. 检查 Logs/ 文件夹下的 log-20251216.txt（JSON 格式）。
4. （可选）安装 Seq（[https://datalust.co/seq），添加](https://datalust.co/seq%EF%BC%89%EF%BC%8C%E6%B7%BB%E5%8A%A0?referrer=grok.com) Sink 后可视化查询日志。

