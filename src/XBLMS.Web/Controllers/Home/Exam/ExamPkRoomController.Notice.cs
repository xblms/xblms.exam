using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Dto;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPkRoomController
    {
        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(RouteNotice)]
        public async Task<ActionResult<BoolResult>> Notice([FromBody] PkRoomResult request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            if (user.PkRoomId > 0)
            {
                var cacheKeyPkRoom = CacheUtils.GetEntityKey("pkroom", user.PkRoomId);
                _cacheManager.AddOrUpdateAbsolute(cacheKeyPkRoom, request, 10);

                await _signalRHubManagerMessage.SendMsg(request);
            }
  
            return new BoolResult
            {
                Value = true
            };
        }

    }
}
