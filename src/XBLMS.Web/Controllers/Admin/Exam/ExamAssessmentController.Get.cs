using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamAssessmentController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> GetManage([FromQuery] GetRequest request)
        {
            var (total, list) = await _examAssessmentRepository.GetListAsync(request.Keyword, request.PageIndex, request.PageSize);

            if (total > 0)
            {
                foreach (var item in list)
                {
                    var config = await _examAssessmentConfigRepository.GetAsync(item.ConfigId);
                    item.Set("Config", config);
                    item.Set("ExamBeginDateTimeStr", item.ExamBeginDateTime.Value.ToString(DateUtils.FormatStringDateTimeCN));
                    item.Set("ExamEndDateTimeStr", item.ExamEndDateTime.Value.ToString(DateUtils.FormatStringDateTimeCN));

                    var (userTotal, userSubmitTotal) = await _examAssessmentUserRepository.GetCountAsync(item.Id);
                    item.Set("UserTotal", userTotal);
                    item.Set("UserSubmitTotal", userSubmitTotal);
                }
            }
            return new GetResult
            {
                Total = total,
                List = list,
            };
        }

    }
}
