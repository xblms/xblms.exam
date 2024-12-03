using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ExamAssessmentTxType
    {
        /// <summary>
        /// 单选题
        /// </summary>
        [DataEnum(DisplayName = "单选题", Value = "Danxuanti")]
        Danxuanti,
        /// <summary>
        /// 多选题
        /// </summary>
        [DataEnum(DisplayName = "多选题", Value = "Duoxuanti")]
        Duoxuanti,
    }
}
