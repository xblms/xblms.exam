using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamTmCorrectionController
    {
        [HttpPost, Route(RouteSubmit)]
        public async Task<ActionResult<BoolResult>> Submit([FromBody] GetRequest request)
        {
            var user = await _authManager.GetUserAsync();

            var tm = await _examTmRepository.GetAsync(request.TmId);
            var tmTitle = _examManager.GetTmTitle(tm);
            var item = new ExamTmCorrection
            {
                ExamPaperId = request.ExamPaperId,
                UserId = user.Id,
                TmId = request.TmId,
                Reason = request.Reason,
                TmTitle = tmTitle,
                TmSourceObject = TranslateUtils.JsonSerialize(tm),
                KeyWords = tmTitle,
                KeyWordsAdmin = await _organManager.GetUserKeyWords(user.Id),
                CreatorId = user.Id,
                CompanyId = user.CompanyId,
                DepartmentId = user.DepartmentId,
                CompanyParentPath = user.CompanyParentPath,
                DepartmentParentPath = user.DepartmentParentPath
            };
            await _examTmCorrectionRepository.InsertAsync(item);

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
