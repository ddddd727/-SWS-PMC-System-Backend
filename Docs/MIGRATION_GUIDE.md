# API迁移指南 - SaveSpecRules接口

## 概述
`SaveSpecRules` 接口已进行重构优化，请求DTO已更改。如果前端正在调用此接口，请参考本指南进行适配。

---

## API变更

### 端点信息（未变更）
- **路径**: `POST /api/PmcSpec/SpecRules`
- **Content-Type**: `application/json`

### 请求体变更

#### 旧格式（已废弃）
```json
{
  "pmcCode": "A1B2C3D",
  "standardInfos": [
    {
      "standardName": "ASME B16.9",
      "standardType": "Elbow",
      "diameterRange": {
        "diameterMin": 15,
        "diameterMax": 100
      },
      "material": "Carbon Steel"
    }
  ]
}
```

#### 新格式（当前使用）
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
            "standardFileId": "std001",
            "standardFileName": "ASME B16.9",
            "materialId": "mat001",
            "materialName": "Carbon Steel",
            "npdRange": [15, 100],
            "bendRadiusMultiple": 1.5
          }
        ],
        "duplicateRangeDefaults": [
          {
            "overlapMin": 50,
            "overlapMax": 80,
            "defaultStandardFileId": "std001",
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
    }
  ],
  "metadata": {
    "source": "web",
    "version": "1.0"
  }
}
```

---

## 字段映射对照表

| 旧字段路径 | 新字段路径 | 说明 |
|-----------|-----------|------|
| - | `shipType` | **新增必填**：船型 |
| - | `shipNumber` | **新增必填**：船号 |
| `pmcCode` | `pmcCode` | PMC编码（未变更） |
| `standardInfos[]` | `configurations[]` | 重命名并结构调整 |
| `standardInfos[].standardType` | `configurations[].componentType` | 部件类型字段重命名 |
| `standardInfos[].standardName` | `configurations[].fullConfig.configurations[].standardFileName` | 嵌套层级改变 |
| `standardInfos[].material` | `configurations[].fullConfig.configurations[].materialName` | 嵌套层级改变 |
| `standardInfos[].diameterRange.diameterMin` | `configurations[].fullConfig.configurations[].npdRange[0]` | 结构变更：对象→数组 |
| `standardInfos[].diameterRange.diameterMax` | `configurations[].fullConfig.configurations[].npdRange[1]` | 结构变更：对象→数组 |
| - | `configurations[].fullConfig.standardFileConfigs[]` | **可选**：标准文件配置（简化版），与 configurations 二选一或同时使用 |
| - | `configurations[].fullConfig.duplicateRangeDefaults[]` | **新增**：重复范围默认配置 |
| - | `metadata` | **新增可选**：元数据 |

---

## 必填字段验证

### 新增必填字段
1. `shipType` - 船型（不能为空）
2. `shipNumber` - 船号（不能为空）
3. `configurations` - 至少包含一个部件类型配置
4. `configurations[].componentType` - 部件类型（不能为空）

### 验证规则
- 所有必填字段在前端提交前应先验证
- 后端会进行二次验证，返回 400 错误如果验证失败

---

## 响应格式（未变更）

### 成功响应 (200 OK)
```json
{
  "success": true,
  "message": "规格书配置保存成功",
  "data": null,
  "errorCode": null
}
```

### 失败响应 (400 Bad Request)
```json
{
  "success": false,
  "message": "请求参数验证失败",
  "data": null,
  "errorCode": "ValidationError"
}
```

### 业务规则失败 (400) - 记录不存在
```json
{
  "code": 400,
  "message": "未找到PMC编码 A1B2C3D（船型: 散货船, 船号: H1234）对应的数据"
}
```

---

## 前端适配建议

### Vue/React 示例

#### 1. 定义TypeScript类型
```typescript
// types/pipeSpec.ts
export interface SavePipeSpecRequest {
  shipType: string;
  shipNumber: string;
  pmcCode: string;
  configurations: ComponentTypeConfiguration[];
  metadata?: Record<string, any>;
}

export interface ComponentTypeConfiguration {
  componentType: string;
  configResult?: string;
  fullConfig?: ComponentFullConfiguration;
}

export interface ComponentFullConfiguration {
  standardFileConfigs?: StandardFileConfig[];   // 标准文件配置（简化版）
  configurations?: StandardFileConfiguration[];
  duplicateRangeDefaults?: DuplicateRangeDefault[];
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
  npdRange?: [number, number];
  bendRadiusMultiple?: any;
}

export interface DuplicateRangeDefault {
  overlapMin: number;
  overlapMax: number;
  defaultStandardFileId?: any;
  defaultStandardFileName?: string;
  ranges?: DiameterRange[];
}

export interface DiameterRange {
  minNpdValue: number;
  maxNpdValue: number;
  standardFile?: any;
}
```

#### 2. API调用示例
```typescript
// api/pipeSpec.ts
import axios from 'axios';
import type { SavePipeSpecRequest } from '@/types/pipeSpec';

export async function savePipeSpec(request: SavePipeSpecRequest) {
  try {
    const response = await axios.post('/api/PmcSpec/SpecRules', request);
    return response.data;
  } catch (error) {
    console.error('保存失败:', error);
    throw error;
  }
}
```

#### 3. 表单验证
```typescript
// composables/usePipeSpecForm.ts
import { ref } from 'vue';
import type { SavePipeSpecRequest } from '@/types/pipeSpec';

export function usePipeSpecForm() {
  const formData = ref<SavePipeSpecRequest>({
    shipType: '',
    shipNumber: '',
    pmcCode: '',
    configurations: []
  });

  const validateForm = (): boolean => {
    if (!formData.value.shipType) {
      alert('请输入船型');
      return false;
    }
    if (!formData.value.shipNumber) {
      alert('请输入船号');
      return false;
    }
    if (!formData.value.pmcCode) {
      alert('请输入PMC编码');
      return false;
    }
    if (formData.value.configurations.length === 0) {
      alert('请至少配置一个部件类型');
      return false;
    }
    return true;
  };

  return {
    formData,
    validateForm
  };
}
```

---

## 测试用例

### 最小有效请求
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

### 完整请求示例
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
        "duplicateRangeDefaults": []
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

---

## 常见问题

### Q1: 为什么新增了 shipType 和 shipNumber 字段？
**A**: 这些字段用于精确匹配数据库中的 PMC 记录。后端按 `(pmcCode, shipType, shipNumber)` 查找对应记录进行更新，三者须与已存在的数据一致。

### Q2: 旧的API还能用吗？
**A**: 不能，旧的请求格式已废弃。请尽快迁移到新格式。

### Q3: npdRange 为什么从对象变成了数组？
**A**: 数组格式更简洁，前端处理更方便。`[min, max]` 的形式也更符合范围的语义。后端同时支持 `[number, number]` 和 `[string, string]` 格式。

### Q4: 业务规则失败时错误消息的格式是什么？
**A**: 当未找到对应记录时，返回格式为 `"未找到PMC编码 {pmcCode}（船型: {shipType}, 船号: {shipNumber}）对应的数据"`，便于前端提示用户检查船型、船号和 PMC 编码的组合是否正确。

### Q5: 如果我的前端代码已经使用了旧格式怎么办？
**A**: 建议创建一个转换函数，将旧格式转换为新格式：

```typescript
function convertOldToNewFormat(oldRequest: any): SavePipeSpecRequest {
  return {
    shipType: '默认船型', // 需要从其他地方获取
    shipNumber: '默认船号', // 需要从其他地方获取
    pmcCode: oldRequest.pmcCode,
    configurations: oldRequest.standardInfos.map((info: any) => ({
      componentType: info.standardType,
      fullConfig: {
        configurations: [{
          standardFileName: info.standardName,
          materialName: info.material,
          npdRange: [
            info.diameterRange.diameterMin,
            info.diameterRange.diameterMax
          ]
        }],
        duplicateRangeDefaults: []
      }
    }))
  };
}
```

---

## 联系与支持

如有疑问，请联系后端开发团队。

**更新日期**: 2026-02-04
