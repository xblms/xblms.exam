using Datory;
using SqlKata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;


namespace XBLMS.Core.Repositories
{
    public partial class ExamTmRepository : IExamTmRepository
    {
        private readonly ICacheManager _cacheManager;
        private readonly Repository<ExamTm> _repository;

        public ExamTmRepository(ISettingsManager settingsManager, ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
            _repository = new Repository<ExamTm>(settingsManager.Database, settingsManager.Redis);
        }
        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;
        public async Task<List<ExamTm>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _repository.ExistsAsync(id);
        }
        public async Task<bool> ExistsAsync(string title, int txId)
        {
            return await _repository.ExistsAsync(Q.Where(nameof(ExamTm.TxId), txId).Where(nameof(ExamTm.Title), title));
        }

        public async Task<int> InsertAsync(ExamTm item)
        {
            return await _repository.InsertAsync(item);
        }

        public async Task<bool> UpdateAsync(ExamTm item)
        {
            return await _repository.UpdateAsync(item);
        }
        public async Task UpdateTmGroupIdsAsync(ExamTm item)
        {
            await _repository.UpdateAsync(Q.Set(nameof(ExamTm.TmGroupIds), item.TmGroupIds).Where(nameof(ExamTm.Id), item.Id));
        }
        public async Task UpdateTmGroupIdsAsync(int groupId)
        {
            var tmList = await _repository.GetAllAsync(Q.WhereNullOrEmpty(nameof(ExamTm.TmGroupIds)).WhereLike(nameof(ExamTm.TmGroupIds), $"%'{groupId}'%"));
            if (tmList != null && tmList.Count > 0)
            {
                foreach (var tm in tmList)
                {
                    tm.TmGroupIds.Remove($"'{groupId}'");
                    await UpdateTmGroupIdsAsync(tm);
                }
            }
        }
        public async Task<(int total, List<ExamTm> list)> GetSelectListAsync(AdminAuth auth, int groupId, bool isSelect, List<int> treeIds, int txId, int nandu, string keyword, string order, string orderType, int pageIndex, int pageSize)
        {
            var query = Q.NewQuery();

            if (isSelect)//已安排
            {
                query.WhereNotNullOrEmpty(nameof(ExamTm.TmGroupIds)).WhereLike(nameof(ExamTm.TmGroupIds), $"%'{groupId}'%");
            }
            else
            {
                query.Where(q =>
                {
                    q.WhereNullOrEmpty(nameof(ExamTm.TmGroupIds)).OrWhereNotLike(nameof(ExamTm.TmGroupIds), $"%'{groupId}'%");
                    return q;
                });
            }

            query = GetQueryByAuth(query, auth);
            query = GetQuery(query, null, treeIds, txId, nandu, keyword, order, orderType);
            var count = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (count, list);
        }

        public async Task<(int total, List<ExamTm> list)> GetListAsync(AdminAuth auth, ExamTmGroup group, List<int> treeIds, int txId, int nandu, string keyword, string order, string orderType, bool? isStop, int pageIndex, int pageSize)
        {
            var query = Q.NewQuery();

            query = GetQueryByAuth(query, auth);
            query = GetQuery(query, group, treeIds, txId, nandu, keyword, order, orderType);
            var count = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (count, list);
        }

        private Query GetQueryByAuth(Query query, AdminAuth auth)
        {
            if (auth.AuthDataType == AuthorityDataType.DataCreator)
            {
                query.Where(nameof(Role.CreatorId), auth.AdminId);
            }
            else
            {
                if (auth.AuthDataShowAll)
                {
                    if (auth.CurCompanyId != 1)
                    {
                        query.WhereLike(nameof(Role.CompanyParentPath), $"%'{auth.CurCompanyId}'%");
                    }
                }
                else
                {
                    query.Where(nameof(Role.CompanyId), auth.CurCompanyId);
                }
            }

            return query;
        }
        private Query GetQuery(Query query, ExamTmGroup group, List<int> treeIds, int txId, int nandu, string keyword, string order, string orderType)
        {
            if (group != null)
            {
                if (group.GroupType == TmGroupType.Fixed)
                {
                    query.WhereLike(nameof(ExamTm.TmGroupIds), $"%'{group.Id}'%");
                }
                if (group.GroupType == TmGroupType.Range)
                {
                    query = Group_GetRangeQuery(group, query);
                }
            }

            if (treeIds != null && treeIds.Count > 0)
            {
                query.WhereIn(nameof(ExamTm.TreeId), treeIds);
            }
            if (txId > 0)
            {
                query.Where(nameof(ExamTm.TxId), txId);
            }
            if (nandu > 0)
            {
                query.Where(nameof(ExamTm.Nandu), nandu);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                var like = $"%{keyword}%";
                query.Where(q => q
                    .WhereLike(nameof(ExamTm.Title), like)
                    .OrWhereLike(nameof(ExamTm.Zhishidian), like)
                    .OrWhereLike(nameof(ExamTm.Jiexi), like)
                    .OrWhereLike(nameof(ExamTm.Answer), like)
                );
            }
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "nandu")
                {
                    if (orderType == OrderType.asc.ToString())
                    {
                        query.OrderBy(nameof(ExamTm.Nandu));
                    }
                    else
                    {
                        query.OrderByDesc(nameof(ExamTm.Nandu));
                    }
                }
                if (order == "score")
                {
                    if (orderType == OrderType.asc.ToString())
                    {
                        query.OrderBy(nameof(ExamTm.Score));
                    }
                    else
                    {
                        query.OrderByDesc(nameof(ExamTm.Score));
                    }
                }
                if (order == "tx")
                {
                    if (orderType == OrderType.asc.ToString())
                    {
                        query.OrderBy(nameof(ExamTm.TxId));
                    }
                    else
                    {
                        query.OrderByDesc(nameof(ExamTm.TxId));
                    }
                }
            }
            else
            {
                if (orderType == OrderType.asc.ToString())
                {
                    query.OrderBy(nameof(ExamTm.Id));
                }
                else
                {
                    query.OrderByDesc(nameof(ExamTm.Id));
                }
            }


            return query;
        }


        public async Task<ExamTm> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
            return await _repository.DeleteAsync(id);
        }
        public async Task<int> GetCountByTxIdAsync(int txId)
        {
            return await _repository.CountAsync(Q.Where(nameof(ExamTm.TxId), txId));
        }

        private static Query GetTmGroupQuery(List<int> treeIds, List<int> txIds, List<int> nandus, List<string> zhishidianKeywords, DateTime? dateFrom, DateTime? dateTo)
        {
            var query = Q.NewQuery();

            if (treeIds != null && treeIds.Count > 0)
            {
                query.WhereIn(nameof(ExamTm.TreeId), treeIds);
            }
            if (txIds != null && txIds.Count > 0)
            {
                query.WhereIn(nameof(ExamTm.TxId), txIds);
            }
            if (nandus != null && nandus.Count > 0)
            {
                query.WhereIn(nameof(ExamTm.Nandu), nandus);
            }
            if (zhishidianKeywords != null && zhishidianKeywords.Count > 0)
            {
                query.Where(q =>
                {
                    foreach (var zhishidian in zhishidianKeywords)
                    {
                        var like = $"%{zhishidian}%";
                        q.OrWhereLike(nameof(ExamTm.Zhishidian), like);
                    }
                    return q;
                });
            }
            if (dateFrom.HasValue)
            {
                query.Where(nameof(ExamTm.CreatedDate), ">=", DateUtils.ToString(dateFrom));
            }
            if (dateTo.HasValue)
            {
                query.Where(nameof(ExamTm.CreatedDate), "<=", DateUtils.ToString(dateTo));
            }
            return query;
        }
        public async Task<int> GetCountAsync(List<int> treeIds, List<int> txIds, List<int> nandus, List<string> zhishidianKeywords, DateTime? dateFrom, DateTime? dateTo)
        {
            var query = GetTmGroupQuery(treeIds, txIds, nandus, zhishidianKeywords, dateFrom, dateTo);
            query.WhereNullOrFalse(nameof(ExamTm.Locked));

            return await _repository.CountAsync(query);
        }
        public async Task<List<int>> GetIdsAsync(List<int> treeIds, List<int> txIds, List<int> nandus, List<string> zhishidianKeywords, DateTime? dateFrom, DateTime? dateTo)
        {
            var query = GetTmGroupQuery(treeIds, txIds, nandus, zhishidianKeywords, dateFrom, dateTo);
            query.WhereNullOrFalse(nameof(ExamTm.Locked));

            return await _repository.GetAllAsync<int>(query.Select(nameof(ExamTm.Id)));
        }
        public async Task<List<int>> GetIdsWithOutLockedAsync(int companyId = 0)
        {
            var query = Q.WhereNullOrFalse(nameof(ExamTm.Locked));
            if (companyId > 0)
            {
                query.Where(nameof(ExamTm.CompanyId), companyId);
            }
            return await _repository.GetAllAsync<int>(query.Select(nameof(ExamTm.Id)));
        }
        public async Task<int> GetRealTotalAsync()
        {
            var query = Q.WhereNullOrFalse(nameof(ExamTm.Locked));
            return await _repository.CountAsync(query);
        }
        public async Task<int> GetCountAsync(bool isAll, List<int> tmIds, int txId, int nandu)
        {
            var query = Q.
                WhereNullOrFalse(nameof(ExamTm.Locked)).
                Where(nameof(ExamTm.TxId), txId).
                Where(nameof(ExamTm.Nandu), nandu);
            if (!isAll)
            {
                query.WhereIn(nameof(ExamTm.Id), tmIds);
            }
            return await _repository.CountAsync(query);
        }

        public async Task<List<ExamTm>> GetListByRandomAsync(bool allTm, bool hasGroup, List<int> tmIds, int txId, int nandu1Count = 0, int nandu2Count = 0, int nandu3Count = 0, int nandu4Count = 0, int nandu5Count = 0)
        {
            var query = Q.
                   WhereNullOrFalse(nameof(ExamTm.Locked)).
                   Where(nameof(ExamTm.TxId), txId);

            if (!allTm && hasGroup)
            {
                if (tmIds == null || tmIds.Count == 0)
                {
                    return null;
                }
                else
                {
                    query.WhereIn(nameof(ExamTm.Id), tmIds);
                }
            }


            if (txId > 0)
            {
                query.Where(nameof(ExamTm.TxId), txId);
            }

            if (nandu1Count > 0)
            {
                query.Where(nameof(ExamTm.Nandu), 1);

                var list = await _repository.GetAllAsync(query);
                return list.OrderBy(order => StringUtils.Guid()).Take(nandu1Count).ToList();
            }
            if (nandu2Count > 0)
            {
                query.Where(nameof(ExamTm.Nandu), 2);

                var list = await _repository.GetAllAsync(query);
                return list.OrderBy(order => StringUtils.Guid()).Take(nandu2Count).ToList();
            }
            if (nandu3Count > 0)
            {
                query.Where(nameof(ExamTm.Nandu), 3);

                var list = await _repository.GetAllAsync(query);
                return list.OrderBy(order => StringUtils.Guid()).Take(nandu3Count).ToList();
            }
            if (nandu4Count > 0)
            {
                query.Where(nameof(ExamTm.Nandu), 4);

                var list = await _repository.GetAllAsync(query);
                return list.OrderBy(order => StringUtils.Guid()).Take(nandu4Count).ToList();
            }
            if (nandu5Count > 0)
            {
                query.Where(nameof(ExamTm.Nandu), 5);

                var list = await _repository.GetAllAsync(query);
                return list.OrderBy(order => StringUtils.Guid()).Take(nandu5Count).ToList();
            }
            return null;

        }
        public async Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount(AdminAuth auth)
        {
            var countQuery = Q.NewQuery();
            var lockedCountQuery = Q.WhereTrue(nameof(ExamTm.Locked));
            var unLockedCountQuery = Q.WhereNullOrFalse(nameof(ExamTm.Locked));

            countQuery = GetQueryByAuth(countQuery, auth);
            lockedCountQuery = GetQueryByAuth(lockedCountQuery, auth);
            unLockedCountQuery = GetQueryByAuth(unLockedCountQuery, auth);

            var count = await _repository.CountAsync(countQuery);
            var lockedCount = await _repository.CountAsync(lockedCountQuery);
            var unLockedCount = await _repository.CountAsync(unLockedCountQuery);
            return (count, 0, 0, lockedCount, unLockedCount);
        }

        public async Task<(int count, int total)> GetTotalAndCountByTreeIdAsync(AdminAuth auth, int treeId)
        {
            var countquery = Q.NewQuery();
            var totalquery = Q.NewQuery();

            countquery = GetQueryByAuth(countquery, auth);
            totalquery = GetQueryByAuth(totalquery, auth);

            var count = await _repository.CountAsync(countquery.Where(nameof(ExamTm.TreeId), treeId));
            var total = await _repository.CountAsync(totalquery.WhereLike(nameof(ExamTm.TreeParentPath), $"%'{treeId}'%"));

            return (count, total);
        }

    }
}
