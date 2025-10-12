using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Core.Services
{
    public partial class StudyManager
    {
        public async Task PlanArrangeUser(StudyPlan plan, AdminAuth auth)
        {
            var userIds = await GetUserIdsByUserGroups(plan.UserGroupIds);

            if (userIds != null && userIds.Count > 0)
            {
                userIds = userIds.Distinct().ToList();

                var planCourseList = await _studyPlanCourseRepository.GetListAsync(plan.Id);
                foreach (int userId in userIds)
                {
                    var exist = await _studyPlanUserRepository.ExistsAsync(plan.Id, userId);
                    if (!exist)
                    {
                        var user = await _userRepository.GetByUserIdAsync(userId);

                        await _studyPlanUserRepository.InsertAsync(new StudyPlanUser
                        {
                            State = StudyStatType.Weikaishi,
                            PlanId = plan.Id,
                            PlanYear = plan.PlanYear,
                            Credit = plan.PlanCredit,
                            UserId = user.Id,
                            KeyWordsAdmin = await _organManager.GetUserKeyWords(userId),
                            KeyWords = plan.PlanName,
                            PlanBeginDateTime = plan.PlanBeginDateTime,
                            PlanEndDateTime = plan.PlanEndDateTime,
                            Locked = plan.Locked,
                            CompanyId = user.CompanyId,
                            DepartmentId = user.DepartmentId,
                            CreatorId = user.Id,
                            CompanyParentPath = user.CompanyParentPath,
                            DepartmentParentPath = user.DepartmentParentPath,
                        });

                        if (planCourseList != null && planCourseList.Count > 0)
                        {
                            foreach (var planCourse in planCourseList)
                            {
                                if (planCourse.OffLine)
                                {
                                    await _studyCourseUserRepository.InsertAsync(new StudyCourseUser
                                    {
                                        UserId = user.Id,
                                        PlanId = plan.Id,
                                        CourseId = planCourse.CourseId,
                                        IsSelectCourse = planCourse.IsSelectCourse,
                                        CompanyId = user.CompanyId,
                                        DepartmentId = user.DepartmentId,
                                        CreatorId = user.Id,
                                        State = StudyStatType.Weikaishi,
                                        TotalDuration = 0,
                                        BeginStudyDateTime = DateTime.Now,
                                        LastStudyDateTime = DateTime.Now,
                                        KeyWordsAdmin = await _organManager.GetUserKeyWords(user.Id),
                                        KeyWords = planCourse.CourseName,
                                        CompanyParentPath = user.CompanyParentPath,
                                        DepartmentParentPath = user.DepartmentParentPath,
                                        OffLine = true,
                                    });
                                    await _studyCourseRepository.IncrementTotalUserAsync(planCourse.CourseId);
                                }
                            }
                        }
                    }
                }
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
                    var group = await _userGroupRepository.GetAsync(groupId);
                    if (group.GroupType == UsersGroupType.All)
                    {
                        isAll = true;
                        userIds = await _userRepository.UserGroupGetUserIdsAsync(group);
                        break;
                    }
                }
                if (!isAll)
                {
                    foreach (int groupId in userGroupIds)
                    {
                        var group = await _userGroupRepository.GetAsync(groupId);
                        var resultUserIds = await _userRepository.UserGroupGetUserIdsAsync(group);
                        if (resultUserIds != null && resultUserIds.Count > 0)
                        {
                            userIds.AddRange(resultUserIds);
                        }
                    }
                }

            }
            return userIds.Distinct().ToList();
        }
    }
}
