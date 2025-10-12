using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TableStyleType
    {
        [DataEnum(DisplayName = "用户字段", Value = "user")]
        User,
        [DataEnum(DisplayName = "题目字段", Value = "tm")]
        Tm,
    }
}
