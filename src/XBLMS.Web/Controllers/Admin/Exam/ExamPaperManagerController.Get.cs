using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamPaperManagerController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Manage))
            {
                return this.NoAuth();
            }

            var paper = await _examPaperRepository.GetAsync(request.Id);
            var maxScore = await _examPaperStartRepository.GetMaxScoreAsync(request.Id);
            var minScore = await _examPaperStartRepository.GetMinScoreAsync(request.Id);

            var sumScore = await _examPaperStartRepository.SumScoreAsync(request.Id);
            //var sumScoreDistinct = await _examPaperStartRepository.SumScoreDistinctAsync(request.Id);

            var scoreCount = await _examPaperStartRepository.CountAsync(request.Id);
            var scoreCountDistinct = await _examPaperStartRepository.CountDistinctAsync(request.Id);


            var userTotal = await _examPaperUserRepository.CountAsync(request.Id);

            var passTotal = await _examPaperStartRepository.CountByPassAsync(request.Id, paper.PassScore);
            var passTotalDistinct = await _examPaperStartRepository.CountByPassDistinctAsync(request.Id, paper.PassScore);

            var markers = new List<GetSelectMarkInfo>();
            var adminList = await _administratorRepository.GetListAsync();
            if (adminList != null && adminList.Count > 0)
            {
                foreach (var admin in adminList)
                {
                    markers.Add(new GetSelectMarkInfo
                    {
                        Id = admin.Id,
                        DisplayName = admin.DisplayName,
                        UserName = admin.UserName
                    });
                }
            }

            return new GetResult
            {
                Title = paper.Title,
                TotalScore = paper.TotalScore,
                PassScore = paper.PassScore,
                TotalUser = userTotal,
                MaxScore = maxScore,
                MinScore = minScore,
                TotalPass = passTotal,
                TotalPassDistinct = passTotalDistinct,
                TotalUserScore = sumScore,
                TotalExamTimes = scoreCount,
                TotalExamTimesDistinct = scoreCountDistinct,
                MarkerList = markers,
            };
        }
    }
}
