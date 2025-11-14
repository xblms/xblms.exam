using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ExamTmCorrectionAuditType
    {
        /// <summary>
        /// 待审核
        /// </summary>
        [DataEnum(DisplayName = "待审核", Value = "Info")] Info,
        /// <summary>
        /// 审核通过
        /// </summary>
        [DataEnum(DisplayName = "审核通过", Value = "Success")] Success,
        /// <summary>
        /// 审核未通过
        /// </summary>
        [DataEnum(DisplayName = "审核未通过", Value = "Danger")] Danger
    }
}
