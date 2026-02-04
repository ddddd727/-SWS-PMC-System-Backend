# PMC管系规格配置API契约文档

## 文档信息

| 项目               | 内容             |
| ------------------ | ---------------- |
| **版本号**   | v1.0             |
| **生成日期** | 2026-02-03       |
| **基础路径** | `/api/PmcSpec` |
| **协议**     | HTTP/HTTPS       |
| **数据格式** | JSON             |
| **字符编码** | UTF-8            |
| **最后更新** | 2026-02-03       |

---

## 目录

- [1. API概述](#1-api概述)
- [2. 通用约定](#2-通用约定)
- [3. 数据模型](#3-数据模型)
- [4. 接口列表](#4-接口列表)
  - [4.1 获取船型船号信息](#41-获取船型船号信息)
  - [4.2 获取部件类型列表](#42-获取部件类型列表)
  - [4.3 根据船号获取PMC编码](#43-根据船号获取pmc编码)
  - [4.4 解析PMC编码](#44-解析pmc编码)
  - [4.5 获取NPD信息](#45-获取npd信息)
  - [4.6 获取管附件规格](#46-获取管附件规格)
  - [4.7 保存规格书配置](#47-保存规格书配置)
- [5. 错误码说明](#5-错误码说明)
- [6. 前端调用示例](#6-前端调用示例)

---

## 1. API概述

本文档定义了PMC管系规格配置模块的所有API接口，用于前后端开发人员之间的协作。

### 1.1 主要功能

- 船型船号管理
- PMC编码查询与解析
- 部件类型配置
- 规格书信息管理

### 1.2 技术栈

- **后端框架**: ASP.NET Core 6.0+
- **前端建议**: Vue 3 / React 18+
- **HTTP客户端**: Axios / Fetch API

---

## 2. 通用约定

### 2.1 统一响应格式

所有接口均返回统一的响应格式：

#### 成功响应（带数据）

```json
{
  "code": 200,
  "message": "操作成功",
  "data": {
    // 具体业务数据
  },
  "timestamp": "2026-02-03T10:30:00Z",
  "traceId": "0HMVD7K3QH1A2"
}
```

#### 成功响应（无数据）

```json
{
  "code": 200,
  "message": "保存成功",
  "timestamp": "2026-02-03T10:30:00Z",
  "traceId": "0HMVD7K3QH1A3"
}
```

#### 错误响应

```json
{
  "code": 400,
  "message": "请求参数验证失败",
  "data": [
    {
      "field": "shipType",
      "message": "船型不能为空",
      "errorCode": "VALIDATION_ERROR"
    }
  ],
  "timestamp": "2026-02-03T10:30:00Z",
  "traceId": "0HMVD7K3QH1A4"
}
```

### 2.2 HTTP状态码

| 状态码 | 说明                  | 场景               |
| ------ | --------------------- | ------------------ |
| 200    | OK                    | 请求成功           |
| 400    | Bad Request           | 参数验证失败       |
| 401    | Unauthorized          | 未授权（需要登录） |
| 403    | Forbidden             | 无权限访问         |
| 404    | Not Found             | 资源不存在         |
| 500    | Internal Server Error | 服务器内部错误     |

### 2.3 业务错误码

| 错误码 | 说明             | 处理建议            |
| ------ | ---------------- | ------------------- |
| 400    | 参数验证错误     | 检查请求参数        |
| 404    | 资源不存在       | 提示用户资源未找到  |
| 1001   | 业务规则校验失败 | 根据message提示用户 |
| 1002   | 重复资源         | 提示用户资源已存在  |

### 2.4 命名规范

- **请求参数**: 使用camelCase（驼峰命名）
- **响应字段**: 使用camelCase（驼峰命名）
- **日期时间**: ISO 8601格式 (`YYYY-MM-DDTHH:mm:ssZ`)
- **布尔值**: `true` / `false`

### 2.5 分页约定

暂无分页接口，所有列表接口返回全量数据。

---

## 3. 数据模型

### 3.1 ShipInfo - 船型船号信息

```typescript
interface ShipInfo {
  shipNumber: string;   // 船号，如: "H1234"
  shipType: string;     // 船型，如: "散货船"
}
```

**示例：**

```json
{
  "shipNumber": "H1234",
  "shipType": "散货船"
}
```

---

### 3.2 ComponentTypeInfo - 部件类型信息

```typescript
interface ComponentTypeInfo {
  componentTypeName: string;         // 部件类型名称，如: "Elbow"
  componentTypeDescription: string;  // 部件类型描述，如: "弯头"
}
```

**示例：**

```json
{
  "componentTypeName": "Elbow",
  "componentTypeDescription": "弯头"
}
```

**可用部件类型：**

- Pipe - 管道
- Elbow - 弯头
- Reducer - 异径管
- Tee - 三通
- Sleeve - 管套
- Bosses - 凸台
- Saddles - 鞍座
- Caps - 管帽
- Overpass - 跨接
- Accessories - 附件
- Flange - 法兰
- BlindFlange - 盲板
- Gasket - 垫片
- Bolt - 螺栓
- Nut - 螺母
- Washer - 垫圈

---

### 3.3 PmcSelectInfo - PMC编码选择信息

```typescript
interface PmcSelectInfo {
  pmcCode: string;       // PMC 7位编码
  shipNumber: string;    // 船号
  material: string;      // 主材料
  pipeStandard: string;  // 管材标准
  status: string;        // PMC编码状态: "未配置" | "已配置"
}
```

**示例：**

```json
{
  "pmcCode": "A1B2C3D",
  "shipNumber": "H1234",
  "material": "Carbon Steel",
  "pipeStandard": "ASME B36.10",
  "status": "已配置"
}
```

---

### 3.4 PmcBaseInfo - PMC基础信息

```typescript
interface PmcBaseInfo {
  pmcCode: string;           // PMC 7位编码（必填）
  shipNumber?: string;       // 船号
  status?: string;           // PMC编码状态
  pipingClass?: string;      // 管道等级
  materialGrade?: string;    // 牌号
  pressureRating?: string;   // 法兰压力等级
  pipeStandard?: string;     // 管材标准
  materialCategory?: string; // 管材材料
  wallThickness?: string;    // 壁厚系列
}
```

**示例：**

```json
{
  "pmcCode": "A1B2C3D",
  "shipNumber": "H1234",
  "status": "已配置",
  "pipingClass": "150#",
  "materialGrade": "A105",
  "pressureRating": "Class 150",
  "pipeStandard": "ASME B36.10",
  "materialCategory": "Carbon Steel",
  "wallThickness": "Sch40"
}
```

---

### 3.5 SpecNPDInfo - NPD信息

```typescript
interface SpecNPDInfo {
  endStandard?: string;          // 端面标准
  schedule?: string;             // 壁厚系列
  npd?: number[];               // 通径范围（单位: mm）
  outsideDiameter?: number[];   // 外径范围（单位: mm）
  wallThickness?: number[];     // 壁厚范围（单位: mm）
}
```

**示例：**

```json
{
  "endStandard": "ASME B16.9",
  "schedule": "Sch40",
  "npd": [15, 20, 25, 32, 40, 50, 65, 80, 100],
  "outsideDiameter": [21.3, 26.9, 33.7, 42.4, 48.3, 60.3, 76.1, 88.9, 114.3],
  "wallThickness": [2.77, 2.87, 3.38, 3.56, 3.68, 3.91, 5.16, 5.49, 6.02]
}
```

---

### 3.6 PipeFittingSpec - 管附件规格

```typescript
interface PipeFittingSpec {
  standardName: string;     // 标准名称
  materialList: string[];   // 材料列表
}
```

**示例：**

```json
{
  "standardName": "ASME B16.9",
  "materialList": [
    "Carbon Steel",
    "Stainless Steel 304",
    "Stainless Steel 316"
  ]
}
```

---

### 3.7 SavePipeSpecRequest - 保存规格书请求

```typescript
interface SavePipeSpecRequest {
  shipType: string;                           // 船型（必填）
  shipNumber: string;                         // 船号（必填）
  pmcCode: string;                           // PMC编码（必填）
  configurations: ComponentTypeConfiguration[]; // 部件类型配置列表（至少1个）
  metadata?: Record<string, any>;            // 可选元数据
}

interface ComponentTypeConfiguration {
  componentType: string;                     // 部件类型（必填），支持首字母大写规范化，如 "elbow"→"Elbow"
                                            // 支持: Elbow, Reducer, Tee, Pipe, Sleeve, Bosses, Saddles, Caps,
                                            //       Overpass, Accessories, Flange, BlindFlange, Gasket, Bolt, Nut, Washer
  configResult?: string;                     // 配置结果描述
  fullConfig?: ComponentFullConfiguration;   // 完整配置信息
}

interface ComponentFullConfiguration {
  standardFileIds?: any[];                   // 标准文件ID列表
  standardFileConfigs?: StandardFileConfig[]; // 标准文件配置（简化版），与 configurations 二选一或同时使用
  configurations?: StandardFileConfiguration[]; // 标准文件配置（完整版）
  duplicateRangeDefaults?: DuplicateRangeDefault[]; // 重复范围默认配置
}

/** 标准文件配置（简化版），当使用 standardFileConfigs 时采用此结构 */
interface StandardFileConfig {
  standardFile?: any;        // 标准文件ID或名称
  material?: any;            // 材料ID或名称
  minNpdValue?: number;      // 最小NPD值
  maxNpdValue?: number;      // 最大NPD值
  bendRadiusMultiple?: any;  // 弯管半径倍数
}

interface StandardFileConfiguration {
  standardFileId?: any;      // 标准文件ID
  standardFileName?: string; // 标准文件名称
  materialId?: any;          // 材料ID
  materialName?: string;     // 材料名称
  npdRange?: [number, number] | [string, string]; // NPD范围 [最小值, 最大值]，支持 number 或 string
  bendRadiusMultiple?: any;  // 弯管半径倍数
}

interface DuplicateRangeDefault {
  overlapMin: number;               // 重叠范围最小值
  overlapMax: number;               // 重叠范围最大值
  defaultStandardFileId?: any;      // 默认标准文件ID
  defaultStandardFileName?: string; // 默认标准文件名称
  ranges?: DiameterRange[];         // 范围列表
  standardFiles?: any[];            // 标准文件列表
  rangeKey?: string;                // 范围键
}

interface DiameterRange {
  minNpdValue: number;     // 最小NPD值
  maxNpdValue: number;     // 最大NPD值
  standardFile?: any;      // 标准文件
}
```

**完整示例：**

```json
{
  "shipType": "散货船",
  "shipNumber": "H1234",
  "pmcCode": "A1B2C3D",
  "configurations": [
    {
      "componentType": "Elbow",
      "configResult": "配置成功",
      "fullConfig": {
        "configurations": [
          {
            "standardFileId": 1,
            "standardFileName": "ASME B16.9",
            "materialId": 10,
            "materialName": "Carbon Steel",
            "npdRange": [15, 100],
            "bendRadiusMultiple": 1.5
          }
        ],
        "duplicateRangeDefaults": []
      }
    }
  ],
  "metadata": {
    "source": "web",
    "operator": "admin"
  }
}
```

---

### 3.8 GetNPDInfoRequest - 获取NPD信息请求

```typescript
interface GetNPDInfoRequest {
  endStandard: string;   // 端面标准（必填）
  schedule: string;      // 壁厚系列（必填）
}
```

---

### 3.9 GetPipeFittingSpecRequest - 获取管附件规格请求

```typescript
interface GetPipeFittingSpecRequest {
  componentTypeName: string;  // 部件类型名称（必填）
}
```

---

## 4. 接口列表

### 4.1 获取船型船号信息

获取系统中所有可用的船型船号信息。

#### 基本信息

- **接口地址**: `GET /api/PmcSpec/ShipInfos`
- **请求方式**: GET
- **权限要求**: 无
- **内容类型**: application/json

#### 请求参数

无

#### 响应数据

**成功响应 (200)**

```json
{
  "code": 200,
  "message": "获取成功",
  "data": [
    {
      "shipNumber": "H1234",
      "shipType": "散货船"
    },
    {
      "shipNumber": "H1235",
      "shipType": "集装箱船"
    }
  ],
  "timestamp": "2026-02-03T10:30:00Z",
  "traceId": "0HMVD7K3QH1A2"
}
```

**失败响应 (404)**

```json
{
  "code": 404,
  "message": "未找到船型船号信息",
  "timestamp": "2026-02-03T10:30:00Z",
  "traceId": "0HMVD7K3QH1A3"
}
```

#### TypeScript类型定义

```typescript
// 响应类型
type GetShipInfosResponse = ApiResponse<ShipInfo[]>;

interface ShipInfo {
  shipNumber: string;
  shipType: string;
}
```

#### 前端调用示例

```typescript
// Axios
const response = await axios.get<GetShipInfosResponse>('/api/PmcSpec/ShipInfos');
const shipInfos = response.data.data;

// Fetch
const response = await fetch('/api/PmcSpec/ShipInfos');
const result: GetShipInfosResponse = await response.json();
```

---

### 4.2 获取部件类型列表

获取所有可用的部件类型信息，用于规格书配置。

#### 基本信息

- **接口地址**: `GET /api/PmcSpec/ComponentTypes`
- **请求方式**: GET
- **权限要求**: 无
- **内容类型**: application/json

#### 请求参数

无

#### 响应数据

**成功响应 (200)**

```json
{
  "code": 200,
  "message": "获取成功",
  "data": [
    {
      "componentTypeName": "Elbow",
      "componentTypeDescription": "弯头"
    },
    {
      "componentTypeName": "Tee",
      "componentTypeDescription": "三通"
    },
    {
      "componentTypeName": "Reducer",
      "componentTypeDescription": "异径管"
    }
  ],
  "timestamp": "2026-02-03T10:30:00Z",
  "traceId": "0HMVD7K3QH1A4"
}
```

**失败响应 (404)**

```json
{
  "code": 404,
  "message": "未找到部件类型信息",
  "timestamp": "2026-02-03T10:30:00Z",
  "traceId": "0HMVD7K3QH1A5"
}
```

#### TypeScript类型定义

```typescript
type GetComponentTypesResponse = ApiResponse<ComponentTypeInfo[]>;

interface ComponentTypeInfo {
  componentTypeName: string;
  componentTypeDescription: string;
}
```

---

### 4.3 根据船号获取PMC编码

根据指定的船号查询对应的PMC编码列表。

#### 基本信息

- **接口地址**: `GET /api/PmcSpec/PmcRules/{shipNumber}`
- **请求方式**: GET
- **权限要求**: 无
- **内容类型**: application/json

#### 请求参数

| 参数名     | 类型   | 位置 | 必填 | 说明 | 示例  |
| ---------- | ------ | ---- | ---- | ---- | ----- |
| shipNumber | string | Path | 是   | 船号 | H1234 |

#### 请求示例

```
GET /api/PmcSpec/PmcRules/H1234
```

#### 响应数据

**成功响应 (200)**

```json
{
  "code": 200,
  "message": "获取成功",
  "data": [
    {
      "pmcCode": "A1B2C3D",
      "shipNumber": "H1234",
      "material": "Carbon Steel",
      "pipeStandard": "ASME B36.10",
      "status": "已配置"
    },
    {
      "pmcCode": "A1B2C3E",
      "shipNumber": "H1234",
      "material": "Stainless Steel",
      "pipeStandard": "ASME B36.19",
      "status": "未配置"
    }
  ],
  "timestamp": "2026-02-03T10:30:00Z",
  "traceId": "0HMVD7K3QH1A6"
}
```

**失败响应 (400) - 参数验证失败**

```json
{
  "code": 400,
  "message": "请求参数验证失败",
  "data": [
    {
      "field": "shipNumber",
      "message": "船号不能为空",
      "errorCode": "VALIDATION_ERROR"
    }
  ],
  "timestamp": "2026-02-03T10:30:00Z",
  "traceId": "0HMVD7K3QH1A7"
}
```

**失败响应 (404) - 未找到数据**

```json
{
  "code": 404,
  "message": "未找到对应的PMC编码数据",
  "timestamp": "2026-02-03T10:30:00Z",
  "traceId": "0HMVD7K3QH1A8"
}
```

#### TypeScript类型定义

```typescript
type GetPmcRulesResponse = ApiResponse<PmcSelectInfo[]>;

interface PmcSelectInfo {
  pmcCode: string;
  shipNumber: string;
  material: string;
  pipeStandard: string;
  status: string;
}
```

---

### 4.4 解析PMC编码

解析PMC 7位编码，获取详细的基础信息。

#### 基本信息

- **接口地址**: `GET /api/PmcSpec/Analyze/{pmcCode}`
- **请求方式**: GET
- **权限要求**: 无
- **内容类型**: application/json

#### 请求参数

| 参数名  | 类型   | 位置 | 必填 | 说明        | 示例    |
| ------- | ------ | ---- | ---- | ----------- | ------- |
| pmcCode | string | Path | 是   | PMC 7位编码 | A1B2C3D |

#### 请求示例

```
GET /api/PmcSpec/Analyze/A1B2C3D
```

#### 响应数据

**成功响应 (200)**

```json
{
  "code": 200,
  "message": "解析成功",
  "data": {
    "pmcCode": "A1B2C3D",
    "shipNumber": "H1234",
    "status": "已配置",
    "pipingClass": "150#",
    "materialGrade": "A105",
    "pressureRating": "Class 150",
    "pipeStandard": "ASME B36.10",
    "materialCategory": "Carbon Steel",
    "wallThickness": "Sch40"
  },
  "timestamp": "2026-02-03T10:30:00Z",
  "traceId": "0HMVD7K3QH1A9"
}
```

**失败响应 (400) - 格式错误**

```json
{
  "code": 400,
  "message": "PMC编码格式不正确，请检查编码是否为7位有效字符",
  "timestamp": "2026-02-03T10:30:00Z",
  "traceId": "0HMVD7K3QH1AA"
}
```

#### TypeScript类型定义

```typescript
type AnalyzePmcCodeResponse = ApiResponse<PmcBaseInfo>;

interface PmcBaseInfo {
  pmcCode: string;
  shipNumber?: string;
  status?: string;
  pipingClass?: string;
  materialGrade?: string;
  pressureRating?: string;
  pipeStandard?: string;
  materialCategory?: string;
  wallThickness?: string;
}
```

---

### 4.5 获取NPD信息

根据端面标准和壁厚系列获取通径、外径、壁厚信息。

#### 基本信息

- **接口地址**: `GET /api/PmcSpec/NPDInfo`
- **请求方式**: GET
- **权限要求**: 无
- **内容类型**: application/json

#### 请求参数

| 参数名      | 类型   | 位置  | 必填 | 说明     | 示例       |
| ----------- | ------ | ----- | ---- | -------- | ---------- |
| endStandard | string | Query | 是   | 端面标准 | ASME B16.9 |
| schedule    | string | Query | 是   | 壁厚系列 | Sch40      |

#### 请求示例

```
GET /api/PmcSpec/NPDInfo?endStandard=ASME%20B16.9&schedule=Sch40
```

#### 响应数据

**成功响应 (200)**

```json
{
  "code": 200,
  "message": "获取成功",
  "data": {
    "endStandard": "ASME B16.9",
    "schedule": "Sch40",
    "npd": [15, 20, 25, 32, 40, 50, 65, 80, 100, 125, 150],
    "outsideDiameter": [21.3, 26.9, 33.7, 42.4, 48.3, 60.3, 76.1, 88.9, 114.3, 141.3, 168.3],
    "wallThickness": [2.77, 2.87, 3.38, 3.56, 3.68, 3.91, 5.16, 5.49, 6.02, 6.55, 7.11]
  },
  "timestamp": "2026-02-03T10:30:00Z",
  "traceId": "0HMVD7K3QH1AB"
}
```

**失败响应 (400) - 参数验证失败**

```json
{
  "code": 400,
  "message": "请求参数验证失败",
  "data": [
    {
      "field": "endStandard",
      "message": "端面标准不能为空",
      "errorCode": "VALIDATION_ERROR"
    }
  ],
  "timestamp": "2026-02-03T10:30:00Z",
  "traceId": "0HMVD7K3QH1AC"
}
```

**失败响应 (404) - 未找到数据**

```json
{
  "code": 404,
  "message": "未找到对应的NPD信息",
  "timestamp": "2026-02-03T10:30:00Z",
  "traceId": "0HMVD7K3QH1AD"
}
```

#### TypeScript类型定义

```typescript
type GetNPDInfoResponse = ApiResponse<SpecNPDInfo>;

interface SpecNPDInfo {
  endStandard?: string;
  schedule?: string;
  npd?: number[];
  outsideDiameter?: number[];
  wallThickness?: number[];
}
```

---

### 4.6 获取管附件规格

根据部件类型获取对应的标准列表和材料列表。

#### 基本信息

- **接口地址**: `GET /api/PmcSpec/PipeFittingSpec`
- **请求方式**: GET
- **权限要求**: 无
- **内容类型**: application/json

#### 请求参数

| 参数名            | 类型   | 位置  | 必填 | 说明         | 示例  |
| ----------------- | ------ | ----- | ---- | ------------ | ----- |
| componentTypeName | string | Query | 是   | 部件类型名称 | Elbow |

#### 请求示例

```
GET /api/PmcSpec/PipeFittingSpec?componentTypeName=Elbow
```

#### 响应数据

**成功响应 (200)**

```json
{
  "code": 200,
  "message": "获取成功",
  "data": [
    {
      "standardName": "ASME B16.9",
      "materialList": [
        "Carbon Steel",
        "Stainless Steel 304",
        "Stainless Steel 316"
      ]
    },
    {
      "standardName": "JIS B2311",
      "materialList": [
        "Carbon Steel",
        "Stainless Steel"
      ]
    }
  ],
  "timestamp": "2026-02-03T10:30:00Z",
  "traceId": "0HMVD7K3QH1AE"
}
```

**失败响应 (400) - 参数验证失败**

```json
{
  "code": 400,
  "message": "请求参数验证失败",
  "data": [
    {
      "field": "componentTypeName",
      "message": "部件类型名称不能为空",
      "errorCode": "VALIDATION_ERROR"
    }
  ],
  "timestamp": "2026-02-03T10:30:00Z",
  "traceId": "0HMVD7K3QH1AF"
}
```

**失败响应 (404) - 未找到数据**

```json
{
  "code": 404,
  "message": "未找到对应的标准列表和材料列表",
  "timestamp": "2026-02-03T10:30:00Z",
  "traceId": "0HMVD7K3QH1AG"
}
```

#### TypeScript类型定义

```typescript
type GetPipeFittingSpecResponse = ApiResponse<PipeFittingSpec[]>;

interface PipeFittingSpec {
  standardName: string;
  materialList: string[];
}
```

---

### 4.7 保存规格书配置

保存管系规格书的完整配置信息。后端按 `(pmcCode, shipType, shipNumber)` 精确匹配数据库中已存在的记录进行更新，**请确保该组合对应的 PMC 数据已预先存在**。

#### 基本信息

- **接口地址**: `POST /api/PmcSpec/SpecRules`
- **请求方式**: POST
- **权限要求**: 需要登录
- **内容类型**: application/json

#### 请求参数

| 参数名         | 类型   | 位置 | 必填 | 说明                        |
| -------------- | ------ | ---- | ---- | --------------------------- |
| shipType       | string | Body | 是   | 船型                        |
| shipNumber     | string | Body | 是   | 船号                        |
| pmcCode        | string | Body | 是   | PMC编码                     |
| configurations | array  | Body | 是   | 部件类型配置列表（至少1个） |
| metadata       | object | Body | 否   | 可选元数据                  |

#### 请求体示例

**最小请求体：**

```json
{
  "shipType": "散货船",
  "shipNumber": "H1234",
  "pmcCode": "A1B2C3D",
  "configurations": [
    {
      "componentType": "Elbow"
    }
  ]
}
```

**完整请求体：**

```json
{
  "shipType": "散货船",
  "shipNumber": "H1234",
  "pmcCode": "A1B2C3D",
  "configurations": [
    {
      "componentType": "Elbow",
      "configResult": "配置成功",
      "fullConfig": {
        "configurations": [
          {
            "standardFileId": 1,
            "standardFileName": "ASME B16.9",
            "materialId": 10,
            "materialName": "Carbon Steel",
            "npdRange": [15, 100],
            "bendRadiusMultiple": 1.5
          },
          {
            "standardFileId": 2,
            "standardFileName": "JIS B2311",
            "materialId": 10,
            "materialName": "Carbon Steel",
            "npdRange": [100, 300],
            "bendRadiusMultiple": 1.5
          }
        ],
        "duplicateRangeDefaults": [
          {
            "overlapMin": 50,
            "overlapMax": 80,
            "defaultStandardFileId": 1,
            "defaultStandardFileName": "ASME B16.9",
            "ranges": [
              {
                "minNpdValue": 50,
                "maxNpdValue": 65
              },
              {
                "minNpdValue": 65,
                "maxNpdValue": 80
              }
            ]
          }
        ]
      }
    },
    {
      "componentType": "Tee",
      "fullConfig": {
        "configurations": [
          {
            "standardFileName": "ASME B16.9",
            "materialName": "Stainless Steel",
            "npdRange": [15, 200]
          }
        ]
      }
    }
  ],
  "metadata": {
    "source": "web",
    "operator": "admin",
    "timestamp": "2026-02-03T10:30:00Z"
  }
}
```

#### 响应数据

**成功响应 (200)**

```json
{
  "code": 200,
  "message": "规格书配置保存成功",
  "timestamp": "2026-02-03T10:30:00Z",
  "traceId": "0HMVD7K3QH1AH"
}
```

**失败响应 (400) - 参数验证失败**

```json
{
  "code": 400,
  "message": "请求参数验证失败",
  "data": [
    {
      "field": "shipType",
      "message": "船型不能为空",
      "errorCode": "VALIDATION_ERROR"
    },
    {
      "field": "configurations",
      "message": "请至少配置一个部件类型",
      "errorCode": "VALIDATION_ERROR"
    }
  ],
  "timestamp": "2026-02-03T10:30:00Z",
  "traceId": "0HMVD7K3QH1AI"
}
```

**失败响应 (400) - 业务规则失败（如记录不存在）**

```json
{
  "code": 400,
  "message": "未找到PMC编码 A1B2C3D（船型: 散货船, 船号: H1234）对应的数据",
  "timestamp": "2026-02-03T10:30:00Z",
  "traceId": "0HMVD7K3QH1AJ"
}
```

> **说明**：后端按 `(pmcCode, shipType, shipNumber)` 精确查找记录，三者须与数据库中已存在的数据一致。

#### TypeScript类型定义

```typescript
type SaveSpecRulesResponse = ApiResponse<null>;

interface SavePipeSpecRequest {
  shipType: string;
  shipNumber: string;
  pmcCode: string;
  configurations: ComponentTypeConfiguration[];
  metadata?: Record<string, any>;
}
```

---

## 5. 错误码说明

### 5.1 HTTP状态码对应关系

| HTTP状态码 | 业务Code | 说明             | 前端处理建议       |
| ---------- | -------- | ---------------- | ------------------ |
| 200        | 200      | 成功             | 正常处理           |
| 400        | 400      | 参数验证错误     | 显示字段级错误信息 |
| 404        | 404      | 资源未找到       | 提示用户资源不存在 |
| 400        | 1001     | 业务规则校验失败 | 显示错误消息       |
| 500        | 500      | 服务器内部错误   | 提示用户稍后重试   |

### 5.2 常见错误处理

#### 参数验证错误 (400)

```json
{
  "code": 400,
  "message": "请求参数验证失败",
  "data": [
    {
      "field": "shipType",
      "message": "船型不能为空",
      "errorCode": "VALIDATION_ERROR"
    }
  ]
}
```

**前端处理：**

```typescript
if (response.data.code === 400 && Array.isArray(response.data.data)) {
  // 显示字段级错误
  response.data.data.forEach(error => {
    showFieldError(error.field, error.message);
  });
}
```

#### 资源未找到 (404)

```json
{
  "code": 404,
  "message": "未找到船型船号信息"
}
```

**前端处理：**

```typescript
if (response.data.code === 404) {
  showMessage('warning', response.data.message);
}
```

---

## 6. 前端调用示例

### 6.1 Axios封装

```typescript
// api/client.ts
import axios, { AxiosInstance, AxiosResponse } from 'axios';

const apiClient: AxiosInstance = axios.create({
  baseURL: '/api',
  timeout: 30000,
  headers: {
    'Content-Type': 'application/json',
  },
});

// 请求拦截器
apiClient.interceptors.request.use(
  (config) => {
    // 添加token
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// 响应拦截器
apiClient.interceptors.response.use(
  (response: AxiosResponse<ApiResponse<any>>) => {
    const { code, message } = response.data;
  
    if (code === 200) {
      return response;
    } else {
      // 处理业务错误
      ElMessage.error(message || '操作失败');
      return Promise.reject(new Error(message));
    }
  },
  (error) => {
    // 处理HTTP错误
    if (error.response) {
      const { status, data } = error.response;
      switch (status) {
        case 401:
          ElMessage.error('未授权，请重新登录');
          // 跳转登录页
          router.push('/login');
          break;
        case 403:
          ElMessage.error('无权限访问');
          break;
        case 404:
          ElMessage.error(data.message || '资源不存在');
          break;
        case 500:
          ElMessage.error('服务器错误，请稍后重试');
          break;
        default:
          ElMessage.error(data.message || '请求失败');
      }
    } else {
      ElMessage.error('网络错误，请检查网络连接');
    }
    return Promise.reject(error);
  }
);

export default apiClient;
```

### 6.2 API封装

```typescript
// api/pmcSpec.ts
import apiClient from './client';
import type {
  ShipInfo,
  ComponentTypeInfo,
  PmcSelectInfo,
  PmcBaseInfo,
  SpecNPDInfo,
  PipeFittingSpec,
  SavePipeSpecRequest,
  ApiResponse
} from '@/types';

export const pmcSpecApi = {
  // 获取船型船号信息
  getShipInfos() {
    return apiClient.get<ApiResponse<ShipInfo[]>>('/PmcSpec/ShipInfos');
  },

  // 获取部件类型列表
  getComponentTypes() {
    return apiClient.get<ApiResponse<ComponentTypeInfo[]>>('/PmcSpec/ComponentTypes');
  },

  // 根据船号获取PMC编码
  getPmcRulesByShipNumber(shipNumber: string) {
    return apiClient.get<ApiResponse<PmcSelectInfo[]>>(`/PmcSpec/PmcRules/${shipNumber}`);
  },

  // 解析PMC编码
  analyzePmcCode(pmcCode: string) {
    return apiClient.get<ApiResponse<PmcBaseInfo>>(`/PmcSpec/Analyze/${pmcCode}`);
  },

  // 获取NPD信息
  getNPDInfo(params: { endStandard: string; schedule: string }) {
    return apiClient.get<ApiResponse<SpecNPDInfo>>('/PmcSpec/NPDInfo', { params });
  },

  // 获取管附件规格
  getPipeFittingSpec(componentTypeName: string) {
    return apiClient.get<ApiResponse<PipeFittingSpec[]>>('/PmcSpec/PipeFittingSpec', {
      params: { componentTypeName }
    });
  },

  // 保存规格书配置
  saveSpecRules(data: SavePipeSpecRequest) {
    return apiClient.post<ApiResponse<null>>('/PmcSpec/SpecRules', data);
  }
};
```

### 6.3 Vue 3 使用示例

```vue
<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { pmcSpecApi } from '@/api/pmcSpec';
import type { ShipInfo, SavePipeSpecRequest } from '@/types';

// 船型船号列表
const shipInfos = ref<ShipInfo[]>([]);
const loading = ref(false);

// 获取船型船号信息
const fetchShipInfos = async () => {
  loading.value = true;
  try {
    const response = await pmcSpecApi.getShipInfos();
    shipInfos.value = response.data.data || [];
  } catch (error) {
    console.error('获取船型船号失败:', error);
  } finally {
    loading.value = false;
  }
};

// 保存规格书配置
const saveConfig = async (formData: SavePipeSpecRequest) => {
  loading.value = true;
  try {
    const response = await pmcSpecApi.saveSpecRules(formData);
    ElMessage.success(response.data.message || '保存成功');
    // 执行后续操作
  } catch (error) {
    console.error('保存失败:', error);
  } finally {
    loading.value = false;
  }
};

onMounted(() => {
  fetchShipInfos();
});
</script>
```

### 6.4 React使用示例

```typescript
// hooks/usePmcSpec.ts
import { useState, useEffect } from 'react';
import { pmcSpecApi } from '@/api/pmcSpec';
import type { ShipInfo, SavePipeSpecRequest } from '@/types';

export const usePmcSpec = () => {
  const [shipInfos, setShipInfos] = useState<ShipInfo[]>([]);
  const [loading, setLoading] = useState(false);

  // 获取船型船号信息
  const fetchShipInfos = async () => {
    setLoading(true);
    try {
      const response = await pmcSpecApi.getShipInfos();
      setShipInfos(response.data.data || []);
    } catch (error) {
      console.error('获取船型船号失败:', error);
    } finally {
      setLoading(false);
    }
  };

  // 保存规格书配置
  const saveSpecRules = async (data: SavePipeSpecRequest) => {
    setLoading(true);
    try {
      const response = await pmcSpecApi.saveSpecRules(data);
      message.success(response.data.message || '保存成功');
      return true;
    } catch (error) {
      console.error('保存失败:', error);
      return false;
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchShipInfos();
  }, []);

  return {
    shipInfos,
    loading,
    fetchShipInfos,
    saveSpecRules
  };
};
```

---

## 附录

### A. TypeScript完整类型定义

```typescript
// types/api.ts

// ============ 通用响应类型 ============
export interface ApiResponse<T = any> {
  code: number;
  message?: string;
  data?: T;
  timestamp: string;
  traceId?: string;
}

export interface ValidationError {
  field: string;
  message?: string;
  errorCode: string;
}

// ============ 数据模型 ============
export interface ShipInfo {
  shipNumber: string;
  shipType: string;
}

export interface ComponentTypeInfo {
  componentTypeName: string;
  componentTypeDescription: string;
}

export interface PmcSelectInfo {
  pmcCode: string;
  shipNumber: string;
  material: string;
  pipeStandard: string;
  status: string;
}

export interface PmcBaseInfo {
  pmcCode: string;
  shipNumber?: string;
  status?: string;
  pipingClass?: string;
  materialGrade?: string;
  pressureRating?: string;
  pipeStandard?: string;
  materialCategory?: string;
  wallThickness?: string;
}

export interface SpecNPDInfo {
  endStandard?: string;
  schedule?: string;
  npd?: number[];
  outsideDiameter?: number[];
  wallThickness?: number[];
}

export interface PipeFittingSpec {
  standardName: string;
  materialList: string[];
}

export interface DiameterRange {
  minNpdValue: number;
  maxNpdValue: number;
  diameterUnit?: string;
  standardFile?: any;
}

export interface StandardFileConfig {
  standardFile?: any;
  material?: any;
  minNpdValue?: number;
  maxNpdValue?: number;
  bendRadiusMultiple?: any;
}

export interface StandardFileConfiguration {
  standardFileId?: any;
  standardFileName?: string;
  materialId?: any;
  materialName?: string;
  npdRange?: [number, number] | [string, string];
  bendRadiusMultiple?: any;
}

export interface DuplicateRangeDefault {
  overlapMin: number;
  overlapMax: number;
  defaultStandardFileId?: any;
  defaultStandardFileName?: string;
  ranges?: DiameterRange[];
  standardFiles?: any[];
  rangeKey?: string;
}

export interface ComponentFullConfiguration {
  standardFileIds?: any[];
  standardFileConfigs?: StandardFileConfig[];
  configurations?: StandardFileConfiguration[];
  duplicateRangeDefaults?: DuplicateRangeDefault[];
}

export interface ComponentTypeConfiguration {
  componentType: string;
  configResult?: string;
  fullConfig?: ComponentFullConfiguration;
}

export interface SavePipeSpecRequest {
  shipType: string;
  shipNumber: string;
  pmcCode: string;
  configurations: ComponentTypeConfiguration[];
  metadata?: Record<string, any>;
}
```

### B. 更新日志

| 版本 | 日期       | 修改内容                                                     | 修改人       |
| ---- | ---------- | ------------------------------------------------------------ | ------------ |
| v1.0 | 2026-02-03 | 初始版本，定义所有7个API接口                                 | AI Assistant |
| v1.1 | 2026-02-04 | SaveSpecRules: 补充 StandardFileConfig 定义；npdRange 支持 string；componentType 支持类型及规范化说明；业务规则失败错误消息含船型船号；明确 (pmcCode, shipType, shipNumber) 精确匹配规则 | AI Assistant |

---

**文档结束**
