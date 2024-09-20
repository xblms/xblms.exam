using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TmGroupType
    {
        [DataEnum(DisplayName = "所有题目",Value = "All")] All,
        [DataEnum(DisplayName = "固定题目", Value = "Fixed")] Fixed,
        [DataEnum(DisplayName = "范围题目", Value = "Range")] Range,
    }
}
