using Datory;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using XBLMS.Configuration;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public partial class SettingsManager
    {
        public List<Menu> GetMenus(SystemCode systemCode, AuthorityType auth = AuthorityType.Admin, bool isRole = false, List<string> menuIds = null)
        {
            var section = Configuration.GetSection("extensions:menus");
            return GetMenus(systemCode, section, auth, isRole, menuIds);
        }
        public string GetPermissionId(string menuId, string menuPermissionType)
        {
            return $"{menuId}_{menuPermissionType}";
        }
        private List<Menu> GetMenus(SystemCode systemCode, IConfigurationSection section, AuthorityType auth = AuthorityType.Admin, bool isRole = false, List<string> menuIds = null)
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

                        if (systemCode == SystemCode.Exam)
                        {
                            if (menu.SystemCode != null && menu.SystemCode.Count > 0 && !menu.SystemCode.Contains(systemCode.GetValue()))
                            {
                                continue;
                            }
                        }
                        if (systemCode == SystemCode.Elearning)
                        {
                            if (menu.SystemCode != null && menu.SystemCode.Count > 0 && !menu.SystemCode.Contains(systemCode.GetValue()))
                            {
                                continue;
                            }
                        }

                        var childSection = child.GetSection("children");
                        var permissions = menu.Permissions;

                        if (auth == AuthorityType.AdminCompany && !menu.IsPermission) continue;

                        if (isRole && !menu.IsPermission) continue;

                        if (permissions != null && permissions.Count > 0 && isRole)
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
                            if (menuIds == null || (menuIds != null && menuIds.Count > 0 && menuIds.Contains(menu.Id)))
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
                                    Children = childMenus
                                });
                            }

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
                                    Children = GetMenus(systemCode, childSection, auth, isRole)
                                });
                            }
                            else
                            {
                                if (menuIds.Contains(menu.Id))
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
                                        Children = GetMenus(systemCode, childSection, auth, isRole, menuIds)
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
