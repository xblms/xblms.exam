using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ExamPaperTmRandomType
    {
        [DataEnum(DisplayName = "固定试卷", Value = "RandomNone")] RandomNone,
        [DataEnum(DisplayName = "随机试卷",Value = "RandomNow")] RandomNow,
        [DataEnum(DisplayName = "考前随机", Value = "RandomExaming")] RandomExaming
    }
}
