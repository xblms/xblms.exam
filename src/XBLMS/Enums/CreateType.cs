using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CreateType
    {
        [DataEnum(DisplayName = "提交答案")]
        SubmitAnswer,
        [DataEnum(DisplayName = "提交答卷")]
        SubmitPaper,
        [DataEnum(DisplayName = "颁发证书")]
        ExamAwardCer,
        [DataEnum(DisplayName = "提交组合题答案")]
        SubmitAnswerSmall
    }
}
