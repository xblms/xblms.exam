using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Core.Services
{
    public partial class ExamManager
    {
        public async Task ClearQuestionnaire(int examPaperId)
        {
            await _examQuestionnaireAnswerRepository.ClearByPaperAsync(examPaperId);
            await _examQuestionnaireTmRepository.DeleteByPaperAsync(examPaperId);
            await _examQuestionnaireUserRepository.ClearByPaperAsync(examPaperId);
        }

        public async Task SetQuestionnairTm(List<ExamQuestionnaireTm> tmList, int paperId)
        {
            if (tmList != null && tmList.Count > 0)
            {
                var parentId = 0;
                foreach (var tm in tmList)
                {
                    tm.Remove("TmIndex");
                    if (tm.ParentId > 0)
                    {
                        tm.Remove("Answer");
                        tm.Remove("Answers");
                        tm.ParentId = parentId;
                        tm.ExamPaperId = paperId;
                        var tmId = await _examQuestionnaireTmRepository.InsertAsync(tm);
                    }
                    else
                    {
                        tm.Remove("SmallList");
                        tm.ExamPaperId = paperId;
                        var tmId = await _examQuestionnaireTmRepository.InsertAsync(tm);
                        if (tm.Tx == ExamQuestionnaireTxType.DanxuantiErwei || tm.Tx == ExamQuestionnaireTxType.DuoxuantiErwei || tm.Tx == ExamQuestionnaireTxType.DanxuantiSanwei || tm.Tx == ExamQuestionnaireTxType.DuoxuantiSanwei)
                        {
                            parentId = tmId;
                        }
                        else
                        {
                            parentId = 0;
                        }
                    }
                }
            }
        }

        public async Task ArrangeQuestionnaire(ExamQuestionnaire paper)
        {
            if (paper.Published)
            {
                return;
            }
            var userIds = new List<int>();

            if (paper.UserGroupIds != null && paper.UserGroupIds.Count > 0)
            {
                foreach (int groupId in paper.UserGroupIds)
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
                    var exist = await _examQuestionnaireUserRepository.ExistsAsync(paper.Id, userId);
                    if (!exist)
                    {
                        await _examQuestionnaireUserRepository.InsertAsync(new ExamQuestionnaireUser
                        {
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
    }
}
