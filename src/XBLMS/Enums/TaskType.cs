using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TaskType
    {
        [DataEnum(DisplayName = "Ping")]
        Ping,
        [DataEnum(DisplayName = "考试", Value = "Exam")]
        Exam,
        [DataEnum(DisplayName = "问卷", Value = "ExamQ")]
        ExamQ,
        [DataEnum(DisplayName = "测评", Value = "ExamAss")]
        ExamAss,
        [DataEnum(DisplayName = "学习任务", Value = "StudyPlan")]
        StudyPlan,
    }
}
