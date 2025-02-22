using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace XBLMS.Web.Controllers.Home.Knowledges
{
    public partial class KnowledgesController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) { return Unauthorized(); }


            var (total, list) = await _knowlegesRepository.GetListAsync(user.Id, request.TreeId, true, request.Like, request.Collect, request.Keywords, request.PageIndex, request.PageSize);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    item.Url = "";
                }
            }
            return new GetResult
            {
                Total = total,
                List = list
            };
        }
    }
}
