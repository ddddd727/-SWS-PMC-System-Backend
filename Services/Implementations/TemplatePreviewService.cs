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
        public byte[] ExportTemplate(string templateId, Dictionary<string, string> parameters)
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

            // 4. 读取原始Excel模板
            using var sourcePackage = new OfficeOpenXml.ExcelPackage(new FileInfo(filePath));
            if (sourcePackage.Workbook.Worksheets.Count == 0)
            {
                throw new InvalidOperationException("Excel file contains no worksheets");
            }
            var sourceSheet = sourcePackage.Workbook.Worksheets[0];
            var dim = sourceSheet.Dimension ?? throw new InvalidOperationException("Worksheet is empty");

            // 4. 创建新的Excel包用于导出
            using var exportPackage = new OfficeOpenXml.ExcelPackage();
            var exportSheet = exportPackage.Workbook.Worksheets.Add("Export");

            // 5. 复制所有单元格内容、样式和合并信息
            CopySheetContent(sourceSheet, exportSheet, dim, parameters);

            // 6. 填充业务数据
            FillBusinessData(exportSheet, parameters);

            // 7. 生成Excel文件并返回字节流
            return exportPackage.GetAsByteArray();
        }

        /// <summary>
        /// 复制Excel表格内容、样式和格式
        /// </summary>
        private void CopySheetContent(OfficeOpenXml.ExcelWorksheet sourceSheet, OfficeOpenXml.ExcelWorksheet exportSheet, OfficeOpenXml.ExcelAddress dim, Dictionary<string, string> parameters)
        {
            // 确保parameters不为null
            parameters ??= new Dictionary<string, string>();

            // 复制普通单元格内容和样式
            for (int r = 1; r <= dim.Rows; r++)
            {
                for (int c = 1; c <= dim.Columns; c++)
                {
                    var sourceCell = sourceSheet.Cells[r, c];
                    var exportCell = exportSheet.Cells[r, c];

                    // 复制值并替换参数
                    var cellValue = sourceCell.Text;
                    foreach (var param in parameters)
                    {
                        if (param.Key != null && param.Value != null)
                        {
                            cellValue = cellValue.Replace($"{{{{{param.Key}}}}}", param.Value);
                        }
                    }
                    exportCell.Value = cellValue;

                    // 复制样式
                    CopyCellStyle(sourceCell, exportCell);
                }
            }

            // 复制合并单元格
            if (sourceSheet.MergedCells != null)
            {
                foreach (var mergedCell in sourceSheet.MergedCells.ToList())
                {
                    exportSheet.Cells[mergedCell].Merge = true;
                }
            }

            // 复制列宽
            for (int c = 1; c <= dim.Columns; c++)
            {
                exportSheet.Column(c).Width = sourceSheet.Column(c).Width;
            }

            // 复制行高
            for (int r = 1; r <= dim.Rows; r++)
            {
                exportSheet.Row(r).Height = sourceSheet.Row(r).Height;
            }
        }

        /// <summary>
        /// 复制单元格样式
        /// </summary>
        private void CopyCellStyle(OfficeOpenXml.ExcelRange sourceCell, OfficeOpenXml.ExcelRange exportCell)
        {
            // 复制字体
            exportCell.Style.Font.Name = sourceCell.Style.Font.Name;
            exportCell.Style.Font.Size = sourceCell.Style.Font.Size;
            exportCell.Style.Font.Bold = sourceCell.Style.Font.Bold;
            exportCell.Style.Font.Italic = sourceCell.Style.Font.Italic;

            // 复制背景颜色
            if (sourceCell.Style.Fill.BackgroundColor?.Rgb != null)
            {
                exportCell.Style.Fill.PatternType = sourceCell.Style.Fill.PatternType;
                try
                {
                    exportCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml($"#{sourceCell.Style.Fill.BackgroundColor.Rgb[2..]}"));
                }
                catch
                {
                    // 如果颜色转换失败，跳过颜色复制
                }
            }

            // 复制对齐方式
            exportCell.Style.HorizontalAlignment = sourceCell.Style.HorizontalAlignment;
            exportCell.Style.VerticalAlignment = sourceCell.Style.VerticalAlignment;

            // 复制边框
            exportCell.Style.Border.Left.Style = sourceCell.Style.Border.Left.Style;
            exportCell.Style.Border.Right.Style = sourceCell.Style.Border.Right.Style;
            exportCell.Style.Border.Top.Style = sourceCell.Style.Border.Top.Style;
            exportCell.Style.Border.Bottom.Style = sourceCell.Style.Border.Bottom.Style;
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
