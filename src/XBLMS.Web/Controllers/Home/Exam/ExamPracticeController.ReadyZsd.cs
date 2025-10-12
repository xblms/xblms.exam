using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPracticeController
    {
        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(RouteReadyZsd)]
        public async Task<ActionResult<GetReadyZsdResult>> GetReadyZsd([FromBody] GetReadyZsdRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            var tmGroupIds = request.TmGroupIds;
            var zsdAndTmTotalList = new List<KeyValuePair<string, int>>();
            var zsdAndTmIdsList = new List<KeyValuePair<string, List<int>>>();
            var zsdList = new List<string>();
            if (tmGroupIds != null && tmGroupIds.Count > 0)
            {
                foreach (var tmGroupId in tmGroupIds)
                {
                    var tmGroup = await _examTmGroupRepository.GetAsync(tmGroupId);
                    var zsds = await _examTmRepository.Group_Practice_GetZsdsAsync(tmGroup, request.TxIds, request.Nds);
                    if (zsds != null)
                    {
                        foreach (var zsd in zsds)
                        {
                            if (!string.IsNullOrEmpty(zsd) && !zsdList.Contains(zsd))
                            {
                                zsdList.Add(zsd);
                                zsdAndTmIdsList.Add(new KeyValuePair<string, List<int>>(zsd, []));
                            }
                        }
                    }
                }
                foreach (var tmGroupId in tmGroupIds)
                {
                    var tmGroup = await _examTmGroupRepository.GetAsync(tmGroupId);
                    if (zsdAndTmIdsList.Count > 0)
                    {
                        foreach (var zsdAndTmIds in zsdAndTmIdsList)
                        {
                            var zsd = zsdAndTmIds.Key;
                            var tmIds = await _examTmRepository.Group_Practice_GetTmidsAsync(tmGroup, request.TxIds, request.Nds, zsd);
                            if (tmIds != null)
                            {
                                zsdAndTmIds.Value.AddRange(tmIds);
                            }
                        }
                    }
                }
            }
            if (zsdAndTmIdsList.Count > 0)
            {
                foreach(var zsdids in zsdAndTmIdsList)
                {
                    zsdAndTmTotalList.Add(new KeyValuePair<string, int>(zsdids.Key, zsdids.Value.Distinct().ToList().Count));
                }
                zsdAndTmTotalList = zsdAndTmTotalList.OrderByDescending(zsd => zsd.Value).ToList();
                if (request.Order == "name")
                {
                    zsdAndTmTotalList = zsdAndTmTotalList.OrderBy(zsd => zsd.Key).ToList();
                }
             
            }

            return new GetReadyZsdResult
            {
                ZsdList = zsdAndTmTotalList
            };

        }
    }
}
