using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum UsersGroupType
    {
        [DataEnum(DisplayName = "所有用户",Value = "All")] All,
        [DataEnum(DisplayName = "固定用户", Value = "Fixed")] Fixed,
        [DataEnum(DisplayName = "范围用户", Value = "Range")] Range,
    }
}
