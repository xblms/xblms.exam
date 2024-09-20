using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BlockMethod
    {
        [DataEnum(DisplayName = "跳转至指定网址")] RedirectUrl,
        [DataEnum(DisplayName = "显示拦截信息")] Warning,
        [DataEnum(DisplayName = "输入密码验证")] Password,
    }
}
