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
    public class StudyPlanRepository : IStudyPlanRepository
    {
        private readonly ISettingsManager _settingsManager;
        private readonly Repository<StudyPlan> _repository;

        public StudyPlanRepository(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            _repository = new Repository<StudyPlan>(settingsManager.Database, settingsManager.Redis);
        }
        private string GetCacheKey(int Id)
        {
            return CacheUtils.GetEntityKey(TableName, "id", Id.ToString());
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;
        public async Task<bool> ExistsAsync(int id)
        {
            return await _repository.ExistsAsync(id);
        }
        public async Task<int> InsertAsync(StudyPlan item)
        {
            return await _repository.InsertAsync(item);
        }

        public async Task<bool> UpdateAsync(StudyPlan item)
        {
            return await _repository.UpdateAsync(item, Q.CachingRemove(GetCacheKey(item.Id)));
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id, Q.CachingRemove(GetCacheKey(id)));
        }

        public async Task<StudyPlan> GetAsync(int id)
        {
            return await _repository.GetAsync(id, Q.CachingGet(GetCacheKey(id)));
        }


        public async Task<(int total, List<StudyPlan> list)> GetListAsync(AdminAuth auth, string keyWords, int pageIndex, int pageSize)
        {
            var query = Q.OrderByDesc(nameof(StudyPlan.Id));

            query = GetQueryByAuth(query, auth);

            if (!string.IsNullOrEmpty(keyWords))
            {
                query.Where(q => q.WhereLike(nameof(StudyPlan.PlanName), $"%{keyWords}%").OrWhereLike(nameof(StudyPlan.Description), $"%{keyWords}%"));
            }

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }


        public async Task<int> MaxAsync()
        {
            var maxId = await _repository.MaxAsync(nameof(StudyPlan.Id));
            if (maxId.HasValue)
            {
                return maxId.Value + 1;
            }
            return 1;
        }


        public async Task<List<int>> GetYearListAsync()
        {
            var query = Q.Select(nameof(StudyPlan.PlanYear));
            var list = await _repository.GetAllAsync<int>(query);
            if (list != null && list.Count > 0)
            {
                return list.Distinct().ToList();
            }
            return null;
        }
        public async Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount(AdminAuth auth)
        {
            var countQuery = Q.NewQuery();
            var lockedCountQuery = Q.WhereTrue(nameof(StudyPlan.Locked));
            var unLockedCountQuery = Q.WhereNullOrFalse(nameof(StudyPlan.Locked));

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
            var allGroupIds = await _repository.GetAllAsync<string>(Q.Select(nameof(StudyPlan.UserGroupIds)));
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

        public async Task<int> GetPaperUseCount(int paperId)
        {
            return await _repository.CountAsync(Q.Where(nameof(StudyPlan.ExamId), paperId));
        }

        public async Task<(int total, List<StudyPlan> list)> GetListByCreateMAsync(AdminAuth auth, bool onlyTotal = true)
        {
            var query = Q.Where(nameof(StudyPlan.SubmitType), SubmitType.Submit.GetValue());

            query = GetQueryByAuth(query, auth);

            DateTime today = DateTime.Now;
            DateTime firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var dateFromStr = firstDayOfMonth.ToString("yyyy-MM-dd 00:00:00");
            var dateToStr = lastDayOfMonth.ToString("yyyy-MM-dd 23:59:59");

            query.Where(nameof(StudyPlan.CreatedDate), ">=", DateUtils.ToString(dateFromStr));
            query.Where(nameof(StudyPlan.CreatedDate), "<=", DateUtils.ToString(dateToStr));

            query.OrderByDesc(nameof(StudyPlan.Id));

            var total = await _repository.CountAsync(query);
            if (onlyTotal)
            {
                return (total, null);
            }
            var list = await _repository.GetAllAsync(query);
            return (total, list);
        }
        public async Task<(int total, List<StudyPlan> list)> GetListByOverMAsync(AdminAuth auth, bool onlyTotal = true)
        {
            var query = Q.Where(nameof(StudyPlan.SubmitType), SubmitType.Submit.GetValue());

            query = GetQueryByAuth(query, auth);

            DateTime today = DateTime.Now;
            DateTime firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var dateFromStr = firstDayOfMonth.ToString("yyyy-MM-dd 00:00:00");
            var dateToStr = lastDayOfMonth.ToString("yyyy-MM-dd 23:59:59");

            query.Where(nameof(StudyPlan.PlanEndDateTime), ">=", DateUtils.ToString(dateFromStr));
            query.Where(nameof(StudyPlan.PlanEndDateTime), "<=", DateUtils.ToString(dateToStr));

            query.OrderByDesc(nameof(StudyPlan.Id));

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
                query.Where(nameof(StudyPlan.CreatorId), auth.AdminId);
            }
            else
            {
                if (auth.AuthDataShowAll)
                {
                    if (auth.CurCompanyId != 1)
                    {
                        query.WhereLike(nameof(StudyPlan.CompanyParentPath), $"%'{auth.CurCompanyId}'%");
                    }
                }
                else
                {
                    query.Where(nameof(StudyPlan.CompanyId), auth.CurCompanyId);
                }
            }

            return query;
        }
    }
}
