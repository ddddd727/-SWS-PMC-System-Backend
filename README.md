# SWS-PMC-System-Backend - 后端 API 项目

## 项目简介
本项目是一个典型的 **前后端分离** 后端服务，使用 **ASP.NET Core Web API (.NET 8)** 开发，为 Vue 3 前端提供 RESTful 接口。

项目采用干净的分层架构，代码结构清晰、易于扩展，已集成常用企业级组件，适合多人协作开发。

**当前核心特性**：

+ Swagger 在线 API 文档与测试
+ EF Core + SQL Server 数据库访问
+ AutoMapper 自动对象映射
+ CORS 跨域支持
+ 依赖注入、Swagger、日志等基础服务

## 当前安装的依赖（NuGet 包）
| 包名 | 作用 | 备注 |
| --- | --- | --- |
| Microsoft.EntityFrameworkCore.SqlServer | EF Core SQL Server 数据库提供程序 | 核心 ORM |
| Microsoft.EntityFrameworkCore.Tools | EF Core 迁移工具（dotnet ef） | 用于生成迁移 |
| Microsoft.EntityFrameworkCore.Design | 设计时支持 | 迁移命令必需 |
| Swashbuckle.AspNetCore | Swagger / Swagger UI | API 文档与在线测试 |
| AutoMapper | 对象映射（Entity ↔ DTO） | 减少手动赋值代码 |
| AutoMapper.Extensions.Microsoft.DependencyInjection | AutoMapper 的 DI 集成 | 支持注入 IMapper |


**附属服务**：

+ **数据库**：SQL Server LocalDB（开发环境），连接字符串在 `appsettings.json`
+ **Swagger UI**：访问 `/swagger`
+ **CORS**：允许 `http://localhost:3000`（Vue 默认端口）

## 项目结构
```plain
BackendProject/
├── Controllers/          # API 控制器
├── Entities/             # 数据库实体类（EF Core）
├── Models/               # DTO（数据传输对象）
├── Services/             # 业务逻辑层
    ├──Interfaces					# 业务接口
    ├──Impletation				# 接口对应实现
├── MappingProfiles/      # AutoMapper 配置
├── Data/                 # DbContext
├── Properties/           # 启动配置
├── Migrations/           # EF Core 迁移文件（自动生成）
├── appsettings.json      # 配置（连接字符串等）
├── Program.cs            # 服务注册与中间件配置
└── README.md
```

## 快速上手（新成员 10 分钟运行项目）
### 1. 环境准备（只需一次）
+ 安装 **.NET 8 SDK**：[https://dotnet.microsoft.com/download/dotnet/8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
+ 安装 **Git**
+ （推荐）安装 Visual Studio 2022 Community

### 2. 获取代码并运行
```bash
git clone https://your-repo-url/BackendProject.git
cd BackendProject

dotnet restore          # 恢复所有 NuGet 依赖
dotnet build            # 构建检查

# 应用数据库迁移（首次运行必须）
dotnet ef database update

dotnet run              # 或在 Visual Studio 按 F5
dotnet run --launch-profile http
http://localhost:5022/swagger/index.html
```

运行成功后，浏览器自动打开 **Swagger UI**：`https://localhost:{端口}/swagger`

## 开发流程（标准步骤）
### 1. 拉取最新代码
```bash
git pull origin main
```

### 2. 创建功能分支（必须）
```bash
git checkout -b feature/功能描述   # 如 feature/add-user-module
```

### 3. 新增功能典型流程
1. **定义 DTO**（Models 文件夹）  
新建如 `UserDto.cs`
2. **定义实体**（Entities 文件夹）  
新建如 `UserEntity.cs`
3. **添加 AutoMapper 配置**（MappingProfiles）  
新建或修改 Profile：

```csharp
CreateMap<UserEntity, UserDto>();
CreateMap<UserDto, UserEntity>();
```

4. **扩展 DbContext**（Data/MyDbContext.cs）  
添加 `public DbSet<UserEntity> Users { get; set; }`
5. **创建迁移并更新数据库(如果已有数据库，则可忽略此步)**

```bash
dotnet ef migrations add AddUserTable
dotnet ef database update
```

6. **实现业务逻辑**（Services）  
修改或新建 Service，注入 `MyDbContext` 和 `IMapper`，使用 `_mapper.Map<>()` 进行转换
7. **添加控制器接口**（Controllers）  
注入 Service，为每个 Action 添加明确路由模板（如 `[HttpGet("list")]`），避免 Swagger 冲突
8. **本地测试**  
运行项目 → Swagger UI → 测试新接口

### 4. 提交与合并
```bash
git add .
git commit -m "feat: 添加用户模块（实体、DTO、映射、服务、接口）"
git push origin feature/功能描述
```

+ 在 Git 仓库网页创建 Pull Request → main
+ 等待代码审查通过后合并
+ 合并后切换回 main 并拉取最新：

```bash
git checkout main
git pull origin main
```
## 掌握核心协作规范（团队必守规则）
### 1. 分支管理
+ **main 分支**：始终保持可部署状态，仅接受经审查的 PR 合并
+ **功能分支**：以 `feature/` 开头，开发完成后删除
+ **禁止直接推送至 main**

### 2. 代码规范
+ **路由必须显式**：避免同一路径多个相同 HTTP 方法导致 Swagger 失败
+ **业务逻辑放 Services 层**：控制器只处理请求/响应
+ **所有服务使用接口 + DI**：便于测试和替换
+ **DTO 命名以 Dto 结尾**：如 `UserRegisterRequestDto`
+ **使用 XML 注释**（可选但推荐）：

```csharp
/// <summary>
/// 用户登录
/// </summary>

```

### 3. 提交规范（强烈推荐）
使用清晰的 Commit Message：

+ `feat:` 新功能
+ `fix:` 修复 Bug
+ `refactor:` 重构
+ `docs:` 文档
+ `test:` 测试  
示例：

```plain
feat: 添加用户注册接口和验证逻辑
fix: 修复示例接口路由冲突导致 Swagger 加载失败
```

### 4. Swagger 使用规范
+ 所有新接口必须能在 Swagger 中正常显示和测试
+ 如出现 “Failed to load API definition”，优先检查路由冲突

### 5. 前后端协作注意
+ CORS 已允许 `http://localhost:3000`，若前端端口变更，请及时修改 `Program.cs`
+ 接口变更必须提前沟通，或在 PR 中@前端成员


## 常见命令汇总
```bash
# 恢复依赖
dotnet restore

# 构建
dotnet build

# 添加迁移
dotnet ef migrations add 迁移名称

# 更新数据库
dotnet ef database update

# 移除最后一次迁移（未应用时）
dotnet ef migrations remove

# 运行项目
dotnet run
```

## 测试示例接口
+ GET `/api/example/get` → 返回单条数据（DTO）
+ GET `/api/example/all` → 返回列表（AutoMapper 映射）
+ POST `/api/example` → 保存数据（DTO → Entity）

---

**欢迎贡献代码！**  
严格遵循上述流程和规范，可大幅减少冲突、提升团队效率。  
有问题优先查看本 README，其次在 Issues 中提问。

