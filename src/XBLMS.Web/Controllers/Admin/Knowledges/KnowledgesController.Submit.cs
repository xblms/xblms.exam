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
            var admin = await _authManager.GetAdminAsync();

            if (request != null && request.List != null && request.List.Count > 0)
            {
                var tree = await _knowlegesTreeRepository.GetAsync(request.TreeId);

                foreach (var item in request.List)
                {
                    item.CreatorId = admin.Id;
                    item.CompanyId = admin.CompanyId;
                    item.DepartmentId = admin.DepartmentId;

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
