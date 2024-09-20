using System.Collections.Generic;
using System.Threading.Tasks;
using Datory;
using XBLMS.Models;


namespace XBLMS.Repositories
{
    public interface IExamQuestionnaireTmRepository : IRepository
    {
        Task<ExamQuestionnaireTm> GetAsync(int Id);
        Task<int> InsertAsync(ExamQuestionnaireTm item);
        Task<int> DeleteByPaperAsync(int examPaperId);
        Task<List<ExamQuestionnaireTm>> GetListAsync(int examPaperId);
    }
}
