using System.Collections.Generic;
using XBLMS.Dto;

namespace XBLMS.Services
{
    partial interface ICreateManager
    {
        void AddPendingTask(CreateTask task);

        int PendingTaskCount { get; }

        void ClearAllTask();

        CreateTaskSummary GetTaskSummary();
        List<int> GetTaskStartIds();
    }
}
