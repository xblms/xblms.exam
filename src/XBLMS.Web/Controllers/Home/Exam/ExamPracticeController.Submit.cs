using Datory;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPracticeController
    {
        [HttpPost, Route(RouteSubmit)]
        public async Task<ActionResult<GetSubmitResult>> Submit([FromBody] GetSubmitRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            var tmIds = new List<int>();
            var zsds = new List<string>();
            var success = true;
            var error = "";

            var tmGroups = await _examTmGroupRepository.GetListWithoutLockedAsync();

            if (request.PracticeType == PracticeType.Group)
            {
                tmIds.Add(1);
            }
            else if (request.PracticeType == PracticeType.Wrong)
            {
                var item = await _examPracticeWrongRepository.GetAsync(user.Id);
                if (item != null && item.TmIds != null && item.TmIds.Count > 0)
                {
                    tmIds.AddRange(item.TmIds);
                }

            }
            else if (request.PracticeType == PracticeType.Collect)
            {
                var item = await _examPracticeCollectRepository.GetAsync(user.Id);
                if (item != null && item.TmIds != null && item.TmIds.Count > 0)
                {
                    tmIds.AddRange(item.TmIds);
                }
            }
            else
            {
                success = false;
                error = "这是个错误的开始";

            }

            var practiceId = 0;
            if (tmIds.Count > 0)
            {
                if (request.PracticeType == PracticeType.Group)
                {
                    var createPractice = await _examPracticeRepository.GetAsync(request.GroupId);
                    createPractice.KeyWords = await _organManager.GetUserKeyWords(user.Id);
                    createPractice.IsCreate = false;
                    createPractice.ParentId = createPractice.Id;
                    practiceId = await _examPracticeRepository.InsertAsync(createPractice);
                }
                else
                {
                    practiceId = await _examPracticeRepository.InsertAsync(new ExamPractice
                    {
                        Title = request.PracticeType.GetDisplayName(),
                        IsCreate = false,
                        PracticeType = request.PracticeType,
                        UserId = user.Id,
                        TmCount = tmIds.Count,
                        TmIds = tmIds,
                        Zsds = zsds,
                        KeyWords = await _organManager.GetUserKeyWords(user.Id)
                    });
                }

                if (practiceId <= 0)
                {
                    success = false;
                    error = "创建练习失败，请重新提交";
                }
            }
            else
            {
                success = false;
                error = "没有找到可以练习的题目，请尝试其他方式";
            }

            return new GetSubmitResult
            {
                Id = practiceId,
                Success = success,
                Error = error
            };

        }
    }
}
