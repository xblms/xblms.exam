using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Configuration;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPracticeController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var user = await _authManager.GetUserAsync();

            var tmGroups = await _examTmGroupRepository.GetListWithoutLockedAsync();
            var resultList = new List<GetResultItem>();
            var resultTotal = 0;

            if (tmGroups != null && tmGroups.Count > 0)
            {
                foreach (var tmGroup in tmGroups)
                {
                    if (tmGroup.OpenUser)
                    {
                        var (total, tmList) = await _examTmRepository.GetListAsync(tmGroup, 0, int.MaxValue);
                        if (total > 0)
                        {
                            resultTotal++;
                            var zsds = new List<string>();
                            foreach (var tmItem in tmList)
                            {
                                resultTotal++;
                                if (!zsds.Contains(tmItem.Zhishidian))
                                {
                                    zsds.Add(tmItem.Zhishidian);
                                }
                            }
                            resultList.Add(new GetResultItem
                            {
                                Id = tmGroup.Id,
                                TmTotal = total,
                                Zsds = zsds
                            });
                        }
                    }

                }
            }

            var collectTotal = 0;
            var collect = await _examPracticeCollectRepository.GetAsync(user.Id);
            if (collect != null && collect.TmIds != null)
            {
                collectTotal = collect.TmIds.Count;
            }

            var wrongTotal = 0;
            var wrong = await _examPracticeWrongRepository.GetAsync(user.Id);
            if (wrong != null && wrong.TmIds != null)
            {
                wrongTotal = wrong.TmIds.Count;
            }

            return new GetResult
            {
                List = resultList,
                Total = resultTotal,
                CollectTotal = collectTotal,
                WrongTotal = wrongTotal
            };
        }
    }
}
