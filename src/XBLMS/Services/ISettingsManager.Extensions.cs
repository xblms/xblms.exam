using System.Collections.Generic;
using XBLMS.Configuration;

namespace XBLMS.Services
{
    public partial interface ISettingsManager
    {
        string GetPermissionId(string menuId, string menuPermissionType);
        List<Menu> GetMenus(bool isAuth = false, List<string> menuIds = null);
    }
}
