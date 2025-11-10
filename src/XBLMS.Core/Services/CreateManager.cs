using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Services;

namespace XBLMS.Core.Services
{
    public partial class CreateManager : ICreateManager
    {
        private readonly IPathManager _pathManager;
        private readonly ITaskManager _taskManager;
        private readonly IOrganManager _organManager;
        private readonly ISettingsManager _settingsManager;
        private readonly IDatabaseManager _databaseManager;

        public CreateManager(IPathManager pathManager, ITaskManager taskManager, IDatabaseManager databaseManager, ISettingsManager settingsManager, IOrganManager organManager)
        {
            _pathManager = pathManager;
            _taskManager = taskManager;
            _databaseManager = databaseManager;
            _settingsManager = settingsManager;
            _organManager = organManager;
        }

        public void CreateSubmitAnswerAsync(ExamPaperAnswer answer)
        {
            var taskInfo = new CreateTask(CreateType.SubmitAnswer, answer);
            AddPendingTask(taskInfo);
        }
        public void CreateSubmitAnswerSmallAsync(ExamPaperAnswerSmall answer)
        {
            var taskInfo = new CreateTask(CreateType.SubmitAnswerSmall, answer);
            AddPendingTask(taskInfo);
        }
        public void CreateSubmitPaperAsync(int taskId)
        {
            var taskInfo = new CreateTask(CreateType.SubmitPaper, taskId);
            AddPendingTask(taskInfo);
        }
        public void CreateExamAwardCerAsync(int taskId)
        {
            var taskInfo = new CreateTask(CreateType.ExamAwardCer, taskId);
            taskInfo.Wait = WaitingForExaming();
            AddPendingTask(taskInfo);
        }
    }
}
