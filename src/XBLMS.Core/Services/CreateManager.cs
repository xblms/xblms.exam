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
        private readonly ISettingsManager _settingsManager;
        private readonly IDatabaseManager _databaseManager;

        public CreateManager(IPathManager pathManager, ITaskManager taskManager, IDatabaseManager databaseManager, ISettingsManager settingsManager)
        {
            _pathManager = pathManager;
            _taskManager = taskManager;
            _databaseManager = databaseManager;
            _settingsManager = settingsManager;
        }

        public void CreateSubmitAnswerAsync(ExamPaperAnswer answer)
        {
            var taskInfo = new CreateTask(CreateType.SubmitAnswer, answer);
            AddPendingTask(taskInfo);
        }
        public void CreateSubmitPaperAsync(int startId)
        {
            var taskInfo = new CreateTask(CreateType.SubmitPaper, startId);
            AddPendingTask(taskInfo);
        }
    }
}
