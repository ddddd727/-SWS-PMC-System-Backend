# SWS-PMC-System-Backend - 后端 API 项目

## 项目简介

本项目是一个 **前后端分离** 后端服务，使用 **ASP.NET Core Web API (.NET 8)** 开发，为 Vue 3 前端提供 RESTful 接口。

项目采用 **模块化分层架构**，代码结构清晰、易于扩展，已集成常用企业级组件，适合多人协作开发。

**当前核心特性**：

- 模块化设计（按业务域划分 Module）
- 统一 DbContext（AppDbContext 合并多数据源，实体配置按模块拆分至 DataConfigurations）
- Shared 共享实体（跨模块同名实体统一管理）
- Swagger 在线 API 文档与测试
- EF Core + SQL Server 数据库访问（Service 直接使用 DbContext，无仓储层）
- AutoMapper 对象映射
- 依赖注入、CORS、Serilog 日志等

---

## 项目结构（新架构）

```
PMCSystem_Backend/
│
├── Core/                           # 核心基础设施
│   ├── Data/
│   │   └── AppDbContext.cs         # 统一 DbContext（合并原 4 个 Context，配置自动扫描）
│   ├── Middlewares/
│   │   └── ExceptionMiddleware.cs  # 全局异常处理
│   ├── Exceptions/                 # 自定义异常（预留）
│   ├── Extensions/                 # 扩展方法（预留）
│   └── DependencyInjection/        # DI 配置（预留）
│
├── Shared/                         # 跨模块共享
│   ├── ApiControllerBase.cs        # 控制器基类（统一响应格式）
│   ├── DataConfigurations/         # 共享实体的 EF Core 配置
│   ├── Entities/                   # 跨模块共享实体
│   │   ├── S3dCommonCodeListTable.cs
│   │   ├── S3dCommonCodeListValue.cs
│   │   ├── S3dCommonCodeListHierarchy.cs
│   │   ├── S3dCommonPlainPipingGenericData.cs
│   │   ├── S3dDictPipingComponentType.cs
│   │   └── S3dRuleShortCodeMap.cs
│   ├── Enums/                      # 枚举
│   ├── Models/                     # 通用模型（ApiResponse、PagedResult 等）
│   └── Constants/                  # 常量
│
├── Modules/                        # 业务模块（按域划分）
│   ├── DesignRules/                # 设计规则模块
│   │   ├── Controllers/
│   │   ├── DataConfigurations/     # 实体表/视图的 EF Core 配置
│   │   ├── Entities/               # 模块专属实体
│   │   └── Dtos/
│   │
│   ├── PipingSpecifications/       # 管系规格模块
│   │   ├── Controllers/
│   │   ├── DataConfigurations/
│   │   ├── Entities/
│   │   └── Dtos/
│   │
│   ├── PMCRuleConfig/              # PMC 规则配置模块
│   │   ├── Controllers/
│   │   ├── DataConfigurations/
│   │   ├── Entities/
│   │   └── Dtos/
│   │
│   └── StandardComponents/         # 标准件（字典、CodeList 等）
│       ├── Controllers/
│       └── Dtos/
│
├── Services/                       # 服务层（集中管理）
│   ├── Interfaces/                 # 服务接口
│   │   ├── DesignRule/
│   │   ├── CodeListManagement/
│   │   └── ...
│   └── Implementations/            # 服务实现
│       ├── DesignRule/
│       ├── CodeListManagement/
│       ├── DictStrategies/
│       └── ...
│
├── MappingProfiles/                # AutoMapper 配置
├── Configs/                        # 配置文件（如字典 JSON）
├── Docs/                           # 项目文档
├── Program.cs
├── appsettings.json
└── README.md
```

---

## 模块化文件划分规范

### 1. 实体（Entity）放置规则

| 类型 | 放置位置 | 说明 |
|------|----------|------|
| **跨模块共用** | `Shared/Entities/` | 被多个模块引用（如 CodeList、ShortCodeMap） |
| **模块专属** | `Modules/{ModuleName}/Entities/` | 仅该模块使用（如 PipeSpecVersion、S3dRuleAb2b3c2） |

**判断标准**：若实体被 2 个及以上模块引用，应迁移至 Shared。

### 2. DTO 放置规则

- **请求/响应 DTO**：`Modules/{ModuleName}/Dtos/`
- **复杂请求**：`Modules/{ModuleName}/Dtos/Requests/`
- **子模型**：`Modules/{ModuleName}/Dtos/Models/`
- **跨模块 DTO**：可放在 `Shared/` 或主使用模块的 Dtos

### 3. 控制器放置规则

- 每个模块的 API 控制器：`Modules/{ModuleName}/Controllers/`
- 继承 `Shared.ApiControllerBase` 获得统一响应格式

### 4. 服务层放置规则

- **接口**：`Services/Interfaces/`（可按子目录如 DesignRule 分类）
- **实现**：`Services/Implementations/`（与接口对应）
- 未来可逐步将实现迁入 `Modules/{ModuleName}/Services/` 以强化模块内聚

---

## 数据访问说明

本项目 **不使用仓储层**，Service 层直接注入 `AppDbContext` 进行数据访问。EF Core 的 DbContext + DbSet 已提供工作单元和查询抽象，足以满足多数场景。实体映射配置按模块拆分至 `DataConfigurations`，由 `ApplyConfigurationsFromAssembly` 自动加载。

---

## 新增功能标准流程

### 步骤 1：确定所属模块

根据业务域选择或新建模块：`DesignRules`、`PipingSpecifications`、`PMCRuleConfig`、`StandardComponents`。

### 步骤 2：定义实体（Entity）

- **模块专属**：`Modules/{ModuleName}/Entities/YourEntity.cs`
- **跨模块**：`Shared/Entities/YourEntity.cs`

```csharp
namespace PMCSystem_Backend.Modules.YourModule.Entities;

public class YourEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    // ...
}
```

### 步骤 3：注册 DbSet 与实体配置

1. 在 `Core/Data/AppDbContext.cs` 中添加：

```csharp
public virtual DbSet<YourEntity> YourEntities { get; set; }
```

2. 在对应模块的 `DataConfigurations/` 下新建 `IEntityTypeConfiguration<YourEntity>` 实现（如 `YourEntityConfiguration.cs`），配置表名、列名、索引等。AppDbContext 会自动扫描并应用。

### 步骤 4：创建数据库迁移（如为新表）

```bash
dotnet ef migrations add AddYourEntityTable
dotnet ef database update
```

### 步骤 5：定义 DTO

在 `Modules/{ModuleName}/Dtos/` 下创建请求/响应 DTO。

### 步骤 6：配置 AutoMapper

在 `MappingProfiles/` 相应 Profile 中：

```csharp
CreateMap<YourEntity, YourEntityDto>().ReverseMap();
CreateMap<CreateYourEntityDto, YourEntity>();
```

### 步骤 7：实现服务

- 接口：`Services/Interfaces/IYourEntityService.cs`
- 实现：`Services/Implementations/YourEntityService.cs`
- 注入 `AppDbContext`、`IMapper`，直接使用 `_context.YourEntities` 进行数据访问

### 步骤 8：添加控制器

在 `Modules/{ModuleName}/Controllers/YourEntityController.cs`：

```csharp
[ApiController]
[Route("api/[controller]")]
public class YourEntityController : ApiControllerBase
{
    private readonly IYourEntityService _service;

    public YourEntityController(IYourEntityService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _service.GetAllAsync();
        return Success(list);
    }
}
```

### 步骤 9：注册服务

在 `Program.cs` 中：

```csharp
builder.Services.AddScoped<IYourEntityService, YourEntityService>();
```

### 步骤 10：验证

运行项目，在 Swagger UI 中测试新接口。

---

## 依赖与配置

| 包名 | 作用 |
|------|------|
| Microsoft.EntityFrameworkCore.SqlServer | EF Core SQL Server |
| Microsoft.EntityFrameworkCore.Tools | 迁移工具 |
| AutoMapper | 对象映射 |
| Swashbuckle.AspNetCore | Swagger |
| Serilog | 日志 |

- **数据库**：SQL Server，连接字符串在 `appsettings.json`
- **Swagger**：`/swagger`
- **CORS**：在 Program.cs 中配置允许的前端域名

---

## 快速运行

```bash
git clone <repo-url>
cd PMCSystem_Backend

dotnet run --launch-profile http
http://localhost:5022/swagger/index.html
taskkill /F /IM PMCSystem_Backend.exe

dotnet restore
dotnet build

# 首次运行：应用迁移
dotnet ef database update

dotnet run
# 访问 https://localhost:{端口}/swagger
```

---

## 协作规范

### 分支

- `main`：可部署，仅接受 PR
- `feature/xxx`：功能开发

### 提交信息

- `feat:` 新功能
- `fix:` 修复
- `refactor:` 重构
- `docs:` 文档

### 代码约定

- 控制器只处理请求/响应，业务逻辑在 Service
- 服务使用接口 + DI
- DTO 以 `Dto` 结尾
- 显式路由，避免 Swagger 冲突

---

**更多文档**：参见 `Docs/` 目录（如 DEPLOYMENT.md、统一响应格式使用指南.md 等）。
