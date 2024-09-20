using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AreaType
    {
        [DataEnum(DisplayName = "不拦截区域")] None,
        [DataEnum(DisplayName = "拦截指定区域")] Includes,
        [DataEnum(DisplayName = "拦截指定区域外其他区域")] Excludes
    }
}
