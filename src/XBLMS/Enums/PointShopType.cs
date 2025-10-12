using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PointShopType
    {
        [DataEnum(DisplayName = "用户选择", Value = "OnSelf")]
        OnSelf,
        [DataEnum(DisplayName = "在线发", Value = "Online")]
        Online,
        [DataEnum(DisplayName = "现场领",Value = "Offline")]
        Offline,
    }
}
