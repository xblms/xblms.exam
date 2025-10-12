using Datory;
using SqlKata;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Core.Repositories
{
    public class ExamAssessmentRepository : IExamAssessmentRepository
    {
        private readonly Repository<ExamAssessment> _repository;

        public ExamAssessmentRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamAssessment>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;


        public async Task<bool> ExistsAsync(int id)
        {
            return await _repository.ExistsAsync(id);
        }
        public async Task<int> InsertAsync(ExamAssessment item)
        {
            return await _repository.InsertAsync(item);
        }
        public async Task<bool> UpdateAsync(ExamAssessment item)
        {
            return await _repository.UpdateAsync(item);
        }
        private Query GetQueryByAuth(Query query, AdminAuth auth)
        {
            if (auth.AuthDataType == AuthorityDataType.DataCreator)
            {
                query.Where(nameof(ExamAssessment.CreatorId), auth.AdminId);
            }
            else
            {
                if (auth.AuthDataShowAll)
                {
                    if (auth.CurCompanyId != 1)
                    {
                        query.WhereLike(nameof(ExamAssessment.CompanyParentPath), $"%'{auth.CurCompanyId}'%");
                    }
                }
                else
                {
                    query.Where(nameof(ExamAssessment.CompanyId), auth.CurCompanyId);
                }
            }

            return query;
        }
        public async Task<(int total, List<ExamAssessment> list)> GetListAsync(AdminAuth auth, string keyword, int pageIndex, int pageSize)
        {
            var query = new Query();

            query = GetQueryByAuth(query, auth);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var like = $"%{keyword}%";
                query.Where(q => q
                    .WhereLike(nameof(ExamAssessment.Title), like)
                );
            }
            query.OrderByDesc(nameof(ExamAssessment.Id));

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }
        public async Task<ExamAssessment> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }
        public async Task<ExamAssessment> GetAsync(string guId)
        {
            return await _repository.GetAsync(Q.Where(nameof(ExamAssessment.Guid), guId));
        }
        public async Task<bool> DeleteAsync(int Id)
        {
            var result = await _repository.DeleteAsync(Id);
            return result;
        }

        public async Task<int> MaxIdAsync()
        {
            var maxId = await _repository.MaxAsync(nameof(ExamAssessment.Id));
            if (maxId.HasValue)
            {
                return maxId.Value;
            }
            return 0;
        }

        public async Task IncrementAsync(int id)
        {
            await _repository.IncrementAsync(nameof(ExamAssessment.AnswerTotal), Q.Where(nameof(ExamAssessment.Id), id));
        }

        public async Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount(AdminAuth auth)
        {
            var countQuery = Q.NewQuery();
            var lockedCountQuery = Q.WhereTrue(nameof(ExamAssessment.Locked));
            var unLockedCountQuery = Q.WhereNullOrFalse(nameof(ExamAssessment.Locked));

            countQuery = GetQueryByAuth(countQuery, auth);
            lockedCountQuery = GetQueryByAuth(lockedCountQuery, auth);
            unLockedCountQuery = GetQueryByAuth(unLockedCountQuery, auth);

            var count = await _repository.CountAsync(countQuery);
            var lockedCount = await _repository.CountAsync(lockedCountQuery);
            var unLockedCount = await _repository.CountAsync(unLockedCountQuery);
            return (count, 0, 0, lockedCount, unLockedCount);
        }

        public async Task<int> GetConfigCount(int configId)
        {
            return await _repository.CountAsync(Q.Where(nameof(ExamAssessment.ConfigId), configId));
        }
        public async Task<int> GetGroupCount(int groupId)
        {
            var total = 0;
            var allGroupIds = await _repository.GetAllAsync<string>(Q.Select(nameof(ExamAssessment.UserGroupIds)));
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
    }
}
