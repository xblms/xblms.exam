using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ExecutionStatusType
    {
        [DataEnum(DisplayName = "等待执行", Value = "Waiting")]
        Waiting,
        [DataEnum(DisplayName = "执行中", Value = "Executing")]
        Executing,
        [DataEnum(DisplayName = "成功", Value = "Success")]
        Success,
        [DataEnum(DisplayName = "完成", Value = "TaskSuccess")]
        TaskSuccess,
        [DataEnum(DisplayName = "失败", Value = "Error")]
        Error
    }
}
