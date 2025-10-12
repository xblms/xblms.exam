using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTmAnalysisController
    {
        [HttpPost, Route(RouteNewGroup)]
        public async Task<ActionResult<BoolResult>> NewGroup([FromBody] GetNewGroupRequest request)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }

            var adminAuth = await _authManager.GetAdminAuth();
            var admin = adminAuth.Admin;

            var groupId = await _examTmGroupRepository.InsertAsync(new ExamTmGroup
            {
                GroupName = request.GroupName,
                GroupType = TmGroupType.Fixed,
                TmTotal = request.TmIdList.Count,
                CreatorId = admin.Id,
                CompanyId = adminAuth.CurCompanyId,
                DepartmentId = admin.DepartmentId,
                CompanyParentPath = adminAuth.CompanyParentPath,
                DepartmentParentPath = admin.DepartmentParentPath,
            });

            foreach (var tmId in request.TmIdList)
            {
                var tm = await _examTmRepository.GetAsync(tmId);
                if (tm.TmGroupIds != null)
                {
                    tm.TmGroupIds.Add($"'{groupId}'");
                }
                else
                {
                    tm.TmGroupIds = [$"'{groupId}'"];
                }
                await _examTmRepository.UpdateTmGroupIdsAsync(tm);
            }

            return new BoolResult
            {
                Value = true
            };
        }

    }
}
