using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Core.Services
{
    public partial class ExamManager
    {
        public async Task Arrange(ExamPaper paper)
        {
            var userIds = new List<int>();

            if (paper.UserGroupIds != null && paper.UserGroupIds.Count > 0)
            {
                foreach (int groupId in paper.UserGroupIds)
                {
                    var group = await _userGroupRepository.GetAsync(groupId);
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

            if (userIds != null && userIds.Count > 0)
            {
                foreach (int userId in userIds)
                {
                    var exist = await _examPaperUserRepository.ExistsAsync(paper.Id, userId);
                    if (!exist)
                    {
                        await _examPaperUserRepository.InsertAsync(new ExamPaperUser
                        {
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
        public async Task Arrange(int paperId, int userId)
        {
            var paper = await _examPaperRepository.GetAsync(paperId);
            var exists = await _examPaperUserRepository.ExistsAsync(paperId, userId);
            if (!exists)
            {
                await _examPaperUserRepository.InsertAsync(new ExamPaperUser
                {
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
