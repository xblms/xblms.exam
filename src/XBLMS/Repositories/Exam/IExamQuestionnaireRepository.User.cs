using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IExamQuestionnaireRepository : IRepository
    {
        Task<(int total, List<ExamQuestionnaire> list)> GetListByUserAsync(List<int> paperIds, string keyWords, int pageIndex, int pageSize);
    }
}
