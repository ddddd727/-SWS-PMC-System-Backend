# SWS-PMC-System-Backend - 后端 API 项目

## 项目概述

本项目是一个基于 **ASP.NET Core Web API** 的后端服务，采用前后端分离架构设计，使用 **C# (.NET 8)** 开发。主要为 Vue 3 前端提供 RESTful API 接口。

项目采用分层架构（Controllers → Services → Models），代码结构清晰、易于维护和扩展，便于多人协作开发。

## 技术栈

- **后端框架**：ASP.NET Core Web API (.NET 8)
- **语言**：C#
- **API 文档**：Swashbuckle.AspNetCore（Swagger UI）
- **依赖注入**：内置 DI 容器
- **跨域支持**：CORS（允许 Vue 前端访问）
- **版本控制**：Git
- **开发工具推荐**：Visual Studio 2022 / Visual Studio Code / Rider

## 项目结构

```
BackendProject/
├── Controllers/          # API 控制器（处理 HTTP 请求）
├── Models/               # DTO（数据传输对象）
├── Services/             # 业务逻辑层（接口 + 实现）
├── Properties/           # 启动配置（launchSettings.json）
├── appsettings.json      # 配置文件
├── Program.cs            # 应用入口、服务注册与中间件配置
├── BackendProject.csproj # 项目文件
└── README.md             # 本文档
```

## 快速配置环境（新成员 10 分钟内上手）

### 1. 安装必备工具（只需一次）
- **.NET 8 SDK**  
  下载地址：https://dotnet.microsoft.com/download/dotnet/8.0  
  安装后打开终端/PowerShell 执行：  
  ```bash
  dotnet --version
  ```
  应显示 `8.x.x`（如 8.0.100）

- **Git**  
  下载地址：https://git-scm.com/downloads  
  安装后配置用户名和邮箱（只需一次）：
  ```bash
  git config --global user.name "Your Name"
  git config --global user.email "your@email.com"
  ```

- **推荐 IDE**  
  - Visual Studio 2022 Community（免费，内置一切）：https://visualstudio.microsoft.com/vs/community/  
  - 或 Visual Studio Code + C# 扩展（轻量级）

### 2. 获取代码并首次运行
```bash
# 克隆仓库
git clone https://your-repo-url/BackendProject.git
cd BackendProject

# 恢复 NuGet 包（自动下载依赖）
dotnet restore

# 构建项目（检查是否有编译错误）
dotnet build

# 运行项目
dotnet run
```
运行成功后，控制台会显示类似：
```
Now listening on: https://localhost:7xxx
Now listening on: http://localhost:5xxx
```
浏览器会自动打开 `https://localhost:{端口}/swagger`

### 3. Visual Studio 快速启动（推荐）
1. 双击项目根目录下的 `.sln` 文件（如果没有，可执行 `code .` 用 VS Code 打开）
2. 在顶部工具栏选择 **IIS Express** 或项目名称
3. 按 **F5**（调试运行）或 **Ctrl+F5**（无调试运行）
4. 浏览器自动打开 Swagger UI 页面

**恭喜！环境配置完成，你已成功运行后端项目。**

## 理解开发流程（标准功能开发步骤）

1. **拉取最新代码**
   ```bash
   git pull origin main
   ```

2. **创建功能分支（必须）**
   ```bash
   git checkout -b feature/你的功能描述   # 如 feature/user-login
   ```

3. **开发新功能（示例流程）**
   - 在 `Models` 新建 DTO（如 `LoginRequestDto.cs`、`LoginResponseDto.cs`）
   - 在 `Services` 新建接口 `IUserService.cs` 和实现 `UserService.cs`
   - 在 `Controllers` 新建或修改控制器（如 `UserController.cs`），注入服务
   - 为每个 Action 添加明确路由模板，避免 Swagger 冲突：
     ```csharp
     [HttpPost("login")]
     [HttpGet("profile")]
     ```

4. **本地测试**
   - 运行项目 → 打开 Swagger → 测试新接口

5. **提交代码**
   ```bash
   git add .
   git commit -m "feat: 添加用户登录接口"
   git push origin feature/你的功能描述
   ```

6. **提交 Pull Request**
   - 去 Git 仓库网页（GitHub/GitLab 等）
   - 创建 PR 从你的分支 → main
   - 填写 PR 描述，@ 审查人
   - 等待代码审查通过后合并

7. **合并后更新本地 main**
   ```bash
   git checkout main
   git pull origin main
   ```

## 掌握核心协作规范（团队必守规则）

### 1. 分支管理
- **main 分支**：始终保持可部署状态，仅接受经审查的 PR 合并
- **功能分支**：以 `feature/` 开头，开发完成后删除
- **禁止直接推送至 main**

### 2. 代码规范
- **路由必须显式**：避免同一路径多个相同 HTTP 方法导致 Swagger 失败
- **业务逻辑放 Services 层**：控制器只处理请求/响应
- **所有服务使用接口 + DI**：便于测试和替换
- **DTO 命名以 Dto 结尾**：如 `UserRegisterRequestDto`
- **使用 XML 注释**（可选但推荐）：
  ```csharp
  /// <summary>
  /// 用户登录
  /// </summary>
  ```

### 3. 提交规范（强烈推荐）
使用清晰的 Commit Message：
- `feat:` 新功能
- `fix:` 修复 Bug
- `refactor:` 重构
- `docs:` 文档
- `test:` 测试
示例：
```
feat: 添加用户注册接口和验证逻辑
fix: 修复示例接口路由冲突导致 Swagger 加载失败
```

### 4. Swagger 使用规范
- 所有新接口必须能在 Swagger 中正常显示和测试
- 如出现 “Failed to load API definition”，优先检查路由冲突

### 5. 前后端协作注意
- CORS 已允许 `http://localhost:3000`，若前端端口变更，请及时修改 `Program.cs`
- 接口变更必须提前沟通，或在 PR 中@前端成员

## 测试接口示例

- **GET** `/api/example/get`  
  返回服务层数据
- **POST** `/api/example`  
  请求体示例：
  ```json
  {
    "message": "Hello from Vue",
    "timestamp": "2025-12-16T00:00:00Z"
  }
  ```

## 后续扩展建议

- 集成数据库（EF Core）
- 添加 JWT 认证
- 引入日志（Serilog）
- 编写单元测试（xUnit）

---

**欢迎加入开发！遵守以上流程和规范，可最大程度减少冲突、提升协作效率。**  

有任何疑问，优先查阅本 README，其次在仓库 Issues 中搜索或新建提问。
