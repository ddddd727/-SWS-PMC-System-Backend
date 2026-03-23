using Microsoft.AspNetCore.Mvc;
using PMCSystem_Backend.Services.Implementations;

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PMCSystem_Backend.Modules.StandardComponents.Controllers
{
    [Route("api/dict-config")]
    [ApiController]
    public class DictConfigController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly DictConfigManager _configManager;
        private static readonly object _fileLock = new();

        public DictConfigController(IWebHostEnvironment env, DictConfigManager configManager)
        {
            _env = env;
            _configManager = configManager;
        }

        // =================================================================
        // 新增自定义列
        // POST /api/dict-config/columns/{type}
        // =================================================================
        [HttpPost("columns/{type}")]
        public IActionResult AddColumn(string type, [FromBody] AddColumnDto input)
        {
            if (string.IsNullOrWhiteSpace(input.Title))
                return BadRequest(new { message = "列标题不能为空" });

            if (string.IsNullOrWhiteSpace(input.UiType))
                input.UiType = "Input";

            try
            {
                lock (_fileLock)
                {
                    // 1. 在 Configs/DictConfigs/ 下找包含该 type 的文件
                    var configDir = Path.Combine(_env.ContentRootPath, "Configs", "DictConfigs");
                    if (!Directory.Exists(configDir))
                        return NotFound(new { message = "配置目录 Configs/DictConfigs 不存在" });

                    string? targetFile = null;
                    JsonNode? rootNode = null;

                    var docOptions = new JsonDocumentOptions
                    {
                        CommentHandling = JsonCommentHandling.Skip,
                        AllowTrailingCommas = true
                    };

                    foreach (var file in Directory.GetFiles(configDir, "*.json"))
                    {
                        var content = System.IO.File.ReadAllText(file);
                        var node = JsonNode.Parse(content, documentOptions: docOptions);
                        if (node?[type] != null)
                        {
                            targetFile = file;
                            rootNode = node;
                            break;
                        }
                    }

                    if (targetFile == null || rootNode == null)
                        return NotFound(new { message = $"未在任何配置文件中找到类型 '{type}'" });

                    // 2. 定位 Columns 数组
                    var targetConfig = rootNode[type]!;
                    var columnsNode = targetConfig["Columns"] as JsonArray;
                    if (columnsNode == null)
                    {
                        columnsNode = new JsonArray();
                        targetConfig["Columns"] = columnsNode;
                    }

                    // 3. 生成唯一 DbField（Ext_ 前缀，存入 JsonData）
                    string autoDbField;
                    do
                    {
                        autoDbField = $"Ext_{Guid.NewGuid().ToString("N")[..8]}";
                    }
                    while (columnsNode.Any(x => x?["DbField"]?.GetValue<string>() == autoDbField));

                    // 4. 构造新列对象
                    var newCol = new JsonObject
                    {
                        ["DbField"] = autoDbField,
                        ["Title"] = input.Title,
                        ["UiType"] = input.UiType,
                        ["IsRequired"] = input.IsRequired,
                        ["IsHidden"] = false
                    };

                    // 5. Select 类型：Options 写成 {Label, Value} 对象数组（新格式）
                    //    放在 DataSource.Options 下
                    if (input.UiType.Equals("Select", StringComparison.OrdinalIgnoreCase)
                        && !string.IsNullOrWhiteSpace(input.Options))
                    {
                        var optionsArray = new JsonArray();
                        foreach (var opt in input.Options.Split(new[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            var trimmed = opt.Trim();
                            optionsArray.Add(new JsonObject
                            {
                                ["Label"] = trimmed,
                                ["Value"] = trimmed
                            });
                        }

                        newCol["DataSource"] = new JsonObject
                        {
                            ["Options"] = optionsArray,
                            ["LabelField"] = "Label",
                            ["ValueField"] = "Value"
                        };
                    }

                    // 6. 追加列
                    columnsNode.Add(newCol);

                    // 7. 写回原文件（保留中文，格式化缩进）
                    var writeOptions = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    };
                    System.IO.File.WriteAllText(targetFile, rootNode.ToJsonString(writeOptions));

                    // 8. 通知 DictConfigManager 重新加载，立即生效
                    _configManager.LoadAllConfigs();
                }

                return Ok(new { message = "列添加成功" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "写入配置失败: " + ex.Message });
            }
        }
    }

    public class AddColumnDto
    {
        public string Title { get; set; } = string.Empty;
        public string UiType { get; set; } = "Input";
        public bool IsRequired { get; set; } = false;
        public string? Options { get; set; }
    }
}