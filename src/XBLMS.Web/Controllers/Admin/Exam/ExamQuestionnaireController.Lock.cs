using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using XBLMS.Configuration;
using XBLMS.Core.Utils;
using XBLMS.Core.Utils.Office;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;
namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamQuestionnaireController
    {
        [HttpPost, Route(RouteLock)]
        public async Task<ActionResult<BoolResult>> Lock([FromBody] IdRequest request)
        {
            var paper = await _examQuestionnaireRepository.GetAsync(request.Id);
            if (paper == null) return NotFound();
            paper.Locked = true;

            await _examQuestionnaireRepository.UpdateAsync(paper);
            if (!paper.Published)
            {
                await _examQuestionnaireUserRepository.UpdateLockedAsync(paper.Id, paper.Locked);
            }

            await _authManager.AddAdminLogAsync("锁定问卷调查", $"名称：{paper.Title}");

            return new BoolResult
            {
                Value = true
            };
        }

    }
}
