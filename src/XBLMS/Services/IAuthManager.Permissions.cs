﻿using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;

namespace XBLMS.Services
{
    public partial interface IAuthManager
    {
        Task<bool> HasPermissionsAsync(MenuPermissionType menuPermissionType = MenuPermissionType.Select);
        Task<AdminAuth> GetAdminAuth();
        List<Select<string>> AuthorityDataTypes();
        List<Select<string>> AuthorityTypes();
    }
}
