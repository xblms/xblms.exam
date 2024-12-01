using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IExamPkRoomRepository : IRepository
    {
        Task<ExamPkRoom> GetAsync(int id);
        Task<int> InsertAsync(ExamPkRoom examPkRoom);
        Task UpdateAsync(ExamPkRoom examPkRoom);
        Task RandomPositonAsync(int pkId, int userId);
        Task DeleteAsync(int id);
        Task DeleteByPkIdAsync(int pkId);
        Task<(int total, List<ExamPkRoom> list)> GetListAsync(int pkId);
        Task<(int total, List<ExamPkRoom> list)> GetUserListAsync(int userId, int pageIndex, int pageSize);
    }
}
