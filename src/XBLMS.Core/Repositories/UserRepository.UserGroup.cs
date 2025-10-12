using Datory;
using SqlKata;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Core.Repositories
{
    public partial class UserRepository
    {
        public async Task<int> UserGroupGetUserCountAsync(UserGroup userGroup)
        {
            var query = UserGroupQuery(userGroup);
            query.WhereNullOrFalse(nameof(User.Locked));
            return await _repository.CountAsync(query);
        }
        public async Task<List<int>> UserGroupGetUserIdsAsync(UserGroup userGroup)
        {
            var query = UserGroupQuery(userGroup);
            query.WhereNullOrFalse(nameof(User.Locked));
            return await _repository.GetAllAsync<int>(query.Select(nameof(User.Id)));
        }

        public async Task<(int total, List<User> list)> UserGroupRnageGetUserListAsync(AdminAuth auth, int organId, string organType, int groupId, int range, int dayOfLastActivity, string keyword, string order, int pageIndex, int pageSize)
        {
            var query = Q.NewQuery();

            if (range == 0)//待安排
            {
                query.Where(q =>
                {
                    q.WhereNullOrEmpty(nameof(User.UserGroupIds)).OrWhereNotLike(nameof(User.UserGroupIds), $"%'{groupId}'%");
                    return q;
                });
            }
            else
            {
                query.WhereNotNullOrEmpty(nameof(User.UserGroupIds)).WhereLike(nameof(User.UserGroupIds), $"%'{groupId}'%");
            }

            query = GetUserQuery(auth, query, organId, organType, dayOfLastActivity, keyword, order);

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }

        private static Query UserGroupQuery(UserGroup userGroup)
        {
            var query = Q.NewQuery();

            if (userGroup.GroupType == UsersGroupType.Fixed)
            {
                query.WhereLike(nameof(User.UserGroupIds), $"%'{userGroup.Id}'%");
            }
            else if (userGroup.GroupType == UsersGroupType.Range)
            {
                bool existWhere = false;

                if (userGroup.GroupRangeWithChildren)
                {
                    if (userGroup.CompanyIds != null && userGroup.CompanyIds.Count > 0)
                    {
                        existWhere = true;

                        query.Where(q =>
                        {
                            userGroup.CompanyIds.ForEach(cid =>
                            {
                                q.OrWhereLike(nameof(User.CompanyParentPath), $"%'{cid}'%");
                            });


                            if (userGroup.DepartmentIds != null && userGroup.DepartmentIds.Count > 0)
                            {
                                userGroup.DepartmentIds.ForEach(did =>
                                {
                                    q.OrWhereLike(nameof(User.DepartmentParentPath), $"%'{did}'%");
                                });


                                if (userGroup.DutyNames != null && userGroup.DutyNames.Count > 0)
                                {
                                    foreach (var dutyName in userGroup.DutyNames)
                                    {
                                        q.OrWhereLike(nameof(User.DutyName), $"%{dutyName}%");
                                    }
                                }
                            }


                            return q;
                        });

                    }
                    else if (userGroup.DepartmentIds != null && userGroup.DepartmentIds.Count > 0)
                    {
                        existWhere = true;

                        query.Where(q =>
                        {
                            userGroup.DepartmentIds.ForEach(did =>
                            {
                                q.OrWhereLike(nameof(User.DepartmentParentPath), $"%'{did}'%");
                            });

                            if (userGroup.DutyNames != null && userGroup.DutyNames.Count > 0)
                            {
                                foreach (var dutyName in userGroup.DutyNames)
                                {
                                    q.OrWhereLike(nameof(User.DutyName), $"%{dutyName}%");
                                }
                            }


                            return q;
                        });

                    }
                    else if (userGroup.DutyNames != null && userGroup.DutyNames.Count > 0)
                    {
                        existWhere = true;
                        query.Where(q =>
                        {
                            foreach (var dutyName in userGroup.DutyNames)
                            {
                                q.OrWhereLike(nameof(User.DutyName), $"%{dutyName}%");
                            }

                            return q;
                        });

                    }
                }
                else
                {
                    if (userGroup.CompanyIds != null && userGroup.CompanyIds.Count > 0)
                    {
                        existWhere = true;
                        query.WhereIn(nameof(User.CompanyId), userGroup.CompanyIds);
                    }
                    if (userGroup.DepartmentIds != null && userGroup.DepartmentIds.Count > 0)
                    {
                        existWhere = true;
                        query.WhereIn(nameof(User.DepartmentId), userGroup.DepartmentIds);
                    }
                    if (userGroup.DutyNames != null && userGroup.DutyNames.Count > 0)
                    {
                        existWhere = true;
                        foreach (var dutyName in userGroup.DutyNames)
                        {
                            query.Where(q =>
                            {
                                foreach (var dutyName in userGroup.DutyNames)
                                {
                                    q.OrWhereLike(nameof(User.DutyName), $"%{dutyName}%");
                                }

                                return q;
                            });
                        }
                    }
                }



                if (!existWhere)
                {
                    query.Where(nameof(User.Id), -1);
                }
            }
            return query;
        }
    }
}

