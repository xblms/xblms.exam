using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Core.Services
{
    public partial class ScheduledHostedService
    {
        private async Task PingAsync(ScheduledTask task)
        {
            await Task.Delay(1000 * task.Every * 60);

            task.PingCounts = task.PingCounts + 1;
            await _databaseManager.ScheduledTaskRepository.UpdateAsync(task);

            await DbBackupAsync();
        }
    }
}
