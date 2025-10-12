using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;

namespace XBLMS.Web.Controllers.Admin.Knowledges
{
    public partial class KnowledgesController
    {
        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(RouteSubmit)]
        public async Task<ActionResult<BoolResult>> Submit([FromBody] GetSubmitRequest request)
        {
            var adminAuth = await _authManager.GetAdminAuth();
            var admin = adminAuth.Admin;

            if (request != null && request.List != null && request.List.Count > 0)
            {
                var tree = await _knowlegesTreeRepository.GetAsync(request.TreeId);

                foreach (var item in request.List)
                {
                    item.CreatorId = admin.Id;
                    item.CompanyId = adminAuth.CurCompanyId;
                    item.DepartmentId = admin.DepartmentId;
                    item.CompanyParentPath = adminAuth.CompanyParentPath;
                    item.DepartmentParentPath = admin.DepartmentParentPath;

                    item.TreeId = tree.Id;
                    item.TreeParentPath = tree.ParentPath;

                    await _knowlegesRepository.InsertAsync(item);

                    await _authManager.AddAdminLogAsync("新增知识库", item.Name);
                    await _authManager.AddStatLogAsync(StatType.KnowledgesAdd, "新增知识库", item.Id, item.Name);
                    await _authManager.AddStatCount(StatType.KnowledgesAdd);
                }
            }

            return new BoolResult { Value = true };
        }
    }
}
