using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SystemCode
    {
        [DataEnum(DisplayName = "Exam",Value = "Exam")] Exam,
        [DataEnum(DisplayName = "Elearning", Value = "Elearning")] Elearning
    }

}
