using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IExamPkUserRepository : IRepository
    {
        Task<ExamPkUser> GetAsync(int pkId, int userId);
        Task<int> InsertAsync(ExamPkUser examPkUser);
        Task UpdateAsync(ExamPkUser examPkUser);
        Task DeleteByPkIdAsync(int pkId);
        Task<int> CountAsync(int pkId);
        Task<List<int>> GetUserIdsAsync(int pkId);
        Task<(int total, List<ExamPkUser> list)> GetListAsync(int pkId, string keyWords);
    }
}
