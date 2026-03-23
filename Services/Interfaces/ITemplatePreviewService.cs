using PMCSystem_Backend.Modules.PipingSpecifications.Dtos;

namespace PMCSystem_Backend.Services.Interfaces
{
    /// <summary>
    /// 模板预览与导出服务契约：支持按自定义参数或按规格书（PMC 编码）填充模板中的 "{{key}}" 占位符。
    /// </summary>
    public interface ITemplatePreviewService
    {
        /// <summary>
        /// 获取指定模板的预览数据，使用自定义键值对替换模板中的 "{{key}}" 占位符。
        /// </summary>
        /// <param name="templateId">模板唯一标识</param>
        /// <param name="queryParams">占位符键值对，模板中 {{key}} 将被替换为 queryParams[key]</param>
        /// <returns>预览响应（标题、网格、合并单元格、单元格列表）</returns>
        TemplatePreviewResponse GetTemplatePreview(string templateId, Dictionary<string, string> queryParams);

        /// <summary>
        /// 导出模板为 Excel 文件，使用自定义键值对替换模板中的 "{{key}}" 占位符。
        /// </summary>
        /// <param name="templateId">模板唯一标识</param>
        /// <param name="parameters">占位符键值对，可为 null（按空字典处理）</param>
        /// <returns>填充后的 xlsx 文件字节流</returns>
        byte[] ExportTemplate(string templateId, Dictionary<string, string>? parameters);

        /// <summary>
        /// 使用已保存的规格书数据填充模板并返回预览。包含 PMC 基础信息、管附件标准配置、通径范围信息（NPD、外径、壁厚）。
        /// 标准信息填入格式为「标准名 材料」，同类型多标准用逗号分隔。
        /// </summary>
        /// <remarks>
        /// 模板中可用的占位符（规格书驱动）：<br/>
        /// PMC 基础信息：pmcCode, shipNumber, pipingClass, materialGrade, pressureRating, pipeStandard, materialCategory, wallThickness；<br/>
        /// 规格按行（1-based）：standard_1（标准名 材料）, standard_2, ...；以及 standardName_N, standardType_N, material_N；<br/>
        /// 规格按类型（同类型多标准用逗号分隔）：standard_Pipe, standard_Elbow, standard_Tee 等，值为「标准名 材料, 标准名 材料」；<br/>
        /// 通径范围信息：npd（通径列表，逗号分隔）、npd_N（第 N 个通径值）、outsideDiameter（外径列表）、outsideDiameter_N（第 N 个外径值）、wallThicknessList（壁厚列表）、wallThicknessList_N（第 N 个壁厚值）、endStandard（端面标准）、schedule（壁厚系列）。
        /// </remarks>
        /// <param name="templateId">模板唯一标识</param>
        /// <param name="pmcCode">PMC 编码，用于查询已保存的规格书数据</param>
        /// <returns>预览响应</returns>
        TemplatePreviewResponse GetTemplatePreviewBySpec(string templateId, string pmcCode);

        /// <summary>
        /// 使用已保存的规格书数据填充模板并导出为 Excel 文件。
        /// </summary>
        /// <param name="templateId">模板唯一标识</param>
        /// <param name="pmcCode">PMC 编码，用于查询已保存的规格书数据</param>
        /// <returns>填充后的 xlsx 文件字节流</returns>
        byte[] ExportTemplateBySpec(string templateId, string pmcCode);
    }
}
