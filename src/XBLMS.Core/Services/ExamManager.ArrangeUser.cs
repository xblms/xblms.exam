using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Core.Services
{
    public partial class ExamManager
    {
        public void ArrangeExamTask(int paperId)
        {
            _taskManager.RunOnceAt(async () =>
            {
                await ArrangeExam(paperId);
            }, DateTime.Now);
        }
        private async Task ArrangeExam(int paperId)
        {
            var paper = await _databaseManager.ExamPaperRepository.GetAsync(paperId);

            if (paper.IsCourseUse)
            {
                return;
            }

            var userIds = await GetUserIdsByUserGroups(paper.UserGroupIds);

            if (userIds != null && userIds.Count > 0)
            {
                foreach (int userId in userIds)
                {
                    var exist = await _databaseManager.ExamPaperUserRepository.ExistsAsync(paper.Id, userId);
                    if (!exist)
                    {
                        await _databaseManager.ExamPaperUserRepository.InsertAsync(new ExamPaperUser
                        {
                            PlanId = 0,
                            CourseId = 0,
                            ExamTimes = paper.ExamTimes,
                            ExamBeginDateTime = paper.ExamBeginDateTime,
                            ExamEndDateTime = paper.ExamEndDateTime,
                            ExamPaperId = paper.Id,
                            UserId = userId,
                            KeyWordsAdmin = await _organManager.GetUserKeyWords(userId),
                            KeyWords = paper.Title,
                            Locked = paper.Locked,
                            Moni = paper.Moni
                        });
                    }
                }
            }
        }
        public void ArrangeAssessmentTask(int assId)
        {
            _taskManager.RunOnceAt(async () =>
            {
                await ArrangeAssessment(assId);
            }, DateTime.Now);
        }
        private async Task ArrangeAssessment(int assId)
        {
            var ass = await _databaseManager.ExamAssessmentRepository.GetAsync(assId);
            var userIds = await GetUserIdsByUserGroups(ass.UserGroupIds);

            if (userIds.Count > 0)
            {
                foreach (int userId in userIds)
                {
                    var exist = await _databaseManager.ExamAssessmentUserRepository.ExistsAsync(ass.Id, userId);
                    if (!exist)
                    {
                        await _databaseManager.ExamAssessmentUserRepository.InsertAsync(new ExamAssessmentUser
                        {
                            ExamAssId = ass.Id,
                            UserId = userId,
                            KeyWords = ass.Title,
                            KeyWordsAdmin = await _organManager.GetUserKeyWords(userId),
                            Locked = ass.Locked,
                            ExamBeginDateTime = ass.ExamBeginDateTime,
                            ExamEndDateTime = ass.ExamEndDateTime,
                        });
                    }
                }
            }
        }
        public void ArrangeQuestionnaireTask(int qId)
        {
            _taskManager.RunOnceAt(async () =>
            {
                await ArrangeQuestionnaire(qId);
            }, DateTime.Now);
        }
        private async Task ArrangeQuestionnaire(int qId)
        {
            var paper = await _databaseManager.ExamQuestionnaireRepository.GetAsync(qId);
            if (paper.Published || paper.IsCourseUse)
            {
                return;
            }
            var userIds = await GetUserIdsByUserGroups(paper.UserGroupIds);

            if (userIds.Count > 0)
            {
                foreach (int userId in userIds)
                {
                    var exist = await _databaseManager.ExamQuestionnaireUserRepository.ExistsAsync(paper.Id, userId);
                    if (!exist)
                    {
                        await _databaseManager.ExamQuestionnaireUserRepository.InsertAsync(new ExamQuestionnaireUser
                        {
                            PlanId = 0,
                            CourseId = 0,
                            ExamPaperId = paper.Id,
                            UserId = userId,
                            KeyWords = paper.Title,
                            KeyWordsAdmin = await _organManager.GetUserKeyWords(userId),
                            Locked = paper.Locked,
                            ExamBeginDateTime = paper.ExamBeginDateTime,
                            ExamEndDateTime = paper.ExamEndDateTime,
                        });
                    }
                }
            }
        }
        public async Task ArrangeOnlyOne(int paperId, int userId)
        {
            var paper = await _databaseManager.ExamPaperRepository.GetAsync(paperId);

            if (paper.IsCourseUse)
            {
                return;
            }

            var exists = await _databaseManager.ExamPaperUserRepository.ExistsAsync(paperId, userId);
            if (!exists)
            {
                await _databaseManager.ExamPaperUserRepository.InsertAsync(new ExamPaperUser
                {
                    PlanId = 0,
                    CourseId = 0,
                    ExamTimes = paper.ExamTimes,
                    ExamBeginDateTime = paper.ExamBeginDateTime,
                    ExamEndDateTime = paper.ExamEndDateTime,
                    ExamPaperId = paper.Id,
                    UserId = userId,
                    KeyWordsAdmin = await _organManager.GetUserKeyWords(userId),
                    KeyWords = paper.Title,
                    Locked = paper.Locked,
                    Moni = paper.Moni
                });
            }
        }


        public async Task<List<int>> GetUserIdsByUserGroups(List<int> userGroupIds)
        {
            var userIds = new List<int>();

            if (userGroupIds != null && userGroupIds.Count > 0)
            {
                var isAll = false;
                foreach (int groupId in userGroupIds)
                {
                    var group = await _databaseManager.UserGroupRepository.GetAsync(groupId);
                    if (group.GroupType == UsersGroupType.All)
                    {
                        isAll = true;
                        userIds = await _databaseManager.UserRepository.UserGroupGetUserIdsAsync(group);
                        break;
                    }
                }
                if (!isAll)
                {
                    foreach (int groupId in userGroupIds)
                    {
                        var group = await _databaseManager.UserGroupRepository.GetAsync(groupId);
                        var resultUserIds = await _databaseManager.UserRepository.UserGroupGetUserIdsAsync(group);
                        if (resultUserIds != null && resultUserIds.Count > 0)
                        {
                            userIds.AddRange(resultUserIds);
                        }
                    }
                }

            }
            return userIds.Distinct().ToList();
        }
        public async Task<List<int>> GetTmIdsByTmGroups(List<int> tmGroupIds, List<int> txIds = null)
        {
            var tmIds = new List<int>();

            if (tmGroupIds != null && tmGroupIds.Count > 0)
            {
                var isAll = false;
                foreach (int groupId in tmGroupIds)
                {
                    var group = await _databaseManager.ExamTmGroupRepository.GetAsync(groupId);
                    if (group.GroupType == TmGroupType.All)
                    {
                        isAll = true;
                        tmIds = await _databaseManager.ExamTmRepository.Group_GetTmIdsAsync(group, txIds);
                        break;
                    }
                }
                if (!isAll)
                {
                    foreach (int groupId in tmGroupIds)
                    {
                        var group = await _databaseManager.ExamTmGroupRepository.GetAsync(groupId);
                        var resultTmIds = await _databaseManager.ExamTmRepository.Group_GetTmIdsAsync(group, txIds);
                        if (resultTmIds != null && resultTmIds.Count > 0)
                        {
                            tmIds.AddRange(resultTmIds);
                        }
                    }
                }

            }
            return tmIds.Distinct().ToList();
        }
    }
}
