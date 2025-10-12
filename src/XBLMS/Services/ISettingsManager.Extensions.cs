using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Enums;

namespace XBLMS.Services
{
    public partial interface ISettingsManager
    {
        string GetPermissionId(string menuId, string menuPermissionType);
        List<Menu> GetMenus(SystemCode systemCode, AuthorityType auth = AuthorityType.Admin, bool isRole = false, List<string> menuIds = null);
    }
}
