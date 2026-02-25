# Pipe-Spec.xlsx 模板分析报告

## 1. 基本信息


| 项目    | 值      |
| ----- | ------ |
| 工作表   | Sheet1 |
| 行数    | 26     |
| 列数    | 22     |
| 合并单元格 | 47 个区域 |


## 2. 占位符分析

### 2.1 与 API 标准对比

模板 `Pipe-Spec.xlsx` 已按 API 契约完成占位符标准化（2026-02-25 更新）。所有占位符均符合 API 标准。

### 2.2 当前占位符（已全部标准化）

| 类别 | 占位符 | 说明 |
|------|--------|------|
| PMC 基础信息 | `{{pmcCode}}`、`{{shipNumber}}`、`{{pressureRating}}`、`{{pipeStandard}}`、`{{materialCategory}}`、`{{wallThickness}}` | 与 API 一致 |
| 通径信息 | `{{npd_1}}` ~ `{{npd_18}}` | 通径值（索引） |
| 外径信息 | `{{outsideDiameter_1}}` ~ `{{outsideDiameter_18}}` | 外径值（索引） |
| 壁厚信息 | `{{wallThicknessList_1}}` ~ `{{wallThicknessList_18}}` | 壁厚值（索引） |
| 组件标准 | `{{standard_Bend}}`、`{{standard_Elbow}}`、`{{standard_Red}}`、`{{standard_Tee}}`、`{{standard_Flange}}`、`{{standard_Blind_Flange}}` 等 | 按组件类型填充 |

### 2.3 历史变更（2026-02-25）

以下占位符已从模板中替换为标准名称：

| 原占位符 | 现占位符 |
|----------|----------|
| `{{materialCategoryl}}` | `{{materialCategory}}` |
| `{{pipingStandard}}` | `{{pipeStandard}}` |
| `{{OD_N}}` | `{{outsideDiameter_N}}` |
| `{{Thickness_N}}` | `{{wallThicknessList_N}}` |
| `{{standard_BlindFlinge}}` | `{{standard_Blind_Flange}}` |

## 3. 服务端兼容

`TemplatePreviewService` 中保留 `ApplyTemplatePlaceholderAliases`，用于向后兼容其他可能使用旧占位符的模板或旧版 Pipe-Spec 文件。

## 4. 模板布局概览

- **A1:B1**：标题合并
- **D1**：船号 `{{shipNumber}}`
- **A4:V4**：PMC 编码、材料类别、压力等级、壁厚等基础信息
- **C5:V5**：通径 `{{npd_1}}` ~ `{{npd_18}}`
- **C6:V6**：外径 `{{outsideDiameter_1}}` ~ `{{outsideDiameter_18}}`
- **C7:V7**：壁厚 `{{wallThicknessList_1}}` ~ `{{wallThicknessList_18}}`
- **C8:V8**：管材标准 `{{pipeStandard}}`
- **C9:C25**：各类组件标准（Bend、Elbow、Red、Tee、Flange、Sleeve 等）

## 5. 使用建议

1. **当前 Pipe-Spec 模板**：已使用 API 标准占位符，可直接对接 `TemplatePreviewService`
2. **自定义参数调用**：传入标准键（如 `outsideDiameter_1`、`pipeStandard`）即可填充
3. **按规格书导出**：传入 `pmcCode`，由服务自动构建占位符并填充

