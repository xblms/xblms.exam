using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamQuestionnairingController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> GetList([FromQuery] GetRequest request)
        {
            var paper = await _examQuestionnaireRepository.GetAsync(request.Id);
            if (!string.IsNullOrEmpty(request.ps))
            {
                paper = await _examQuestionnaireRepository.GetAsync(request.ps);
            }

            if (paper == null) { return this.Error("无效的问卷"); }

            if (paper.ExamEndDateTime < DateTime.Now || paper.ExamBeginDateTime >= DateTime.Now)
            {
                return this.Error("不在有效期内");
            }


            var tmTotal = 0;
            var tmList = await _examQuestionnaireTmRepository.GetListAsync(paper.Id);
            if (tmList != null && tmList.Count > 0)
            {
                foreach (var tm in tmList)
                {
                    tmTotal++;
                    tm.Set("OptionsValues", new List<string>());
                    tm.Set("Answer", "");

                    var optionsRandom = new List<KeyValuePair<string, string>>();
                    var options = ListUtils.ToList(tm.Get("options"));
                    var abcList = StringUtils.GetABC();
                    for (var i = 0; i < options.Count; i++)
                    {
                        optionsRandom.Add(new KeyValuePair<string, string>(abcList[i], options[i]));
                    }
                    tm.Set("OptionsRandom", optionsRandom);
                    tm.Set("AnswerStatus", false);
                }
            }
            paper.Set("TmTotal", tmTotal);

            return new GetResult
            {
                Watermark = DateTime.Now.ToString(),
                Item = paper,
                TmList = tmList
            };
        }
    }
}
