using System.Threading.Tasks;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPaperController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var user = await _authManager.GetUserAsync();

            var paperIds = await _examPaperUserRepository.GetPaperIdsByUser(user.Id, request.Date);
            var (total, list) = await _examPaperRepository.GetListByUserAsync(paperIds,request.KeyWords, request.PageIndex, request.PageSize);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    await _examManager.GetPaperInfo(item, user);
                }
            }
            return new GetResult
            {
                Total = total,
                List = list
            };
        }

        [HttpGet, Route(RouteItem)]
        public async Task<ActionResult<ItemResult<ExamPaper>>> GetItem([FromQuery] IdRequest request)
        {
            var user = await _authManager.GetUserAsync();
            var paper = await _examPaperRepository.GetAsync(request.Id);
            await _examManager.GetPaperInfo(paper, user);
            return new ItemResult<ExamPaper>
            {
                Item = paper
            };
        }
    }
}
