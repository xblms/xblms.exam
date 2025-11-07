using Datory;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTmGroupController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var adminAuth = await _authManager.GetAdminAuth();

            var resultGroups = new List<ExamTmGroup>();
            var allGroups = await _examTmGroupRepository.GetListAsync(adminAuth, request.Search);

            foreach (var group in allGroups)
            {
                var creator = await _administratorRepository.GetByUserIdAsync(group.CreatorId);
                if (creator != null)
                {
                    group.Set("Creator", creator.DisplayName);
                }
                else
                {
                    group.Set("Creator", "无");
                    group.CreatorId = 0;
                }

                group.Set("TypeName", group.GroupType.GetDisplayName());
                group.Set("UseCount", 0);
                group.Set("TotalScore", 0);
                resultGroups.Add(group);

            }
            return new GetResult
            {
                Groups = resultGroups
            };
        }

        [HttpGet, Route(RouteTmTotal)]
        public async Task<ActionResult<GetTmTotalResult>> GetTmTotal([FromQuery] IdRequest request)
        {
            var group = await _examTmGroupRepository.GetAsync(request.Id);

            var tmTotal = await _examTmRepository.Group_GetTmTotalAsync(group);
            var useTotal = await _examPaperRepository.GetTmGroupCount(group.Id);
            decimal totalScore = 0;
            if (group.GroupType == TmGroupType.Fixed)
            {
                totalScore = await _examTmRepository.Group_GetTotalScoreAsync(group);
            }

            return new GetTmTotalResult { TmTotal = tmTotal, UseTotal = useTotal, ScoreTotal = totalScore };
        }
    }
}
