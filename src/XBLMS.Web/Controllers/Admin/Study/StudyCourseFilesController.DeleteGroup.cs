using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Study
{
    public partial class StudyCourseFilesController
    {
        [HttpPost, Route(RouteActionsDeleteGroup)]
        public async Task<ActionResult<BoolResult>> DeleteGroup([FromBody] IdRequest request)
        {
            if (_settingsManager.IsSafeMode)
            {
                return this.Error(Constants.ErrorSafe);
            }

            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Delete))
            {
                return this.NoAuth();
            }

            var admin = await _authManager.GetAdminAsync();


            var group = await _studyCourseFilesGroupRepository.GetAsync(request.Id);
            if (group == null) return NotFound();

            await _studyCourseFilesGroupRepository.DeleteAsync(request.Id);
            await _authManager.AddAdminLogAsync("删除文件夹", group.GroupName);

            var path = _pathManager.GetCourseFilesUploadPath(admin.CompanyId.ToString());
            path = PathUtils.Combine(path, group.GroupName);
            DirectoryUtils.DeleteDirectoryIfExists(path);

            return new BoolResult
            {
                Value = true
            };
        }

        [HttpPost, Route(RouteActionsDeleteGroupAndFile)]
        public async Task<ActionResult<BoolResult>> DeleteGroupAndFile([FromBody] DeleteGroupAndFileRequest request)
        {
            var admin = await _authManager.GetAdminAsync();
   

            if (request.Files.Count > 0)
            {
                foreach(var file in request.Files)
                {
                    if (file.Type == "Group")
                    {
                        var group = await _studyCourseFilesGroupRepository.GetAsync(file.Id);
                        if (group != null)
                        {
                            await _studyCourseFilesGroupRepository.DeleteAsync(file.Id);
                            await _authManager.AddAdminLogAsync("删除文件夹", group.GroupName);

                            var path = _pathManager.GetCourseFilesUploadPath(admin.CompanyId.ToString());
                            path = PathUtils.Combine(path, group.GroupName);
                            DirectoryUtils.DeleteDirectoryIfExists(path);
                        }
               
                    }
                    else
                    {
                        var fileInfo = await _studyCourseFilesRepository.GetAsync(file.Id);
                        if (fileInfo != null)
                        {
                            await _studyCourseFilesRepository.DeleteAsync(fileInfo.Id);
                            var filePath = PathUtils.Combine(_settingsManager.WebRootPath, fileInfo.Url);
                            FileUtils.DeleteFileIfExists(filePath);
                            await _authManager.AddAdminLogAsync("删除文件", fileInfo.FileName);
                        }
                    }
                }
            }


            return new BoolResult
            {
                Value = true
            };
        }
    }
}
