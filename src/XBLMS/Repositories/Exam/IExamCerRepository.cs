using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IExamCerRepository : IRepository
    {
        Task<List<ExamCer>> GetListAsync(AdminAuth auth, string keyWords = null);
        Task<bool> ExistsAsync(int id);
        Task<ExamCer> GetAsync(int id);
        Task<int> InsertAsync(ExamCer item);
        Task UpdateAsync(ExamCer item);
        Task DeleteAsync(int id);
        Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount(AdminAuth auth);

    }
}
