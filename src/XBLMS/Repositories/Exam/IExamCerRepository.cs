using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IExamCerRepository : IRepository
    {
        Task<bool> ExistsAsync(int id);
        Task<ExamCer> GetAsync(int id);
        Task<List<ExamCer>> GetListAsync();
        Task<int> InsertAsync(ExamCer item);
        Task UpdateAsync(ExamCer item);
        Task DeleteAsync(int id);
        Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount();

    }
}
