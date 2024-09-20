using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PracticeType
    {
        [DataEnum(DisplayName = "快速练习",Value = "All")] All,
        [DataEnum(DisplayName = "题库练习", Value = "Group")] Group,
        [DataEnum(DisplayName = "错题练习", Value = "Wrong")] Wrong,
        [DataEnum(DisplayName = "收藏练习", Value = "Collect")] Collect
    }
}
