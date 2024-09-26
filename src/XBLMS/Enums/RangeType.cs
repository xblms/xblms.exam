using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum RangeType
    {
        [DataEnum(DisplayName = "考试安排",Value = "Exam")] Exam,
    }
}
