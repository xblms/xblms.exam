using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class UserDocLayerViewController
    {
        [HttpGet, Route(RoutePractice)]
        public async Task<ActionResult<GetExamPracticeResult>> GetExamPractice([FromQuery] GetRequest request)
        {
            var (total, list) = await _databaseManager.ExamPracticeRepository.Analysis_GetListAsync(request.Id, request.DateFrom, request.DateTo, request.PageIndex, request.PageSize);
            return new GetExamPracticeResult
            {
                Total = total,
                List = list
            };
        }

        public class GetExamPracticeResult
        {
            public int Total { get; set; }
            public List<ExamPractice> List { get; set; }
        }
    }
}
