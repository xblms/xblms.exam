using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ExamPaperTmArrangeType
    {
        [DataEnum(DisplayName = "快速匹配", Value = "ArrangeFast")] ArrangeFast,
        [DataEnum(DisplayName = "精准匹配", Value = "ArrangeJingzhun")] ArrangeSlow
    }
}
