using PMCSystem_Backend.Dtos.TemplatePreview;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface ITemplatePreviewService
    {
        TemplatePreviewResponse GetTemplatePreview(string templateId, Dictionary<string, string> queryParams);

        /// <summary>
        /// 导出模板为Excel文件
        /// </summary>
        /// <param name="templateId">模板ID</param>
        /// <param name="parameters">模板参数</param>
        /// <returns>Excel文件的字节流</returns>
        byte[] ExportTemplate(string templateId, Dictionary<string, string> parameters);
    }
}
