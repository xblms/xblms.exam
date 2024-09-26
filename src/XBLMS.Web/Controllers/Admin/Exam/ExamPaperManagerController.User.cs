using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamPaperManagerController
    {
        [HttpGet, Route(RouteUser)]
        public async Task<ActionResult<GetUserResult>> GetUserList([FromQuery] GetUserRequest request)
        {
            var (total, list) = await _examPaperUserRepository.GetListAsync(request.Id, request.Keywords, request.PageIndex, request.PageSize);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    var user = await _organManager.GetUser(item.UserId);

                    var userExamTimes = await _examPaperStartRepository.CountAsync(item.ExamPaperId, user.Id);
                    user.Set("ExamTimes", userExamTimes);

                    var maxScore = await _examPaperStartRepository.GetMaxScoreAsync(user.Id, item.ExamPaperId);
                    user.Set("MaxScore", userExamTimes);

                    item.Set("User", user);

                    if (!StringUtils.Equals(user.KeyWordsAdmin, item.KeyWords))
                    {
                        await _examPaperUserRepository.UpdateKeyWordsAdminAsync(item.Id, user.KeyWordsAdmin);
                    }
                }
            }
            return new GetUserResult
            {
                Total = total,
                List = list
            };
        }
        [HttpPost, Route(RouteUserUpdateDateTime)]
        public async Task<ActionResult<BoolResult>> UpdateDateTime([FromQuery] GetUserUpdateRequest request)
        {
            if (request.Ids != null && request.Ids.Count > 0)
            {
                foreach (var id in request.Ids)
                {
                    await _examPaperUserRepository.UpdateExamDateTimeAsync(id, request.ExamBeginDateTime, request.ExamEndDateTime);
                }
            }

            return new BoolResult
            {
                Value = true
            };
        }

        [HttpPost, Route(RouteUserUpdateExamTimes)]
        public async Task<ActionResult<BoolResult>> UpdateExamTimes([FromQuery] GetUserUpdateRequest request)
        {
            if (request.Ids != null && request.Ids.Count > 0)
            {
                foreach (var id in request.Ids)
                {
                    if (request.Increment)
                    {
                        await _examPaperUserRepository.IncrementAsync(id);
                    }
                    else
                    {
                        await _examPaperUserRepository.DecrementAsync(id);
                    }
                }
            }

            return new BoolResult
            {
                Value = true
            };
        }


        [HttpPost, Route(RouteUserDelete)]
        public async Task<ActionResult<BoolResult>> Delete([FromQuery] GetUserUpdateRequest request)
        {
            var admin = await _authManager.GetAdminAsync();

            if (request.Ids != null && request.Ids.Count > 0)
            {
                foreach (var id in request.Ids)
                {
                    var paperUser = await _examPaperUserRepository.GetAsync(id);

                    if (paperUser != null)
                    {
                        await _examPaperStartRepository.ClearByPaperAndUserAsync(paperUser.ExamPaperId, paperUser.UserId);
                        await _examPaperAnswerRepository.ClearByPaperAndUserAsync(paperUser.ExamPaperId, paperUser.UserId);
                        await _examPaperUserRepository.DeleteAsync(id);
                    }
                }
            }

            return new BoolResult
            {
                Value = true
            };
        }
        [HttpPost, Route(RouteUserDeleteOne)]
        public async Task<ActionResult<BoolResult>> DeleteOne([FromQuery] IdRequest request)
        {
            var paperUser = await _examPaperUserRepository.GetAsync(request.Id);

            if (paperUser != null)
            {
                await _examPaperStartRepository.ClearByPaperAndUserAsync(paperUser.ExamPaperId, paperUser.UserId);
                await _examPaperAnswerRepository.ClearByPaperAndUserAsync(paperUser.ExamPaperId, paperUser.UserId);
                await _examPaperUserRepository.DeleteAsync(request.Id);
            }

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
