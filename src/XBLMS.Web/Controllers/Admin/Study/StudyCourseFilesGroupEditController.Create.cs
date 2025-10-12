using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Study
{
    public partial class StudyCourseFilesGroupEditController
    {
        [HttpPost, Route(RouteAdd)]
        public async Task<ActionResult<BoolResult>> Create([FromBody] CreateRequest request)
        {
            if (_settingsManager.IsSafeMode)
            {
                return this.Error(Constants.ErrorSafe);
            }

            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Add))
            {
                return this.NoAuth();
            }

            var adminAuth = await _authManager.GetAdminAuth();
            var admin = adminAuth.Admin;

            var names = ListUtils.GetStringList(request.GroupName, "\n");
            foreach (var item in names)
            {
                if (DirectoryUtils.IsDirectoryNameCompliant(item) && !await _studyCourseFilesGroupRepository.IsExistsAsync(request.ParentId, item))
                {
                    await _studyCourseFilesGroupRepository.InsertAsync(new StudyCourseFilesGroup
                    {
                        GroupName = item,
                        ParentId = request.ParentId,
                        CompanyId = adminAuth.CurCompanyId,
                        DepartmentId = admin.DepartmentId,
                        CreatorId = admin.Id,
                        CompanyParentPath = adminAuth.CompanyParentPath,
                        DepartmentParentPath = admin.DepartmentParentPath,
                    });

                    var path = _pathManager.GetCourseFilesUploadPath(adminAuth.CurCompanyId.ToString());
                    if (request.ParentId > 0)
                    {
                        var group = await _studyCourseFilesGroupRepository.GetAsync(request.ParentId);
                        path = PathUtils.Combine(path, group.GroupName);
                    }

                    path = PathUtils.Combine(path, item);
                    DirectoryUtils.CreateDirectoryIfNotExists(path);

                    await _authManager.AddAdminLogAsync("新建文件夹", path);
                }
            }



            return new BoolResult
            {
                Value = true
            };
        }
    }
}
