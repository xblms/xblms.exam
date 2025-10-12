using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamQuestionnaireEditController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] IdRequest request)
        {
            var adminAuth = await _authManager.GetAdminAuth();

            var paper = new ExamQuestionnaire();
            paper.Title = "调查问卷-" + StringUtils.PadZeroes(await _questionnaireRepository.MaxIdAsync() + 1, 5);
            if (request.Id > 0)
            {
                paper = await _questionnaireRepository.GetAsync(request.Id);
            }

            var userGroups = await _userGroupRepository.GetListAsync(adminAuth, true);
            var tmList = await _questionnaireTmRepository.GetListAsync(paper.Id);

            var tmIndex = 0;
            foreach (var item in tmList)
            {
                if (item.ParentId == 0)
                {
                    tmIndex++;
                }
                if (item.Tx == ExamQuestionnaireTxType.DanxuantiErwei || item.Tx == ExamQuestionnaireTxType.DuoxuantiErwei || item.Tx == ExamQuestionnaireTxType.DanxuantiSanwei || item.Tx == ExamQuestionnaireTxType.DuoxuantiSanwei)
                {
                    var smallList = tmList.Where(tm => tm.ParentId == item.Id).ToList();
                    foreach (var small in smallList)
                    {
                        small.Set("Answer", "");
                        small.Set("Answers", new List<string>());
                    }
                    if (smallList != null && smallList.Count > 0)
                    {
                        item.Set("SmallList", smallList);
                    }
                }
                item.Set("TmIndex", tmIndex);
            }

            var config = await _configRepository.GetAsync();

            return new GetResult
            {
                SystemCode = config.SystemCode,
                Item = paper,
                UserGroupList = userGroups,
                TmList = tmList
            };

        }

    }
}
