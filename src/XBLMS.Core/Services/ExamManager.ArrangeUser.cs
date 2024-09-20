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
                        userIds = group.UserIds;
                    }
                    if (group.GroupType == Enums.UsersGroupType.Range)
                    {
                        userIds = await _userRepository.GetUserIdsWithOutLockedAsync(group.CompanyIds, group.DepartmentIds, group.DutyIds);
                    }
                }
            }
            else
            {
                userIds = await _userRepository.GetUserIdsWithOutLockedAsync();
            }

            if (userIds.Count > 0)
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
                        });
                    }
                }
            }
        }
    }
}
