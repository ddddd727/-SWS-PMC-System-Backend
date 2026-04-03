using Microsoft.Extensions.Logging;
using PMCSystem_Backend.Modules.StandardComponents.Dtos;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PMCSystem_Backend.Services.Implementations
{
    public class DictConfigManager
    {
        private readonly ConcurrentDictionary<string, DictItemConfig> _allConfigs = new();
        private readonly ILogger<DictConfigManager> _logger;

        // ✅ 统一序列化配置：支持 object? 反序列化 + 大小写不敏感
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            // object? 字段（DefaultValue / ConditionRule.Value 等）反序列化为 JsonElement
            UnknownTypeHandling = JsonUnknownTypeHandling.JsonElement
        };

        public DictConfigManager(ILogger<DictConfigManager> logger)
        {
            _logger = logger;
            LoadAllConfigs();
        }

        public void LoadAllConfigs()
        {
            _allConfigs.Clear();

            string configDir = Path.Combine(Directory.GetCurrentDirectory(), "Configs", "DictConfigs");
            string oldConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "Configs", "dicts.json");

            if (Directory.Exists(configDir))
            {
                var files = Directory.GetFiles(configDir, "*.json");
                if (files.Length == 0)
                    _logger.LogWarning("DictConfigs 目录存在，但没有找到任何 .json 文件，路径: {Path}", configDir);

                foreach (var file in files)
                    LoadFromFile(file);
            }
            else if (File.Exists(oldConfigPath))
            {
                LoadFromFile(oldConfigPath);
            }
            else
            {
                _logger.LogError(
                    "找不到字典配置目录或文件！已查找路径：\n  {Dir}\n  {File}",
                    configDir, oldConfigPath);
            }

            _logger.LogInformation("字典配置加载完成，共加载 {Count} 个配置项", _allConfigs.Count);
        }

        private void LoadFromFile(string filePath)
        {
            try
            {
                string jsonContent = File.ReadAllText(filePath);

                // ✅ 跳过空文件
                if (string.IsNullOrWhiteSpace(jsonContent))
                {
                    _logger.LogWarning("配置文件为空，已跳过: {File}", filePath);
                    return;
                }

                var configs = JsonSerializer.Deserialize<Dictionary<string, DictItemConfig>>(jsonContent, _jsonOptions);

                if (configs == null || configs.Count == 0)
                {
                    _logger.LogWarning("配置文件反序列化结果为空: {File}", filePath);
                    return;
                }

                foreach (var kvp in configs)
                {
                    // ✅ 跳过 $schema 字段（如果有人在 JSON 里加了 $schema 引用）
                    if (kvp.Key.StartsWith("$")) continue;

                    _allConfigs[kvp.Key] = kvp.Value;
                    _logger.LogDebug("  已加载配置项: {Key} ({File})", kvp.Key, Path.GetFileName(filePath));
                }
            }
            catch (JsonException ex)
            {
                // ✅ JSON 格式错误：给出行列号帮助定位
                _logger.LogError(ex,
                    "配置文件 JSON 格式错误: {File}\n  位置: Line {Line}, Col {Col}\n  详情: {Msg}",
                    filePath, ex.LineNumber, ex.BytePositionInLine, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载配置文件失败: {File}", filePath);
            }
        }

        public DictItemConfig GetConfig(string dictId)
        {
            if (_allConfigs.IsEmpty) LoadAllConfigs();

            if (_allConfigs.TryGetValue(dictId, out var config))
                return config;

            // ✅ 报错时把已加载的 key 列出来，方便排查拼写问题
            var available = string.Join(", ", _allConfigs.Keys);
            throw new Exception(
                $"未找到字典 '{dictId}' 的配置。\n" +
                $"当前已加载的配置项: [{available}]\n" +
                $"请检查 Configs/DictConfigs/ 目录下的 JSON 文件。");
        }
    }
}