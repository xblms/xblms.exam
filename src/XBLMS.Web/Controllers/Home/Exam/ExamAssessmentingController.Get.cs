using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamAssessmentingController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> GetList([FromQuery] GetRequest request)
        {
            var assInfo = await _examAssessmentRepository.GetAsync(request.Id);
            if (!string.IsNullOrEmpty(request.ps))
            {
                assInfo = await _examAssessmentRepository.GetAsync(request.ps);
            }

            if (assInfo == null) { return this.Error("无效的测评"); }

            if (assInfo.Locked)
            {
                return this.Error("无效的测评");
            }

            if (assInfo.ExamEndDateTime < DateTime.Now || assInfo.ExamBeginDateTime >= DateTime.Now)
            {
                return this.Error("不在有效期内");
            }


            var tmTotal = 0;
            var tmList = await _examAssessmentTmRepository.GetListAsync(assInfo.Id);
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
            assInfo.Set("TmTotal", tmTotal);

            return new GetResult
            {
                Watermark = await _authManager.GetWatermark(),
                Item = assInfo,
                TmList = tmList
            };
        }
    }
}
