# SWS-PMC-System-Backend - 后端 API 项目

## 项目简介

本项目是一个 **前后端分离** 后端服务，使用 **ASP.NET Core Web API (.NET 8)** 开发，为 Vue 3 前端提供 RESTful 接口。

项目采用 **模块化分层架构**，代码结构清晰、易于扩展，已集成常用企业级组件，适合多人协作开发。

**当前核心特性**：

- 模块化设计（按业务域划分 Module）
- 统一 DbContext（AppDbContext 合并多数据源）
- Shared 共享实体（跨模块同名实体统一管理）
- Swagger 在线 API 文档与测试
- EF Core + SQL Server 数据库访问
- AutoMapper 对象映射
- 依赖注入、CORS、Serilog 日志等

---

## 项目结构（新架构）

```
PMCSystem_Backend/
│
├── Core/                           # 核心基础设施
│   ├── Data/
│   │   └── AppDbContext.cs         # 统一 DbContext（合并原 4 个 Context）
│   ├── Middlewares/
│   │   └── ExceptionMiddleware.cs  # 全局异常处理
│   ├── Exceptions/                 # 自定义异常（预留）
│   ├── Extensions/                 # 扩展方法（预留）
│   └── DependencyInjection/        # DI 配置（预留）
│
├── Shared/                         # 跨模块共享
│   ├── ApiControllerBase.cs        # 控制器基类（统一响应格式）
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
│   │   ├── Entities/               # 模块专属实体
│   │   ├── Dtos/
│   │   ├── Repositories/           # 仓储层（预留）
│   │   └── Services/               # 模块服务（预留，当前集中于 Services/）
│   │
│   ├── PipingSpecifications/       # 管系规格模块
│   │   ├── Controllers/
│   │   ├── Entities/
│   │   ├── Dtos/
│   │   ├── Repositories/
│   │   └── Services/
│   │
│   ├── PMCRuleConfig/              # PMC 规则配置模块
│   │   ├── Controllers/
│   │   ├── Entities/
│   │   ├── Dtos/
│   │   ├── Repositories/
│   │   └── Services/
│   │
│   └── StandardComponents/         # 标准件（字典、CodeList 等）
│       ├── Controllers/
│       ├── Dtos/
│       ├── Repositories/
│       └── Services/
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

## 仓储层（Repository）设计

### 为何需要仓储层

- **解耦**：Service 不直接依赖 DbContext，便于单元测试和替换数据源
- **复用**：通用 CRUD 逻辑集中，减少重复代码
- **职责分离**：数据访问与业务逻辑清晰划分

### 仓储层目录结构（建议）

```
Modules/{ModuleName}/
└── Repositories/
    ├── I{Entity}Repository.cs      # 仓储接口
    └── {Entity}Repository.cs       # 仓储实现
```

### 仓储接口示例

```csharp
// Modules/PipingSpecifications/Repositories/IPipeSpecVersionRepository.cs
namespace PMCSystem_Backend.Modules.PipingSpecifications.Repositories;

public interface IPipeSpecVersionRepository
{
    Task<PipeSpecVersion?> GetByKeyAsync(string pmcCode, string shipType, string shipNo);
    Task<List<PipeSpecVersion>> GetListAsync(string? shipType, string? shipNo);
    Task<PipeSpecVersion> AddAsync(PipeSpecVersion entity);
    Task UpdateAsync(PipeSpecVersion entity);
    Task DeleteAsync(PipeSpecVersion entity);
}
```

### 仓储实现示例

```csharp
// Modules/PipingSpecifications/Repositories/PipeSpecVersionRepository.cs
using Microsoft.EntityFrameworkCore;
using PMCSystem_Backend.Core.Data;
using PMCSystem_Backend.Modules.PipingSpecifications.Entities;

namespace PMCSystem_Backend.Modules.PipingSpecifications.Repositories;

public class PipeSpecVersionRepository : IPipeSpecVersionRepository
{
    private readonly AppDbContext _context;

    public PipeSpecVersionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PipeSpecVersion?> GetByKeyAsync(string pmcCode, string shipType, string shipNo)
    {
        return await _context.PipeSpecVersions
            .FirstOrDefaultAsync(x =>
                x.PmcCode == pmcCode && x.ShipType == shipType && x.ShipNo == shipNo);
    }

    public async Task<PipeSpecVersion> AddAsync(PipeSpecVersion entity)
    {
        _context.PipeSpecVersions.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    // ...
}
```

### 仓储注册（Program.cs）

```csharp
// 按模块注册
builder.Services.AddScoped<IPipeSpecVersionRepository, PipeSpecVersionRepository>();
```

### Service 使用仓储

```csharp
public class PipeSpecVersionService
{
    private readonly IPipeSpecVersionRepository _repository;
    private readonly IMapper _mapper;

    public PipeSpecVersionService(IPipeSpecVersionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PipeSpecVersionDto> GetAsync(string pmcCode, string shipType, string shipNo)
    {
        var entity = await _repository.GetByKeyAsync(pmcCode, shipType, shipNo);
        return _mapper.Map<PipeSpecVersionDto>(entity);
    }
}
```

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

### 步骤 3：注册 DbSet（如为表实体）

在 `Core/Data/AppDbContext.cs` 中：

```csharp
public virtual DbSet<YourEntity> YourEntities { get; set; }
```

并在 `OnModelCreating` 中配置映射（表名、列名、索引等）。

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

### 步骤 7：（可选）添加仓储层

- 创建 `Modules/{ModuleName}/Repositories/IYourEntityRepository.cs`
- 创建 `Modules/{ModuleName}/Repositories/YourEntityRepository.cs`
- 在 Program.cs 注册

### 步骤 8：实现服务

- 接口：`Services/Interfaces/IYourEntityService.cs`
- 实现：`Services/Implementations/YourEntityService.cs`
- 注入 `AppDbContext` 或 `IYourEntityRepository`、`IMapper`

### 步骤 9：添加控制器

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

### 步骤 10：注册服务

在 `Program.cs` 中：

```csharp
builder.Services.AddScoped<IYourEntityService, YourEntityService>();
```

### 步骤 11：验证

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
