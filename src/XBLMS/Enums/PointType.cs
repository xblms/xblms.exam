using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PointType
    {
        [DataEnum(DisplayName = "登录系统", Value = "PointLogin")]
        PointLogin,
        [DataEnum(DisplayName = "完成学习任务", Value = "PointPlanOver")]
        PointPlanOver,
        [DataEnum(DisplayName = "观看视频", Value = "PointVideo")]
        PointVideo,
        [DataEnum(DisplayName = "预览文档", Value = "PointDocument")]
        PointDocument,
        [DataEnum(DisplayName = "完成课程", Value = "PointCourseOver")]
        PointCourseOver,
        [DataEnum(DisplayName = "参与评价", Value = "PointEvaluation")]
        PointEvaluation,
        [DataEnum(DisplayName = "参加考试", Value = "PointExam")]
        PointExam,
        [DataEnum(DisplayName = "考试及格", Value = "PointExamPass")]
        PointExamPass,
        [DataEnum(DisplayName = "考试满分", Value = "PointExamFull")]
        PointExamFull,
        [DataEnum(DisplayName = "参与问卷", Value = "PointExamQ")]
        PointExamQ,
        [DataEnum(DisplayName = "参与测评", Value = "PointExamAss")]
        PointExamAss,
        [DataEnum(DisplayName = "刷题练习", Value = "PointExamPractice")]
        PointExamPractice,
        [DataEnum(DisplayName = "答题正确", Value = "PointExamPracticeRight")]
        PointExamPracticeRight
    }
}
