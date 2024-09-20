using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Services
{
    public partial interface IExamManager
    {
        Task<ExamTm> GetTmInfo(int tmId);
        Task GetTmInfo(ExamTm tm);
        Task GetTmInfoByPaper(ExamTm tm);
        Task GetTmInfoByPaperUser(ExamPaperRandomTm tm, ExamPaper paper, int startId, bool paperView = false);
        Task GetTmInfoByPracticing(ExamTm tm);

        Task<List<Cascade<int>>> GetExamTmTreeCascadesAsync(bool isTotal = false);
        Task<List<Cascade<int>>> GetExamPaperTreeCascadesAsync(bool isTotal = false);
        Task GetPaperInfo(ExamPaper paper, User user, bool cjList = false);
        Task<(bool Success, string msg)> CheckExam(int paperId, int userId);

    }

}
