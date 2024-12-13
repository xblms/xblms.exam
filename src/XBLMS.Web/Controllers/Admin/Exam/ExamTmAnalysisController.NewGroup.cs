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

            var admin = await _authManager.GetAdminAsync();

            await _examTmGroupRepository.InsertAsync(new ExamTmGroup
            {
                GroupName = request.GroupName,
                GroupType = TmGroupType.Fixed,
                TmIds = request.TmIdList,
                TmTotal = request.TmIdList.Count,
                CreatorId = admin.Id,
                CompanyId = admin.CompanyId,
                DepartmentId = admin.DepartmentId,
            });

            return new BoolResult
            {
                Value = true
            };
        }

    }
}
