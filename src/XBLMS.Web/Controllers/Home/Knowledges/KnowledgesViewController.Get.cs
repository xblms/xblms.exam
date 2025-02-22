using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;

namespace XBLMS.Web.Controllers.Home.Knowledges
{
    public partial class KnowledgesViewController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] IdRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) { return Unauthorized(); }

            var konwledge = await _knowlegesRepository.GetAsync(request.Id);
            var isLike = false;
            if (konwledge.LikeUserIds!=null && konwledge.LikeUserIds.Contains($"'{user.Id}'"))
            {
                isLike = true;
            }
            var isCollect = false;
            if (konwledge.CollectUserIds != null && konwledge.CollectUserIds.Contains($"'{user.Id}'"))
            {
                isCollect = true;
            }

            return new GetResult
            {
                Name = konwledge.Name,
                IsLike = isLike,
                IsCollect = isCollect,
                Likes = konwledge.Likes,
                Collects = konwledge.Collects,
                Url = konwledge.Url
            };
        }
    }
}
