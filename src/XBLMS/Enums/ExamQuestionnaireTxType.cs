using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ExamQuestionnaireTxType
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
        /// 简答题
        /// </summary>
        [DataEnum(DisplayName = "简答题", Value = "Jiandati")]
        Jiandati,
        /// <summary>
        /// 二维单选题
        /// </summary>
        [DataEnum(DisplayName = "二维单选题", Value = "DanxuantiErwei")]
        DanxuantiErwei,
        /// <summary>
        /// 二维多选题
        /// </summary>
        [DataEnum(DisplayName = "二维多选题", Value = "DuoxuantiErwei")]
        DuoxuantiErwei,
        /// <summary>
        /// 三维单选题
        /// </summary>
        [DataEnum(DisplayName = "三维单选题", Value = "DanxuantiSanwei")]
        DanxuantiSanwei,
        /// <summary>
        /// 三维多选题
        /// </summary>
        [DataEnum(DisplayName = "三维多选题", Value = "DuoxuantiSanwei")]
        DuoxuantiSanwei
    }
}
