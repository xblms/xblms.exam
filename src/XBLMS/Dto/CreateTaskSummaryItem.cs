using Datory;

namespace XBLMS.Dto
{
    public class CreateTaskSummaryItem
    {
        public CreateTaskSummaryItem(CreateTask task, string timeSpan, bool isPending, bool isSuccess, string errorMessage)
        {
            type = task.CreateType.GetDisplayName();
            this.timeSpan = timeSpan;
            this.isPending = isPending;
            this.isSuccess = isSuccess;
            this.errorMessage = errorMessage;
        }

        public CreateTaskSummaryItem(CreateTaskLog log)
        {
            type = log.CreateType.GetDisplayName();
            timeSpan = log.TimeSpan;
            isPending = false;
            isSuccess = log.IsSuccess;
            errorMessage = log.ErrorMessage;
        }

        public string type { get; set; }
        public string timeSpan { get; set; }
        public bool isPending { get; set; }
        public bool isSuccess { get; set; }
        public string errorMessage { get; set; }
    }
}
