using AutoMapper.Execution;
using PMCSystem_Backend.Dtos.TemplatePreview;
using PMCSystem_Backend.Services.Interfaces;
using System.Text.RegularExpressions;

namespace PMCSystem_Backend.Services.Implementations
{
    public class TemplatePreviewService : ITemplatePreviewService
    {
        private readonly string _templateBasePath;

        public TemplatePreviewService(string templateBasePath)
        {
            _templateBasePath = templateBasePath;
        }

        public TemplatePreviewResponse GetTemplatePreview(string templateId, Dictionary<string, string> parameters)
        {
            // 1. 验证templateId
            if (string.IsNullOrWhiteSpace(templateId))
            {
                throw new ArgumentException("TemplateId cannot be null or empty", nameof(templateId));
            }
            if (!Regex.IsMatch(templateId, "^[a-zA-Z0-9_-]+$"))
            {
                throw new ArgumentException("Invalid templateId format", nameof(templateId));
            }

            // 2. 验证parameters
            parameters ??= new Dictionary<string, string>();

            // 3. 构建文件路径
            string filePath = Path.Combine(_templateBasePath, $"{templateId}.xlsx");
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Template file not found", filePath);
            }

            // 4. 解析Excel
            using var package = new OfficeOpenXml.ExcelPackage(new FileInfo(filePath));
            if (package.Workbook.Worksheets.Count == 0)
            {
                throw new InvalidOperationException("Excel file contains no worksheets");
            }
            var sheet = package.Workbook.Worksheets[0];
            var dim = sheet.Dimension ?? throw new InvalidOperationException("Worksheet is empty");

            // 5. 提取合并单元格
            var mergedCells = new List<MergedCell>();
            if (sheet.MergedCells != null)
            {
                foreach (var mergedCellAddress in sheet.MergedCells)
                {
                    var range = sheet.Cells[mergedCellAddress];
                    mergedCells.Add(new MergedCell
                    {
                        StartRow = range.Start.Row - 1,
                        StartColumn = range.Start.Column - 1,
                        EndRow = range.End.Row - 1,
                        EndColumn = range.End.Column - 1,
                        Value = range.Text,
                        Style = ExtractCellStyle(range)
                    });
                }
            }

            // 5. 跳过被合并覆盖的单元格
            var skipSet = new HashSet<(int, int)>();
            foreach(var cell in mergedCells)
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

            // 6. 提取普通单元格
            var cells = new List<PreviewCell>();
            for (int r = 0; r < dim.Rows; r++)
            {
                for (int c = 0; c < dim.Columns; c++)
                {
                    if (skipSet.Contains((r, c))) continue;
                    var cell = sheet.Cells[r + 1, c + 1];
                    var cellValue = cell.Text;
                    // 替换参数
                    foreach (var param in parameters)
                    {
                        if (param.Key != null && param.Value != null)
                        {
                            cellValue = cellValue.Replace($"{{{{{param.Key}}}}}", param.Value);
                        }
                    }
                    cells.Add(new PreviewCell
                    {
                        Row = r,
                        Column = c,
                        Value = cellValue,
                        IsHeader = r == 0,
                        IsData = r > 0,
                        Field = $"col{c + 1}",
                        Style = ExtractCellStyle(cell)
                    });
                }
            }

            // 7. 注入动态数据
            var businessData = GetBusinessData(parameters);
            if (businessData != null && businessData.Count > 0)
            {
                foreach (var cell in cells)
                {
                    if (cell.Field?.StartsWith("material") == true)
                        cell.Value = businessData[0];
                }
            }

            // 8. 安全获取标题
            string title = string.Empty;
            try
            {
                title = sheet.Cells["A1"]?.Text ?? string.Empty;
            }
            catch
            {
                // 如果A1单元格不存在，使用空字符串
                title = string.Empty;
            }

            return new TemplatePreviewResponse
            {
                TemplateId = templateId,
                Title = title,
                Grid = new GridInfo
                {
                    RowCount = dim.Rows,
                    ColumnCount = dim.Columns
                },
                MergedCells = mergedCells,
                Cells = cells
            };
        }

        /// <summary>
        /// 导出模板为Excel文件
        /// </summary>
        /// <param name="templateId">模板ID</param>
        /// <param name="parameters">模板参数</param>
        /// <returns>Excel文件的字节流</returns>
        public byte[] ExportTemplate(string templateId, Dictionary<string, string>? parameters)
        {
            // 1. 验证templateId
            if (string.IsNullOrWhiteSpace(templateId))
            {
                throw new ArgumentException("TemplateId cannot be null or empty", nameof(templateId));
            }
            if (!Regex.IsMatch(templateId, "^[a-zA-Z0-9_-]+$"))
            {
                throw new ArgumentException("Invalid templateId format", nameof(templateId));
            }

            // 2. 验证parameters
            parameters ??= new Dictionary<string, string>();

            // 3. 构建文件路径
            string filePath = Path.Combine(_templateBasePath, $"{templateId}.xlsx");
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Template file not found", filePath);
            }

            // 4. 读取模板并就地修改（不新建工作簿复制，避免样式/合并复制导致文件损坏）
            using var package = new OfficeOpenXml.ExcelPackage(new FileInfo(filePath));
            if (package.Workbook.Worksheets.Count == 0)
            {
                throw new InvalidOperationException("Excel file contains no worksheets");
            }
            var sheet = package.Workbook.Worksheets[0];
            var dim = sheet.Dimension ?? throw new InvalidOperationException("Worksheet is empty");

            // 5. 合并区域中非左上角单元格视为“幽灵”，不写入避免破坏合并
            var ghostSet = BuildGhostCellSet(sheet);

            // 6. 替换模板参数（仅修改非幽灵单元格）
            for (int r = 1; r <= dim.Rows; r++)
            {
                for (int c = 1; c <= dim.Columns; c++)
                {
                    if (ghostSet.Contains((r, c))) continue;
                    var cell = sheet.Cells[r, c];
                    var cellValue = cell.Text ?? string.Empty;
                    foreach (var p in parameters)
                    {
                        if (p.Key != null && p.Value != null)
                        {
                            cellValue = cellValue.Replace($"{{{{{p.Key}}}}}", p.Value);
                        }
                    }
                    cell.Value = cellValue;
                }
            }

            // 7. 填充业务数据
            FillBusinessData(sheet, parameters);

            // 8. 使用 MemoryStream + SaveAs 替代 GetAsByteArray，避免已知的文件损坏问题
            using var ms = new MemoryStream();
            package.SaveAs(ms);
            ms.Position = 0;
            return ms.ToArray();
        }

        /// <summary>
        /// 构建合并区域内“幽灵”单元格集合（除每个合并区左上角外的单元格）。写入这些单元格会破坏合并结构。
        /// </summary>
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
        /// 填充业务数据到导出的Excel文件中
        /// 需要根据具体的业务逻辑进行实现
        /// </summary>
        private void FillBusinessData(OfficeOpenXml.ExcelWorksheet exportSheet, Dictionary<string, string> parameters)
        {
            // 空实现，后续根据业务逻辑补充
            // 此方法用于将动态业务数据填充到Excel表格中
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

        private CellStyle? ExtractCellStyle(OfficeOpenXml.ExcelRange cell)
        {
            var rgb = cell.Style.Fill.BackgroundColor?.Rgb;
            string? hex = rgb != null && rgb.Length == 8 ? $"#{rgb[2..]}" : null;

            var align = cell.Style.HorizontalAlignment.ToString().ToLower();

            var weight = cell.Style.Font.Bold ? "bold" : "normal";

            // 如果所有样式都是默认值，返回 null
            if (hex == null && align == "left" && weight == "normal")
            {
                return null;
            }

            // CellStyle 构造函数接受可空参数，所以可以安全传递 null
            return new CellStyle(hex, align, weight);
        }

        // 业务数据模拟（实际项目需要替换）
        private List<string> GetBusinessData(Dictionary<string, string> _) => (new List<string> { "20#", "SSL 304" });

        /// <summary>
        /// 获取导出所需的业务数据
        /// 空实现，需要根据具体业务逻辑进行补充
        /// </summary>
        /// <param name="parameters">模板参数</param>
        /// <returns>业务数据列表</returns>
        private List<string> GetExportBusinessData(Dictionary<string, string> parameters)
        {
            // 空实现 - 后续补充具体的业务数据获取逻辑
            return new List<string>();
        }
    }
}
