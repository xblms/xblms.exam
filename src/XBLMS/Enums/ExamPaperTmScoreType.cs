using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ExamPaperTmScoreType
    {
        [DataEnum(DisplayName = "原题分数", Value = "ScoreTypeTm")] ScoreTypeTm,
        [DataEnum(DisplayName = "题型分数", Value = "ScoreTypeTx")] ScoreTypeTx,
        [DataEnum(DisplayName = "折算分数", Value = "ScoreTypeRate")] ScoreTypeRate
    }
}
