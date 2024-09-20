using Datory;
using NPOI.SS.UserModel;
using SqlKata;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;


namespace XBLMS.Core.Repositories
{
    public class ExamTmRepository : IExamTmRepository
    {
        private readonly Repository<ExamTm> _repository;

        public ExamTmRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamTm>(settingsManager.Database, settingsManager.Redis);
        }


        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

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
        public async Task<List<ExamTm>> GetListAsync(List<int> tmIds)
        {
            if (tmIds != null && tmIds.Count > 0)
            {
                var list = await _repository.GetAllAsync(Q.WhereIn(nameof(ExamTm.Id), tmIds));
                return list;
            }
            return null;
        }
        public async Task<List<ExamTm>> GetListWithOutLockedAsync(List<int> tmIds)
        {
            if (tmIds != null && tmIds.Count > 0)
            {
                var list = await _repository.GetAllAsync(Q.WhereNullOrFalse(nameof(ExamTm.Locked)).WhereIn(nameof(ExamTm.Id), tmIds));
                return list;
            }
            return null;
        }
        public async Task<List<ExamTm>> GetListWithOutLockedAsync(List<int> tmIds, int txId, int nandu1Count = 0, int nandu2Count = 0, int nandu3Count = 0, int nandu4Count = 0, int nandu5Count = 0)
        {
            var query = Q.WhereNullOrFalse(nameof(ExamTm.Locked));
            if (tmIds != null && tmIds.Count > 0)
            {
                query.WhereIn(nameof(ExamTm.Id), tmIds);
            }
            else
            {
                return null;
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
        public async Task<(int total, List<ExamTm> list)> GetListAsync(List<int> withoutIds, List<int> treeIds, int txId, int nandu, string keyword, string order, string orderType, bool? isStop, int pageIndex, int pageSize)
        {
            var query = await GetQuery(treeIds, txId, nandu, keyword, order, orderType, isStop, withoutIds);
            var count = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (count, list);
        }
        public async Task<(int total, List<ExamTm> list)> GetListAsync(ExamTmGroup group, List<int> treeIds, int txId, int nandu, string keyword, string order, string orderType, bool? isStop, int pageIndex, int pageSize)
        {
            var query = await GetQuery(treeIds, txId, nandu, keyword, order, orderType, isStop, null, group);
            var count = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (count, list);
        }
        public async Task<int> GetCountAsync(ExamTmGroup group, List<int> treeIds, int txId, int nandu, string keyword, string order, string orderType, bool? isStop, int pageIndex, int pageSize)
        {
            var query = await GetQuery(treeIds, txId, nandu, keyword, order, orderType, isStop, null, group);
            var count = await _repository.CountAsync(query);
            return count;
        }
        private async Task<Query> GetQuery(List<int> treeIds, int txId, int nandu, string keyword, string order, string orderType, bool? isStop, List<int> withoutIds = null, ExamTmGroup group = null)
        {
            var query = Q.NewQuery();

            if (group != null)
            {
                if (group.GroupType == TmGroupType.Fixed)
                {
                    if (group.TmIds != null && group.TmIds.Count > 0)
                    {
                        query.WhereIn(nameof(ExamTm.Id), group.TmIds);
                    }
                    else
                    {
                        return query.Where(nameof(ExamTm.Id), -1);
                    }
                }
                if (group.GroupType == TmGroupType.Range)
                {
                    var ids = await GetIdsAsync(group.TreeIds, group.TxIds, group.Nandus, group.Zhishidians, group.DateFrom, group.DateTo);
                    if (ids != null && ids.Count > 0)
                    {
                        query.WhereIn(nameof(ExamTm.Id), ids);
                    }
                    else
                    {
                        return query.Where(nameof(ExamTm.Id), -1);
                    }
                }
            }


            if (treeIds != null && treeIds.Count > 0)
            {
                query.WhereIn(nameof(ExamTm.TreeId), treeIds);
            }
            if (withoutIds != null && withoutIds.Count > 0)
            {
                query.WhereNotIn(nameof(ExamTm.Id), withoutIds);
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
            if (isStop.HasValue)
            {
                if (isStop.Value)
                {
                    query.WhereTrue(nameof(ExamTm.Locked));
                }
                else
                {
                    query.WhereNullOrFalse(nameof(ExamTm.Locked));
                }
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
        public async Task<int> GetCountByTreeIdAsync(int treeId)
        {
            return await _repository.CountAsync(Q.Where(nameof(ExamTm.TreeId), treeId));
        }
        public async Task<int> GetCountByTreeIdsAsync(List<int> treeIds)
        {
            return await _repository.CountAsync(Q.WhereIn(nameof(ExamTm.TreeId), treeIds));
        }


        public async Task<int> GetCountByWithoutStopAsync()
        {
            return await _repository.CountAsync(Q.WhereNullOrFalse(nameof(ExamTm.Locked)));
        }
        public async Task<int> GetCountByWithoutStopAndInIdsAsync(List<int> ids)
        {
            if (ids != null && ids.Count > 0)
            {
                return await _repository.CountAsync(Q.WhereNullOrFalse(nameof(ExamTm.Locked)).WhereIn(nameof(ExamTm.Id), ids));
            }
            return 0;

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
        public async Task<List<int>> GetIdsWithOutLockedAsync()
        {

            var query = Q.WhereNullOrFalse(nameof(ExamTm.Locked));

            return await _repository.GetAllAsync<int>(query.Select(nameof(ExamTm.Id)));
        }

        public async Task<int> GetCountAsync(List<int> tmIds, int txId, int nandu)
        {
            var query = Q.
                WhereNullOrFalse(nameof(ExamTm.Locked)).
                Where(nameof(ExamTm.TxId), txId).
                Where(nameof(ExamTm.Nandu), nandu);
            if (tmIds != null && tmIds.Count > 0)
            {
                query.WhereIn(nameof(ExamTm.Id), tmIds);
            }

            return await _repository.CountAsync(query);



        }

        public async Task<List<ExamTm>> GetListByRandomAsync(List<int> tmIds, bool hasTmGroup, int txId, int nandu1Count = 0, int nandu2Count = 0, int nandu3Count = 0, int nandu4Count = 0, int nandu5Count = 0)
        {
            var query = Q.WhereNullOrFalse(nameof(ExamTm.Locked));
            if (tmIds != null && tmIds.Count > 0)
            {
                query.WhereIn(nameof(ExamTm.Id), tmIds);
            }
            else
            {
                if (hasTmGroup)
                {
                    return null;
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
    }
}
