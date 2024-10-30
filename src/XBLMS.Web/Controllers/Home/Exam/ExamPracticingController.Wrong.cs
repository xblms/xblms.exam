using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Configuration;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPracticingController
    {
        [HttpPost, Route(RouteWrongRemove)]
        public async Task<ActionResult<BoolResult>> ErrorRemove([FromBody] IdRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            var wrong = await _examPracticeWrongRepository.GetAsync(user.Id);
            if (wrong != null) {
                wrong.TmIds.Remove(request.Id);
                await _examPracticeWrongRepository.UpdateAsync(wrong);
            }

            return new BoolResult
            {
                Value = true
            };
        }
    }
}



