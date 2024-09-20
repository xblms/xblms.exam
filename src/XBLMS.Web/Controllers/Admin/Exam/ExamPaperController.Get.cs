using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamPaperController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> GetManage([FromQuery] GetRequest request)
        {

            var treeIds = new List<int>();
            if (request.TreeId > 0)
            {
                if (request.TreeIsChildren)
                {
                    treeIds = await _examPaperTreeRepository.GetIdsAsync(request.TreeId);
                }
                else
                {
                    treeIds.Add(request.TreeId);
                }
            }
            var (total, list) = await _examPaperRepository.GetListAsync(treeIds,request.Keyword, request.PageIndex, request.PageSize);
 
            return new GetResult
            {
                Total = total,
                Items = list,
            };
        }

    }
}
