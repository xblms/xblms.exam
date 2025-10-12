using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTmSelectController
    {
        [RequestSizeLimit(long.MaxValue)]
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetSearchResults>> Get([FromQuery] GetSearchRequest request)
        {
            var adminAuth = await _authManager.GetAdminAuth();
            var treeIds = new List<int>();
            if (request.TreeId > 0)
            {
                if (request.TreeIsChildren)
                {
                    treeIds = await _examTmTreeRepository.GetIdsAsync(request.TreeId);
                }
                else
                {
                    treeIds.Add(request.TreeId);
                }
            }

            var (total, list) = await _examTmRepository.GetSelectListAsync(adminAuth, request.Id, false, treeIds, request.TxId, request.Nandu, request.Keyword, request.Order, request.OrderType, request.PageIndex, request.PageSize);
            if (total > 0)
            {
                foreach (var tm in list)
                {
                    await _examManager.GetTmInfo(tm);
                }
            }

            return new GetSearchResults
            {
                Items = list,
                Total = total,
            };
        }
        [RequestSizeLimit(long.MaxValue)]
        [HttpGet, Route(RouteGetIn)]
        public async Task<ActionResult<GetSearchResults>> GetSelect([FromQuery] GetSearchRequest request)
        {
            var adminAuth = await _authManager.GetAdminAuth();
            var group = await _examTmGroupRepository.GetAsync(request.Id);

            var (total, list) = await _examTmRepository.GetSelectListAsync(adminAuth, request.Id, true, null, 0, 0, string.Empty, request.Order, string.Empty, request.PageIndex, request.PageSize);
            if (total > 0)
            {
                foreach (var tm in list)
                {
                    await _examManager.GetTmInfo(tm);
                }
            }

            return new GetSearchResults
            {
                Items = list,
                Total = total,
            };
        }
    }
}
