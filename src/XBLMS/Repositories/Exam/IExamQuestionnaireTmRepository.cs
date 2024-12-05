using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;


namespace XBLMS.Repositories
{
    public interface IExamQuestionnaireTmRepository : IRepository
    {
        Task<int> InsertAsync(ExamQuestionnaireTm item);
        Task<int> DeleteByPaperAsync(int examPaperId);
        Task<List<ExamQuestionnaireTm>> GetListAsync(int examPaperId);
    }
}
