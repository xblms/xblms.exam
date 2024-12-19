using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ExamPaperExportType
    {
        [DataEnum(DisplayName = "导出一份试卷", Value = "PaperOnlyOne")]
        PaperOnlyOne,
        [DataEnum(DisplayName = "打包试卷", Value = "PaperRar")]
        PaperRar,
        [DataEnum(DisplayName = "导出一份成绩", Value = "PaperScoreOnlyOne")]
        PaperScoreOnlyOne,
        [DataEnum(DisplayName = "打包成绩", Value = "PaperScoreRar")]
        PaperScoreRar
    }
}
