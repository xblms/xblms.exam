using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Study
{
    public partial class StudyCourseEditController
    {
        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(Route)]
        public async Task<ActionResult<BoolResult>> Submit([FromBody] GetSubmitRequest request)
        {
            if (request.Item.Id > 0)
            {
                if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
                {
                    return this.NoAuth();
                }
            }
            else
            {
                if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Add))
                {
                    return this.NoAuth();
                }
            }

            var adminAuth = await _authManager.GetAdminAuth();
            var admin = adminAuth.Admin;

            var course = request.Item;

            if (PageUtils.IsProtocolUrl(course.CoverImg))
            {
                var config = await _configRepository.GetAsync();
                if (config.BushuFilesServer)
                {
                    course.CoverImg = StringUtils.ReplaceStartsWithIgnoreCase(course.CoverImg, config.BushuFilesServerUrl, string.Empty);
                }
            }

            var tree = await _studyCourseTreeRepository.GetAsync(course.TreeId);
            if (tree != null)
            {
                course.TreeParentPath = tree.ParentPath;
            }

            if (course.Id > 0)
            {
                await _studyCourseRepository.UpdateAsync(course);
                var courseWareIds = new List<int>();

                if (request.WareList != null && request.WareList.Count > 0)
                {
                    foreach (var ware in request.WareList)
                    {
                        if (ware.Id > 0)
                        {
                            await _studyCourseWareRepository.UpdateAsync(ware);
                        }
                        else
                        {
                            var file = await _studyCourseFilesRepository.GetAsync(ware.CourseFileId);
                            ware.Id = await _studyCourseWareRepository.InsertAsync(new StudyCourseWare
                            {
                                CourseId = course.Id,
                                CourseFileId = file.Id,
                                FileName = ware.FileName,
                                Duration = file.Duration,
                                Url = file.Url,
                                Taxis = ware.Taxis,
                                CompanyId = adminAuth.CurCompanyId,
                                DepartmentId = admin.DepartmentId,
                                CreatorId = admin.Id,
                                CompanyParentPath = adminAuth.CompanyParentPath,
                                DepartmentParentPath = admin.DepartmentParentPath,
                            });
                        }
                        courseWareIds.Add(ware.Id);
                    }
                }

                await _studyCourseWareRepository.DeleteByNotIdsAsync(courseWareIds, course.Id);
                await _studyCourseUserRepository.UpdateByCourseAsync(course);

                await _authManager.AddAdminLogAsync("修改课程", course.Name);
                await _authManager.AddStatLogAsync(StatType.StudyCourseUpdate, "修改课程", course.Id, course.Name);
            }
            else
            {
                course.CompanyId = adminAuth.CurCompanyId;
                course.DepartmentId = admin.DepartmentId;
                course.CreatorId = admin.Id;
                course.TotaEvaluationlUser = 0;
                course.TotalEvaluation = 0;
                course.TotalUser = 0;
                course.DepartmentParentPath = admin.DepartmentParentPath;
                course.CompanyParentPath = adminAuth.CompanyParentPath;

                var courseId = await _studyCourseRepository.InsertAsync(course);
                if (request.WareList != null && request.WareList.Count > 0)
                {
                    foreach (var ware in request.WareList)
                    {
                        var file = await _studyCourseFilesRepository.GetAsync(ware.CourseFileId);
                        await _studyCourseWareRepository.InsertAsync(new StudyCourseWare
                        {
                            CourseId = courseId,
                            CourseFileId = file.Id,
                            FileName = ware.FileName,
                            Duration = file.Duration,
                            Url = file.Url,
                            Taxis = ware.Taxis,
                            CompanyId = admin.CompanyId,
                            DepartmentId = admin.DepartmentId,
                            CreatorId = admin.Id,
                            CompanyParentPath = admin.CompanyParentPath,
                            DepartmentParentPath = admin.DepartmentParentPath,
                        });
                    }
                }

                await _authManager.AddAdminLogAsync("新增课程", course.Name);
                await _authManager.AddStatLogAsync(StatType.StudyCourseAdd, "新增课程", courseId, course.Name);
                await _authManager.AddStatCount(StatType.StudyCourseAdd);
            }

            return new BoolResult
            {
                Value = true
            };
        }
    }


}
