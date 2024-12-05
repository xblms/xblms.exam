using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;
namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamPkController
    {
        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<BoolResult>> Delete([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Delete))
            {
                return this.NoAuth();
            }

            var pk = await _examPkRepository.GetAsync(request.Id);
            if (pk != null)
            {
                var childList = await _examPkRepository.GetChildList(pk.Id);
                if (childList != null && childList.Count > 0)
                {
                    foreach (var child in childList)
                    {
                        var (roomTotal, roomList) = await _examPkRoomRepository.GetListAsync(child.Id);
                        if (roomTotal > 0)
                        {
                            foreach (var room in roomList)
                            {
                                await _examPkRoomRepository.DeleteAsync(room.Id);
                                await _examPkRoomAnswerRepository.DeleteByRoomIdAsync(room.Id);
                            }
                        }
                    }
                }
                await _examPkRepository.DeleteAsync(pk.Id);
                await _examPkUserRepository.DeleteByPkIdAsync(pk.Id);

                await _authManager.AddAdminLogAsync("删除竞赛", $"{pk.Name}");
                await _authManager.AddStatLogAsync(StatType.ExamPkDelete, "删除竞赛", pk.Id, pk.Name, pk);
                await _authManager.AddStatCount(StatType.ExamPkDelete);

            }
            return new BoolResult
            {
                Value = true
            };
        }

    }
}
