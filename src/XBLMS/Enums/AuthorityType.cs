using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AuthorityType
    {
        [DataEnum(DisplayName = "超级管理员",Value = "Admin")] Admin,
        [DataEnum(DisplayName = "单位管理员", Value = "AdminCompany")] AdminCompany,
        [DataEnum(DisplayName = "普通管理员", Value = "AdminNormal")] AdminNormal,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum AuthorityDataType
    {
        [DataEnum(DisplayName = "管理权限内所有数据", Value = "DataAll")] DataAll,
        [DataEnum(DisplayName = "管理权限内自己创建的数据", Value = "DataCreator")] DataCreator,
    }
}
