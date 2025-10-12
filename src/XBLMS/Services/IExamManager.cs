using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Services
{
    public partial interface IExamManager
    {
        Task<ExamTmSmall> GetSmallTmInfo(int tmId);
        Task<ExamTm> GetTmInfo(int tmId);
        Task GetTmInfo(ExamTm tm);
        Task GetTmDeleteInfo(ExamTm tm);
        Task GetTmInfoByPaper(ExamTm tm);
        Task GetTmInfoByPaperUser(ExamPaperRandomTm tm, ExamPaper paper, int startId, bool paperView = false);
        Task GetTmInfoByPaperAdmin(ExamPaperRandomTm tm, ExamPaper paper, int startId);
        Task GetTmInfoByPracticing(ExamTm tm);
        Task GetTmInfoByPracticeView(ExamTm tm, int practiceId);

        Task<List<Cascade<int>>> GetExamTmTreeCascadesAsync(AdminAuth auth, bool isTotal = false);
        Task<List<Cascade<int>>> GetExamPaperTreeCascadesAsync(AdminAuth auth, bool isTotal = false);
        Task<List<Cascade<int>>> GetKnowlegesTreeCascadesAsync(AdminAuth auth, bool isTotal = false);
        Task GetPaperInfo(ExamPaper paper, User user, int planId = 0, int courseId = 0, bool cjList = false);
        Task GetPaperInfo(ExamPaper paper, User user, ExamPaperStart start);
        Task<(bool Success, string msg)> CheckExam(int paperId, int userId);

    }

}
