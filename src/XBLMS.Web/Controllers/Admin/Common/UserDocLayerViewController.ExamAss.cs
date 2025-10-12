using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class UserDocLayerViewController
    {
        [HttpGet, Route(RouteAss)]
        public async Task<ActionResult<GetExamAssResult>> GetExamAss([FromQuery] GetRequest request)
        {
            var (total, list) = await _databaseManager.ExamAssessmentUserRepository.Analysis_GetListAsync(request.Id, request.DateFrom, request.DateTo, request.PageIndex, request.PageSize);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    var ass = await _databaseManager.ExamAssessmentRepository.GetAsync(item.ExamAssId);
                    if (ass == null) continue;
                    item.Set("AssName", ass.Title);
                }
            }
            return new GetExamAssResult
            {
                Total = total,
                List = list
            };
        }

        public class GetExamAssResult
        {
            public int Total { get; set; }
            public List<ExamAssessmentUser> List { get; set; }
        }
    }
}
