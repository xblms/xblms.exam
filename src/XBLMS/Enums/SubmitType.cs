using Datory;
using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SubmitType
    {
        [DataEnum(DisplayName = "保存", Value = "Save")] Save,
        [DataEnum(DisplayName = "提交/发布", Value = "Submit")] Submit,
    }

}
