using AutoMapper.Execution;
using PMCSystem_Backend.Modules.PipingSpecifications.Dtos;
using PMCSystem_Backend.Services.Interfaces;
using PMCSystem_Backend.Shared.Constants;
using System.Text.RegularExpressions;
using System.Xml;

namespace PMCSystem_Backend.Services.Implementations
{
    /// <summary>
    /// 模板预览与导出服务：读取 Excel 模板、解析单元格与合并区域、替换 "{{key}}" 占位符；
    /// 支持自定义参数或按规格书（PMC 编码）自动填充。
    /// </summary>
    public class TemplatePreviewService : ITemplatePreviewService
    {
        /// <summary>Excel 模板文件所在目录的根路径（不含文件名）。</summary>
        private readonly string _templateBasePath;

        /// <summary>规格书服务，用于按 PMC 编码拉取已保存的规格数据。</summary>
        private readonly IPmcSpecService _pmcSpecService;

        private readonly ILogger<TemplatePreviewService> _logger;

        /// <summary>
        /// 构造函数，指定模板根目录、规格书服务与日志。
        /// </summary>
        /// <param name="templateBasePath">模板根路径，模板文件名为 {templateId}.xlsx</param>
        /// <param name="pmcSpecService">用于按 pmcCode 获取规格书数据的服务</param>
        /// <param name="logger">日志</param>
        public TemplatePreviewService(string templateBasePath, IPmcSpecService pmcSpecService, ILogger<TemplatePreviewService> logger)
        {
            _templateBasePath = templateBasePath;
            _pmcSpecService = pmcSpecService ?? throw new ArgumentNullException(nameof(pmcSpecService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// 获取指定模板的预览数据：解析首张工作表，提取合并单元格与普通单元格，替换 {{key}} 占位符并注入业务数据。
        /// </summary>
        /// <param name="templateId">模板唯一标识，仅允许字母、数字、下划线、中划线</param>
        /// <param name="parameters">用于替换模板中 {{key}} 的键值对，key 与 value 均不可为 null</param>
        /// <returns>包含标题、网格信息、合并单元格与单元格列表的预览响应</returns>
        /// <exception cref="ArgumentException">templateId 为空或格式不合法</exception>
        /// <exception cref="FileNotFoundException">模板文件不存在</exception>
        /// <exception cref="InvalidOperationException">Excel 无工作表或工作表为空</exception>
        public TemplatePreviewResponse GetTemplatePreview(string templateId, Dictionary<string, string> parameters)
        {
            _logger.LogInformation("开始获取模板预览，TemplateId: {TemplateId}, 占位符数量: {Count}", templateId, parameters?.Count ?? 0);

            // 1. 验证 templateId 非空且格式合法（防路径遍历）
            if (string.IsNullOrWhiteSpace(templateId))
            {
                _logger.LogWarning("模板预览失败：TemplateId 为空");
                throw new ArgumentException("TemplateId cannot be null or empty", nameof(templateId));
            }
            if (!Regex.IsMatch(templateId, "^[a-zA-Z0-9_-]+$"))
            {
                _logger.LogWarning("模板预览失败：TemplateId 格式不合法，TemplateId: {TemplateId}", templateId);
                throw new ArgumentException("Invalid templateId format", nameof(templateId));
            }

            // 2. 统一 parameters 为非 null
            parameters ??= new Dictionary<string, string>();

            // 3. 解析模板文件路径并校验存在
            string filePath = Path.Combine(_templateBasePath, $"{templateId}.xlsx");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning("模板文件不存在，TemplateId: {TemplateId}, Path: {FilePath}", templateId, filePath);
                throw new FileNotFoundException("Template file not found", filePath);
            }

            // 4. 打开 Excel 并取首张工作表与维度
            using var package = new OfficeOpenXml.ExcelPackage(new FileInfo(filePath));
            if (package.Workbook.Worksheets.Count == 0)
            {
                _logger.LogWarning("模板 Excel 无工作表，TemplateId: {TemplateId}", templateId);
                throw new InvalidOperationException("Excel file contains no worksheets");
            }
            var sheet = package.Workbook.Worksheets[0];
            var dim = sheet.Dimension;
            if (dim == null)
            {
                _logger.LogWarning("模板工作表为空，TemplateId: {TemplateId}", templateId);
                throw new InvalidOperationException("Worksheet is empty");
            }

            // 5. 创建占位符替换函数（优化性能，统一处理占位符替换）
            var placeholderReplacer = CreatePlaceholderReplacer(parameters);

            // 6. 提取合并区域（仅保留左上角代表整块，避免重复渲染），并替换占位符
            var mergedCells = new List<MergedCell>();
            if (sheet.MergedCells != null)
            {
                foreach (var mergedCellAddress in sheet.MergedCells)
                {
                    var range = sheet.Cells[mergedCellAddress];
                    var mergedValue = range.Text ?? string.Empty;
                    // 合并单元格的值也需要替换占位符
                    mergedValue = placeholderReplacer(mergedValue);
                    mergedCells.Add(new MergedCell
                    {
                        StartRow = range.Start.Row - 1,
                        StartColumn = range.Start.Column - 1,
                        EndRow = range.End.Row - 1,
                        EndColumn = range.End.Column - 1,
                        Value = mergedValue,
                        Style = ExtractCellStyle(range)
                    });
                }
            }

            // 6. 构建“被合并覆盖”的单元格集合，后续遍历时跳过，避免与合并块重复
            var skipSet = new HashSet<(int, int)>();
            foreach (var cell in mergedCells)
            {
                for (int r = cell.StartRow; r <= cell.EndRow; r++)
                {
                    for (int c = cell.StartColumn; c <= cell.EndColumn; c++)
                    {
                        if (r == cell.StartRow && c == cell.StartColumn) continue;
                        skipSet.Add((r, c));
                    }
                }
            }

            // 8. 遍历非合并区域单元格，替换 {{key}} 占位符并收集样式
            var cells = new List<PreviewCell>();
            
            for (int r = 0; r < dim.Rows; r++)
            {
                for (int c = 0; c < dim.Columns; c++)
                {
                    if (skipSet.Contains((r, c))) continue;
                    var cell = sheet.Cells[r + 1, c + 1];
                    var cellValue = cell.Text ?? string.Empty;
                    // 使用优化的占位符替换方法
                    cellValue = placeholderReplacer(cellValue);
                    
                    var cellStyle = ExtractCellStyle(cell);
                    // 性能优化：空值且无样式时，Value 设为 null 以减少 JSON 大小
                    var finalValue = string.IsNullOrEmpty(cellValue) && cellStyle == null ? null : cellValue;
                    
                    cells.Add(new PreviewCell
                    {
                        Row = r,
                        Column = c,
                        Value = finalValue,
                        IsHeader = r == 0,
                        IsData = r > 0,
                        Field = $"col{c + 1}",
                        Style = cellStyle
                    });
                }
            }

            // 9. 业务数据已通过占位符替换完成，无需额外注入
            // 注：GetBusinessData 方法保留用于扩展，当前通过占位符字典已包含所有业务数据

            // 10. 从 A1 安全读取标题并替换占位符（避免无 A1 时抛错）
            string title = string.Empty;
            try
            {
                var titleCell = sheet.Cells["A1"];
                if (titleCell != null)
                {
                    title = titleCell.Text ?? string.Empty;
                    title = placeholderReplacer(title);
                }
            }
            catch
            {
                title = string.Empty;
            }

            // 11. 优化响应数据：减少 JSON 大小
            // - 空列表设为 null
            // - 合并单元格空值设为 null
            // - 移除不必要的 Field 字段（前端可通过 Row/Column 计算）
            var optimizedMergedCells = mergedCells.Count > 0 
                ? mergedCells.Select(mc => new MergedCell
                {
                    StartRow = mc.StartRow,
                    StartColumn = mc.StartColumn,
                    EndRow = mc.EndRow,
                    EndColumn = mc.EndColumn,
                    Value = string.IsNullOrEmpty(mc.Value) ? null : mc.Value,
                    Style = mc.Style
                }).ToList()
                : null;

            var optimizedCells = cells.Count > 0
                ? cells.Select(c => new PreviewCell
                {
                    Row = c.Row,
                    Column = c.Column,
                    Value = c.Value, // 已在步骤 8 中优化为空值 null
                    IsHeader = c.IsHeader,
                    IsData = c.IsData,
                    Field = null, // 性能优化：移除 Field 字段，前端可通过 Row/Column 计算
                    Style = c.Style
                }).ToList()
                : null;

            _logger.LogInformation("模板预览成功，TemplateId: {TemplateId}, 行数: {Rows}, 列数: {Cols}, 单元格数: {CellCount}, 合并单元格数: {MergedCount}", 
                templateId, dim.Rows, dim.Columns, cells.Count, mergedCells.Count);
            
            return new TemplatePreviewResponse
            {
                TemplateId = templateId,
                Title = string.IsNullOrEmpty(title) ? null : title,
                Grid = new GridInfo
                {
                    RowCount = dim.Rows,
                    ColumnCount = dim.Columns
                },
                MergedCells = optimizedMergedCells,
                Cells = optimizedCells
            };
        }

        /// <summary>
        /// 创建占位符替换函数，优化字符串替换性能（避免多次字符串分配）。
        /// </summary>
        /// <param name="parameters">占位符键值对</param>
        /// <returns>替换函数：输入原始文本，输出替换后的文本</returns>
        private static Func<string, string> CreatePlaceholderReplacer(Dictionary<string, string> parameters)
        {
            if (parameters == null || parameters.Count == 0)
                return text => string.IsNullOrEmpty(text) ? text : Regex.Replace(text, @"\{\{[^}]*\}\}", string.Empty);

            // 构建替换映射，按键长度降序排序，避免短键替换长键的问题（如 {{a}} 和 {{ab}}）
            var replacements = parameters
                .Where(kvp => !string.IsNullOrEmpty(kvp.Key) && kvp.Value != null)
                .OrderByDescending(kvp => kvp.Key.Length)
                .Select(kvp => new { Placeholder = $"{{{{{kvp.Key}}}}}", Value = kvp.Value })
                .ToList();

            if (replacements.Count == 0)
                return text => string.IsNullOrEmpty(text) ? text : Regex.Replace(text, @"\{\{[^}]*\}\}", string.Empty);

            return text =>
            {
                if (string.IsNullOrEmpty(text))
                    return text;

                var result = text;
                foreach (var replacement in replacements)
                {
                    result = result.Replace(replacement.Placeholder, replacement.Value);
                }
                // 移除未被替换的占位符，避免用户看到 {{key}} 标记
                result = Regex.Replace(result, @"\{\{[^}]*\}\}", string.Empty);
                return result;
            };
        }

        /// <summary>
        /// 导出模板为 Excel 文件：在模板副本上替换占位符并填充业务数据后，返回 xlsx 字节数组。
        /// </summary>
        /// <param name="templateId">模板唯一标识，仅允许字母、数字、下划线、中划线</param>
        /// <param name="parameters">用于替换模板中 {{key}} 的键值对，可为 null</param>
        /// <returns>填充后的 Excel 文件字节流（application/vnd.openxmlformats-officedocument.spreadsheetml.sheet）</returns>
        /// <exception cref="ArgumentException">templateId 为空或格式不合法</exception>
        /// <exception cref="FileNotFoundException">模板文件不存在</exception>
        /// <exception cref="InvalidOperationException">Excel 无工作表或工作表为空</exception>
        public byte[] ExportTemplate(string templateId, Dictionary<string, string>? parameters)
        {
            _logger.LogInformation("开始导出模板，TemplateId: {TemplateId}, 占位符数量: {Count}", templateId, parameters?.Count ?? 0);

            // 1. 验证 templateId
            if (string.IsNullOrWhiteSpace(templateId))
            {
                _logger.LogWarning("模板导出失败：TemplateId 为空");
                throw new ArgumentException("TemplateId cannot be null or empty", nameof(templateId));
            }
            if (!Regex.IsMatch(templateId, "^[a-zA-Z0-9_-]+$"))
            {
                _logger.LogWarning("模板导出失败：TemplateId 格式不合法，TemplateId: {TemplateId}", templateId);
                throw new ArgumentException("Invalid templateId format", nameof(templateId));
            }

            // 2. 统一 parameters（并过滤非法 XML 字符，避免导出后 Excel 判定文件损坏）
            parameters ??= new Dictionary<string, string>();
            parameters = SanitizeParametersForExcel(parameters);

            // 3. 定位模板文件
            string filePath = Path.Combine(_templateBasePath, $"{templateId}.xlsx");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning("模板文件不存在，TemplateId: {TemplateId}, Path: {FilePath}", templateId, filePath);
                throw new FileNotFoundException("Template file not found", filePath);
            }
            // 4. 打开模板并就地修改
            using var package = new OfficeOpenXml.ExcelPackage(new FileInfo(filePath));
            if (package.Workbook.Worksheets.Count == 0)
            {
                _logger.LogWarning("导出模板 Excel 无工作表，TemplateId: {TemplateId}", templateId);
                throw new InvalidOperationException("Excel file contains no worksheets");
            }
            var sheet = package.Workbook.Worksheets[0];
            var dim = sheet.Dimension;
            if (dim == null)
            {
                _logger.LogWarning("导出模板工作表为空，TemplateId: {TemplateId}", templateId);
                throw new InvalidOperationException("Worksheet is empty");
            }

            // 5. 创建占位符替换器，并识别合并区域内除左上角外的“幽灵”单元格
            var placeholderReplacer = CreatePlaceholderReplacer(parameters);
            var ghostSet = BuildGhostCellSet(sheet);

            // 6. 仅替换“包含占位符的文本单元格”，避免全表重写导致公式/共享公式被破坏
            for (int r = 1; r <= dim.Rows; r++)
            {
                for (int c = 1; c <= dim.Columns; c++)
                {
                    if (ghostSet.Contains((r, c))) continue;
                    var cell = sheet.Cells[r, c];
                    // 跳过公式单元格，避免把公式结果文本化后回写，破坏公式结构
                    if (!string.IsNullOrEmpty(cell.Formula)) continue;

                    if (cell.Value is not string rawText) continue;
                    if (!rawText.Contains("{{")) continue;

                    var replaced = SanitizeExcelText(placeholderReplacer(rawText));
                    if (!string.Equals(replaced, rawText, StringComparison.Ordinal))
                    {
                        cell.Value = replaced;
                    }
                }
            }

            // 7. 将业务数据写入工作表（由子类或扩展实现具体逻辑）
            FillBusinessData(sheet, parameters);

            // 8. 通过 MemoryStream + SaveAs 输出字节，避免 GetAsByteArray 导致的文件损坏
            using var ms = new MemoryStream();
            package.SaveAs(ms);
            ms.Flush();  // 确保所有缓冲数据写入
            ms.Position = 0;
            var bytes = ms.ToArray();
            if (bytes.Length < 4 || bytes[0] != 0x50 || bytes[1] != 0x4B)
            {
                _logger.LogError("模板导出失败：生成内容不是有效的 xlsx(zip) 文件，TemplateId: {TemplateId}", templateId);
                throw new InvalidDataException("Generated file is not a valid xlsx package");
            }
            _logger.LogInformation("模板导出成功，TemplateId: {TemplateId}, 文件大小: {Size} bytes", templateId, bytes.Length);
            return bytes;
        }

        /// <summary>
        /// 使用已保存的规格书数据获取模板预览：按 pmcCode 拉取 PMC 基础信息与规格规则，扁平化为占位符字典后调用预览逻辑。
        /// </summary>
        public async Task<TemplatePreviewResponse> GetTemplatePreviewBySpec(string templateId, string pmcCode)
        {
            _logger.LogInformation("开始按规格书获取模板预览，TemplateId: {TemplateId}, PmcCode: {PmcCode}", templateId, pmcCode);
            if (string.IsNullOrWhiteSpace(pmcCode))
            {
                _logger.LogWarning("按规格书预览失败：PmcCode 为空");
                throw new ArgumentException("PmcCode cannot be null or empty", nameof(pmcCode));
            }
            var parameters = await BuildSpecPlaceholderDictionaryAsync(pmcCode);
            var result = GetTemplatePreview(templateId, parameters);
            _logger.LogInformation("按规格书模板预览成功，TemplateId: {TemplateId}, PmcCode: {PmcCode}", templateId, pmcCode);
            return result;
        }

        /// <summary>
        /// 使用已保存的规格书数据导出模板：按 pmcCode 拉取规格书并填充占位符后导出 xlsx。
        /// </summary>
        public async Task<byte[]> ExportTemplateBySpec(string templateId, string pmcCode)
        {
            _logger.LogInformation("开始按规格书导出模板，TemplateId: {TemplateId}, PmcCode: {PmcCode}", templateId, pmcCode);
            if (string.IsNullOrWhiteSpace(pmcCode))
            {
                _logger.LogWarning("按规格书导出失败：PmcCode 为空");
                throw new ArgumentException("PmcCode cannot be null or empty", nameof(pmcCode));
            }
            var parameters = await BuildSpecPlaceholderDictionaryAsync(pmcCode);
            // 生成规格书时，将配置状态更新为待审核
            _pmcSpecService.SetSpecConfigStatus(pmcCode, SpecConfigStatus.Review);
            var result = ExportTemplate(templateId, parameters);
            _logger.LogInformation("按规格书模板导出成功，TemplateId: {TemplateId}, PmcCode: {PmcCode}", templateId, pmcCode);
            return result;
        }

        /// <summary>
        /// 根据 PMC 编码从规格书服务获取基础信息与规格规则，组装为模板占位符字典。
        /// 包含：PMC 基础信息、管附件标准配置（标准名、材料、类型）、通径范围信息（NPD、外径、壁厚）。
        /// 标准信息填入格式为「标准名 材料」；同一类型下多个标准时，用英文逗号分隔（如 "GB/T 8163 20#, GB/T 3091 Q235"）。
        /// </summary>
        /// <remarks>
        /// 字段映射关系（保存 → 读取 → 模板占位符）：
        /// - SimpleStandardConfig.StandardFile (object) → PmcStandardInfo.StandardName (string) → {{standardName_N}}
        /// - SimpleStandardConfig.Material (object) → PmcStandardInfo.Material (string) → {{material_N}}
        /// - SimpleComponentTypeConfiguration.ComponentType (string) → PmcStandardInfo.StandardType (string) → {{standardType_N}}
        /// 通径范围信息（通过 GetNPDInfoByPmc 获取）：
        /// - {{npd}} - 通径列表（逗号分隔，如 "15, 20, 25, 32"）
        /// - {{npd_N}} - 第 N 个通径值（N 从 1 开始）
        /// - {{outsideDiameter}} - 外径列表（逗号分隔）
        /// - {{outsideDiameter_N}} - 第 N 个外径值
        /// - {{wallThicknessList}} - 壁厚列表（逗号分隔）
        /// - {{wallThicknessList_N}} - 第 N 个壁厚值
        /// - {{endStandard}} - 端面标准
        /// - {{schedule}} - 壁厚系列
        /// 注意：StandardFile 和 Material 在保存时通过 .ToString() 转换，前端应传入名称字符串（而非 ID）以确保模板中显示为可读名称。
        /// </remarks>
        /// <param name="pmcCode">PMC 编码</param>
        /// <returns>占位符键值对：PMC 基础信息 + 标准信息 + 通径范围信息</returns>
        private async Task<Dictionary<string, string>> BuildSpecPlaceholderDictionaryAsync(string pmcCode)
        {
            _logger.LogDebug("构建规格书占位符字典，PmcCode: {PmcCode}", pmcCode);
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            // PMC 基础信息（来自 7 位编码解析）
            var baseInfo = _pmcSpecService.AnalyzeCodeFromPMC(pmcCode);
            dict["pmcCode"] = baseInfo.PmcCode ?? string.Empty;
            dict["shipNumber"] = baseInfo.ShipNumber ?? string.Empty;
            dict["pipingClass"] = baseInfo.PipingClass ?? string.Empty;
            dict["materialGrade"] = baseInfo.MaterialGrade ?? string.Empty;
            dict["pressureRating"] = baseInfo.PressureRating ?? string.Empty;
            dict["pipeStandard"] = baseInfo.PipeStandard ?? string.Empty;
            dict["materialCategory"] = baseInfo.MaterialCategory ?? string.Empty;
            dict["wallThickness"] = baseInfo.WallThickness ?? string.Empty;

            // 通径范围信息（NPD、外径、壁厚）：根据端面标准（PipeStandard）和壁厚系列（WallThickness）获取
            if (!string.IsNullOrWhiteSpace(baseInfo.PipeStandard) && !string.IsNullOrWhiteSpace(baseInfo.WallThickness))
            {
                try
                {
                    var npdInfo = await _pmcSpecService.GetNPDInfoByPmcAsync(baseInfo.PipeStandard, baseInfo.WallThickness);
                    if (npdInfo != null)
                    {
                        // 通径列表（NPD）：逗号分隔的字符串，如 "15, 20, 25, 32, 40"
                        if (npdInfo.NPD != null && npdInfo.NPD.Count > 0)
                        {
                            dict["npd"] = string.Join(", ", npdInfo.NPD.Select(n => n.ToString("F1", System.Globalization.CultureInfo.InvariantCulture)));
                            // 按索引提供单个通径值：{{npd_1}}, {{npd_2}}, ...
                            for (var i = 0; i < npdInfo.NPD.Count; i++)
                            {
                                dict[$"npd_{i + 1}"] = npdInfo.NPD[i].ToString("F1", System.Globalization.CultureInfo.InvariantCulture);
                            }
                        }
                        else
                        {
                            dict["npd"] = string.Empty;
                        }

                        // 外径列表（OutsideDiameter）：逗号分隔的字符串
                        if (npdInfo.OutsideDiameter != null && npdInfo.OutsideDiameter.Count > 0)
                        {
                            dict["outsideDiameter"] = string.Join(", ", npdInfo.OutsideDiameter.Select(d => d.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)));
                            // 按索引提供单个外径值：{{outsideDiameter_1}}, {{outsideDiameter_2}}, ...
                            for (var i = 0; i < npdInfo.OutsideDiameter.Count; i++)
                            {
                                dict[$"outsideDiameter_{i + 1}"] = npdInfo.OutsideDiameter[i].ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
                            }
                        }
                        else
                        {
                            dict["outsideDiameter"] = string.Empty;
                        }

                        // 壁厚列表（WallThickness）：逗号分隔的字符串
                        if (npdInfo.WallThickness != null && npdInfo.WallThickness.Count > 0)
                        {
                            dict["wallThicknessList"] = string.Join(", ", npdInfo.WallThickness.Select(t => t.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)));
                            // 按索引提供单个壁厚值：{{wallThicknessList_1}}, {{wallThicknessList_2}}, ...
                            for (var i = 0; i < npdInfo.WallThickness.Count; i++)
                            {
                                dict[$"wallThicknessList_{i + 1}"] = npdInfo.WallThickness[i].ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
                            }
                        }
                        else
                        {
                            dict["wallThicknessList"] = string.Empty;
                        }

                        // 端面标准和壁厚系列（用于显示）
                        dict["endStandard"] = npdInfo.EndStandard ?? string.Empty;
                        dict["schedule"] = npdInfo.Schedule ?? string.Empty;

                        _logger.LogDebug("已加载通径范围信息，PmcCode: {PmcCode}, 通径数量: {NpdCount}, 外径数量: {OdCount}, 壁厚数量: {WtCount}",
                            pmcCode, npdInfo.NPD?.Count ?? 0, npdInfo.OutsideDiameter?.Count ?? 0, npdInfo.WallThickness?.Count ?? 0);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "获取通径范围信息失败，PmcCode: {PmcCode}, PipeStandard: {PipeStandard}, WallThickness: {WallThickness}",
                        pmcCode, baseInfo.PipeStandard, baseInfo.WallThickness);
                    // 失败时设置空值，避免模板中显示错误
                    dict["npd"] = string.Empty;
                    dict["outsideDiameter"] = string.Empty;
                    dict["wallThicknessList"] = string.Empty;
                    dict["endStandard"] = string.Empty;
                    dict["schedule"] = string.Empty;
                }
            }
            else
            {
                _logger.LogDebug("缺少端面标准或壁厚系列，无法获取通径范围信息，PmcCode: {PmcCode}", pmcCode);
                dict["npd"] = string.Empty;
                dict["outsideDiameter"] = string.Empty;
                dict["wallThicknessList"] = string.Empty;
                dict["endStandard"] = string.Empty;
                dict["schedule"] = string.Empty;
            }

            // 管附件标准配置：填入格式「标准名 材料」；同类型多标准用逗号分隔
            // 数据来源：GetSpecRules 返回的 PmcStandardInfo 列表，与保存时 MapSimpleConfigurationsToStandardInfos 转换后的结构一致
            if (_pmcSpecService.GetSpecRules(pmcCode, out var standardInfos) && standardInfos != null && standardInfos.Count > 0)
            {
                _logger.LogDebug("已加载规格书标准信息，PmcCode: {PmcCode}, 标准条数: {Count}", pmcCode, standardInfos.Count);
                for (var i = 0; i < standardInfos.Count; i++)
                {
                    var n = i + 1;
                    var s = standardInfos[i];
                    // StandardName 和 Material 直接来自 PmcStandardInfo，与保存时的映射一致
                    var nameMaterial = FormatStandardNameMaterial(s.StandardName, s.Material);
                    dict[$"standard_{n}"] = nameMaterial;
                    dict[$"standardName_{n}"] = s.StandardName ?? string.Empty;
                    dict[$"standardType_{n}"] = s.StandardType ?? string.Empty;
                    dict[$"material_{n}"] = s.Material ?? string.Empty;
                }
                dict["material"] = standardInfos[0].Material ?? string.Empty;

                // 按标准类型分组，同类型多个标准用逗号分隔，格式「标准名 材料, 标准名 材料」
                var byType = standardInfos
                    .Where(s => !string.IsNullOrWhiteSpace(s.StandardType))
                    .GroupBy(s => s.StandardType!.Trim(), StringComparer.OrdinalIgnoreCase);
                foreach (var g in byType)
                {
                    var typeKey = "standard_" + SanitizePlaceholderKey(g.Key);
                    var value = string.Join(", ", g.Select(s => FormatStandardNameMaterial(s.StandardName, s.Material)));
                    dict[typeKey] = value;
                }
            }
            else
            {
                _logger.LogDebug("未找到规格书标准信息或列表为空，PmcCode: {PmcCode}", pmcCode);
                dict["material"] = string.Empty;
            }

            return dict;
        }

        /// <summary>格式化为「标准名 材料」。</summary>
        private static string FormatStandardNameMaterial(string? standardName, string? material)
        {
            var a = (standardName ?? string.Empty).Trim();
            var b = (material ?? string.Empty).Trim();
            return string.IsNullOrEmpty(a) && string.IsNullOrEmpty(b) ? string.Empty : $"{a} {b}".Trim();
        }

        /// <summary>将类型名转为占位符安全键（仅保留字母、数字、下划线，空格转下划线）。</summary>
        private static string SanitizePlaceholderKey(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName)) return "Unknown";
            var sb = new System.Text.StringBuilder();
            foreach (var c in typeName.Trim())
            {
                if (char.IsLetterOrDigit(c) || c == '_') sb.Append(c);
                else if (char.IsWhiteSpace(c) && (sb.Length == 0 || sb[sb.Length - 1] != '_')) sb.Append('_');
            }
            var s = sb.ToString().Trim('_');
            return string.IsNullOrEmpty(s) ? "Unknown" : s;
        }

        /// <summary>
        /// 构建合并区域内的“幽灵”单元格集合（每个合并块只保留左上角，其余行列加入集合）。
        /// 导出时跳过这些单元格的写入，避免破坏 Excel 合并结构。
        /// </summary>
        /// <param name="sheet">当前工作表</param>
        /// <returns>幽灵单元格 (1-based Row, Col) 集合</returns>
        private static HashSet<(int Row, int Col)> BuildGhostCellSet(OfficeOpenXml.ExcelWorksheet sheet)
        {
            var set = new HashSet<(int, int)>();
            if (sheet.MergedCells == null) return set;
            foreach (var addr in sheet.MergedCells)
            {
                var range = sheet.Cells[addr];
                int sr = range.Start.Row, sc = range.Start.Column, er = range.End.Row, ec = range.End.Column;
                for (int r = sr; r <= er; r++)
                {
                    for (int c = sc; c <= ec; c++)
                    {
                        if (r != sr || c != sc)
                            set.Add((r, c));
                    }
                }
            }
            return set;
        }

        /// <summary>
        /// 将业务数据填充到导出用工作表中。当前为扩展点，可根据占位符或行列规则写入数据。
        /// </summary>
        /// <param name="exportSheet">正在导出的首张工作表</param>
        /// <param name="parameters">本次导出请求的模板参数</param>
        private void FillBusinessData(OfficeOpenXml.ExcelWorksheet exportSheet, Dictionary<string, string> parameters)
        {
            var businessData = GetExportBusinessData(parameters);

            // 示例：填充业务数据
            // for (int r = 2; r <= exportSheet.Dimension?.Rows; r++)
            // {
            //     for (int c = 1; c <= exportSheet.Dimension?.Columns; c++)
            //     {
            //         if (exportSheet.Cells[r, c].Text.Contains("{{material}}"))
            //         {
            //             exportSheet.Cells[r, c].Value = businessData[0];
            //         }
            //     }
            // }
        }

        /// <summary>
        /// 从 EPPlus 单元格中提取前端可用的样式（背景色、水平对齐、字重）。
        /// </summary>
        /// <param name="cell">Excel 单元格区域</param>
        /// <returns>若有非默认样式则返回 <see cref="CellStyle"/>，否则返回 null</returns>
        private static CellStyle? ExtractCellStyle(OfficeOpenXml.ExcelRange cell)
        {
            var rgb = cell.Style.Fill.BackgroundColor?.Rgb;
            string? hex = rgb != null && rgb.Length == 8 ? $"#{rgb[2..]}" : null;

            var align = cell.Style.HorizontalAlignment.ToString().ToLower();

            var weight = cell.Style.Font.Bold ? "bold" : "normal";

            // 全为默认值时返回 null，减少前端数据量
            if (hex == null && align == "left" && weight == "normal")
            {
                return null;
            }

            return new CellStyle(hex, align, weight);
        }

        /// <summary>
        /// 获取预览用的业务数据（如材质等）。当前为模拟数据，实际项目应改为从数据库或配置获取。
        /// </summary>
        /// <param name="_">模板参数，预留用于按参数筛选数据</param>
        /// <returns>业务数据项列表，用于填充预览中 material 等占位</returns>
        private static List<string> GetBusinessData(Dictionary<string, string> _) => new List<string> { "20#", "SSL 304" };

        /// <summary>
        /// 获取导出时需写入工作表的业务数据。当前返回空列表，后续可按 parameters 从数据库或服务拉取并返回。
        /// </summary>
        /// <param name="parameters">本次导出请求的模板参数</param>
        /// <returns>待写入的业务数据列表</returns>
        private static List<string> GetExportBusinessData(Dictionary<string, string> parameters)
        {
            return new List<string>();
        }

        /// <summary>
        /// 清洗导出参数：移除 Excel(OpenXML) 不允许的 XML 字符，避免生成损坏工作簿。
        /// </summary>
        private static Dictionary<string, string> SanitizeParametersForExcel(Dictionary<string, string> parameters)
        {
            var sanitized = new Dictionary<string, string>(parameters.Count, StringComparer.Ordinal);
            foreach (var kv in parameters)
            {
                if (string.IsNullOrWhiteSpace(kv.Key)) continue;
                sanitized[kv.Key] = SanitizeExcelText(kv.Value ?? string.Empty);
            }
            return sanitized;
        }

        /// <summary>
        /// 仅保留 XML 合法字符，过滤控制字符（如 \0），防止写入 xlsx 后 Excel 无法打开。
        /// </summary>
        private static string SanitizeExcelText(string? value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            var sb = new System.Text.StringBuilder(value.Length);
            foreach (var ch in value)
            {
                if (XmlConvert.IsXmlChar(ch))
                {
                    sb.Append(ch);
                }
            }
            return sb.ToString();
        }
    }
}
