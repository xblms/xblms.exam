using Datory;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTmGroupController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {

            var resultGroups = new List<ExamTmGroup>();
            var allGroups = await _examTmGroupRepository.GetListAsync();
 
            foreach (var group in allGroups)
            {
                var creator = await _administratorRepository.GetByUserIdAsync(group.CreatorId);
                if (creator != null)
                {
                    group.Set("CreatorId", creator.Id);
                    group.Set("CreatorDisplayName", creator.DisplayName);
                }
                group.Set("TypeName", group.GroupType.GetDisplayName());


                var tmTotal = 0;
                if (group.GroupType == TmGroupType.All)
                {
                    tmTotal = await _examTmRepository.GetCountByWithoutStopAsync();
                }
                else if (group.GroupType == TmGroupType.Fixed)
                {
                    var tmIds = group.TmIds;
                    tmTotal = await _examTmRepository.GetCountByWithoutStopAndInIdsAsync(tmIds);
                }
                else
                {
                    tmTotal = await _examTmRepository.GetCountAsync(group.TreeIds, group.TxIds, group.Nandus, group.Zhishidians, group.DateFrom, group.DateTo);

                }
                group.TmTotal = tmTotal;

                var keyWord = request.Search;
                if (!string.IsNullOrEmpty(keyWord))
                {
                    if (StringUtils.Contains(group.GroupName, keyWord) || (StringUtils.Contains(group.Description, keyWord)))
                    {
                        resultGroups.Add(group);
                    }
                }
                else
                {
                    resultGroups.Add(group);
                }


            }
            return new GetResult
            {
                Groups = resultGroups
            };
        }
    }
}
