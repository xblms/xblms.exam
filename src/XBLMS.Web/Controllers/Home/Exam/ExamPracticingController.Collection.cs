using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPracticingController
    {
        [HttpPost, Route(RouteCollection)]
        public async Task<ActionResult<BoolResult>> SetCollection([FromBody] IdRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            var collection = await _examPracticeCollectRepository.GetAsync(user.Id);
            if (collection == null) {
                await _examPracticeCollectRepository.InsertAsync(new ExamPracticeCollect
                {
                    TmIds = new List<int> { request.Id },
                    UserId=user.Id,
                });
            }
            else
            {
                collection.TmIds.Add(request.Id);
                await _examPracticeCollectRepository.UpdateAsync(collection);
            }
            return new BoolResult
            {
                Value = true
            };
        }
        [HttpPost, Route(RouteCollectionRemove)]
        public async Task<ActionResult<BoolResult>> RemoveCollection([FromBody] IdRequest request)
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            var collection = await _examPracticeCollectRepository.GetAsync(user.Id);
            collection.TmIds.Remove(request.Id);
            await _examPracticeCollectRepository.UpdateAsync(collection);

            return new BoolResult
            {
                Value = true
            };
        }


    }
}



