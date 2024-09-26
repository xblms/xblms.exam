using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ExamTxBase
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
        /// <summary>
        /// 判断题
        /// </summary>
        [DataEnum(DisplayName = "判断题", Value = "Panduanti")]
        Panduanti,
        /// <summary>
        /// 填空题
        /// </summary>
        [DataEnum(DisplayName = "填空题", Value = "Tiankongti")]
        Tiankongti,
        /// <summary>
        /// 简答题
        /// </summary>
        [DataEnum(DisplayName = "简答题", Value = "Jiandati")]
        Jiandati
    }
    public enum ExamTmType
    {
        [DataEnum(DisplayName = "客观题", Value = "Objective")]
        Objective,
        [DataEnum(DisplayName = "主观题", Value = "Subjective")]
        Subjective,
    }
}
