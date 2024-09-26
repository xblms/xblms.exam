using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
                    user.Set("MaxScore", maxScore.HasValue ? maxScore.Value : 0);

                    item.Set("User", user);
                }
            }
            return new GetUserResult
            {
                Total = total,
                List = list
            };
        }
        [HttpPost, Route(RouteUserUpdateDateTime)]
        public async Task<ActionResult<BoolResult>> UpdateDateTime([FromBody] GetUserUpdateRequest request)
        {
            if (request.Ids != null && request.Ids.Count > 0)
            {
                foreach (var id in request.Ids)
                {
                    await _examPaperUserRepository.UpdateExamDateTimeByIdAsync(id, request.ExamBeginDateTime, request.ExamEndDateTime);
                }
            }

            return new BoolResult
            {
                Value = true
            };
        }

        [HttpPost, Route(RouteUserUpdateExamTimes)]
        public async Task<ActionResult<BoolResult>> UpdateExamTimes([FromBody] GetUserUpdateRequest request)
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
        public async Task<ActionResult<BoolResult>> Delete([FromBody] GetUserUpdateRequest request)
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
        public async Task<ActionResult<BoolResult>> DeleteOne([FromBody] IdRequest request)
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



        [HttpPost, Route(RouteUserExport)]
        public async Task<ActionResult<StringResult>> UserExport([FromBody] GetUserRequest request)
        {
            var (total, list) = await _examPaperUserRepository.GetListAsync(request.Id, request.Keywords, 1, int.MaxValue);
       

            var paper = await _examPaperRepository.GetAsync(request.Id);

            var fileName = $"{paper.Title}-考生列表.xlsx";
            var filePath = _pathManager.GetDownloadFilesPath(fileName);

            DirectoryUtils.CreateDirectoryIfNotExists(DirectoryUtils.GetDirectoryPath(filePath));
            FileUtils.DeleteFileIfExists(filePath);

            var head = new List<string>
            {
                "序号",
                "账号",
                "姓名",
                "组织",
                "参考次数",
                "考试次数",
                "考试时间",
                "最高分"
            };
            var rows = new List<List<string>>();


            if (total > 0)
            {
                var index = 1;
                foreach (var item in list)
                {
                    var user = await _organManager.GetUser(item.UserId);

                    var userExamTimes = await _examPaperStartRepository.CountAsync(item.ExamPaperId, user.Id);
                    var maxScore = await _examPaperStartRepository.GetMaxScoreAsync(user.Id, item.ExamPaperId);

                    rows.Add(new List<string>
                    {
                        index.ToString(),
                        user.UserName,
                        user.DisplayName,
                        user.Get("OrganNames").ToString(),
                        $"{userExamTimes}",
                        $"{item.ExamTimes}",
                        $"{item.ExamBeginDateTime.Value}-{item.ExamEndDateTime.Value}",
                        (maxScore.HasValue ? maxScore.Value : 0).ToString()
                    });
                    index++;

                }
            }

            ExcelUtils.Write(filePath, head, rows);

            var downloadUrl = _pathManager.GetDownloadFilesUrl(fileName);

            return new StringResult
            {
                Value = downloadUrl
            };
        }
    }
}
