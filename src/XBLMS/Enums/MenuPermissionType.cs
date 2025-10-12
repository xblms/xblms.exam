using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MenuPermissionType
    {
        [DataEnum(DisplayName = "查询",Value = "Select")] Select,
        [DataEnum(DisplayName = "添加", Value = "Add")] Add,
        [DataEnum(DisplayName = "修改", Value = "Update")] Update,
        [DataEnum(DisplayName = "删除", Value = "Delete")] Delete,
        [DataEnum(DisplayName = "管理", Value = "Manage")] Manage,

        [DataEnum(DisplayName = "导出", Value = "Export")] Export,
        [DataEnum(DisplayName = "导入", Value = "Import")] Import,

        [DataEnum(DisplayName = "清除缓存", Value = "SystemClearCache")] SystemClearCache,
        [DataEnum(DisplayName = "重启系统", Value = "SystemRestart")] SystemRestart,

        [DataEnum(DisplayName = "下载", Value = "Download")] Download,
    }
}
