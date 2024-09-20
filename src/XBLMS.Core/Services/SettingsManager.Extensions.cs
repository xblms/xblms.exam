using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using XBLMS.Configuration;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public partial class SettingsManager
    {

        public List<Menu> GetMenus(bool isAuth = false, List<string> menuIds = null)
        {
            var section = Configuration.GetSection("extensions:menus");
            return GetMenus(section, isAuth, menuIds);
        }
        public string GetPermissionId(string menuId,string menuPermissionType)
        {
            return $"{menuId}_{menuPermissionType}"; 
        }
        private List<Menu> GetMenus(IConfigurationSection section, bool isAuth = false, List<string> menuIds = null)
        {
            var menus = new List<Menu>();
            if (section.Exists())
            {
                var children = section.GetChildren();
                if (children != null)
                {
                    foreach (var child in children)
                    {
                        var menu = child.Get<Menu>();
                        var childSection = child.GetSection("children");

                        var permissions = menu.Permissions;
                        if (permissions != null && permissions.Count > 0 && isAuth)
                        {
                            var childMenus = new List<Menu>();

                            var selectPermissions = ListUtils.GetSelects<MenuPermissionType>();
                            selectPermissions = selectPermissions.Where(p => permissions.Contains(p.Value)).ToList();
                            if (selectPermissions != null && selectPermissions.Count > 0)
                            {
                                foreach (var item in selectPermissions)
                                {
                                    var permissionId = GetPermissionId(menu.Id, item.Value);
                                    childMenus.Add(new Menu
                                    {
                                        Id = permissionId,
                                        Text = item.Label,
                                        IsPermission = true
                                    });
                                }
                            }

                            menus.Add(new Menu
                            {
                                Id = menu.Id,
                                Text = menu.Text,
                                IconClass = menu.IconClass,
                                Link = menu.Link,
                                Permissions = menu.Permissions,
                                Order = menu.Order,
                                Target = menu.Target,
                                Children = childMenus
                            });

                        }
                        else
                        {
                            if (menuIds == null)
                            {
                                menus.Add(new Menu
                                {
                                    Id = menu.Id,
                                    Text = menu.Text,
                                    IconClass = menu.IconClass,
                                    Link = menu.Link,
                                    Permissions = menu.Permissions,
                                    Order = menu.Order,
                                    Target = menu.Target,
                                    Children = GetMenus(childSection, isAuth)
                                });
                            }
                            else
                            {
                                if (menuIds.Contains(menu.Id)) {
                                    menus.Add(new Menu
                                    {
                                        Id = menu.Id,
                                        Text = menu.Text,
                                        IconClass = menu.IconClass,
                                        Link = menu.Link,
                                        Permissions = menu.Permissions,
                                        Order = menu.Order,
                                        Target = menu.Target,
                                        Children = GetMenus(childSection, isAuth, menuIds)
                                    });
                                }

                            }
                        }

                    }
                }
            }

            return menus.OrderByDescending(x => x.Order.HasValue).ThenBy(x => x.Order).ToList();
        }
    }
}
