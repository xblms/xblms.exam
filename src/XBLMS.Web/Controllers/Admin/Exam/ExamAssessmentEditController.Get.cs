using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamAssessmentEditController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] IdRequest request)
        {
            var assInfo = new ExamAssessment();
            assInfo.Title = "测评-" + StringUtils.PadZeroes(await _examAssessmentRepository.MaxIdAsync() + 1, 5);
            if (request.Id > 0)
            {
                assInfo = await _examAssessmentRepository.GetAsync(request.Id);
            }

            var userGroups = await _userGroupRepository.GetListWithoutLockedAsync();
            var configList = await _examAssessmentConfigRepository.GetListWithoutLockedAsync();
            var tmList = await _examAssessmentTmRepository.GetListAsync(assInfo.Id);

            return new GetResult
            {
                Item = assInfo,
                UserGroupList = userGroups,
                ConfigList = configList,
                TmList = tmList
            };

        }

    }
}
