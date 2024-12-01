using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using XBLMS.Core.Utils;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPkRoomsController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var pk = await _examPkRepository.GetAsync(request.Id);

            var list = await _examPkRepository.GetChildList(request.Id);
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    item.Set("PkBeginDateTimeStr", item.PkBeginDateTime.Value.ToString(DateUtils.FormatStringDateTimeCN));
                    item.Set("PkEndDateTimeStr", item.PkEndDateTime.Value.ToString(DateUtils.FormatStringDateTimeCN));


                    var (roomTotal, roomList) = await _examPkRoomRepository.GetListAsync(item.Id);

                    if (roomTotal > 0)
                    {
                        foreach (var room in roomList)
                        {
                            var inRoom = true;
                            var inRoomMsg = "";

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

                    item.Set("RoomList", roomList);
                    item.Set("RoomTotal", roomTotal);

                }
            }
            return new GetResult
            {
                Title = pk.Name,
                List = list
            };
        }
    }
}
