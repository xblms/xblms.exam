using Datory;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPkRoomController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            var room = await _examPkRoomRepository.GetAsync(request.Id);
            var tmList = new List<ExamTm>();

            var cacheRoom = new PkRoomResult();

            if (_settingsManager.IsSafeMode)
            {
                var pk = await _examPkRepository.GetAsync(room.PkId);

                var userA = await _userRepository.GetByUserIdAsync(room.UserId_A);
                var userB = await _userRepository.GetByUserIdAsync(room.UserId_B);

                cacheRoom = new PkRoomResult
                {
                    RoomId = room.Id,
                    RoomName = pk.Mark,

                    AUserId = userA.Id,
                    AUserDisplayName = userA.DisplayName,
                    AUserAvatarUrl = userA.AvatarbgUrl,
                    AUserState = PkRoomUserState.OffLine,
                    AUserStateStr = PkRoomUserState.OffLine.GetDisplayName(),

                    BUserId = userB.Id,
                    BUserDisplayName = userB.DisplayName,
                    BUserAvatarUrl = userB.AvatarbgUrl,
                    BUserState = PkRoomUserState.OffLine,
                    BUserStateStr = PkRoomUserState.OffLine.GetDisplayName(),

                    AnswerTmIds = new List<int>(),
                    AAnswers = new List<string>(),
                    BAnswers = new List<string>(),


                };
            }
            else
            {
                if (room.TmIds != null && room.TmIds.Count > 0)
                {
                    foreach (var tmId in room.TmIds)
                    {
                        var tm = await _examManager.GetTmInfo(tmId);
                        await _examManager.GetTmInfoByPracticing(tm);
                        tmList.Add(tm);
                    }
                }

                user.PkRoomId = request.Id;

                await _userRepository.UpdateByPkRoomAsync(user);
            }


            return new GetResult
            {
                IsSafeMode = _settingsManager.IsSafeMode,
                CacheRoom = cacheRoom,
                UserId = user.Id,
                TmList = tmList
            };
        }

    }
}
