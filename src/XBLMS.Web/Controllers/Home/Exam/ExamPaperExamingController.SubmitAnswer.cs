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
        [HttpPost, Route(RouteSubmitAnswer)]
        public void SubmitAnswer([FromBody] GetSubmitAnswerRequest request)
        {
            _createManager.CreateSubmitAnswerAsync(request.Answer);
        }
    }
}
