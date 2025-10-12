using Datory;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTmGroupController
    {
        [HttpGet, Route(RouteEditGet)]
        public async Task<ActionResult<GetEditResult>> GetEdit([FromQuery] IdRequest request)
        {
            var adminAuth = await _authManager.GetAdminAuth();

            var group = new ExamTmGroup
            {
                GroupType = TmGroupType.All,
                Locked = false
            };

            var groupTypeSelects = ListUtils.GetSelects<TmGroupType>();
            if (adminAuth.AuthType != AuthorityType.Admin)
            {
                group.GroupType = TmGroupType.Range;
                groupTypeSelects = groupTypeSelects.Where(g => g.Value != TmGroupType.All.GetValue()).ToList();
            }

            if (request.Id > 0)
            {
                group = await _examTmGroupRepository.GetAsync(request.Id);
            }
            var tmTree = await _examManager.GetExamTmTreeCascadesAsync(adminAuth);

            var txList = await _examTxRepository.GetListAsync();

            var userGroups = await _userGroupRepository.GetListAsync(adminAuth, true);

            return new GetEditResult
            {
                Group = group,
                GroupTypeSelects = groupTypeSelects,
                TmTree = tmTree,
                TxList = txList,
                UserGroups = userGroups
            };
        }


        [HttpPost, Route(RouteEditPost)]
        public async Task<ActionResult<BoolResult>> PostEdit([FromBody] GetEditRequest request)
        {

            if (request.Group.Id > 0)
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

            var exists = await _examTmGroupRepository.ExistsAsync(request.Group.GroupName, admin.CompanyId);

            if (request.Group.Id > 0)
            {
                var group = await _examTmGroupRepository.GetAsync(request.Group.Id);

                if (exists && !StringUtils.Equals(group.GroupName, request.Group.GroupName))
                {
                    return this.Error("已存在相同名称的题目组");
                }

                await _examTmGroupRepository.UpdateAsync(request.Group);
                await _authManager.AddAdminLogAsync("修改题目组", $"{group.GroupName}");
                await _authManager.AddStatLogAsync(StatType.ExamTmGroupUpdate, "修改题目组", group.Id, group.GroupName, group);
            }
            else
            {
                if (exists)
                {
                    return this.Error("已存在相同名称的题目组");
                }

                request.Group.CreatorId = admin.Id;
                request.Group.CompanyId = adminAuth.CurCompanyId;
                request.Group.DepartmentId = admin.DepartmentId;
                request.Group.CompanyParentPath = adminAuth.CompanyParentPath;
                request.Group.DepartmentParentPath = admin.DepartmentParentPath;

                var groupId = await _examTmGroupRepository.InsertAsync(request.Group);
                await _authManager.AddAdminLogAsync("新增题目组", $"{request.Group.GroupName}");
                await _authManager.AddStatLogAsync(StatType.ExamTmGroupAdd, "新增题目组", groupId, request.Group.GroupName);
                await _authManager.AddStatCount(StatType.ExamTmGroupAdd);
            }

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
