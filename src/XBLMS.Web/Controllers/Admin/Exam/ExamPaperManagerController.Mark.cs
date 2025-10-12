using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Dto;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamPaperManagerController
    {
        [HttpGet, Route(RouteMark)]
        public async Task<ActionResult<GetScoreResult>> GetMarkList([FromQuery] GetSocreRequest request)
        {
            var (total, list) = await _examPaperStartRepository.GetListByAdminAsync(request.Id, 0, 0, request.DateFrom, request.DateTo, request.Keywords, request.PageIndex, request.PageSize, false);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    var user = await _organManager.GetUser(item.UserId);

                    user.Set("UseTime", DateUtils.SecondToHms(item.ExamTimeSeconds));

                    item.Set("User", user);

                    if (item.MarkTeacherId > 0)
                    {
                        var marker = await _administratorRepository.GetByUserIdAsync(item.MarkTeacherId);
                        item.Set("Marker", marker.DisplayName);
                    }

                }
            }
            var markers = new List<GetSelectMarkInfo>();
            var adminList = await _administratorRepository.GetListAsync();
            if (adminList != null && adminList.Count > 0)
            {
                foreach (var admin in adminList)
                {
                    markers.Add(new GetSelectMarkInfo
                    {
                        Id = admin.Id,
                        DisplayName = admin.DisplayName,
                        UserName = admin.UserName
                    });
                }
            }
            return new GetScoreResult
            {
                MarkerList = markers,
                Total = total,
                List = list
            };
        }



        [HttpPost, Route(RouteMarkSetMarker)]
        public async Task<ActionResult<BoolResult>> SetMarker([FromBody] GetSetMarkerRequest request)
        {
            if (request.Ids != null && request.Ids.Count > 0 && request.Id > 0)
            {
                foreach (var id in request.Ids)
                {
                    var start = await _examPaperStartRepository.GetAsync(id);
                    start.MarkTeacherId = request.Id;
                    await _examPaperStartRepository.UpdateAsync(start);
                }
            }
            return new BoolResult
            {
                Value = true
            };
        }
    }
}
