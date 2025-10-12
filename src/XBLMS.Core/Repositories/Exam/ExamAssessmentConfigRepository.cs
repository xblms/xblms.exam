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
    public class ExamAssessmentConfigRepository : IExamAssessmentConfigRepository
    {
        private readonly ISettingsManager _settingsManager;
        private readonly Repository<ExamAssessmentConfig> _repository;

        public ExamAssessmentConfigRepository(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            _repository = new Repository<ExamAssessmentConfig>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<int> InsertAsync(ExamAssessmentConfig item)
        {
            return await _repository.InsertAsync(item);
        }

        public async Task<bool> UpdateAsync(ExamAssessmentConfig item)
        {
            return await _repository.UpdateAsync(item);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<ExamAssessmentConfig> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }
        public async Task<List<ExamAssessmentConfig>> GetListWithoutLockedAsync(AdminAuth auth)
        {
            var query = Q.WhereNullOrFalse(nameof(ExamAssessmentConfig.Locked));

            query = GetQueryByAuth(query, auth);

            query.OrderByDesc(nameof(ExamAssessmentConfig.Id));

            return await _repository.GetAllAsync(query);
        }

        public async Task<(int total, List<ExamAssessmentConfig> list)> GetListAsync(AdminAuth auth, string keyWords, int pageIndex, int pageSize)
        {
            var query = Q.NewQuery();

            query = GetQueryByAuth(query, auth);

            if (!string.IsNullOrEmpty(keyWords))
            {
                query.WhereLike(nameof(ExamAssessmentConfig.Title), $"%{keyWords}%");
            }
            query.OrderByDesc(nameof(ExamAssessmentConfig.Id));

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }

        public async Task<int> MaxAsync()
        {
            var maxId = await _repository.MaxAsync(nameof(ExamAssessmentConfig.Id));
            if (maxId.HasValue)
            {
                return maxId.Value + 1;
            }
            return 1;
        }

        private Query GetQueryByAuth(Query query, AdminAuth auth)
        {
            if (auth.AuthDataType == AuthorityDataType.DataCreator)
            {
                query.Where(nameof(ExamAssessmentConfig.CreatorId), auth.AdminId);
            }
            else
            {
                if (auth.AuthDataShowAll)
                {
                    if (auth.CurCompanyId != 1)
                    {
                        query.WhereLike(nameof(ExamAssessmentConfig.CompanyParentPath), $"%'{auth.CurCompanyId}'%");
                    }
                }
                else
                {
                    query.Where(nameof(ExamAssessmentConfig.CompanyId), auth.CurCompanyId);
                }
            }

            return query;
        }
    }
}
