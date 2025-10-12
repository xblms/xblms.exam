using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Study
{
    public partial class StudyPlanEditController
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

            var plan = request.Item;

            if (plan.Id > 0)
            {
                var oldPlan = await _studyPlanRepository.GetAsync(plan.Id);

                await _studyPlanRepository.UpdateAsync(plan);
                var planCourseIds = new List<int>();

                if (request.CourseList != null && request.CourseList.Count > 0)
                {
                    foreach (var item in request.CourseList)
                    {

                        if (item.Id > 0)
                        {
                            await _studyPlanCourseRepository.UpdateAsync(item);
                        }
                        else
                        {
                            item.CompanyId = adminAuth.CurCompanyId;
                            item.DepartmentId = admin.DepartmentId;
                            item.CreatorId = admin.Id;
                            item.CompanyParentPath = adminAuth.CompanyParentPath;
                            item.DepartmentParentPath = admin.DepartmentParentPath;
                            item.PlanId = plan.Id;

                            item.Id = await _studyPlanCourseRepository.InsertAsync(item);
                        }
                        planCourseIds.Add(item.Id);
                    }
                }

                if (request.CourseSelectList != null && request.CourseSelectList.Count > 0)
                {
                    foreach (var item in request.CourseSelectList)
                    {
                        if (item.Id > 0)
                        {
                            await _studyPlanCourseRepository.UpdateAsync(item);
                        }
                        else
                        {
                            item.CompanyId = admin.CompanyId;
                            item.DepartmentId = admin.DepartmentId;
                            item.CreatorId = admin.Id;
                            item.CompanyParentPath = admin.CompanyParentPath;
                            item.DepartmentParentPath = admin.DepartmentParentPath;
                            item.PlanId = plan.Id;

                            item.Id = await _studyPlanCourseRepository.InsertAsync(item);
                        }
                        planCourseIds.Add(item.Id);
                    }
                }
                await _studyPlanCourseRepository.DeleteByNotIdsAsync(planCourseIds, plan.Id);

                await _studyPlanUserRepository.UpdateByPlanAsync(plan);

                await _authManager.AddAdminLogAsync("修改学习任务", plan.PlanName);
                await _authManager.AddStatLogAsync(StatType.StudyPlanUpdate, "修改学习任务", plan.Id, plan.PlanName);
            }
            else
            {
                plan.CompanyId = adminAuth.CurCompanyId;
                plan.DepartmentId = admin.DepartmentId;
                plan.CreatorId = admin.Id;
                plan.CompanyParentPath = adminAuth.CompanyParentPath;
                plan.DepartmentParentPath = admin.DepartmentParentPath;

                var planId = await _studyPlanRepository.InsertAsync(plan);
                plan.Id = planId;

                if (request.CourseList != null && request.CourseList.Count > 0)
                {
                    foreach (var item in request.CourseList)
                    {
                        item.CompanyId = admin.CompanyId;
                        item.DepartmentId = admin.DepartmentId;
                        item.CreatorId = admin.Id;
                        item.CompanyParentPath = admin.CompanyParentPath;
                        item.DepartmentParentPath = admin.DepartmentParentPath;
                        item.PlanId = planId;

                        await _studyPlanCourseRepository.InsertAsync(item);
                    }
                }


                if (request.CourseSelectList != null && request.CourseSelectList.Count > 0)
                {
                    foreach (var item in request.CourseSelectList)
                    {
                        item.CompanyId = admin.CompanyId;
                        item.DepartmentId = admin.DepartmentId;
                        item.CreatorId = admin.Id;
                        item.CompanyParentPath = admin.CompanyParentPath;
                        item.DepartmentParentPath = admin.DepartmentParentPath;
                        item.PlanId = planId;

                        await _studyPlanCourseRepository.InsertAsync(item);
                    }
                }

                await _authManager.AddAdminLogAsync("新增学习任务", plan.PlanName);
                await _authManager.AddStatLogAsync(StatType.StudyPlanAdd, "新增学习任务", plan.Id, plan.PlanName);
                await _authManager.AddStatCount(StatType.StudyPlanAdd);
            }
            if (plan.SubmitType == SubmitType.Submit)
            {
                await _studyManager.PlanArrangeUser(plan, adminAuth);
            }

            return new BoolResult
            {
                Value = true
            };
        }
    }


}
