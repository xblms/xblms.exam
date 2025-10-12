using System;
using System.Threading;
using System.Threading.Tasks;

namespace XBLMS.Services
{
    public interface ITaskOtherManager
    {
        void Queue(Func<CancellationToken, Task> workItem);

        Task<Func<CancellationToken, Task>> DequeueAsync(
            CancellationToken cancellationToken);

        void RunOnceAt(Action job, DateTime dateTime);
    }
}
