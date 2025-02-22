using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;

namespace XBLMS.Web.Controllers.Home.Knowledges
{
    public partial class KnowledgesViewController
    {
        [HttpPost, Route(RouteLike)]
        public async Task<ActionResult<BoolResult>> Like([FromBody] IdRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) { return Unauthorized(); }

            var konwledge = await _knowlegesRepository.GetAsync(request.Id);
            var isLike = false;
            if (konwledge.LikeUserIds != null && konwledge.LikeUserIds.Contains($"'{user.Id}'"))
            {
                isLike = true;
            }
            if (isLike)
            {
                konwledge.Likes--;
                konwledge.LikeUserIds.Remove($"'{user.Id}'");
                await _knowlegesRepository.UpdateAsync(konwledge);
            }
            else
            {
                konwledge.Likes++;
                if (konwledge.LikeUserIds != null)
                {
                    konwledge.LikeUserIds.Add($"'{user.Id}'");
                }
                else
                {
                    konwledge.LikeUserIds = [$"'{user.Id}'"];
                }
                await _knowlegesRepository.UpdateAsync(konwledge);
            }
            return new BoolResult { Value = true };
        }
    }
}
