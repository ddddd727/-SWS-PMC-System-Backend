# 管系规格书版本管理开发方案

## 文档信息

| 项目       | 内容                          |
| ---------- | ----------------------------- |
| **版本号** | v1.0                          |
| **创建日期** | 2026-02-26                  |
| **技术方案** | 方案一：独立历史快照表       |
| **状态**   | 待实施                        |

---

## 1. 需求概述

对管系规格书对象进行版本管理，实现：

1. **查看历史版本**：按 PMC 编码 + 船型 + 船号查询版本列表及详情
2. **使用历史版本覆盖当前版本**：将指定历史版本恢复为当前生效配置
3. **版本元数据**：版本号、创建时间、创建人、备注等

---

## 2. 技术方案概述

采用 **方案一：独立历史快照表** 实现：

- **主表**：`S3D_Rule_PMCData` 保持不变，继续存储当前生效的管系规格书配置
- **历史表**：新建 `PipeSpecVersion`（或 `S3D_Rule_PMCData_History`），在每次保存前将主表当前数据复制为一条历史快照
- **恢复逻辑**：从历史表读取指定版本的快照数据，写回主表

### 2.1 方案优点

- 主表结构零改动，现有查询与业务逻辑不变
- 版本数据独立存储，便于归档与清理
- 历史查询、恢复逻辑清晰，易于实现与维护

---

## 3. 表结构设计

### 3.1 主表（无改动）

主表 `S3D_Rule_PMCData` 保持现有结构不变，业务主键为 `(PMCCode, ShipType, ShipNo)`。

### 3.2 历史版本表：PipeSpecVersion

| 列名 | 类型 | 说明 |
| --- | --- | --- |
| **Id** | int, PK, IDENTITY | 主键 |
| **PmcCode** | nvarchar(255), NOT NULL | PMC 编码 |
| **ShipType** | nvarchar(255), NOT NULL | 船型 |
| **ShipNo** | nvarchar(255), NOT NULL | 船号 |
| **Version** | int, NOT NULL | 版本号（同一 PMC+船型+船号 下递增） |
| **PipingClassName** | nvarchar(255) | 管系等级名称 |
| **MaterialsCategoryName** | nvarchar(255) | 材料类别名称 |
| **PipingStandardName** | nvarchar(255) | 管材标准名称 |
| **MaterialsGradeName** | nvarchar(255) | 材料等级名称 |
| **FlangeStandardName** | nvarchar(255) | 法兰标准名称 |
| **PressureRatingName** | nvarchar(255) | 压力等级名称 |
| **ScheduleThicknessName** | nvarchar(255) | 壁厚系列名称 |
| **PipeStandard** | nvarchar(max) | 管材标准配置 JSON |
| **ElbowStandard** | nvarchar(max) | 弯头标准配置 JSON |
| **RedStandard** | nvarchar(max) | 异径管标准配置 JSON |
| **TeeStandard** | nvarchar(max) | 三通标准配置 JSON |
| **SleeveStandard** | nvarchar(max) | 套管标准配置 JSON |
| **BossesStandard** | nvarchar(max) | 支管台标准配置 JSON |
| **SaddlesStandard** | nvarchar(max) | 鞍座标准配置 JSON |
| **CapsStandard** | nvarchar(max) | 管帽标准配置 JSON |
| **OverpassStandard** | nvarchar(max) | 跨越件标准配置 JSON |
| **AccessoriesStandard** | nvarchar(max) | 附件标准配置 JSON |
| **FlangeStandard** | nvarchar(max) | 法兰标准配置 JSON |
| **BlindFlangeStandard** | nvarchar(max) | 盲板标准配置 JSON |
| **GasketStandard** | nvarchar(max) | 垫片标准配置 JSON |
| **BoltStandard** | nvarchar(max) | 螺栓标准配置 JSON |
| **NutStandard** | nvarchar(max) | 螺母标准配置 JSON |
| **WasherStandard** | nvarchar(max) | 垫圈标准配置 JSON |
| **Status** | nvarchar(100) | 配置状态 |
| **CreatedAt** | datetime2, NOT NULL | 创建时间 |
| **CreatedBy** | nvarchar(255) | 创建人 |
| **Comment** | nvarchar(500) | 版本备注 |

**索引建议**：

- `IX_PipeSpecVersion_PmcCode_ShipType_ShipNo`：`(PmcCode, ShipType, ShipNo)`
- `IX_PipeSpecVersion_CreatedAt`：`(CreatedAt)` 可选，用于按时间范围查询

### 3.3 SQL 建表脚本

```sql
CREATE TABLE [dbo].[PipeSpecVersion] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PmcCode] nvarchar(255) NOT NULL,
    [ShipType] nvarchar(255) NOT NULL,
    [ShipNo] nvarchar(255) NOT NULL,
    [Version] int NOT NULL,
    [PipingClassName] nvarchar(255) NULL,
    [MaterialsCategoryName] nvarchar(255) NULL,
    [PipingStandardName] nvarchar(255) NULL,
    [MaterialsGradeName] nvarchar(255) NULL,
    [FlangeStandardName] nvarchar(255) NULL,
    [PressureRatingName] nvarchar(255) NULL,
    [ScheduleThicknessName] nvarchar(255) NULL,
    [PipeStandard] nvarchar(max) NULL,
    [ElbowStandard] nvarchar(max) NULL,
    [RedStandard] nvarchar(max) NULL,
    [TeeStandard] nvarchar(max) NULL,
    [SleeveStandard] nvarchar(max) NULL,
    [BossesStandard] nvarchar(max) NULL,
    [SaddlesStandard] nvarchar(max) NULL,
    [CapsStandard] nvarchar(max) NULL,
    [OverpassStandard] nvarchar(max) NULL,
    [AccessoriesStandard] nvarchar(max) NULL,
    [FlangeStandard] nvarchar(max) NULL,
    [BlindFlangeStandard] nvarchar(max) NULL,
    [GasketStandard] nvarchar(max) NULL,
    [BoltStandard] nvarchar(max) NULL,
    [NutStandard] nvarchar(max) NULL,
    [WasherStandard] nvarchar(max) NULL,
    [Status] nvarchar(100) NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] nvarchar(255) NULL,
    [Comment] nvarchar(500) NULL,
    CONSTRAINT [PK_PipeSpecVersion] PRIMARY KEY ([Id])
);

CREATE INDEX [IX_PipeSpecVersion_PmcCode_ShipType_ShipNo]
ON [dbo].[PipeSpecVersion] ([PmcCode], [ShipType], [ShipNo]);
```

---

## 4. 业务流程

### 4.1 保存规格书（含版本快照）

```
1. 按 (PmcCode, ShipType, ShipNo) 查询主表 S3D_Rule_PMCData
2. 若存在且已配置：
   a. 查询历史表该 PMC 下的最大 Version
   b. Version = maxVersion + 1
   c. 将主表当前行数据 + 新 Version + CreatedAt/CreatedBy/Comment 插入 PipeSpecVersion
3. 按现有逻辑更新主表
4. SaveChanges
```

### 4.2 获取历史版本列表

```
1. 按 (PmcCode, ShipType, ShipNo) 查询 PipeSpecVersion
2. 按 Version DESC 或 CreatedAt DESC 排序
3. 支持分页（PageIndex, PageSize）
```

### 4.3 获取历史版本详情

```
1. 按 Id 或 (PmcCode, ShipType, ShipNo, Version) 查询 PipeSpecVersion
2. 返回完整快照数据（可映射为 PmcInfoWithConfigDto 等 DTO）
```

### 4.4 使用历史版本覆盖当前版本

```
1. 按版本 Id 或 (PmcCode, ShipType, ShipNo, Version) 查询 PipeSpecVersion
2. 校验该版本对应的 (PmcCode, ShipType, ShipNo) 主表记录存在
3. 将历史快照的各 Standard 列及基础字段写回主表
4. 主表 Status 置为 review（可选）
5. SaveChanges
```

---

## 5. API 设计

### 5.1 新增接口

| 方法 | 路径 | 说明 |
| --- | --- | --- |
| GET | `/api/PmcSpec/{pmcCode}/versions` | 获取历史版本列表（支持 shipType、shipNumber 查询参数） |
| GET | `/api/PmcSpec/{pmcCode}/versions/{versionId}` | 获取历史版本详情 |
| POST | `/api/PmcSpec/{pmcCode}/versions/{versionId}/revert` | 使用历史版本覆盖当前版本 |

### 5.2 请求/响应约定

- **版本列表**：分页参数 `pageIndex`、`pageSize`；响应包含 `Version`、`CreatedAt`、`CreatedBy`、`Comment`、`Id` 等
- **版本详情**：返回与 `AnalyzeCodeFromPMCWithConfig` 类似的完整配置结构
- **回滚**：请求体可为空或包含 `shipType`、`shipNumber` 用于精确匹配；响应为统一成功/失败格式

---

## 6. 代码结构规划

### 6.1 新增文件

| 类型 | 路径 | 说明 |
| --- | --- | --- |
| 实体 | `Entities/PipeSpecConfig/PipeSpecVersion.cs` | 历史版本实体 |
| DTO | `Dtos/PipeSpecConfig/PipeSpecVersionDto.cs` | 版本列表项 DTO |
| DTO | `Dtos/PipeSpecConfig/PipeSpecVersionDetailDto.cs` | 版本详情 DTO |
| DTO | `Dtos/PipeSpecConfig/Requests/RevertToVersionRequest.cs` | 回滚请求 DTO |
| 服务接口 | `Services/Interfaces/IPipeSpecVersionService.cs` | 版本管理服务接口 |
| 服务实现 | `Services/Implementations/PipeSpecVersionService.cs` | 版本管理服务实现 |
| Controller | `Controllers/PipeSpecConfig/PipeSpecVersionController.cs` | 版本管理控制器（或合并到 PmcSpecController） |

### 6.2 需修改的现有文件

| 文件 | 修改内容 |
| --- | --- |
| `Data/PmcContext.cs` | 注册 `DbSet<PipeSpecVersion>` 及 `OnModelCreating` 配置 |
| `Services/Implementations/PmcSpecService.cs` | 在 `SaveSpecRulesSimple` / `SaveSpecRules` 中增加保存前插入历史快照的调用 |
| `Program.cs` | 注册 `IPipeSpecVersionService` 和 `PipeSpecVersionService` |

---

## 7. 开发计划步骤

### 阶段一：基础设施（约 1 天）

| 步骤 | 任务 | 产出 |
| --- | --- | --- |
| 1.1 | 执行建表 SQL，创建 `PipeSpecVersion` 表 | 数据库表 |
| 1.2 | 创建 `PipeSpecVersion` 实体类，映射所有列 | `Entities/PipeSpecConfig/PipeSpecVersion.cs` |
| 1.3 | 在 `PmcContext` 中注册 `DbSet` 和 Fluent API 配置 | `Data/PmcContext.cs` |

### 阶段二：Service 层（约 1–2 天）

| 步骤 | 任务 | 产出 |
| --- | --- | --- |
| 2.1 | 定义 `IPipeSpecVersionService` 接口 | `Services/Interfaces/IPipeSpecVersionService.cs` |
| 2.2 | 实现 `GetVersionList`（分页） | `PipeSpecVersionService` |
| 2.3 | 实现 `GetVersionDetail` | `PipeSpecVersionService` |
| 2.4 | 实现 `RevertToVersion` | `PipeSpecVersionService` |
| 2.5 | 实现 `CreateVersionSnapshot`（供 PmcSpecService 调用） | `PipeSpecVersionService` |

### 阶段三：与保存流程集成（约 0.5 天）

| 步骤 | 任务 | 产出 |
| --- | --- | --- |
| 3.1 | 在 `SaveSpecRulesSimple` 中，更新主表前调用 `CreateVersionSnapshot` | `PmcSpecService.cs` |
| 3.2 | 在 `SaveSpecRules` 中同理集成 | `PmcSpecService.cs` |
| 3.3 | 在 `Program.cs` 中注册服务 | `Program.cs` |

### 阶段四：API 层（约 1 天）

| 步骤 | 任务 | 产出 |
| --- | --- | --- |
| 4.1 | 创建 DTO：`PipeSpecVersionDto`、`PipeSpecVersionDetailDto`、`RevertToVersionRequest` | `Dtos/` |
| 4.2 | 新建 `PipeSpecVersionController` 或扩展 `PmcSpecController`，实现三个接口 | Controller |
| 4.3 | 更新 `API_CONTRACT.md`，补充版本管理相关接口说明 | 文档 |

### 阶段五：测试与文档（约 0.5 天）

| 步骤 | 任务 | 产出 |
| --- | --- | --- |
| 5.1 | 单元/集成测试：版本快照生成、列表查询、详情、回滚 | 测试用例 |
| 5.2 | 手工验证：保存 → 查看版本 → 回滚 → 对比 | 验证报告 |

---

## 8. 可选增强（后续迭代）

| 功能 | 说明 | 优先级 |
| --- | --- | --- |
| 版本备注 | 保存时支持传入 `comment`，写入历史表 | 中 |
| 版本数量限制 | 每个 PMC 最多保留 N 个版本，超出时删除最旧 | 中 |
| 版本 diff | 比较两个版本配置差异 | 低 |
| 创建人 | 从当前用户上下文获取 `CreatedBy` | 中 |

---

## 9. 依赖与约束

- **数据库**：SQL Server，需执行建表脚本
- **现有主表**：`S3D_Rule_PMCData` 不修改结构
- **现有保存接口**：`SaveSpecRulesSimple`、`SaveSpecRules` 行为兼容，仅增加快照逻辑

---

## 10. 附录

### 10.1 与主表列对应关系

历史表 `PipeSpecVersion` 的配置列与主表 `S3D_Rule_PMCData` 一一对应，JSON 序列化格式与主表一致（使用 `System.Text.Json` 默认选项）。恢复时直接复制列值即可。

### 10.2 版本号生成规则

- 同一 `(PmcCode, ShipType, ShipNo)` 下，每次保存前生成：`Version = MAX(Version) + 1`
- 若该 PMC 尚无历史版本，则 `Version = 1`
