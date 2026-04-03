namespace PMCSystem_Backend.Modules.PipingSpecifications.Dtos
{
    // 模板预览响应
    public class TemplatePreviewResponse
    {
        // 模板ID
        public string TemplateId { get; set; } = string.Empty;

        // 模板名称
        public string? Title { get; set; }

        // 表格朝向： rowHeader（行表头）或 columnHeader（列表头）
        public string Orientation { get; set; } = "rowHeader";

        // 表格范围信息
        public GridInfo? Grid { get; set; }

        // 合并单元格信息
        public List<MergedCell>? MergedCells { get; set; }

        // 预览单元格列表（0-based 索引）
        public List<PreviewCell>? Cells { get; set; }
    }

    // 表格范围信息
    public class GridInfo
    {
        // 行数
        public int RowCount { get; set; }
        // 列数
        public int ColumnCount { get; set; }
    }

    // 合并单元格信息
    public class MergedCell
    {
        // 起始行号
        public int StartRow { get; set; }

        // 结束行号
        public int EndRow { get; set; }

        // 起始列号
        public int StartColumn { get; set; }

        // 结束列号
        public int EndColumn { get; set; }

        // 合并后单元格的值
        public string? Value { get; set; }

        public CellStyle? Style { get; set; }

    }


    // 预览单元格信息
    public class PreviewCell
    {
        // 所在行号
        public int Row { get; set; }
        // 所在列号
        public int Column { get; set; }
        // 单元格值
        public string? Value { get; set; }
        // 是否为表头单元格
        public bool IsHeader { get; set; }
        // 是否为数据单元格
        public bool IsData { get; set; }
        // 对应的数据字段名 （用于调试或映射）
        public string? Field { get; set; }
        // 单元格样式
        public CellStyle? Style { get; set; }
    }


    // 单元格样式
    public class CellStyle
    {
        // 背景颜色：HEX码格式，例如#FFFFFF
        public string? BgColor { get; set; }

        // 文字格式，对齐:left, center, right
        public string? TextAlign { get; set; }
        // 字体样式， normal, bold
        public string? FontWeight { get; set; }

        public CellStyle(string? bgColor, string? textAlign, string? fontWeight)
        {
            BgColor = bgColor;
            TextAlign = textAlign;
            FontWeight = fontWeight;
        }
    }
}
