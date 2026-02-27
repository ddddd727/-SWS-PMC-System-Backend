using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Validation;
using OfficeOpenXml;

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

var templatePath = @"f:\CodeProject\PMC\PMCSystem_Backend\Docs\TemplateFile\Pipe-Spec.xlsx";
var outDir = @"f:\CodeProject\PMC\PMCSystem_Backend\_diag\out";
Directory.CreateDirectory(outDir);

var rawCopy = Path.Combine(outDir, "raw_copy.xlsx");
var epplusCopy = Path.Combine(outDir, "epplus_save_only.xlsx");
var epplusEditCopy = Path.Combine(outDir, "epplus_replace_demo.xlsx");

File.Copy(templatePath, rawCopy, true);

// 1) 仅打开+保存
using (var p = new ExcelPackage(new FileInfo(templatePath)))
{
    p.SaveAs(new FileInfo(epplusCopy));
}

// 2) 模拟导出逻辑：只替换包含占位符的字符串单元格
using (var p = new ExcelPackage(new FileInfo(templatePath)))
{
    var ws = p.Workbook.Worksheets[0];
    var dim = ws.Dimension;
    if (dim != null)
    {
        for (int r = 1; r <= dim.Rows; r++)
        {
            for (int c = 1; c <= dim.Columns; c++)
            {
                var cell = ws.Cells[r, c];
                if (!string.IsNullOrEmpty(cell.Formula)) continue;
                if (cell.Value is not string s) continue;
                if (!s.Contains("{{")) continue;
                cell.Value = s.Replace("{{material}}", "20#");
            }
        }
    }
    p.SaveAs(new FileInfo(epplusEditCopy));
}

Validate("raw_copy", rawCopy);
Validate("epplus_save_only", epplusCopy);
Validate("epplus_replace_demo", epplusEditCopy);

static void Validate(string tag, string path)
{
    Console.WriteLine($"\n=== {tag} ===");
    Console.WriteLine(path);
    Console.WriteLine($"size={new FileInfo(path).Length}");
    using var doc = SpreadsheetDocument.Open(path, false);
    var validator = new OpenXmlValidator();
    var errs = validator.Validate(doc).Take(20).ToList();
    Console.WriteLine($"openxml_errors={errs.Count}");
    foreach (var e in errs)
    {
        Console.WriteLine($"- {e.Description}");
    }
}
