# PMC 管系规格书模板预览与导出功能开发文档

## 文档信息

| 项目 | 内容 |
|------|------|
| **模块名称** | 模板预览与导出服务 |
| **开发日期** | 2026-02-06 |
| **版本** | v1.5 |
| **最后更新** | 2026-02-06 |

---

## 1. 开发需求

### 1.1 核心需求

用户保存规格书配置信息后，需要能够：

1. **预览表格**：查看填充了规格书数据的表格预览（前端渲染）
2. **导出表格**：将填充了规格书数据的表格导出为 Excel 文件

### 1.2 功能要求

- 模板文件使用 Excel 格式（`.xlsx`），支持合并单元格、样式等
- 模板中的数据位置使用 `{{key}}` 占位符标记
- 支持两种数据填充方式：
  - **规格书驱动**：根据 PMC 编码自动从已保存的规格书数据中提取并填充
  - **自定义参数**：通过 API 参数传入键值对进行填充
- 规格书数据包含：
  - PMC 基础信息（编码、船号、管道等级等）
  - 管附件标准配置（标准类型、标准名字、标准材料）
  - 通径范围信息（NPD、外径、壁厚）

### 1.3 业务规则

- 标准信息格式：`标准名 材料`（中间一个空格）
- 同类型多个标准：用英文逗号分隔，如 `GB/T 8163 20#, GB/T 3091 Q235`
- 通径信息：根据 PMC 基础信息中的端面标准（`pipeStandard`）和壁厚系列（`wallThickness`）自动获取
- 数据来源：与简化版保存接口（`SavePipeSpecSimpleRequest`）一致，仅包含标准类型、标准名字、标准材料，不包含通径范围配置

---

## 2. 技术路线

### 2.1 架构设计

```
Controller Layer (TemplatePreviewController)
    ↓
Service Layer (TemplatePreviewService)
    ↓
    ├─→ IPmcSpecService (规格书服务)
    │   ├─→ AnalyzeCodeFromPMC (PMC基础信息)
    │   ├─→ GetSpecRules (规格规则)
    │   └─→ GetNPDInfoByPmc (通径范围信息)
    │
    └─→ Excel Processing (EPPlus)
        ├─→ 读取模板文件
        ├─→ 解析合并单元格
        ├─→ 替换占位符
        └─→ 导出文件流
```

### 2.2 技术栈

- **后端框架**：ASP.NET Core 6.0+
- **Excel 处理**：EPPlus（OfficeOpenXml）
- **日志**：Microsoft.Extensions.Logging (ILogger)
- **依赖注入**：ASP.NET Core DI Container

### 2.3 核心实现流程

#### 2.3.1 模板预览流程

```
1. 验证 templateId（格式：字母、数字、下划线、中划线）
2. 定位模板文件：{templateBasePath}/{templateId}.xlsx
3. 打开 Excel 文件（EPPlus）
4. 解析首张工作表
5. 提取合并单元格信息
6. 构建"幽灵单元格"集合（合并区域内的非左上角单元格）
7. 遍历非合并单元格，替换 {{key}} 占位符
8. 提取单元格样式（背景色、对齐、字重）
9. 构建预览响应（标题、网格、合并单元格、单元格列表）
```

#### 2.3.2 模板导出流程

```
1. 验证 templateId
2. 定位模板文件
3. 打开 Excel 文件（就地修改，避免样式丢失）
4. 构建"幽灵单元格"集合
5. 仅对非幽灵单元格替换 {{key}} 占位符并写回
6. 填充业务数据（扩展点）
7. 通过 MemoryStream + SaveAs 输出字节流
```

#### 2.3.3 规格书数据构建流程

```
1. 调用 AnalyzeCodeFromPMC(pmcCode) 获取 PMC 基础信息
2. 调用 GetSpecRules(pmcCode) 获取规格规则列表
3. 调用 GetNPDInfoByPmc(pipeStandard, wallThickness) 获取通径信息
4. 构建占位符字典：
   - PMC 基础信息 → 直接映射
   - 规格规则 → 格式化「标准名 材料」，按类型分组
   - 通径信息 → 列表格式和索引格式
5. 返回占位符字典供模板替换使用
```

### 2.4 关键算法

#### 2.4.1 合并单元格处理

- **合并单元格提取**：仅保留左上角单元格代表整个合并块
- **幽灵单元格识别**：合并区域内除左上角外的所有单元格标记为"幽灵"
- **替换策略**：仅对非幽灵单元格进行占位符替换，避免破坏合并结构

#### 2.4.2 占位符替换

- **格式**：`{{key}}`（双大括号）
- **替换逻辑**：字符串替换，支持在单元格文本中的任意位置
- **键名规则**：不区分大小写（`StringComparer.OrdinalIgnoreCase`）

#### 2.4.3 标准信息格式化

- **单条标准**：`标准名 材料`（中间一个空格）
- **多条标准（同类型）**：`标准名1 材料1, 标准名2 材料2`（逗号+空格分隔）
- **类型键名**：类型名中的空格转为下划线（如 `Blind Flange` → `standard_Blind_Flange`）

#### 2.4.4 通径信息格式化

- **列表格式**：数值数组转为逗号分隔字符串
- **数值精度**：通径保留 1 位小数，外径和壁厚保留 2 位小数
- **索引格式**：`{{npd_1}}`, `{{npd_2}}`, ... 提供单个值访问

---

## 3. 需求变更历史

### 3.1 初始需求（v1.0）

- 实现模板预览和导出基础功能
- 支持自定义参数填充

### 3.2 需求变更 1：RESTful 规范调整（v1.1）

**变更内容**：

- 导出接口从 `POST /api/template-preview/export/{templateId}` 改为 `GET /api/template-preview/{templateId}/export`
- 系统错误返回 500 状态码（而非 400）

**影响**：API 路由变更，前端调用需要更新

### 3.3 需求变更 2：规格书数据驱动（v1.2）

**变更内容**：

- 新增按 PMC 编码自动填充规格书数据的功能
- 支持 `pmcCode` 查询参数，优先使用规格书数据

**影响**：新增占位符集合，模板可用的字段大幅增加

### 3.4 需求变更 3：简化配置适配（v1.3）

**变更内容**：

- 与简化版保存接口（`SavePipeSpecSimpleRequest`）对齐
- 仅包含标准类型、标准名字、标准材料
- 移除通径范围相关占位符（`diameterRange_N`）

**影响**：占位符集合调整，移除通径范围占位符

### 3.5 需求变更 4：标准信息格式规范（v1.4）

**变更内容**：

- 标准信息格式统一为「标准名 材料」（中间一个空格）
- 同类型多个标准用英文逗号分隔
- 新增按类型分组的占位符（`{{standard_Pipe}}`, `{{standard_Elbow}}` 等）

**影响**：占位符格式和命名规则调整

### 3.6 需求变更 5：通径范围信息支持（v1.5）

**变更内容**：

- 新增通径范围信息（NPD、外径、壁厚）的占位符支持
- 支持列表格式和索引格式两种访问方式
- 根据 PMC 基础信息自动获取通径数据

**影响**：新增通径相关占位符，模板可展示更完整的信息

---

## 4. 输入输出约束

### 4.1 API 接口

#### 4.1.1 模板预览

**接口**：`GET /api/template-preview/{templateId}`

**输入参数**：

| 参数名 | 类型 | 位置 | 必填 | 约束 | 说明 |
|--------|------|------|------|------|------|
| templateId | string | Path | 是 | `^[a-zA-Z0-9_-]+$` | 模板唯一标识，仅允许字母、数字、下划线、中划线 |
| pmcCode | string | Query | 否 | 非空字符串 | PMC 编码，传入时使用规格书数据填充 |
| parameters | Dictionary<string, string> | Query | 否 | 键值对 | 自定义占位符，未传 pmcCode 时使用 |

**输出响应**：

```json
{
  "code": 200,
  "message": "操作成功",
  "data": {
    "templateId": "string",
    "title": "string",
    "orientation": "rowHeader",
    "grid": {
      "rowCount": 0,
      "columnCount": 0
    },
    "mergedCells": [...],
    "cells": [...]
  },
  "timestamp": "2026-02-06T10:00:00Z",
  "traceId": "string"
}
```

**错误响应**：

| HTTP 状态码 | 业务 Code | 说明 |
|------------|-----------|------|
| 400 | 400 | templateId 格式不合法或参数验证失败 |
| 404 | 404 | 模板文件不存在 |
| 500 | 1000 | 服务器内部错误 |

#### 4.1.2 模板导出

**接口**：`GET /api/template-preview/{templateId}/export`

**输入参数**：同模板预览

**输出响应**：

- **成功 (200)**：Excel 文件流（`application/vnd.openxmlformats-officedocument.spreadsheetml.sheet`）
- **失败 (400/404/500)**：JSON 错误响应

**文件名格式**：`{templateId}_{yyyyMMddHHmmss}.xlsx`

### 4.2 模板文件约束

#### 4.2.1 文件命名

- **格式**：`{templateId}.xlsx`
- **templateId 规则**：
  - 仅允许：字母（a-z, A-Z）、数字（0-9）、下划线（_）、中划线（-）
  - 不允许：空格、特殊字符、中文等
  - 示例：`pipe-spec.xlsx`, `template_001.xlsx`

#### 4.2.2 文件位置

- 配置项：`TemplateBasePath`（默认：`Templates`）
- 完整路径：`{TemplateBasePath}/{templateId}.xlsx`

#### 4.2.3 文件格式要求

- **格式**：Excel 2007+ (`.xlsx`)
- **工作表**：至少包含一个工作表，使用首张工作表
- **合并单元格**：支持，但仅对左上角单元格进行占位符替换
- **样式**：支持背景色、对齐方式、字重（粗体）

### 4.3 占位符约束

#### 4.3.1 占位符格式

- **格式**：`{{key}}`（双大括号包裹字段名）
- **键名规则**：
  - 不区分大小写（`{{pmcCode}}` 与 `{{PmcCode}}` 等效）
  - 允许字符：字母、数字、下划线
  - 类型键名：空格转为下划线（如 `Blind Flange` → `standard_Blind_Flange`）

#### 4.3.2 可用占位符列表

**PMC 基础信息**：

| 占位符 | 数据类型 | 说明 | 示例值 |
|--------|---------|------|--------|
| `{{pmcCode}}` | string | PMC 编码 | A1B2C3D |
| `{{shipNumber}}` | string | 船号 | H1234 |
| `{{pipingClass}}` | string | 管道等级 | 150# |
| `{{materialGrade}}` | string | 牌号 | A105 |
| `{{pressureRating}}` | string | 法兰压力等级 | Class 150 |
| `{{pipeStandard}}` | string | 管材标准 | ASME B36.10 |
| `{{materialCategory}}` | string | 管材材料 | Carbon Steel |
| `{{wallThickness}}` | string | 壁厚系列 | Sch40 |

**规格书标准信息**：

| 占位符 | 数据类型 | 说明 | 示例值 |
|--------|---------|------|--------|
| `{{material}}` | string | 首条规格的材料 | 20# |
| `{{standard_1}}` | string | 第 1 条标准（标准名 材料） | GB/T 8163 20# |
| `{{standard_2}}` | string | 第 2 条标准 | GB/T 3091 Q235 |
| `{{standardName_N}}` | string | 第 N 条标准的名称 | GB/T 8163 |
| `{{standardType_N}}` | string | 第 N 条标准的类型 | Pipe |
| `{{material_N}}` | string | 第 N 条标准的材料 | 20# |
| `{{standard_Pipe}}` | string | 类型为 Pipe 的所有标准（逗号分隔） | GB/T 8163 20#, GB/T 3091 Q235 |
| `{{standard_Elbow}}` | string | 类型为 Elbow 的所有标准 | GB/T 12459 20# |
| `{{standard_<类型名>}}` | string | 其他类型（类型名空格转下划线） | — |

**通径范围信息**：

| 占位符 | 数据类型 | 说明 | 示例值 |
|--------|---------|------|--------|
| `{{npd}}` | string | 通径列表（逗号分隔，单位：mm） | 15, 20, 25, 32, 40 |
| `{{npd_N}}` | string | 第 N 个通径值（N 从 1 开始，单位：mm，1位小数） | 15, 20, ... |
| `{{outsideDiameter}}` | string | 外径列表（逗号分隔，单位：mm） | 21.3, 26.9, 33.7, 42.4 |
| `{{outsideDiameter_N}}` | string | 第 N 个外径值（N 从 1 开始，单位：mm，2位小数） | 21.3, 26.9, ... |
| `{{wallThicknessList}}` | string | 壁厚列表（逗号分隔，单位：mm） | 2.77, 2.87, 3.38, 3.56 |
| `{{wallThicknessList_N}}` | string | 第 N 个壁厚值（N 从 1 开始，单位：mm，2位小数） | 2.77, 2.87, ... |
| `{{endStandard}}` | string | 端面标准（几何工业标准） | ASME B16.9 |
| `{{schedule}}` | string | 壁厚系列 | Sch40 |

**注意**：

- 通径信息需要 PMC 基础信息中包含 `pipeStandard` 和 `wallThickness`，否则相关占位符为空字符串
- 通径数据来自 `GetNPDInfoByPmc`，依赖数据库中的 `S3dWallThicknessInfo` 视图
- 如果获取失败，会记录警告日志，占位符显示为空字符串，不影响其他数据填充

### 4.4 数据映射约束

#### 4.4.1 规格书数据映射

**保存接口 → 数据库 → 读取 → 模板占位符**：

| 保存接口字段 | 转换逻辑 | 数据库/读取字段 | 模板占位符 |
|-------------|---------|----------------|-----------|
| `SimpleStandardConfig.StandardFile` (object) | `.ToString()` | `PmcStandardInfo.StandardName` (string) | `{{standardName_N}}` |
| `SimpleStandardConfig.Material` (object) | `.ToString()` | `PmcStandardInfo.Material` (string) | `{{material_N}}` |
| `SimpleComponentTypeConfiguration.ComponentType` (string) | `NormalizeComponentType()` | `PmcStandardInfo.StandardType` (string) | `{{standardType_N}}` |

**注意事项**：

- `StandardFile` 和 `Material` 在保存时通过 `.ToString()` 转换
- 前端应传入名称字符串（而非 ID）以确保模板中显示为可读名称
- `ComponentType` 通过 `NormalizeComponentType` 规范化（首字母大写，其余小写）

#### 4.4.2 通径信息映射

**PMC 基础信息 → 通径数据**：

| PMC 基础信息字段 | 用途 | 通径数据来源 |
|----------------|------|-------------|
| `pipeStandard` | 端面标准（EndStandard） | `GetNPDInfoByPmc(pipeStandard, wallThickness)` |
| `wallThickness` | 壁厚系列（Schedule） | 同上 |

**通径数据 → 模板占位符**：

| 通径数据字段 | 格式化 | 模板占位符 |
|-------------|--------|-----------|
| `NPD` (List<double>) | 逗号分隔，1位小数 | `{{npd}}`, `{{npd_N}}` |
| `OutsideDiameter` (List<double>) | 逗号分隔，2位小数 | `{{outsideDiameter}}`, `{{outsideDiameter_N}}` |
| `WallThickness` (List<double>) | 逗号分隔，2位小数 | `{{wallThicknessList}}`, `{{wallThicknessList_N}}` |
| `EndStandard` (string) | 直接使用 | `{{endStandard}}` |
| `Schedule` (string) | 直接使用 | `{{schedule}}` |

### 4.5 错误处理约束

#### 4.5.1 参数验证

| 错误场景 | HTTP 状态码 | 业务 Code | 错误消息 |
|---------|------------|-----------|---------|
| templateId 为空 | 400 | 400 | TemplateId cannot be null or empty |
| templateId 格式不合法 | 400 | 400 | Invalid templateId format |
| pmcCode 为空（按规格书填充时） | 400 | 400 | PmcCode cannot be null or empty |

#### 4.5.2 资源验证

| 错误场景 | HTTP 状态码 | 业务 Code | 错误消息 |
|---------|------------|-----------|---------|
| 模板文件不存在 | 404 | 2001 | Template file not found |
| Excel 无工作表 | 500 | 1000 | Excel file contains no worksheets |
| 工作表为空 | 500 | 1000 | Worksheet is empty |

#### 4.5.3 业务逻辑错误

| 错误场景 | HTTP 状态码 | 业务 Code | 处理方式 |
|---------|------------|-----------|---------|
| 获取通径信息失败 | - | - | 记录警告日志，占位符为空字符串 |
| 规格书数据为空 | - | - | 占位符为空字符串，不影响其他数据 |

---

## 5. 技术实现细节

### 5.1 核心类和方法

#### 5.1.1 TemplatePreviewService

**主要方法**：

- `GetTemplatePreview(templateId, parameters)` - 使用自定义参数预览
- `ExportTemplate(templateId, parameters)` - 使用自定义参数导出
- `GetTemplatePreviewBySpec(templateId, pmcCode)` - 使用规格书数据预览
- `ExportTemplateBySpec(templateId, pmcCode)` - 使用规格书数据导出
- `BuildSpecPlaceholderDictionary(pmcCode)` - 构建规格书占位符字典（私有）

**依赖注入**：

- `IPmcSpecService` - 规格书服务
- `ILogger<TemplatePreviewService>` - 日志服务

#### 5.1.2 TemplatePreviewController

**主要端点**：

- `GET /api/template-preview/{templateId}` - 模板预览
- `GET /api/template-preview/{templateId}/export` - 模板导出

**错误处理**：

- `ArgumentException` → 400 BadRequest
- `FileNotFoundException` → 404 NotFound
- `Exception` → 500 InternalServerError

### 5.2 日志记录

#### 5.2.1 日志级别

- **Information**：正常流程和成功操作
- **Warning**：参数/文件/数据异常
- **Debug**：详细调试信息（占位符构建过程）
- **Error**：由 Controller 层记录，Service 层不重复记录

#### 5.2.2 关键日志点

- 模板预览/导出开始和成功
- templateId 验证失败
- 模板文件不存在
- Excel 文件格式异常
- 规格书数据加载成功/失败
- 通径信息获取成功/失败

### 5.3 性能考虑

- **文件读取**：使用 `using` 语句确保资源释放
- **内存管理**：导出时使用 `MemoryStream` 避免大文件内存占用
- **Excel 处理**：就地修改模板文件，避免复制导致的性能损失
- **数据查询**：规格书数据查询使用 `AsNoTracking()` 提升性能

---

## 6. 测试建议

### 6.1 单元测试

- 占位符替换逻辑
- 合并单元格处理
- 标准信息格式化
- 通径信息格式化
- 类型键名规范化

### 6.2 集成测试

- 模板预览 API（自定义参数）
- 模板预览 API（规格书数据）
- 模板导出 API（自定义参数）
- 模板导出 API（规格书数据）
- 错误场景处理

### 6.3 边界测试

- 空模板文件
- 无合并单元格的模板
- 大量合并单元格的模板
- 规格书数据为空
- 通径信息获取失败
- 特殊字符处理

---

## 7. 后续优化建议

### 7.1 功能增强

- 支持多工作表模板
- 支持图片插入
- 支持公式计算
- 支持条件格式

### 7.2 性能优化

- 模板文件缓存
- 批量导出支持
- 异步处理大文件

### 7.3 用户体验

- 模板验证工具
- 占位符自动补全
- 模板预览优化（支持更多样式）

---

## 8. 相关文档

- [API 契约文档](./API_CONTRACT.md) - 详细的 API 接口说明
- [模板制作指南](./API_CONTRACT.md#48-模板预览与导出) - 模板文件制作规范

---

**文档结束**
