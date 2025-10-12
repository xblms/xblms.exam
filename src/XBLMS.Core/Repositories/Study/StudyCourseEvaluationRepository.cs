using Datory;
using SqlKata;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public class StudyCourseEvaluationRepository : IStudyCourseEvaluationRepository
    {
        private readonly ISettingsManager _settingsManager;
        private readonly Repository<StudyCourseEvaluation> _repository;

        public StudyCourseEvaluationRepository(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            _repository = new Repository<StudyCourseEvaluation>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<int> InsertAsync(StudyCourseEvaluation item)
        {
            return await _repository.InsertAsync(item);
        }

        public async Task<bool> UpdateAsync(StudyCourseEvaluation item)
        {
            return await _repository.UpdateAsync(item);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<StudyCourseEvaluation> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }


        public async Task<(int total, List<StudyCourseEvaluation> list)> GetListAsync(AdminAuth auth, string keyWords, int pageIndex, int pageSize)
        {
            var query = Q.OrderByDesc(nameof(StudyCourseEvaluation.Id));

            query = GetQueryByAuth(query, auth);

            if (!string.IsNullOrEmpty(keyWords))
            {
                query.WhereLike(nameof(StudyCourseEvaluation.Title), $"%{keyWords}%");
            }

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }


        public async Task<int> MaxAsync()
        {
            var maxId = await _repository.MaxAsync(nameof(StudyCourseEvaluation.Id));
            if (maxId.HasValue)
            {
                return maxId.Value + 1;
            }
            return 1;
        }
        public async Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount(AdminAuth auth)
        {
            var countQuery = Q.NewQuery();
            var lockedCountQuery = Q.WhereTrue(nameof(StudyCourseEvaluation.Locked));
            var unLockedCountQuery = Q.WhereNullOrFalse(nameof(StudyCourseEvaluation.Locked));

            countQuery = GetQueryByAuth(countQuery, auth);
            lockedCountQuery = GetQueryByAuth(lockedCountQuery, auth);
            unLockedCountQuery = GetQueryByAuth(unLockedCountQuery, auth);

            var count = await _repository.CountAsync(countQuery);
            var lockedCount = await _repository.CountAsync(lockedCountQuery);
            var unLockedCount = await _repository.CountAsync(unLockedCountQuery);
            return (count, 0, 0, lockedCount, unLockedCount);
        }

        private Query GetQueryByAuth(Query query, AdminAuth auth)
        {
            if (auth.AuthDataType == AuthorityDataType.DataCreator)
            {
                query.Where(nameof(StudyCourseEvaluation.CreatorId), auth.AdminId);
            }
            else
            {
                if (auth.AuthDataShowAll)
                {
                    if (auth.CurCompanyId != 1)
                    {
                        query.WhereLike(nameof(StudyCourseEvaluation.CompanyParentPath), $"%'{auth.CurCompanyId}'%");
                    }
                }
                else
                {
                    query.Where(nameof(StudyCourseEvaluation.CompanyId), auth.CurCompanyId);
                }
            }

            return query;
        }
    }
}
