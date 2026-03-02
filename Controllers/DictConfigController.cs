using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Encodings.Web;

namespace PMCSystem_Backend.Controllers
{
    [Route("api/dict-config")]
    [ApiController]
    public class DictConfigController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        // 文件锁，防止并发写入冲突
        private static readonly object _fileLock = new object();

        public DictConfigController(IWebHostEnvironment env)
        {
            _env = env;
        }

        // =================================================================
        // 接口：新增自定义列 (前端调用此接口来加列)
        // POST /api/dict-config/columns/{type}
        // =================================================================
        [HttpPost("columns/{type}")]
        public IActionResult AddColumn(string type, [FromBody] AddColumnDto input)
        {
            // 1. 校验必填项
            if (string.IsNullOrWhiteSpace(input.Title))
                return BadRequest(new { message = "列标题(Title)不能为空" });

            // 默认为文本框
            if (string.IsNullOrWhiteSpace(input.UiType))
                input.UiType = "Input";

            try
            {
                lock (_fileLock)
                {
                    // 2. 读取 dicts.json 文件
                    var filePath = Path.Combine(_env.ContentRootPath, "Configs", "dicts.json");
                    if (!System.IO.File.Exists(filePath))
                        return NotFound(new { message = "配置文件 dicts.json 不存在" });

                    var jsonString = System.IO.File.ReadAllText(filePath);

                    
                    // 1. 配置 DocumentOptions：告诉解析器直接跳过注释 // 和 /* */
                    var documentOptions = new JsonDocumentOptions
                    {
                        CommentHandling = JsonCommentHandling.Skip,
                        AllowTrailingCommas = true // 顺便允许对象末尾有多余的逗号，容错率更高
                    };

                    // 2. 使用带有选项的 JsonNode 解析，方便动态操作
                    var rootNode = JsonNode.Parse(jsonString, documentOptions: documentOptions);
                    if (rootNode == null) return StatusCode(500, new { message = "配置文件解析失败" });

                    // 3. 定位到对应业务的 Columns 数组
                    // 路径：DictConfiguration -> [type] -> Columns
                    var dictConfig = rootNode["DictConfiguration"];
                    if (dictConfig == null || dictConfig[type] == null)
                        return NotFound(new { message = $"未找到业务类型 '{type}' 的配置" });

                    var targetConfig = dictConfig[type];
                    var columnsNode = targetConfig["Columns"] as JsonArray;

                    // 如果 Columns 数组不存在，创建一个新的
                    if (columnsNode == null)
                    {
                        columnsNode = new JsonArray();
                        targetConfig["Columns"] = columnsNode;
                    }

                    // 4. 自动生成唯一的列代码 (DbField)
                    // 使用 "Ext_" + 8位随机码，确保不和 ID/Status 冲突，且存入 JsonData
                    string autoDbField;
                    do
                    {
                        autoDbField = $"Ext_{Guid.NewGuid().ToString("N").Substring(0, 8)}";
                    }
                    while (columnsNode.Any(x => x["DbField"]?.GetValue<string>() == autoDbField));

                    // 5. 构造新列的配置对象
                    var newColNode = new JsonObject
                    {
                        ["DbField"] = autoDbField,      // 存库用的 Key (自动生成)
                        ["Title"] = input.Title,        // 表头标题
                        ["UiType"] = input.UiType,      // 控件类型 (Input, Select, Switch)
                        ["IsRequired"] = input.IsRequired,
                        ["IsHidden"] = false
                    };

                    // 6. 如果是下拉框(Select)，处理选项
                    if (input.UiType.Equals("Select", StringComparison.OrdinalIgnoreCase)
                        && !string.IsNullOrWhiteSpace(input.Options))
                    {
                        var optionsArray = new JsonArray();
                        // 按逗号拆分选项
                        var opts = input.Options.Split(new[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var opt in opts)
                        {
                            optionsArray.Add(opt.Trim());
                        }
                        newColNode["Options"] = optionsArray;
                    }

                    // 7. 追加到配置数组
                    columnsNode.Add(newColNode);

                    // 8. 写回文件 (格式化 JSON，防止中文转义)
                    var writeOptions = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    };
                    System.IO.File.WriteAllText(filePath, rootNode.ToJsonString(writeOptions));
                }

                return Ok(new { message = "列添加成功" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "写入配置失败: " + ex.Message });
            }
        }
    }

    // 用于接收前端参数的 DTO
    public class AddColumnDto
    {
        public string Title { get; set; }        // e.g. "客户等级"
        public string UiType { get; set; }       // e.g. "Select"
        public bool IsRequired { get; set; }     // e.g. true
        public string? Options { get; set; }     // e.g. "VIP,普通" (仅下拉框用)
    }
}