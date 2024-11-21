using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Services
{
    public partial interface IExamManager
    {
        Task GetQuestionnaireInfo(ExamQuestionnaire paper, User user);
        Task ClearQuestionnaire(int examPaperId);
        Task ArrangeQuestionnaire(ExamQuestionnaire paper);
        Task SetQuestionnairTm(List<ExamQuestionnaireTm> tmList,int paperId);
    }

}
