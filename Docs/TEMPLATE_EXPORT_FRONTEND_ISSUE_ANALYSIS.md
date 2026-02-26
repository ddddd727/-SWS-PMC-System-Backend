# 模板导出文件“损坏”问题定位结论（前端侧）

## 结论

本次问题的根因已定位为：**前端下载链路处理不当**，而不是后端生成的 Excel 文件损坏。

后端侧证据如下（来自 `log-20260225_010.txt`）：

- 导出成功：`模板导出成功，文件大小: 9904 bytes`
- 响应签名：`Signature: 50-4B-03-04`（标准 ZIP/xlsx 头）
- 响应摘要：`Sha256: 3BA5F7DFC834D6D93117715C62F1F403A42EC60A01F300BBC829F72DF9B00F4B`
- 服务端落盘调试文件可正常打开：
  `C:\Users\Imperfectaxy\AppData\Local\Temp\PMCSystem_Backend\ExportDebug\Pipe-Spec_20260225132246346.xlsx`

以上说明：**后端字节流是有效 xlsx**，问题发生在前端拿到响应后的处理过程。

---

## 前端常见错误模式

1. 将二进制响应按 `json/text` 解析，再转存为 `.xlsx`。
2. 未设置 `axios` 的 `responseType: 'blob'`。
3. 将错误响应（JSON）直接当成功文件下载。
4. 对 `blob` 做了二次字符串化处理（例如 `new Blob([JSON.stringify(...)])`）。

---

## 推荐修复方案

### 1) 使用 fetch 下载（推荐）

```ts
export async function exportTemplate(templateId: string, pmcCode: string) {
  const url = `/api/template-preview/${templateId}/export?pmcCode=${encodeURIComponent(pmcCode)}`;
  const res = await fetch(url, { method: "GET" });

  const contentType = res.headers.get("content-type") || "";
  const sha256 = res.headers.get("x-export-sha256") || "";

  // 失败分支：优先按 JSON 读取错误
  if (!res.ok) {
    if (contentType.includes("application/json")) {
      const err = await res.json();
      throw new Error(err?.message || "导出失败");
    }
    const text = await res.text();
    throw new Error(`导出失败(${res.status})：${text.slice(0, 200)}`);
  }

  // 成功分支：必须按 blob 读取
  const blob = await res.blob();
  if (!contentType.includes("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")) {
    throw new Error(`响应类型异常: ${contentType}`);
  }

  const fileName =
    res.headers.get("content-disposition")?.match(/filename="?([^";]+)"?/)?.[1] ||
    `${templateId}_${new Date().toISOString().slice(0, 10)}.xlsx`;

  const a = document.createElement("a");
  a.href = URL.createObjectURL(blob);
  a.download = fileName;
  document.body.appendChild(a);
  a.click();
  a.remove();
  URL.revokeObjectURL(a.href);

  console.info("[Export] done", { size: blob.size, contentType, sha256 });
}
```

### 2) 使用 axios 下载

```ts
import axios from "axios";

export async function exportTemplateByAxios(templateId: string, pmcCode: string) {
  const url = `/api/template-preview/${templateId}/export`;
  const res = await axios.get(url, {
    params: { pmcCode },
    responseType: "blob", // 关键
    validateStatus: () => true
  });

  const contentType = (res.headers["content-type"] || "").toLowerCase();
  const isJson = contentType.includes("application/json");

  if (res.status < 200 || res.status >= 300 || isJson) {
    // 错误响应通常也是 blob，需要转文本再 parse
    const text = await res.data.text();
    let message = text;
    try {
      const obj = JSON.parse(text);
      message = obj?.message || text;
    } catch {}
    throw new Error(`导出失败(${res.status}): ${message}`);
  }

  const blob = new Blob([res.data], {
    type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
  });
  const fileName = `${templateId}_${new Date().toISOString().replace(/[:T]/g, "").slice(0, 14)}.xlsx`;

  const a = document.createElement("a");
  a.href = URL.createObjectURL(blob);
  a.download = fileName;
  document.body.appendChild(a);
  a.click();
  a.remove();
  URL.revokeObjectURL(a.href);
}
```

---

## 联调检查清单（前端）

- Network 面板中导出请求的 `Response Headers`：
  - `content-type` 必须是 `application/vnd.openxmlformats-officedocument.spreadsheetml.sheet`
  - `x-export-sha256` 有值（用于和后端日志比对）
- `Status Code` 必须是 `200`
- 前端代码必须走 `blob` 分支，不得走 `json/text` 分支
- 下载后文件大小应接近日志中的 `Size`（本次示例为 `9904 bytes`）

---

## 备注

后端已加入以下诊断能力，便于继续联调：

- 响应头 `X-Export-Sha256`
- 导出日志：`Size` / `Signature` / `Sha256`
- 开发环境自动落盘导出文件

如果前端按本文修复后仍有问题，请提供浏览器 Network 面板中该请求的：

- `status`
- `content-type`
- `x-export-sha256`
- 前 200 字节响应文本（仅在错误分支）

即可继续快速定位。
