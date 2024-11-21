namespace XBLMS.Web.Controllers.Home.Exam
{
    public partial class ExamPracticingController
    {
        //[HttpPost, Route(RouteHistoryDelete)]
        //public async Task<ActionResult<BoolResult>> DeleteHistory([FromBody] IdRequest request)
        //{
        //    var user = await _authManager.GetAdminAsync();
        //    if (user == null)
        //    {
        //        return Unauthorized();
        //    }
        //    else
        //    {
        //        await _examPracticeUserRepository.DeleteAsync(request.Id);
        //        return new BoolResult
        //        {
        //            Value = true
        //        };
        //    }

        //}
        //[HttpPost, Route(RouteHistory)]
        //public async Task<ActionResult<GetHistoryResult>> GetHistoryList([FromBody] GetHistoryRequest request)
        //{
        //    var user = await _authManager.GetAdminAsync();
        //    if (user == null)
        //    {
        //        return Unauthorized();
        //    }
        //    else
        //    {
        //        var resutList = new List<GetHistoryResultInfo>();
        //        var (total, list) = await _examPracticeUserRepository.GetListForUserAsync(user.Id,request.Order,request.Type,request.Title, request.PageIndex, request.PageSize);
        //        if (total > 0)
        //        {
        //            foreach (var item in list)
        //            {
        //                string title = string.Empty;
        //                if (item.PracticeId > 0)
        //                {
        //                    title = $"题库练习:{await _examPracticeRepository.GetNameAsync(item.PracticeId)}";
        //                }
        //                else if (item.PracticeId == 0)
        //                {
        //                    title = "错题练习";
        //                }
        //                else if (item.PracticeId == -1)
        //                {
        //                    title = "收藏练习";
        //                }
        //                else if (item.PracticeId == -2)
        //                {
        //                    title = "综合练习";
        //                }
        //                else
        //                {
        //                    title = "未知来源";
        //                }
        //                resutList.Add(new GetHistoryResultInfo
        //                {
        //                    Id = item.Id,
        //                    Zsds = item.Zsds,
        //                    TmCount = item.TmCount,
        //                    RightCount = item.RightCount,
        //                    AnswerCount = item.AnswerCount,
        //                    DateTime = DateUtils.ParseThisMoment(item.CreatedDate.Value, DateTime.Now),
        //                    Source = title
        //                });
        //            }
        //        }
        //        return new GetHistoryResult
        //        {
        //            Total = total,
        //            List = resutList
        //        };
        //    }

        //}

    }
}



