using PMCSystem_Backend.Dtos.TemplatePreview;

namespace PMCSystem_Backend.Services.Interfaces
{
    public interface ITemplatePreviewService
    {
        TemplatePreviewResponse GetTemplatePreview(string templateId, Dictionary<string, string> queryParams);
    }
}
