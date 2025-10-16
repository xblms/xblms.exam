using Datory;
using SqlKata;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Core.Repositories
{
    public partial class ExamTmRepository
    {
        public async Task<int> Group_GetTmTotalAsync(ExamTmGroup group)
        {
            var query = Q.Select(nameof(ExamTm.Id)).WhereNullOrFalse(nameof(ExamTm.Locked));

            if (group != null)
            {
                query = Group_GetQuery(group, query);
            }

            return await _repository.CountAsync(query);
        }
        public async Task<List<ExamTm>> Group_GetTmListAsync(ExamTmGroup group)
        {
            var query = Q.WhereNullOrFalse(nameof(ExamTm.Locked));
            query = Group_GetQuery(group, query);
            return await _repository.GetAllAsync(query);
        }
        public async Task<List<int>> Group_GetTmIdsAsync(ExamTmGroup group, List<int> txIds = null)
        {
            var query = Q.WhereNullOrFalse(nameof(ExamTm.Locked));
            if (txIds != null)
            {
                query.WhereIn(nameof(ExamTm.TxId), txIds);
            }
            query = Group_GetQuery(group, query);
            return await _repository.GetAllAsync<int>(query.Select(nameof(ExamTm.Id)));
        }

        public async Task<List<int>> Group_RangeIdsAsync(ExamTmGroup group)
        {
            var query = Q.Select(nameof(ExamTm.Id)).WhereNullOrFalse(nameof(ExamTm.Locked));

            if (group != null)
            {
                query = Group_GetQuery(group, query);
            }

            return await _repository.GetAllAsync<int>(query);
        }
        public async Task<List<int>> Group_Practice_GetTmidsAsync(ExamTmGroup group, List<int> txIds, List<int> nds, List<string> zsds)
        {
            var query = Q.
                Select(nameof(ExamTm.Id)).
                WhereNullOrFalse(nameof(ExamTm.Locked));

            if (txIds != null)
            {
                query.WhereIn(nameof(ExamTm.TxId), txIds);
            }
            else
            {
                return null;
            }
            if (nds != null)
            {
                query.WhereIn(nameof(ExamTm.Nandu), nds);
            }
            else
            {
                return null;
            }
            if (zsds != null)
            {
                query.Where(q =>
                {
                    zsds.ForEach(zsd =>
                    {
                        q.OrWhere(nameof(ExamTm.Zhishidian), zsd);
                    });
                    return q;
                });
            }

            if (group != null)
            {
                query = Group_GetQuery(group, query);
            }

            return await _repository.GetAllAsync<int>(query);
        }
        public async Task<List<int>> Group_Practice_GetTmidsAsync(ExamTmGroup group, List<int> txIds, List<int> nds, string zsd)
        {
            var query = Q.
                Select(nameof(ExamTm.Id)).
                WhereNullOrFalse(nameof(ExamTm.Locked));

            if (txIds != null)
            {
                query.WhereIn(nameof(ExamTm.TxId), txIds);
            }
            else
            {
                return null;
            }
            if (nds != null)
            {
                query.WhereIn(nameof(ExamTm.Nandu), nds);
            }
            else
            {
                return null;
            }
            if (!string.IsNullOrEmpty(zsd))
            {
                query.Where(nameof(ExamTm.Zhishidian), zsd);
            }

            if (group != null)
            {
                query = Group_GetQuery(group, query);
            }

            return await _repository.GetAllAsync<int>(query);
        }
        public async Task<List<string>> Group_Practice_GetZsdsAsync(ExamTmGroup group, List<int> txIds, List<int> nds)
        {
            var query = Q.
                Select(nameof(ExamTm.Zhishidian)).
                WhereNullOrFalse(nameof(ExamTm.Locked));

            if (txIds != null)
            {
                query.WhereIn(nameof(ExamTm.TxId), txIds);
            }
            else
            {
                return null;
            }
            if (nds != null)
            {
                query.WhereIn(nameof(ExamTm.Nandu), nds);
            }
            else
            {
                return null;
            }


            if (group != null)
            {
                query = Group_GetQuery(group, query);
            }
            var zsdList = await _repository.GetAllAsync<string>(query);
            if (zsdList != null)
            {
                zsdList = zsdList.Distinct().ToList();
            }
            return zsdList;
        }

        private Query Group_GetQuery(ExamTmGroup group, Query query)
        {
            if (group.GroupType == TmGroupType.Fixed)
            {
                query.WhereLike(nameof(ExamTm.TmGroupIds), $"%'{group.Id}'%");
            }
            if (group.GroupType == TmGroupType.Range)
            {
                query = Group_GetRangeQuery(group, query);
            }

            return query;
        }
        private Query Group_GetRangeQuery(ExamTmGroup group, Query query)
        {
            var isRange = false;
            if (group.DateFrom.HasValue)
            {
                isRange = true;
                query.Where(nameof(ExamTm.CreatedDate), ">=", DateUtils.ToString(group.DateFrom));
            }
            if (group.DateTo.HasValue)
            {
                isRange = true;
                query.Where(nameof(ExamTm.CreatedDate), "<=", DateUtils.ToString(group.DateTo));
            }
            if (group.TreeIds != null && group.TreeIds.Count > 0)
            {
                isRange = true;
                query.WhereIn(nameof(ExamTm.TreeId), group.TreeIds);
            }
            if (group.TxIds != null && group.TxIds.Count > 0)
            {
                isRange = true;
                query.WhereIn(nameof(ExamTm.TxId), group.TxIds);
            }
            if (group.Nandus != null && group.Nandus.Count > 0)
            {
                isRange = true;
                query.WhereIn(nameof(ExamTm.Nandu), group.Nandus);
            }
            if (group.Zhishidians != null && group.Zhishidians.Count > 0)
            {
                isRange = true;
                query.Where(q =>
                {
                    foreach (var zhishidian in group.Zhishidians)
                    {
                        var like = $"%{zhishidian}%";
                        q.OrWhereLike(nameof(ExamTm.Zhishidian), like);
                    }
                    return q;
                });
            }
            if (!isRange)
            {
                query.Where(nameof(ExamTm.Id), 0);
            }
            return query;
        }


    }
}
