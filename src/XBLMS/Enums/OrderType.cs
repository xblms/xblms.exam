using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OrderType
    {
        [DataEnum(DisplayName = "升序", Value = "asc")]
        asc,
        [DataEnum(DisplayName = "降序", Value = "desc")]
        desc,
    }
}
