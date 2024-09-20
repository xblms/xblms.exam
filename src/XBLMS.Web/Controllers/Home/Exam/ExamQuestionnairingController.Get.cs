using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using Ubiety.Dns.Core;
using XBLMS.Configuration;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamQuestionnairingController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> GetList([FromQuery] IdRequest request)
        {
            var paper = await _examQuestionnaireRepository.GetAsync(request.Id);

            if (paper == null) { return NotFound(); }

            var tmTotal = 0;
            var tmList = await _examQuestionnaireTmRepository.GetListAsync(paper.Id);
            if (tmList != null && tmList.Count > 0)
            {
                foreach (var tm in tmList)
                {
                    tmTotal++;
                    tm.Set("OptionsValues", new List<string>());
                    tm.Set("Answer","");

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
                Watermark = await _authManager.GetWatermark(),
                Item = paper,
                TmList = tmList
            };
        }
    }
}
