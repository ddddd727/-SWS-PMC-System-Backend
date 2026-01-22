# PMC System Backend - 配置说明

## 配置文件结构

本项目使用 ASP.NET Core 的多环境配置机制，配置文件分为：

### 1. `appsettings.json` （通用配置，提交到仓库）
包含所有环境共享的基础配置：
- Logging 基本结构
- Serilog 配置模板
- AllowedHosts
- JsonSettings

**注意**：此文件中的 `ConnectionStrings` 使用占位符，不包含真实连接信息。

### 2. `appsettings.Development.json` （开发环境配置，不提交）
包含开发人员本地环境的个性化配置：
- 本地数据库连接字符串
- 开发环境日志级别
- 开发环境特定的 Serilog 配置

**重要**：此文件已添加到 `.gitignore`，不会被提交到仓库。

### 3. `appsettings.Development.json.template` （配置模板，提交到仓库）
新开发人员加入项目时的参考模板。

## 首次设置步骤

1. **复制配置模板**
   ```bash
   cp appsettings.Development.json.template appsettings.Development.json
   ```

2. **修改连接字符串**
   
   打开 `appsettings.Development.json`，修改以下配置：
   
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Data Source=YOUR_SERVER_NAME;Initial Catalog=PMC;Integrated Security=True;TrustServerCertificate=True"
     }
   }
   ```
   
   替换 `YOUR_SERVER_NAME` 为你的 SQL Server 实例名称，例如：
   - `.\\SQLEXPRESS` （SQL Express 默认实例）
   - `localhost` （默认实例）
   - `(localdb)\\MSSQLLocalDB` （LocalDB）

3. **（可选）调整日志配置**
   
   如果需要更详细的日志，可以修改日志级别：
   ```json
   {
     "Logging": {
       "LogLevel": {
         "Default": "Debug",
         "Microsoft.EntityFrameworkCore.Database.Command": "Information"
       }
     }
   }
   ```

## 环境配置优先级

ASP.NET Core 按以下顺序加载配置（后加载的会覆盖先加载的）：
1. `appsettings.json`
2. `appsettings.{Environment}.json` （例如 `appsettings.Development.json`）
3. 环境变量
4. 命令行参数

## 生产环境配置

生产环境建议使用以下方式之一配置敏感信息：

1. **环境变量**（推荐）
   ```bash
   export ConnectionStrings__DefaultConnection="Production connection string"
   ```

2. **Azure App Service 配置**
   在 Azure Portal 的应用程序设置中添加配置

3. **Docker Secrets**
   在 Docker 环境中使用 secrets 管理敏感信息

4. **appsettings.Production.json**（如需要）
   确保通过安全的部署管道注入，不要提交到代码仓库

## 安全注意事项

?? **切勿提交包含真实数据库连接字符串、密码或其他敏感信息的配置文件到代码仓库！**

- `appsettings.Development.json` 已添加到 `.gitignore`
- 生产环境配置应通过 CI/CD 管道或环境变量注入
- 本地开发使用 Integrated Security（Windows 身份验证）避免硬编码密码

## 常见问题

### Q: 为什么我的配置没有生效？
A: 检查环境变量 `ASPNETCORE_ENVIRONMENT`，确保与配置文件名匹配。在 Visual Studio 中，这个值通常在 `launchSettings.json` 中设置。

### Q: 如何在不同数据库之间切换？
A: 在 `appsettings.Development.json` 中修改 `ConnectionStrings:DefaultConnection`，或者使用 User Secrets。

### Q: 如何使用 User Secrets？
A: 右键项目 → "管理用户机密"，在打开的文件中添加配置，例如：
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your connection string"
  }
}
```

## 相关资源

- [ASP.NET Core 配置文档](https://learn.microsoft.com/zh-cn/aspnet/core/fundamentals/configuration/)
- [安全存储应用机密](https://learn.microsoft.com/zh-cn/aspnet/core/security/app-secrets)
- [Serilog 配置](https://github.com/serilog/serilog-settings-configuration)
