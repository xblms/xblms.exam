using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;

namespace XBLMS.Web.Controllers.Home.Knowledges
{
    public partial class KnowledgesViewController
    {
        [HttpPost, Route(RouteCollect)]
        public async Task<ActionResult<BoolResult>> Collect([FromBody] IdRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) { return Unauthorized(); }

            var konwledge = await _knowlegesRepository.GetAsync(request.Id);

            var isCollect = false;
            if (konwledge.CollectUserIds != null && konwledge.CollectUserIds.Contains($"'{user.Id}'"))
            {
                isCollect = true;
            }
            if (isCollect)
            {
                konwledge.Collects--;
                konwledge.CollectUserIds.Remove($"'{user.Id}'");
                await _knowlegesRepository.UpdateAsync(konwledge);
            }
            else
            {
                konwledge.Collects++;
                if (konwledge.CollectUserIds != null)
                {
                    konwledge.CollectUserIds.Add($"'{user.Id}'");
                }
                else
                {
                    konwledge.CollectUserIds = [$"'{user.Id}'"];
                }
                await _knowlegesRepository.UpdateAsync(konwledge);
            }
            return new BoolResult { Value = true };
        }
    }
}
