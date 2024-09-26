using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamPaperManagerController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<BoolResult>> Get()
        {
            var admin = await _authManager.GetAdminAsync();
            return new BoolResult
            {
                Value = true
            };
        }
    }
}
