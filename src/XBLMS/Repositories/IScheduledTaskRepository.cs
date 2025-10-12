using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IScheduledTaskRepository : IRepository
    {
        Task<bool> ExistsPingTask();
        Task<ScheduledTask> GetAsync(int id);

        Task<List<ScheduledTask>> GetAllAsync();

        Task<ScheduledTask> GetNextAsync();

        Task<int> InsertAsync(ScheduledTask task);

        Task UpdateAsync(ScheduledTask task);

        Task<bool> DeleteAsync(int id);
    }
}
