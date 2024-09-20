using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPaperExamingController
    {
        [HttpPost, Route(RouteSubmitTiming)]
        public async void SubmitTiming([FromBody] IdRequest request)
        {
            await _examPaperStartRepository.IncrementAsync(request.Id);
        }
    }
}
