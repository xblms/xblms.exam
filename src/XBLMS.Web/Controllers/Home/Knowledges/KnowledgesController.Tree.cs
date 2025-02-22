using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;

namespace XBLMS.Web.Controllers.Home.Knowledges
{
    public partial class KnowledgesController
    {
        [HttpGet, Route(RouteTree)]
        public async Task<ActionResult<GetTreeResult>> GetTree([FromQuery] IdRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) { return Unauthorized(); }

            var resultList = new List<GetTreeResultInfo>
            {
                new() { Id = 0, Name = "全部" }
            };
            var total = 0;

            var childList = await _knowlegesTreeRepository.GetChildAsync(request.Id);
            if (childList != null && childList.Count > 0)
            {
                foreach (var item in childList)
                {
                    resultList.Add(new GetTreeResultInfo
                    {
                        Id = item.Id,
                        Name = item.Name,
                    });
                    total++;
                }
            }
            return new GetTreeResult
            {
                Total = total,
                List = resultList
            };
        }
    }
}
