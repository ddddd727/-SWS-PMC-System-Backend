# CodeListManagement 前端开发契约

## 版本管理

| 版本 | 日期 | 描述 | 作者 |
|------|------|------|------|
| 1.0 | 2026-03-20 | 初始版本 | 系统生成 |

## 接口基础信息

### 基础 URL
- 本地开发：`http://localhost:5022/api/CodeListTableCatelog`
- 生产环境：待配置

## 接口列表

### 1. 获取所有代码列表目录

**接口路径**：`GET /`

**功能描述**：获取所有代码列表目录信息

**请求参数**：无

**响应格式**：
```json
[
  {
    "id": 1,
    "codeListTableName": "MaterialsGrade",
    "isUserDefined": true,
    "major": "Materials"
  },
  // 更多数据...
]
```

**数据类型**：
- `id`: number - 唯一标识符
- `codeListTableName`: string - 代码列表表名
- `isUserDefined`: boolean - 是否用户定义
- `major`: string - 主要分类

### 2. 创建代码列表目录

**接口路径**：`POST /`

**功能描述**：创建新的代码列表目录

**请求参数**：
```json
{
  "codeListTableName": "NewCodeList",
  "isUserDefined": true,
  "major": "Test"
}
```

**响应格式**：
```json
{
  "id": 1,
  "codeListTableName": "NewCodeList",
  "isUserDefined": true,
  "major": "Test"
}
```

**数据类型**：同获取所有代码列表目录

### 3. 获取层级信息

**接口路径**：`GET /hierarchy/{codeListTableName}`

**功能描述**：根据代码列表表名获取层级信息

**请求参数**：
- `codeListTableName`: string - 代码列表表名（路径参数）

**响应格式**：
```json
{
  "count": 3,
  "level1": "MaterialsGradePractice",
  "level2": "MaterialsCategory",
  "level3": "MaterialsGrade",
  "level4": null,
  "level5": null
}
```

**数据类型**：
- `count`: number - 层级数量
- `level1`: string - 第一层级名称
- `level2`: string - 第二层级名称
- `level3`: string - 第三层级名称
- `level4`: string - 第四层级名称
- `level5`: string - 第五层级名称

### 4. 获取多层级数据

**接口路径**：`GET /multilevel`

**功能描述**：获取指定层级的数据

**请求参数**：
- `level1`: string - 第一层级名称（查询参数，可选）
- `level2`: string - 第二层级名称（查询参数，可选）
- `level3`: string - 第三层级名称（查询参数，可选）
- `level4`: string - 第四层级名称（查询参数，可选）
- `level5`: string - 第五层级名称（查询参数，可选）

**响应格式**：
```json
{
  "levelData": [
    {
      "level1ShortDesc": "MaterialsGradePractice",
      "level1LongDesc": "材料等级实践",
      "level1CodeNum": 1,
      "level1Status": 1,
      "level2ShortDesc": null,
      "level2LongDesc": null,
      "level2CodeNum": null,
      "level2Status": null,
      "level3ShortDesc": null,
      "level3LongDesc": null,
      "level3CodeNum": null,
      "level3Status": null,
      "level4ShortDesc": null,
      "level4LongDesc": null,
      "level4CodeNum": null,
      "level4Status": null,
      "level5ShortDesc": null,
      "level5LongDesc": null,
      "level5CodeNum": null,
      "level5Status": null
    },
    // 更多数据...
  ]
}
```

**数据类型**：
- `levelData`: array - 层级数据数组
  - `level1ShortDesc`: string - 第一层级短描述
  - `level1LongDesc`: string - 第一层级长描述
  - `level1CodeNum`: number - 第一层级代码编号
  - `level1Status`: number - 第一层级状态（1=启用，0=禁用）
  - `level2ShortDesc`: string - 第二层级短描述
  - `level2LongDesc`: string - 第二层级长描述
  - `level2CodeNum`: number - 第二层级代码编号
  - `level2Status`: number - 第二层级状态
  - `level3ShortDesc`: string - 第三层级短描述
  - `level3LongDesc`: string - 第三层级长描述
  - `level3CodeNum`: number - 第三层级代码编号
  - `level3Status`: number - 第三层级状态
  - `level4ShortDesc`: string - 第四层级短描述
  - `level4LongDesc`: string - 第四层级长描述
  - `level4CodeNum`: number - 第四层级代码编号
  - `level4Status`: number - 第四层级状态
  - `level5ShortDesc`: string - 第五层级短描述
  - `level5LongDesc`: string - 第五层级长描述
  - `level5CodeNum`: number - 第五层级代码编号
  - `level5Status`: number - 第五层级状态

### 5. 获取组合数据

**接口路径**：`GET /combined/{codeListTableName}`

**功能描述**：获取指定代码列表的层级信息和数据

**请求参数**：
- `codeListTableName`: string - 代码列表表名（路径参数）

**响应格式**：
```json
{
  "count": 3,
  "level1": "MaterialsGradePractice",
  "level2": "MaterialsCategory",
  "level3": "MaterialsGrade",
  "level4": null,
  "level5": null,
  "levelData": [
    {
      "level1ShortDesc": "MaterialsGradePractice",
      "level1LongDesc": "材料等级实践",
      "level1CodeNum": 1,
      "level1Status": 1,
      "level2ShortDesc": null,
      "level2LongDesc": null,
      "level2CodeNum": null,
      "level2Status": null,
      "level3ShortDesc": null,
      "level3LongDesc": null,
      "level3CodeNum": null,
      "level3Status": null,
      "level4ShortDesc": null,
      "level4LongDesc": null,
      "level4CodeNum": null,
      "level4Status": null,
      "level5ShortDesc": null,
      "level5LongDesc": null,
      "level5CodeNum": null,
      "level5Status": null
    },
    // 更多数据...
  ]
}
```

**数据类型**：
- 包含层级信息和层级数据的组合结构

### 6. 根据父级短描述获取数据

**接口路径**：`GET /values/by-parent/{shortStringValue}`

**功能描述**：根据短描述获取相关的父级代码列表数据

**请求参数**：
- `shortStringValue`: string - 短描述（路径参数）

**响应格式**：
```json
[
  {
    "shortStringValue": "10#",
    "longStringValue": "10#",
    "codeListNumber": 10001,
    "status": 1
  },
  // 更多数据...
]
```

**数据类型**：
- `shortStringValue`: string - 短描述
- `longStringValue`: string - 长描述
- `codeListNumber`: number - 代码编号
- `status`: number - 状态（1=启用，0=禁用）

## 错误处理

| 状态码 | 描述 | 示例响应 |
|--------|------|----------|
| 400 | 请求参数错误 | `{"type": "https://tools.ietf.org/html/rfc9110#section-15.5.1", "title": "Bad Request", "status": 400, "detail": "codeListTableName cannot be empty", "traceId": "00-...-..."}` |
| 404 | 资源不存在 | `{"type": "https://tools.ietf.org/html/rfc9110#section-15.5.5", "title": "Not Found", "status": 404, "detail": "No hierarchy found", "traceId": "00-...-..."}` |
| 500 | 服务器内部错误 | `{"type": "https://tools.ietf.org/html/rfc9110#section-15.6.1", "title": "Internal Server Error", "status": 500, "detail": "An error occurred while processing your request", "traceId": "00-...-..."}` |

## 示例请求

### 获取层级信息
```bash
curl -X GET "http://localhost:5022/api/CodeListTableCatelog/hierarchy/MaterialsGrade"
```

### 获取多层级数据
```bash
curl -X GET "http://localhost:5022/api/CodeListTableCatelog/multilevel?level1=MaterialsGradePractice&level2=MaterialsCategory"
```

### 获取组合数据
```bash
curl -X GET "http://localhost:5022/api/CodeListTableCatelog/combined/MaterialsGrade"
```

### 根据父级短描述获取数据
```bash
curl -X GET "http://localhost:5022/api/CodeListTableCatelog/values/by-parent/Carbon Steels"
```
