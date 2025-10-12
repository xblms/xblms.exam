using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PointShopState
    {
        [DataEnum(DisplayName = "待发货", Value = "Daifahuo")]
        Daifahuo,
        [DataEnum(DisplayName = "已发货", Value = "Yifahuo")]
        Yifahuo,
        [DataEnum(DisplayName = "待签收", Value = "Daiqianshou")]
        Daiqianshou,
        [DataEnum(DisplayName = "已签收", Value = "Yiqianshou")]
        Yiqianshou,
        [DataEnum(DisplayName = "待领取", Value = "Dailing")]
        Dailing,
        [DataEnum(DisplayName = "已领取", Value = "Yiling")]
        Yiling,
    }
}
