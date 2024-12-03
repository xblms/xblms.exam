using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Core.Services
{
    public partial class ExamManager
    {
        public async Task ClearExamAssessment(int assId)
        {
            await _examAssessmentAnswerRepository.ClearByPaperAsync(assId);
            await _examAssessmentTmRepository.DeleteByPaperAsync(assId);
            await _examAssessmentUserRepository.ClearByPaperAsync(assId);
        }

        public async Task SerExamAssessmentTm(List<ExamAssessmentTm> tmList, int assId)
        {
            if (tmList != null && tmList.Count > 0)
            {
                foreach (var tm in tmList)
                {
                    tm.ExamAssId = assId;
                    await _examAssessmentTmRepository.InsertAsync(tm);
                }
            }
        }

        public async Task ArrangerExamAssessment(ExamAssessment ass)
        {

            var userIds = new List<int>();

            if (ass.UserGroupIds != null && ass.UserGroupIds.Count > 0)
            {
                foreach (int groupId in ass.UserGroupIds)
                {
                    var group = await _userGroupRepository.GetAsync(groupId);
                    if (group != null)
                    {
                        if (group.GroupType == UsersGroupType.Fixed)
                        {
                            if (group.UserIds != null && group.UserIds.Count > 0)
                            {
                                userIds.AddRange(group.UserIds);
                            }

                        }
                        if (group.GroupType == Enums.UsersGroupType.Range)
                        {
                            var letUserIds = await _userRepository.GetUserIdsWithOutLockedAsync(group.CompanyIds, group.DepartmentIds, group.DutyIds);
                            if (letUserIds != null && letUserIds.Count > 0)
                            {
                                userIds.AddRange(letUserIds);
                            }
                        }
                        if (group.GroupType == UsersGroupType.All)
                        {
                            var letUserIds = await _userRepository.GetUserIdsWithOutLockedAsync();
                            if (letUserIds != null && letUserIds.Count > 0)
                            {
                                userIds.AddRange(letUserIds);
                            }
                        }
                    }

                }
            }

            if (userIds.Count > 0)
            {
                foreach (int userId in userIds)
                {
                    var exist = await _examAssessmentUserRepository.ExistsAsync(ass.Id, userId);
                    if (!exist)
                    {
                        await _examAssessmentUserRepository.InsertAsync(new ExamAssessmentUser
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
    }
}
