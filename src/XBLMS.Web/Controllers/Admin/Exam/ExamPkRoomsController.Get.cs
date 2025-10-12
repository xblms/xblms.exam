using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamPkRoomsController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> GetRoomList([FromQuery] GetRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Manage))
            {
                return this.NoAuth();
            }

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
