using Microsoft.Extensions.Logging;
using PMCSystem_Backend.Dtos.Dict;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PMCSystem_Backend.Services.Implementations
{
    public class DictConfigManager
    {
        // 使用线程安全的字典在内存中缓存所有配置
        private readonly ConcurrentDictionary<string, DictItemConfig> _allConfigs = new();
        private readonly ILogger<DictConfigManager> _logger;

        public DictConfigManager(ILogger<DictConfigManager> logger)
        {
            _logger = logger;
            LoadAllConfigs(); // 启动时自动加载一次
        }

        public void LoadAllConfigs()
        {
            _allConfigs.Clear();

            // 新目录：Configs/DictConfigs/
            string configDir = Path.Combine(Directory.GetCurrentDirectory(), "Configs", "DictConfigs");
            // 兼容旧目录：如果没建新文件夹，兜底读原来的 dicts.json
            string oldConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "Configs", "dicts.json");

            if (Directory.Exists(configDir))
            {
                string[] jsonFiles = Directory.GetFiles(configDir, "*.json");
                foreach (var file in jsonFiles)
                {
                    LoadFromFile(file);
                }
            }
            else if (File.Exists(oldConfigPath))
            {
                LoadFromFile(oldConfigPath);
            }
        }

        private void LoadFromFile(string filePath)
        {
            try
            {
                string jsonContent = File.ReadAllText(filePath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                // 🌟 核心：因为我们去掉了外壳，现在直接反序列化为字典对象
                var configs = JsonSerializer.Deserialize<Dictionary<string, DictItemConfig>>(jsonContent, options);

                if (configs != null)
                {
                    foreach (var kvp in configs)
                    {
                        // 把每个文件里的配置合并到统一的内存大字典中
                        _allConfigs[kvp.Key] = kvp.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"加载配置文件失败: {filePath}");
            }
        }

        // 供外部获取配置的方法
        public DictItemConfig GetConfig(string dictId)
        {
            if (_allConfigs.IsEmpty) LoadAllConfigs();

            if (_allConfigs.TryGetValue(dictId, out var config))
            {
                return config;
            }

            throw new Exception($"未找到字典 '{dictId}' 的配置，请检查 JSON 文件！");
        }
    }
}