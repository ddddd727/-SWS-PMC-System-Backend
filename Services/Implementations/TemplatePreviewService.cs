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
            if (!Regex.IsMatch(templateId, "^[a-zA-Z0-9_-]+$"))
            {
                throw new ArgumentException("Invalid templateId format", nameof(templateId));
            }

            // 2. 构建文件路径
            string filePath = Path.Combine(_templateBasePath, $"{templateId}.xlsx");
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Template file not found", filePath);
            }

            // 3. 解析Excel
            using var package = new OfficeOpenXml.ExcelPackage(new FileInfo(filePath));
            var sheet = package.Workbook.Worksheets[0];
            var dim = sheet.Dimension ?? throw new InvalidOperationException("Worksheet is empty");

            // 4. 提取合并单元格
            var mergedCells = new List<MergedCell>();
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
                        cellValue = cellValue.Replace($"{{{{{param.Key}}}}}", param.Value);
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
            foreach (var cell in cells)
            {
                if (cell.Field?.StartsWith("material") == true)
                    cell.Value = businessData[0];
            }

            return new TemplatePreviewResponse
            {
                TemplateId = templateId,
                Title = sheet.Cells["A1"].Text,
                Grid = new GridInfo
                {
                    RowCount = dim.Rows,
                    ColumnCount = dim.Columns
                },
                MergedCells = mergedCells,
                Cells = cells
            };
        }

        private CellStyle ExtractCellStyle(OfficeOpenXml.ExcelRange cell)
        {
            var rgb = cell.Style.Fill.BackgroundColor?.Rgb;
            string? hex = rgb != null && rgb.Length == 8 ? $"#{rgb[2..]}" : null;

            var align = cell.Style.HorizontalAlignment.ToString().ToLower();

            var weight = cell.Style.Font.Bold ? "bold" : "normal";

            return (hex == null && align == "left" && weight == "normal") ? null : new CellStyle(hex, align, weight); 
        }

        // 业务数据模拟（实际项目需要替换）
        private List<string> GetBusinessData(Dictionary<string, string> _) => (new List<string> { "20#", "SSL 304" });
    }
}
