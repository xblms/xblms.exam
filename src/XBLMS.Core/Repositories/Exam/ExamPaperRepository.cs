using Datory;
using SqlKata;
using System;
using System.Collections.Generic;
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
    public partial class ExamPaperRepository : IExamPaperRepository
    {
        private readonly Repository<ExamPaper> _repository;

        public ExamPaperRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamPaper>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;
        public async Task<List<ExamPaper>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }
        public async Task<int> InsertAsync(ExamPaper item)
        {
            return await _repository.InsertAsync(item);
        }
        public async Task<ExamPaper> GetAsync(int id)
        {
            return await _repository.GetAsync(id, Q.CachingGet(GetCacheKey(id)));
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _repository.DeleteAsync(id, Q.CachingRemove(GetCacheKey(id)));
            return result;
        }
        public async Task<bool> UpdateAsync(ExamPaper item)
        {
            return await _repository.UpdateAsync(item, Q.CachingRemove(GetCacheKey(item.Id)));
        }
        public async Task<List<ExamPaper>> GetListAsync(AdminAuth auth, string keyword)
        {
            var query = new Query();

            query = GetQueryByAuth(query, auth);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var like = $"%{keyword}%";
                query.Where(q => q
                    .WhereLike(nameof(ExamPaper.Title), like)
                    .OrWhereLike(nameof(ExamPaper.Subject), like)
                );
            }
            query.OrderByDesc(nameof(ExamPaper.Id));
            return await _repository.GetAllAsync(query);
        }
        public async Task<(int total, List<ExamPaper> list)> GetListByDateAsync(AdminAuth auth, string dateType, bool onlyTotal = true)
        {
            var query = Q.
                WhereNullOrFalse(nameof(ExamPaper.Moni)).
                WhereNullOrFalse(nameof(ExamPaper.IsCourseUse)).
                WhereNullOrFalse(nameof(ExamPaper.Locked));

            query = GetQueryByAuth(query, auth);

            if (!string.IsNullOrWhiteSpace(dateType))
            {
                if (dateType == "today")
                {
                    var dateFromStr = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                    var dateToStr = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");

                    query.Where(nameof(ExamPaperUser.ExamBeginDateTime), ">=", DateUtils.ToString(dateFromStr));
                    query.Where(nameof(ExamPaperUser.ExamBeginDateTime), "<=", DateUtils.ToString(dateToStr));
                }
                if (dateType == "week")
                {
                    DateTime today = DateTime.Now;
                    DateTime startOfWeek = today;
                    DateTime endOfWeek = today;
                    int dayOfWeek = (int)today.DayOfWeek;
                    if (dayOfWeek == 0)
                    {
                        startOfWeek = today.AddDays(-6);
                    }
                    else
                    {
                        startOfWeek = today.AddDays(1 - dayOfWeek);
                    }
                    endOfWeek = startOfWeek.AddDays(6);

                    var dateFromStr = startOfWeek.ToString("yyyy-MM-dd 00:00:00");
                    var dateToStr = endOfWeek.ToString("yyyy-MM-dd 23:59:59");

                    query.Where(nameof(ExamPaperUser.ExamBeginDateTime), ">=", DateUtils.ToString(dateFromStr));
                    query.Where(nameof(ExamPaperUser.ExamBeginDateTime), "<=", DateUtils.ToString(dateToStr));
                }
            }

            query.OrderByDesc(nameof(ExamPaper.Id));

            var total = await _repository.CountAsync(query);
            if (onlyTotal)
            {
                return (total, null);
            }
            var list = await _repository.GetAllAsync(query);
            return (total, list);
        }
        private Query GetQueryByAuth(Query query, AdminAuth auth)
        {
            if (auth.AuthDataType == AuthorityDataType.DataCreator)
            {
                query.Where(nameof(ExamPaper.CreatorId), auth.AdminId);
            }
            else
            {
                if (auth.AuthDataShowAll)
                {
                    if (auth.CurCompanyId != 1)
                    {
                        query.WhereLike(nameof(ExamPaper.CompanyParentPath), $"%'{auth.CurCompanyId}'%");
                    }
                }
                else
                {
                    query.Where(nameof(ExamPaper.CompanyId), auth.CurCompanyId);
                }
            }

            return query;
        }
        public async Task<(int total, List<ExamPaper> list)> GetListAsync(AdminAuth auth, bool treeIsChild, int treeId, string keyword, int pageIndex, int pageSize)
        {
            var query = new Query();

            query = GetQueryByAuth(query, auth);

            if (treeId > 0)
            {
                if (treeIsChild)
                {
                    query.WhereLike(nameof(ExamPaper.TreeParentPath), $"%'{treeId}'%");
                }
                else
                {
                    query.Where(nameof(ExamPaper.TreeId), treeId);
                }
            }
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var like = $"%{keyword}%";
                query.Where(q => q
                    .WhereLike(nameof(ExamPaper.Title), like)
                    .OrWhereLike(nameof(ExamPaper.Subject), like)
                );
            }
            query.OrderByDesc(nameof(ExamPaper.Id));

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }
        public async Task<(int total, int count)> CountAsync(int treeId)
        {
            var keyWords = $"'{treeId}'";
            var total = await _repository.CountAsync(Q.WhereLike(nameof(ExamPaper.TreeParentPath), $"%{keyWords}%"));
            var count = await _repository.CountAsync(Q.Where(nameof(ExamPaper.TreeId), treeId));
            return (total, count);
        }
        public async Task<int> MaxAsync()
        {
            var maxId = await _repository.MaxAsync(nameof(ExamPaper.Id));
            if (maxId.HasValue) return maxId.Value;
            return 0;
        }
        public async Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount(AdminAuth auth)
        {
            var countQuery = Q.WhereNullOrFalse(nameof(ExamPaper.Moni));
            var lockedCountQuery = Q.WhereNullOrFalse(nameof(ExamPaper.Moni)).WhereTrue(nameof(ExamPaper.Locked));
            var unLockedCountQuery = Q.WhereNullOrFalse(nameof(ExamPaper.Moni)).WhereNullOrFalse(nameof(ExamPaper.Locked));

            countQuery = GetQueryByAuth(countQuery, auth);
            lockedCountQuery = GetQueryByAuth(lockedCountQuery, auth);
            unLockedCountQuery = GetQueryByAuth(unLockedCountQuery, auth);

            var count = await _repository.CountAsync(countQuery);
            var lockedCount = await _repository.CountAsync(lockedCountQuery);
            var unLockedCount = await _repository.CountAsync(unLockedCountQuery);
            return (count, 0, 0, lockedCount, unLockedCount);
        }
        public async Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCountMoni(AdminAuth auth)
        {

            var countQuery = Q.WhereTrue(nameof(ExamPaper.Moni));
            var lockedCountQuery = Q.WhereTrue(nameof(ExamPaper.Moni)).WhereTrue(nameof(ExamPaper.Locked));
            var unLockedCountQuery = Q.WhereTrue(nameof(ExamPaper.Moni)).WhereNullOrFalse(nameof(ExamPaper.Locked));

            countQuery = GetQueryByAuth(countQuery, auth);
            lockedCountQuery = GetQueryByAuth(lockedCountQuery, auth);
            unLockedCountQuery = GetQueryByAuth(unLockedCountQuery, auth);

            var count = await _repository.CountAsync(countQuery);
            var lockedCount = await _repository.CountAsync(lockedCountQuery);
            var unLockedCount = await _repository.CountAsync(unLockedCountQuery);
            return (count, 0, 0, lockedCount, unLockedCount);
        }
        public async Task<int> GetGroupCount(int groupId)
        {
            var total = 0;
            var allGroupIds = await _repository.GetAllAsync<string>(Q.Select(nameof(ExamPaper.UserGroupIds)));
            var allGroupIdList = ListUtils.ToList(allGroupIds);
            if (allGroupIdList != null)
            {
                foreach (var groupIds in allGroupIdList)
                {
                    if (groupIds != null && groupIds.Contains(groupId.ToString()))
                    {
                        total++;
                    }
                }
            }
            return total;
        }
        public async Task<int> GetCerCount(int cerId)
        {
            return await _repository.CountAsync(Q.Where(nameof(ExamPaper.CerId), cerId));
        }
        public async Task<int> GetTmGroupCount(int groupId)
        {
            var total = 0;
            var allGroupIds = await _repository.GetAllAsync<string>(Q.Select(nameof(ExamPaper.TmGroupIds)));
            var allGroupIdList = ListUtils.ToList(allGroupIds);
            if (allGroupIdList != null)
            {
                foreach (var groupIds in allGroupIdList)
                {
                    if (groupIds != null && groupIds.Contains(groupId.ToString()))
                    {
                        total++;
                    }
                }
            }
            return total;
        }

        public async Task<(int count, int total)> GetTotalAndCountByTreeIdAsync(AdminAuth auth, int treeId)
        {
            var countquery = Q.NewQuery();
            var totalquery = Q.NewQuery();

            countquery = GetQueryByAuth(countquery, auth);
            totalquery = GetQueryByAuth(totalquery, auth);

            var count = await _repository.CountAsync(countquery.Where(nameof(ExamPaper.TreeId), treeId));
            var total = await _repository.CountAsync(totalquery.WhereLike(nameof(ExamPaper.TreeParentPath), $"%'{treeId}'%"));

            return (count, total);
        }
    }
}
