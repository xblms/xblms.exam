using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StudyStatType
    {
        [DataEnum(DisplayName = "未开始", Value = "Weikaishi")] Weikaishi,
        [DataEnum(DisplayName = "学习中", Value = "Xuexizhong")] Xuexizhong,
        [DataEnum(DisplayName = "已达标", Value = "Yidabiao")] Yidabiao,
        [DataEnum(DisplayName = "未达标", Value = "Weidabiao")] Weidabiao,
        [DataEnum(DisplayName = "已完成", Value = "Yiwancheng")] Yiwancheng,
        [DataEnum(DisplayName = "未完成", Value = "Weiwancheng")] Weiwancheng,
        [DataEnum(DisplayName = "已过期", Value = "Yiguoqi")] Yiguoqi,
    }
}
