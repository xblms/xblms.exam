using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TmAnalysisType
    {
        [DataEnum(DisplayName = "综合统计", Value = "ByExamAll")]
        ByExamAll,
        [DataEnum(DisplayName = "按练习", Value = "ByPractice")]
        ByPractice,
        [DataEnum(DisplayName = "按试卷", Value = "ByExamOnlyOne")]
        ByExamOnlyOne,
    }
}
