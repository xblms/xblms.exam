using System;
using System.Collections.Generic;
using System.Linq;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using XBLMS.Enums;

namespace XBLMS.Core.Services
{
    public partial class CreateManager
    {
        private static readonly List<CreateTask> PendingTasks = new List<CreateTask>();
        private static readonly List<CreateTaskLog> TaskLogs = new List<CreateTaskLog>();
        private static readonly object LockObject = new object();

        public void AddPendingTask(CreateTask task)
        {
            lock (LockObject)
            {
                PendingTasks.Insert(0, task);

                _taskManager.Queue(async token =>
                {
                    try
                    {
                        var start = DateTime.Now;
                        var timeSpan = DateUtils.GetRelatedDateTimeString(start);

                        if (task.CreateType == CreateType.SubmitAnswer)
                        {
                            await ExecuteSubmitAnswerAsync(task.ExamPaperAnswer);
                        }
                        if (task.CreateType == CreateType.SubmitPaper)
                        {
                            await ExecuteSubmitPaperAsync(task.StartId);
                        }

                        AddSuccessLog(task, timeSpan);
                    }
                    catch (Exception ex)
                    {
                        AddFailureLog(task, ex);
                    }
                    finally
                    {
                        RemovePendingTask(task);
                    }
                });
            }
        }

        public int PendingTaskCount => PendingTasks.Count;


        private void RemovePendingTask(CreateTask task)
        {
            lock (LockObject)
            {
                PendingTasks.Remove(task);
            }
        }

        private void AddSuccessLog(CreateTask task, string timeSpan)
        {
            var taskLog = new CreateTaskLog(task.CreateType, timeSpan, true, string.Empty, DateTime.Now);
            lock (LockObject)
            {
                if (TaskLogs.Count > 20)
                {
                    TaskLogs.RemoveAt(20);
                }
                TaskLogs.Insert(0, taskLog);
            }
        }

        private void AddFailureLog(CreateTask task, Exception ex)
        {
            var taskLog = new CreateTaskLog(task.CreateType, string.Empty, false, ex.Message, DateTime.Now);
            lock (LockObject)
            {
                if (TaskLogs.Count > 20)
                {
                    TaskLogs.RemoveAt(20);
                }
                TaskLogs.Insert(0, taskLog);
            }
        }

        public void ClearAllTask()
        {
            lock (LockObject)
            {
                PendingTasks.Clear();
            }
        }
        public List<int> GetTaskStartIds()
        {
            var ids = new List<int>();
            foreach (var taskInfo in PendingTasks.ToArray())
            {
                if (taskInfo.CreateType == CreateType.SubmitPaper)
                {
                    ids.Add(taskInfo.StartId);
                }
            }
            return ids;
        }


        public CreateTaskSummary GetTaskSummary()
        {
            var list = new List<CreateTaskSummaryItem>();
            var answerCount = 0;
            var paperCount = 0;
  

            foreach (var taskInfo in PendingTasks.ToArray())
            {
                if (taskInfo.CreateType == CreateType.SubmitAnswer)
                {
                    answerCount++;
                }
                else if (taskInfo.CreateType == CreateType.SubmitPaper)
                {
                    paperCount++;
                }

                var summaryItem = new CreateTaskSummaryItem(taskInfo, string.Empty, true, false, string.Empty);
                list.Add(summaryItem);
            }

            foreach (var logInfo in TaskLogs.ToList())
            {
                var summaryItem = new CreateTaskSummaryItem(logInfo);
                list.Add(summaryItem);
            }

            var summary = new CreateTaskSummary(list, answerCount, paperCount);

            return summary;
        }
    }
}
