using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTmAnalysisController
    {
        [HttpGet, Route(RouteGetData)]
        public async Task<ActionResult<GetData>> GetDate()
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }

            var typeList = ListUtils.GetSelects<TmAnalysisType>();

            return new GetData
            {
                TypeList = typeList
            };
        }


        [HttpGet, Route(RouteGetPaper)]
        public async Task<ActionResult<GetPaperResult>> GetPaper([FromQuery] GetPaperRequest reqeust)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }

            var adminAuth = await _authManager.GetAdminAuth();

            var list = await _examPaperRepository.GetListAsync(adminAuth, reqeust.Title);
            var resultList = new List<KeyValuePair<int, string>>();
            foreach (var item in list)
            {
                resultList.Add(new KeyValuePair<int, string>(item.Id, item.Title));
            }
            return new GetPaperResult
            {
                List = resultList
            };
        }
    }
}
