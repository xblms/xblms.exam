using Datory;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Web.Controllers.Home
{
    public partial class DoTeskController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get()
        {
            var user = await _authManager.GetUserAsync();
            if (user == null) return Unauthorized();

            var config = await _configRepository.GetAsync();

            var result = new GetResult();
            result.Total = 0;
            result.List = new List<GetResultInfo>();

            var taskList = new List<DoTask>();
            var taskTotal = 0;

            var (taskExamTotal, taskExamList) = await _examPaperUserRepository.GetListByUserTask(user.Id);
            if (taskExamTotal > 0)
            {
                foreach (var taskExam in taskExamList)
                {
                    var paper = await _examPaperRepository.GetAsync(taskExam.ExamPaperId);
                    if (paper != null)
                    {
                        result.Total++;
                        taskTotal++;
                        taskList.Add(new DoTask
                        {
                            TaskId = paper.Id,
                            TaskType = TaskType.Exam,
                            TaskTypeName = TaskType.Exam.GetDisplayName(),
                            TaskTitle = paper.Title,
                            TaskBeginDateTime = taskExam.ExamBeginDateTime.Value.ToString(DateUtils.FormatStringDateTimeCN),
                            TaskEndDateTime = taskExam.ExamEndDateTime.Value.ToString(DateUtils.FormatStringDateTimeCN)
                        });
                    }
                }
                result.List.Add(new GetResultInfo
                {
                    TaskName = TaskType.Exam.GetDisplayName(),
                    TaskTotal = taskTotal,
                    TaskList = taskList
                });
                taskTotal = 0;
                taskList = new List<DoTask>();
            }


            var (taskExamqTotal, taskExamqList) = await _examQuestionnaireUserRepository.GetTaskAsync(user.Id);
            if (taskExamqTotal > 0)
            {
                foreach (var item in taskExamqList)
                {
                    var paper = await _examQuestionnaireRepository.GetAsync(item.ExamPaperId);
                    if (paper != null)
                    {
                        result.Total++;
                        taskTotal++;
                        taskList.Add(new DoTask
                        {
                            TaskId = paper.Id,
                            TaskType = TaskType.ExamQ,
                            TaskTypeName = TaskType.ExamQ.GetDisplayName(),
                            TaskTitle = paper.Title,
                            TaskBeginDateTime = item.ExamBeginDateTime.Value.ToString(DateUtils.FormatStringDateTimeCN),
                            TaskEndDateTime = item.ExamEndDateTime.Value.ToString(DateUtils.FormatStringDateTimeCN)
                        });
                    }
                }
                result.List.Add(new GetResultInfo
                {
                    TaskName = TaskType.ExamQ.GetDisplayName(),
                    TaskTotal = taskTotal,
                    TaskList = taskList
                });
                taskTotal = 0;
                taskList = new List<DoTask>();
            }


            var (assesstantTaskTotal, assesstantTaskList) = await _examAssessmentUserRepository.GetTaskAsync(user.Id);
            if (assesstantTaskTotal > 0)
            {
                foreach (var item in assesstantTaskList)
                {
                    var paper = await _examAssessmentRepository.GetAsync(item.ExamAssId);
                    if (paper != null)
                    {
                        result.Total++;
                        taskTotal++;
                        taskList.Add(new DoTask
                        {
                            TaskId = paper.Id,
                            TaskType = TaskType.ExamAss,
                            TaskTypeName = TaskType.ExamAss.GetDisplayName(),
                            TaskTitle = paper.Title,
                            TaskBeginDateTime = item.ExamBeginDateTime.Value.ToString(DateUtils.FormatStringDateTimeCN),
                            TaskEndDateTime = item.ExamEndDateTime.Value.ToString(DateUtils.FormatStringDateTimeCN)
                        });
                    }
                }
                result.List.Add(new GetResultInfo
                {
                    TaskName = TaskType.ExamAss.GetDisplayName(),
                    TaskTotal = taskTotal,
                    TaskList = taskList
                });
                taskTotal = 0;
                taskList = new List<DoTask>();
            }

            if (config.SystemCode == SystemCode.Elearning)
            {
                var (planTotal, planList) = await _studyPlanUserRepository.GetTaskListAsync(user.Id);
                var planUser = new StudyPlanUser();
                if (planTotal > 0)
                {
                    for (var i = 0; i < planList.Count; i++)
                    {
                        var item = planList[i];
                        var plan = await _studyPlanRepository.GetAsync(item.PlanId);
                        if (plan != null)
                        {
                            result.Total++;
                            taskTotal++;
                            taskList.Add(new DoTask
                            {
                                TaskId = item.Id,
                                TaskType = TaskType.StudyPlan,
                                TaskTypeName = TaskType.StudyPlan.GetDisplayName(),
                                TaskTitle = plan.PlanName,
                                TaskBeginDateTime = plan.PlanBeginDateTime.Value.ToString(DateUtils.FormatStringDateTimeCN),
                                TaskEndDateTime = plan.PlanEndDateTime.Value.ToString(DateUtils.FormatStringDateTimeCN)
                            });
                        }
                    }
                    result.List.Add(new GetResultInfo
                    {
                        TaskName = TaskType.StudyPlan.GetDisplayName(),
                        TaskTotal = taskTotal,
                        TaskList = taskList
                    });
                }
            }
          
            return result;
        }
    }
}
