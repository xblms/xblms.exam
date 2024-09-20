using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StatType
    {
        [DataEnum(DisplayName = "管理员登录成功")]
        AdminLoginSuccess,
        [DataEnum(DisplayName = "管理员登录失败")]
        AdminLoginFailure,
        [DataEnum(DisplayName = "用户登录")]
        UserLogin,
        [DataEnum(DisplayName = "添加题目")]
        ExamTmAdd,
        [DataEnum(DisplayName = "添加证书模板")]
        ExamCerAdd,
    }
}
