using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Core.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamPaperMarkController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var marker = await _authManager.GetAdminAsync();
            var (total, list) = await _examPaperStartRepository.GetListByMarkerAsync(marker.Id, request.Keywords, request.PageIndex, request.PageSize);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    var paper = await _examPaperRepository.GetAsync(item.ExamPaperId);
                    if (paper != null)
                    {
                        item.Set("Title", paper.Title);
                    }
                    item.Set("UseTime", DateUtils.SecondToHms(item.ExamTimeSeconds));

                }
            }
            return new GetResult
            {
                Total = total,
                List = list
            };
        }
    }
}
