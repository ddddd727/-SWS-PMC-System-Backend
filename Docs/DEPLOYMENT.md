# PMCSystem_Backend 部署文档

## 一、环境要求

- .NET 8.0 SDK / Runtime
- SQL Server（支持多个 DbContext：PmcContext、PmcContextCky、PmcContextLr、SpecContext）

## 二、数据库迁移

本项目当前**无 Migrations 目录**，数据库采用以下方式之一管理：

### 方式 A：Code First（若需启用）

```bash
# 安装 EF Core 工具（若未安装）
dotnet tool install --global dotnet-ef

# 创建迁移
dotnet ef migrations add InitialCreate -c PmcContext

# 应用迁移
dotnet ef database update -c PmcContext
```

若存在多个 DbContext，需按需为每个 Context 创建并应用迁移。

### 方式 B：数据库优先

使用 DBA 提供的建库脚本或手动还原数据库，确保生产库结构与开发环境一致。

## 三、环境变量

生产部署时务必设置：

| 变量名 | 说明 |
|--------|------|
| `ASPNETCORE_ENVIRONMENT` | 设为 `Production` |
| `ConnectionStrings__DefaultConnection` | SQL Server 连接字符串，覆盖 appsettings.Production.json 中的占位值 |

示例（SQL 认证）：

```
Server=your-server;Database=PMC;User Id=your_user;Password=your_password;TrustServerCertificate=True
```

## 四、构建与发布

```bash
dotnet publish -c Release -o ./publish
```

## 五、部署检查清单

- [ ] 设置 `ASPNETCORE_ENVIRONMENT=Production`
- [ ] 通过环境变量配置 `ConnectionStrings__DefaultConnection`
- [ ] 将 Program.cs 中 CORS 的 `https://your-domain.com` 替换为实际前端域名
- [ ] 确认 `Docs/TemplateFile/` 模板目录随部署一起发布
- [ ] 确认应用对 `Logs/` 目录有写权限
- [ ] 若使用 Nginx/IIS 反向代理，确认已配置 `ForwardedHeaders`
- [ ] 验证健康检查端点：`GET /health`
- [ ] 验证数据库连接及核心 API

## 六、部署方式

### IIS

1. 安装 ASP.NET Core Hosting Bundle
2. 创建站点，应用程序池设为“无托管代码”
3. 设置环境变量（IIS 应用池高级设置或 web.config）
4. 确保 `Logs` 目录可写

### Kestrel + 反向代理

1. 使用 `dotnet publish` 发布
2. 以前台或 systemd 服务方式运行 `dotnet PMCSystem_Backend.dll`
3. 使用 Nginx/IIS 做反向代理，转发到 Kestrel 监听端口
4. 反向代理需传递 `X-Forwarded-For`、`X-Forwarded-Proto` 头
