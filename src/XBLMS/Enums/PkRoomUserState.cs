using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PkRoomUserState
    {
        [DataEnum(DisplayName = "离线",Value = "OffLine")] OffLine,
        [DataEnum(DisplayName = "在线", Value = "Online")] OnLine,
        [DataEnum(DisplayName = "已准备", Value = "Ready")] Ready,
        [DataEnum(DisplayName = "答题中", Value = "Answer")] Answer,
        [DataEnum(DisplayName = "答题中", Value = "AnswerUpdate")] AnswerUpdate,
        [DataEnum(DisplayName = "确定答案", Value = "AnswerLock")] AnswerLock,
        [DataEnum(DisplayName = "胜", Value = "Win")] Win,
        [DataEnum(DisplayName = "负", Value = "Fail")] Fail,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PkRoomNoticeType
    {
        [DataEnum(DisplayName = "无", Value = "None")] None,
        [DataEnum(DisplayName = "准备答题", Value = "ReadyAnswer")] ReadyAnswer,
        [DataEnum(DisplayName = "结束", Value = "Finished")] Finished,
    }
}
