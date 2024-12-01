using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PkRuleType
    {
        [DataEnum(DisplayName = "1v1 淘汰赛", Value = "Game1")] Game1,
    }

}
