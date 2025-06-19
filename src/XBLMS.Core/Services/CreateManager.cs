using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
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


        public void CreateSubmitAnswerSmallAsync(ExamPaperAnswerSmall answer)
        {
            var taskInfo = new CreateTask(CreateType.SubmitAnswer, answer);
            AddPendingTask(taskInfo);
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
