using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public partial class ExamManager
    {
        public async Task<(int total, List<int>, List<int>)> PracticeGetTmids(User user, List<int> txIds, List<int> nds, List<string> zsds)
        {
            var tmGroupIds = new List<int>();
            var tmIds = new List<int>();
            var tmGroupList = await _databaseManager.ExamTmGroupRepository.GetListByOpenUserAsync();
            if (tmGroupList != null && tmGroupList.Count > 0)
            {
                var practiceGroup = new List<ExamTmGroup>();
                foreach (var tmGroup in tmGroupList)
                {
                    var userGroupIds = tmGroup.OpenUserGroupIds;

                    if (userGroupIds != null && userGroupIds.Count > 0)
                    {
                        foreach (int groupId in userGroupIds)
                        {
                            var group = await _databaseManager.UserGroupRepository.GetAsync(groupId);

                            if (group.GroupType == UsersGroupType.All)
                            {
                                practiceGroup.Add(tmGroup);
                                break;
                            }
                            if (group.GroupType == UsersGroupType.Fixed)
                            {
                                if (user.UserGroupIds != null && user.UserGroupIds.Contains($"'{group.Id}'"))
                                {
                                    practiceGroup.Add(tmGroup);
                                    break;
                                }
                            }
                            if (group.GroupType == UsersGroupType.Range)
                            {
                                var letUserIds = await _databaseManager.UserRepository.UserGroupGetUserIdsAsync(group);
                                if (letUserIds != null && letUserIds.Count > 0 && letUserIds.Contains(user.Id))
                                {
                                    practiceGroup.Add(tmGroup);
                                    break;
                                }
                            }

                        }
                    }
                }
                if (practiceGroup.Count > 0)
                {
                    foreach (var group in practiceGroup)
                    {
                        tmGroupIds.Add(group.Id);
                        var groupTmids = await _databaseManager.ExamTmRepository.Group_Practice_GetTmidsAsync(group, txIds, nds, zsds);
                        if (groupTmids != null)
                        {
                            tmIds.AddRange(groupTmids);
                        }
                    }

                    tmIds = tmIds.Distinct().ToList();
                }
            }
            return (tmIds.Count, tmIds, tmGroupIds);
        }

        public async Task<(int total, List<int>)> PracticeGetTmids(List<int> tmGroupIds, List<int> txIds, List<int> nds, List<string> zsds)
        {
            var tmIds = new List<int>();
            if (tmGroupIds != null && tmGroupIds.Count > 0)
            {
                foreach (var tmGroupId in tmGroupIds)
                {
                    var tmGroup = await _databaseManager.ExamTmGroupRepository.GetAsync(tmGroupId);

                    var groupTmids = await _databaseManager.ExamTmRepository.Group_Practice_GetTmidsAsync(tmGroup, txIds, nds, zsds);
                    if (groupTmids != null)
                    {
                        tmIds.AddRange(groupTmids);
                    }
                }
                tmIds = tmIds.Distinct().ToList();
            }
            return (tmIds.Count, tmIds);
        }

        public async Task GetTmInfoByPracticing(ExamTm tm)
        {
            await GetTmInfoByPaper(tm);
            tm.Answer = "";
            tm.Set("OptionsValues", new List<string>());
            GetTmOptionsRandom(tm);
        }

        public async Task GetTmInfoByPracticeView(ExamTm tm, int practiceId)
        {
            await GetTmInfoByPaper(tm);
            GetTmOptionsRandom(tm);
            await GetSmallTmListByPracticeView(tm, practiceId);
        }
        private async Task GetSmallTmListByPracticeView(ExamTm tm, int practiceId)
        {
            var tx = await _databaseManager.ExamTxRepository.GetAsync(tm.TxId);
            var smallTmList = new List<ExamTmSmall>();
            if (tx.ExamTxBase == ExamTxBase.Zuheti)
            {
                var smallList = await _databaseManager.ExamTmSmallRepository.GetListAsync(tm.Id);
                if (smallList != null && smallList.Count > 0)
                {
                    for (var i = 0; i < smallList.Count; i++)
                    {
                        var smallTm = smallList[i];
                        await GetSmallTm(smallTm);

                        var answer = await _databaseManager.ExamPracticeAnswerSmallRepository.GetAsync(smallTm.Id, practiceId);

                        smallTm.Set("AnswerInfo", answer);
                        smallTm.Set("IsRight", StringUtils.Equals(smallTm.Answer, answer.Answer));

                        smallTm.Set("TmIndex", i + 1);

                        smallTmList.Add(smallTm);
                    }
                }
            }
            tm.Set("SmallLists", smallTmList);
        }

    }
}
