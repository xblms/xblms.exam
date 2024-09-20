using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamQuestionnaireEditController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] IdRequest request)
        {
            var paper = new ExamQuestionnaire();
            paper.Title = "调查问卷-" + StringUtils.PadZeroes(await _questionnaireRepository.MaxIdAsync() + 1, 5);
            if (request.Id > 0)
            {
                paper = await _questionnaireRepository.GetAsync(request.Id);
            }

            var userGroups = await _userGroupRepository.GetListWithoutLockedAsync();
            var tmList = await _questionnaireTmRepository.GetListAsync(paper.Id);

            return new GetResult
            {
                Item = paper,
                UserGroupList = userGroups,
                TmList = tmList
            };

        }

    }
}
