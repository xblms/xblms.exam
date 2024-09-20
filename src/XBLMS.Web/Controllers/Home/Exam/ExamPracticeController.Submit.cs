using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Datory;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPracticeController
    {
        [HttpPost, Route(RouteSubmit)]
        public async Task<ActionResult<GetSubmitResult>> Submit([FromBody] GetSubmitRequest request)
        {
            var user = await _authManager.GetUserAsync();

            var tmIds = new List<int>();
            var zsds = new List<string>();
            var success = true;
            var error = "";

            var tmGroups = await _examTmGroupRepository.GetListWithoutLockedAsync();

            if (request.PracticeType == PracticeType.All)
            {
                if (tmGroups != null && tmGroups.Count > 0)
                {
                    foreach (var tmGroup in tmGroups)
                    {
                        if (tmGroup.OpenUser)
                        {
                            var (total, tmList) = await _examTmRepository.GetListAsync(tmGroup, null, 0, 0, "", "", "", false, 0, int.MaxValue);
                            if (total > 0)
                            {
                                foreach (var tmItem in tmList)
                                {
                                    if (!zsds.Contains(tmItem.Zhishidian))
                                    {
                                        zsds.Add(tmItem.Zhishidian);
                                    }
                                    if (!tmIds.Contains(tmItem.Id))
                                    {
                                        tmIds.Add(tmItem.Id);
                                    }
                                }
                            }
                        }

                    }
                }
            }
            else if (request.PracticeType == PracticeType.Group)
            {
                var group = tmGroups.Single(g => g.Id == request.GroupId);
                if (group != null)
                {
                    var (total, tmList) = await _examTmRepository.GetListAsync(group, null, 0, 0, "", "", "", false, 0, int.MaxValue);
                    if (total > 0)
                    {
                        foreach (var tmItem in tmList)
                        {
                            if (!zsds.Contains(tmItem.Zhishidian))
                            {
                                zsds.Add(tmItem.Zhishidian);
                            }
                            if (!tmIds.Contains(tmItem.Id))
                            {
                                tmIds.Add(tmItem.Id);
                            }
                        }
                    }
                }
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
                practiceId = await _examPracticeRepository.InsertAsync(new ExamPractice
                {
                    Title= request.PracticeType.GetDisplayName(),
                    PracticeType = request.PracticeType,
                    UserId = user.Id,
                    TmCount = tmIds.Count,
                    TmIds = tmIds,
                    Zsds = zsds
                });
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
