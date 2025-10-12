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
    public partial class ExamQuestionnaireRepository : IExamQuestionnaireRepository
    {
        private readonly Repository<ExamQuestionnaire> _repository;

        public ExamQuestionnaireRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamQuestionnaire>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;


        public async Task<bool> ExistsAsync(int id)
        {
            return await _repository.ExistsAsync(id);
        }
        public async Task<int> InsertAsync(ExamQuestionnaire item)
        {
            return await _repository.InsertAsync(item);
        }
        public async Task<bool> UpdateAsync(ExamQuestionnaire item)
        {
            return await _repository.UpdateAsync(item);
        }
        public async Task<(int total, List<ExamQuestionnaire> list)> GetListAsync(AdminAuth auth, string keyword, int pageIndex, int pageSize)
        {
            var query = new Query();

            query = GetQueryByAuth(query, auth);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var like = $"%{keyword}%";
                query.Where(q => q
                    .WhereLike(nameof(ExamQuestionnaire.Title), like)
                );
            }
            query.OrderByDesc(nameof(ExamQuestionnaire.Id));

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }
        public async Task<ExamQuestionnaire> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }
        public async Task<ExamQuestionnaire> GetAsync(string guId)
        {
            return await _repository.GetAsync(Q.Where(nameof(ExamQuestionnaire.Guid), guId));
        }
        public async Task<bool> DeleteAsync(int Id)
        {
            var result = await _repository.DeleteAsync(Id);
            return result;
        }

        public async Task<int> MaxIdAsync()
        {
            var maxId = await _repository.MaxAsync(nameof(ExamQuestionnaire.Id));
            if (maxId.HasValue)
            {
                return maxId.Value;
            }
            return 0;
        }

        public async Task IncrementAsync(int id)
        {
            await _repository.IncrementAsync(nameof(ExamQuestionnaire.AnswerTotal), Q.Where(nameof(ExamQuestionnaire.Id), id));
        }

        public async Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount(AdminAuth auth)
        {
            var countQuery = Q.NewQuery();
            var lockedCountQuery = Q.WhereTrue(nameof(ExamQuestionnaire.Locked));
            var unLockedCountQuery = Q.WhereNullOrFalse(nameof(ExamQuestionnaire.Locked));

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
            var allGroupIds = await _repository.GetAllAsync<string>(Q.Select(nameof(ExamQuestionnaire.UserGroupIds)));
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
        private Query GetQueryByAuth(Query query, AdminAuth auth)
        {
            if (auth.AuthDataType == AuthorityDataType.DataCreator)
            {
                query.Where(nameof(ExamQuestionnaire.CreatorId), auth.AdminId);
            }
            else
            {
                if (auth.AuthDataShowAll)
                {
                    if (auth.CurCompanyId != 1)
                    {
                        query.WhereLike(nameof(ExamQuestionnaire.CompanyParentPath), $"%'{auth.CurCompanyId}'%");
                    }
                }
                else
                {
                    query.Where(nameof(ExamQuestionnaire.CompanyId), auth.CurCompanyId);
                }
            }

            return query;
        }
    }
}
