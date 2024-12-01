using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using XBLMS.Core.Utils;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPkController
    {
        [HttpGet, Route(RouteMyRoom)]
        public async Task<ActionResult<GetMyRoomResult>> GetRoom([FromQuery] GetMyRoomRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            var (roomTotal, roomList) = await _examPkRoomRepository.GetUserListAsync(user.Id, request.PageIndex, request.PageSize);

            if (roomTotal > 0)
            {
                foreach (var room in roomList)
                {
                    var pk = await _examPkRepository.GetAsync(room.PkId);

                    var inRoom = true;
                    var inRoomMsg = "";

                    if (pk != null)
                    {
                        room.Set("Mark", pk.Mark);
                        room.Set("BeginDateTimeStr", pk.PkBeginDateTime.Value.ToString(DateUtils.FormatStringDateTimeCN));
                    }
                    else
                    {
                        room.Set("Mark", "error");
                        room.Set("BeginDateTimeStr", "error");
                    }

                    if (room.UserId_A > 0)
                    {
                        var usera = await _organManager.GetUser(room.UserId_A);
                        room.Set("UserA", usera);
                    }
                    if (room.UserId_B > 0)
                    {
                        var userb = await _organManager.GetUser(room.UserId_B);
                        room.Set("UserB", userb);
                    }

                    if (room.UserId_A == 0 || room.UserId_B == 0)
                    {
                        inRoomMsg = "还没有准备好";
                        inRoom = false;
                    }
                    if (!room.Finished && (pk.PkBeginDateTime > DateTime.Now || pk.PkEndDateTime < DateTime.Now))
                    {
                        inRoomMsg = "不在有限期内";
                        inRoom = false;
                    }


                    room.Set("InRoom", inRoom);
                    room.Set("InRoomMsg", inRoomMsg);

                }
            }

            return new GetMyRoomResult
            {
                List = roomList,
                Total = roomTotal,
            };
        }
    }
}
