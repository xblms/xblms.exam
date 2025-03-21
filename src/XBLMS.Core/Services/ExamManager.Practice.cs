using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Core.Services
{
    public partial class ExamManager
    {
        public async Task<(int total, List<int>)> PracticeGetTmids(int userId, List<int> txIds, List<int> nds, List<string> zsds)
        {
            var tmIds = new List<int>();
            var tmGroupList = await _examTmGroupRepository.GetListWithoutLockedAsync();
            if (tmGroupList != null && tmGroupList.Count > 0)
            {
                var practiceGroup = new List<ExamTmGroup>();
                foreach (var tmGroup in tmGroupList)
                {
                    if (tmGroup.OpenUser)
                    {
                        var userGroupIds = tmGroup.OpenUserGroupIds;

                        if (userGroupIds != null && userGroupIds.Count > 0)
                        {
                            foreach (int groupId in userGroupIds)
                            {
                                var group = await _userGroupRepository.GetAsync(groupId);
                                if (group.GroupType == UsersGroupType.All)
                                {
                                    practiceGroup.Add(tmGroup);
                                    break;
                                }
                                if (group.GroupType == UsersGroupType.Fixed)
                                {
                                    if (group.UserIds != null && group.UserIds.Count > 0 && group.UserIds.Contains(userId))
                                    {
                                        practiceGroup.Add(tmGroup);
                                        break;
                                    }

                                }
                                if (group.GroupType == Enums.UsersGroupType.Range)
                                {
                                    var letUserIds = await _userRepository.GetUserIdsWithOutLockedAsync(group.CompanyIds, group.DepartmentIds, group.DutyIds);
                                    if (letUserIds != null && letUserIds.Count > 0 && letUserIds.Contains(userId))
                                    {
                                        practiceGroup.Add(tmGroup);
                                        break;
                                    }
                                }

                            }
                        }
                    }
                }
                if (practiceGroup.Count > 0)
                {
                    foreach (var group in practiceGroup)
                    {
                        var groupTmids = await _examTmRepository.Group_Practice_GetTmidsAsync(group, txIds, nds, zsds);
                        if (groupTmids != null)
                        {
                            tmIds.AddRange(groupTmids);
                        }
                    }

                    tmIds = tmIds.Distinct().ToList();
                }
            }
            return (tmIds.Count, tmIds);
        }
    }
}
