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
    public partial class StudyCourseRepository : IStudyCourseRepository
    {
        private readonly ISettingsManager _settingsManager;
        private readonly Repository<StudyCourse> _repository;

        public StudyCourseRepository(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            _repository = new Repository<StudyCourse>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        private string GetCacheKey(int Id)
        {
            return CacheUtils.GetEntityKey(TableName, "id", Id.ToString());
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _repository.ExistsAsync(id);
        }
        public async Task<int> InsertAsync(StudyCourse item)
        {
            return await _repository.InsertAsync(item);
        }
        public async Task<int> IncrementTotalUserAsync(int id)
        {
            var course = await GetAsync(id);
            course.TotalUser++;
            return await _repository.UpdateAsync(Q.
                Set(nameof(StudyCourse.Id), course.TotalUser++).
                Where(nameof(StudyCourse.Id), id).
                CachingRemove(GetCacheKey(id)));
        }

        public async Task<bool> UpdateAsync(StudyCourse item)
        {
            return await _repository.UpdateAsync(item, Q.CachingRemove(GetCacheKey(item.Id)));
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id, Q.CachingRemove(GetCacheKey(id)));
        }

        public async Task<StudyCourse> GetAsync(int id)
        {
            return await _repository.GetAsync(id, Q.CachingGet(GetCacheKey(id)));
        }
        public async Task<StudyCourse> GetByIdAsync(int id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task<(int total, List<StudyCourse> list)> GetListAsync(AdminAuth auth, string keyWords, string type, int treeId, bool children, int pageIndex, int pageSize)
        {
            var query = Q.OrderByDesc(nameof(StudyCourse.Id));

            query = GetQueryByAuth(query, auth);

            if (!string.IsNullOrEmpty(type))
            {
                if (type == "online")
                {
                    query.WhereNullOrFalse(nameof(StudyCourse.OffLine));
                }
                if (type == "offline")
                {
                    query.WhereTrue(nameof(StudyCourse.OffLine));
                }
                if (type == "public")
                {
                    query.WhereTrue(nameof(StudyCourse.Public));
                }
            }
            if (!string.IsNullOrEmpty(keyWords))
            {
                query.Where(q => q.WhereLike(nameof(StudyCourse.Name), $"%{keyWords}%").OrWhereLike(nameof(StudyCourse.Mark), $"%{keyWords}%"));
            }
            if (treeId > 0)
            {
                if (children)
                {
                    query.WhereLike(nameof(StudyCourse.TreeParentPath), $"%'{treeId}'%");
                }
                else
                {
                    query.Where(nameof(StudyCourse.TreeId), treeId);
                }

            }

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }

        public async Task<(int total, List<StudyCourse> list)> GetListByTeacherAsync(int teacherId, string keyWords, int pageIndex, int pageSize)
        {
            var query = Q.Where(nameof(StudyCourse.TeacherId), teacherId);

            if (!string.IsNullOrEmpty(keyWords))
            {
                query.Where(q => q.WhereLike(nameof(StudyCourse.Name), $"%{keyWords}%").OrWhereLike(nameof(StudyCourse.Mark), $"%{keyWords}%"));
            }

            query.OrderByDesc(nameof(StudyCourse.Id));
            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }
        public async Task<int> MaxAsync()
        {
            var maxId = await _repository.MaxAsync(nameof(StudyCourse.Id));
            if (maxId.HasValue)
            {
                return maxId.Value + 1;
            }
            return 1;
        }

        public async Task<(int count, int total)> GetTotalAndCountByTreeIdAsync(AdminAuth auth, int treeId)
        {
            var countquery = Q.NewQuery();
            var totalquery = Q.NewQuery();

            countquery = GetQueryByAuth(countquery, auth);
            totalquery = GetQueryByAuth(totalquery, auth);

            var count = await _repository.CountAsync(countquery.Where(nameof(StudyCourse.TreeId), treeId));
            var total = await _repository.CountAsync(totalquery.WhereLike(nameof(StudyCourse.TreeParentPath), $"%'{treeId}'%"));

            return (count, total);
        }


        public async Task<List<string>> GetMarkListAsync(AdminAuth auth)
        {
            var markList = new List<string>();

            var query = Q.Select(nameof(StudyCourse.Mark)).WhereNullOrFalse(nameof(StudyCourse.Locked));
            query = GetQueryByAuth(query, auth);
            var allMark = await _repository.GetAllAsync<string>(query);

            if (allMark != null && allMark.Count > 0)
            {
                foreach (var marks in allMark)
                {
                    var listMark = ListUtils.ToList(marks);
                    foreach (var mark in listMark)
                    {
                        if (!markList.Contains(mark))
                        {
                            markList.Add(mark);
                        }
                    }

                }
            }
            return markList;
        }
        public async Task<int> GetPaperUseCount(int paperId)
        {
            return await _repository.CountAsync(Q.Where(nameof(StudyCourse.ExamId), paperId));
        }
        public async Task<int> GetPaperQUseCount(int paperId)
        {
            return await _repository.CountAsync(Q.Where(nameof(StudyCourse.ExamQuestionnaireId), paperId));
        }
        public async Task<int> GetEvaluationUseCount(int eId)
        {
            return await _repository.CountAsync(Q.Where(nameof(StudyCourse.StudyCourseEvaluationId), eId));
        }
        public async Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount(AdminAuth auth)
        {
            var countQuery = Q.NewQuery();
            var lockedCountQuery = Q.WhereTrue(nameof(StudyCourse.Locked));
            var unLockedCountQuery = Q.WhereNullOrFalse(nameof(StudyCourse.Locked));

            countQuery = GetQueryByAuth(countQuery, auth);
            lockedCountQuery = GetQueryByAuth(lockedCountQuery, auth);
            unLockedCountQuery = GetQueryByAuth(unLockedCountQuery, auth);

            var count = await _repository.CountAsync(countQuery);
            var lockedCount = await _repository.CountAsync(lockedCountQuery);
            var unLockedCount = await _repository.CountAsync(unLockedCountQuery);
            return (count, 0, 0, lockedCount, unLockedCount);
        }


        public async Task<(int total, List<StudyCourse> list)> GetOffTrinListByWeekAsync(AdminAuth auth, bool onlyTotal = true)
        {
            var query = Q.
                WhereNullOrFalse(nameof(StudyCourse.Locked)).
                WhereTrue(nameof(StudyCourse.Public)).
                WhereTrue(nameof(StudyCourse.OffLine));

            query = GetQueryByAuth(query, auth);

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

            query.Where(nameof(StudyCourse.OfflineBeginDateTime), ">=", DateUtils.ToString(dateFromStr));
            query.Where(nameof(StudyCourse.OfflineBeginDateTime), "<=", DateUtils.ToString(dateToStr));


            query.OrderByDesc(nameof(StudyCourse.Id));

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
                query.Where(nameof(StudyCourse.CreatorId), auth.AdminId);
            }
            else
            {
                if (auth.AuthDataShowAll)
                {
                    if (auth.CurCompanyId != 1)
                    {
                        query.WhereLike(nameof(StudyCourse.CompanyParentPath), $"%'{auth.CurCompanyId}'%");
                    }
                }
                else
                {
                    query.Where(nameof(StudyCourse.CompanyId), auth.CurCompanyId);
                }
            }

            return query;
        }

    }
}
