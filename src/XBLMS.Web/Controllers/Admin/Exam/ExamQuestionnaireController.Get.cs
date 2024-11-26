using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamQuestionnaireController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> GetManage([FromQuery] GetRequest request)
        {
            var (total, list) = await _examQuestionnaireRepository.GetListAsync(request.Keyword, request.PageIndex, request.PageSize);

            if (total > 0)
            {
                foreach (var item in list)
                {
                    item.Set("ExamBeginDateTimeStr", item.ExamBeginDateTime.Value.ToString(DateUtils.FormatStringDateTimeCN));
                    item.Set("ExamEndDateTimeStr", item.ExamEndDateTime.Value.ToString(DateUtils.FormatStringDateTimeCN));
                }
            }
            return new GetResult
            {
                Total = total,
                Items = list,
            };
        }

    }
}
