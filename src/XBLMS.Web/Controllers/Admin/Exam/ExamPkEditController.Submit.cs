using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamPkEditController
    {
        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(Route)]
        public async Task<ActionResult<BoolResult>> Submit([FromBody] GetSubmitRequest request)
        {
            if (request.Item.Id > 0)
            {
                if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
                {
                    return this.NoAuth();
                }
            }
            else
            {
                if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Add))
                {
                    return this.NoAuth();
                }
            }

            var vsList = new List<int>();
            vsList.AddRange([2, 4, 8, 16, 32, 64, 128, 256, 512]);

            var adminAuth = await _authManager.GetAdminAuth();
            var admin = adminAuth.Admin;
            var pk = request.Item;


            if (pk.Id > 0)
            {
                var last = await _examPkRepository.GetAsync(pk.Id);
                await _examPkRepository.UpdateAsync(pk);
                await _authManager.AddAdminLogAsync("修改竞赛", $"{pk.Name}");
                await _authManager.AddStatLogAsync(StatType.ExamPkUpdate, "修改竞赛", pk.Id, pk.Name, last);
            }
            else
            {
                var userGroups = new List<int> { pk.UserGroupId };
                var userIds = await _examManager.GetUserIdsByUserGroups(userGroups);
                if (userIds != null && userIds.Count > 0)
                {
                    if (!vsList.Contains(userIds.Count))
                    {
                        return this.Error($"参赛者数量必须是（{ListUtils.ToString(vsList)}）其中一组，最少2人，最多512人。");
                    }
                }
                else
                {
                    return this.Error("参赛者数量为0");
                }

                var objectiveTxIds = await _examTxRepository.GetIdsAsync(true);
                var tmIds = await _examManager.GetTmIdsByTmGroups(pk.TmGroupIds, objectiveTxIds);

                if (tmIds == null || tmIds.Count == 0)
                {
                    return this.Error("竞赛题目不能为空且必须是客观题，请检查题目组");
                }

                var pkTmids = tmIds;
                pk.CompanyId = adminAuth.CurCompanyId;
                pk.CreatorId = admin.Id;
                pk.DepartmentId = admin.DepartmentId;
                pk.CompanyParentPath = adminAuth.CompanyParentPath;
                pk.DepartmentParentPath = admin.DepartmentParentPath;
                pk.Vs = 0;
                pk.Current = 0;
                pk.Mark = "";


                var pkId = await _examPkRepository.InsertAsync(pk);
                pk.Id = pkId;

                foreach (var pkuserId in userIds)
                {
                    await _examPkUserRepository.InsertAsync(new ExamPkUser
                    {
                        CompanyId = admin.CompanyId,
                        DepartmentId = admin.DepartmentId,
                        CreatorId = admin.Id,
                        UserId = pkuserId,
                        PkId = pk.Id,
                        KeyWords = pk.Name,
                        KeyWordsAdmin = await _organManager.GetUserKeyWords(pkuserId),
                    });
                }
                if (pkTmids.Count > 100)
                {
                    pkTmids = pkTmids.OrderBy(o => StringUtils.Guid()).ToList();
                    pkTmids = ListUtils.GetRandomList(pkTmids, 100);
                }
                else
                {
                    SetPkTms(pkTmids);
                }

                await SetPk(pk, userIds, vsList, pkTmids, 1);
                await _authManager.AddAdminLogAsync("新增竞赛", $"{pk.Name}");
                await _authManager.AddStatLogAsync(StatType.ExamPkAdd, "新增竞赛", pk.Id, pk.Name);
                await _authManager.AddStatCount(StatType.ExamPkAdd);
            }



            return new BoolResult
            {
                Value = true
            };
        }

        private void SetPkTms(List<int> tmIds)
        {
            if (tmIds.Count < 100)
            {
                tmIds.AddRange(tmIds);
                if (tmIds.Count > 100)
                {
                    tmIds = ListUtils.GetRandomList(tmIds, 100);
                }
                else
                {
                    SetPkTms(tmIds);
                }
            }
        }
        private async Task SetFirstGroup(List<int> userIds, ExamPk parent, List<int> tmIds)
        {
            userIds = userIds.OrderBy(o => StringUtils.Guid()).ToList();

            var group = ListUtils.GetRandomList(userIds, 2);

            await _examPkRoomRepository.InsertAsync(new ExamPkRoom
            {
                CompanyId = parent.CompanyId,
                DepartmentId = parent.DepartmentId,
                CreatorId = parent.CreatorId,
                TmIds = tmIds,
                UserId_A = group[0],
                UserId_B = group[1],
                PkId = parent.Id,
                State_A = PkRoomUserState.OffLine,
                State_B = PkRoomUserState.OffLine,
            });

            userIds.Remove(group[0]);
            userIds.Remove(group[1]);

            if (userIds.Count > 0)
            {
                await SetFirstGroup(userIds, parent, tmIds);
            }

        }

        private async Task SetPk(ExamPk parent, List<int> userIds, List<int> vsList, List<int> tmIds, int current)
        {
            var userCount = userIds.Count;
            var vsIndex = vsList.IndexOf(userCount);

            var mark = "决赛";
            if (userCount == 2)
            {
                mark = $"决赛";
            }
            else if (userCount > 16)
            {
                mark = $"淘汰赛（{userCount}进{(userCount / 2)}）";
            }
            else
            {
                mark = $"{userCount / 2}强";
            }


            var childPk = new ExamPk()
            {
                Name = $"{parent.Name}",
                Mark = mark,
                CompanyId = parent.CompanyId,
                DepartmentId = parent.DepartmentId,
                CreatorId = parent.CreatorId,
                ParentId = parent.Id,
                PkBeginDateTime = parent.PkBeginDateTime,
                PkEndDateTime = parent.PkEndDateTime,
                Vs = userCount,
                Current = current
            };
            var childPkId = await _examPkRepository.InsertAsync(childPk);
            childPk.Id = childPkId;

            if (current == 1)
            {
                var groupUserIds = new List<int>();
                groupUserIds.AddRange(userIds);

                await SetFirstGroup(groupUserIds, childPk, tmIds);

                if (vsIndex > 0)
                {
                    current++;
                    userIds = ListUtils.GetRandomList(userIds, userCount / 2);
                    await SetPk(parent, userIds, vsList, tmIds, current);
                }
            }
            else
            {
                if (vsIndex == 0)
                {
                    await SetPk_banjuesai(parent, userIds, vsList, tmIds, current);
                }


                var groupCount = userCount / 2;
                for (var i = 0; i < groupCount; i++)
                {
                    await _examPkRoomRepository.InsertAsync(new ExamPkRoom
                    {
                        CompanyId = parent.CompanyId,
                        DepartmentId = parent.DepartmentId,
                        CreatorId = parent.CreatorId,
                        TmIds = tmIds,
                        UserId_A = 0,
                        UserId_B = 0,
                        PkId = childPk.Id,
                        State_A = PkRoomUserState.OffLine,
                        State_B = PkRoomUserState.OffLine,
                    });
                }


                if (vsIndex > 0)
                {
                    current++;
                    userIds = ListUtils.GetRandomList(userIds, userIds.Count / 2);
                    await SetPk(parent, userIds, vsList, tmIds, current);
                }
            }
        }

        /// <summary>
        /// 半决赛 争夺第三名
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="userIds"></param>
        /// <param name="vsList"></param>
        /// <param name="tmIds"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        private async Task SetPk_banjuesai(ExamPk parent, List<int> userIds, List<int> vsList, List<int> tmIds, int current)
        {
            SetPkTms(tmIds);

            var userCount = userIds.Count;
            var vsIndex = vsList.IndexOf(userCount);

            var mark = $"半决赛";

            var childPk = new ExamPk()
            {
                Name = $"{parent.Name}",
                Mark = mark,
                CompanyId = parent.CompanyId,
                DepartmentId = parent.DepartmentId,
                CreatorId = parent.CreatorId,
                ParentId = parent.Id,
                PkBeginDateTime = parent.PkBeginDateTime,
                PkEndDateTime = parent.PkEndDateTime,
                Vs = 3,
                Current = current
            };
            var childPkId = await _examPkRepository.InsertAsync(childPk);
            childPk.Id = childPkId;

            await _examPkRoomRepository.InsertAsync(new ExamPkRoom
            {
                CompanyId = parent.CompanyId,
                DepartmentId = parent.DepartmentId,
                CreatorId = parent.CreatorId,
                TmIds = tmIds,
                UserId_A = 0,
                UserId_B = 0,
                PkId = childPk.Id,
                State_A = PkRoomUserState.OffLine,
                State_B = PkRoomUserState.OffLine,
            });
        }
    }
}
