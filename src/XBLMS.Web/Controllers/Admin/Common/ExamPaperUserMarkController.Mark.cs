using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using XBLMS.Dto;

namespace XBLMS.Web.Controllers.Admin.Common
{
    public partial class ExamPaperUserMarkController
    {
        [HttpPost, Route(RouteSave)]
        public async Task<ActionResult<BoolResult>> Save([FromBody] GetMarkRequest request)
        {
            var admin = await _authManager.GetAdminAsync();

            decimal totalS = 0;


            if (request.List != null && request.List.Count > 0)
            {
                foreach (var mark in request.List)
                {
                    var answer = await _examPaperAnswerRepository.GetAsync(mark.Id);
                    answer.Score = mark.Score;
                    totalS += mark.Score;
                    answer.Set("MarkState", mark.Get("MarkState"));
                    await _examPaperAnswerRepository.UpdateAsync(answer);
                }

                var start = await _examPaperStartRepository.GetAsync(request.StartId);
                start.SubjectiveScore = totalS;
                await _examPaperStartRepository.UpdateAsync(start);
            }

            return new BoolResult
            {
                Value = true
            };
        }


        [HttpPost, Route(RouteSubmit)]
        public async Task<ActionResult<BoolResult>> Submit([FromBody] GetMarkRequest request)
        {
            var admin = await _authManager.GetAdminAsync();

            decimal totalS = 0;

            if (request.List != null && request.List.Count > 0)
            {
                foreach (var mark in request.List)
                {
                    var answer = await _examPaperAnswerRepository.GetAsync(mark.Id);
                    answer.Score = mark.Score;
                    answer.Set("MarkState", mark.Get("MarkState"));
                    totalS += mark.Score;
                    await _examPaperAnswerRepository.UpdateAsync(answer);
                }
            }
            var start = await _examPaperStartRepository.GetAsync(request.StartId);
            var paper = await _examPaperRepository.GetAsync(start.ExamPaperId);

            start.IsMark = true;
            start.SubjectiveScore = totalS;
            start.Score = start.SubjectiveScore + start.ObjectiveScore;
            start.MarkDateTime = DateTime.Now;
            start.MarkTeacherId = admin.Id;

            await _examPaperStartRepository.UpdateAsync(start);

            if (start.IsMark && start.IsSubmit && start.Score >= paper.PassScore)
            {
                await _createManager.AwardCer(paper, start.Id, start.UserId);
            }

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
