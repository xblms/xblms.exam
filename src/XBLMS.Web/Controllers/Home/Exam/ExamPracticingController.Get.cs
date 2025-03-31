using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPracticingController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] IdRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            var item = await _examPracticeRepository.GetAsync(request.Id);
            if (item != null)
            {
                item.TmIds = item.TmIds.OrderBy(tm => { return StringUtils.Guid(); }).ToList();
                var tmCount = item.TmCount;
                if (item.ParentId > 0)
                {
                    tmCount = item.MineTmCount;
                }
                return new GetResult
                {
                    Title = item.Title,
                    TmIds = item.TmIds,
                    Total = tmCount,
                    Watermark = await _authManager.GetWatermark()
                };

            }
            return this.Error("练习错误，请重试");
        }

        [HttpGet, Route(RouteTm)]
        public async Task<ActionResult<ItemResult<ExamTm>>> GetTm([FromQuery] IdRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            var tm = await _examTmRepository.GetAsync(request.Id);
            if (tm != null)
            {
                await _examManager.GetTmInfoByPracticing(tm);

                bool isCollection = false;
                var collection = await _examPracticeCollectRepository.GetAsync(user.Id);
                if(collection!=null && collection.TmIds.Contains(tm.Id))
                {
                    isCollection = true;
                }

                tm.Set("IsCollection", isCollection);

                bool isWrong = false;
                var wrong=await _examPracticeWrongRepository.GetAsync(user.Id);
                if (wrong != null && wrong.TmIds.Contains(tm.Id)) {
                    isWrong = true;
                }
                tm.Set("IsWrong", isWrong);


                return new ItemResult<ExamTm>
                {
                    Item = tm
                };
            }
            return this.Error("题目加载错误，请继续");
        }
    }
}



