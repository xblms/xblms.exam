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
    public partial class ExamTmAiRepository : IExamTmAiRepository
    {
        private readonly ICacheManager _cacheManager;
        private readonly Repository<ExamTmAi> _repository;

        public ExamTmAiRepository(ISettingsManager settingsManager, ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
            _repository = new Repository<ExamTmAi>(settingsManager.Database, settingsManager.Redis);
        }
        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<int> InsertAsync(ExamTmAi item)
        {
            return await _repository.InsertAsync(item);
        }

        public async Task<bool> UpdateAsync(ExamTmAi item)
        {
            return await _repository.UpdateAsync(item);
        }
        public async Task<(int total, List<ExamTmAi> list)> GetListAsync(AdminAuth auth, int status, string keyWords, int pageIndex, int pageSize)
        {
            var query = Q.NewQuery();
            query = GetQueryByAuth(query, auth);
            if (!string.IsNullOrEmpty(keyWords))
            {
                var like = $"%{keyWords}%";
                query.Where(q => q
                    .WhereLike(nameof(ExamTmAi.Title), like)
                    .OrWhereLike(nameof(ExamTmAi.KeyWordsAdmin), like)
                    .OrWhereLike(nameof(ExamTmAi.Zhishidian), like)
                    .OrWhereLike(nameof(ExamTmAi.Jiexi), like)
                    .OrWhereLike(nameof(ExamTmAi.Answer), like)
                );
            }
            if (status > 0)
            {
                if (status == 1)
                {
                    query.WhereTrue(nameof(ExamTmAi.Stocked));
                }
                else
                {
                    query.WhereNullOrFalse(nameof(ExamTmAi.Stocked));
                }
            }
            query.OrderByDesc(nameof(ExamTmAi.Stocked));
            var count = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (count, list);
        }

        private Query GetQueryByAuth(Query query, AdminAuth auth)
        {
            if (auth.AuthDataType == AuthorityDataType.DataCreator)
            {
                query.Where(nameof(ExamTmAi.CreatorId), auth.AdminId);
            }
            else
            {
                if (auth.AuthDataShowAll)
                {
                    if (auth.CurCompanyId != 1)
                    {
                        query.WhereLike(nameof(ExamTmAi.CompanyParentPath), $"%'{auth.CurCompanyId}'%");
                    }
                }
                else
                {
                    query.Where(nameof(ExamTmAi.CompanyId), auth.CurCompanyId);
                }
            }

            return query;
        }

        public async Task<ExamTmAi> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
            return await _repository.DeleteAsync(id);
        }
        public async Task DeleteAllAsync(AdminAuth auth)
        {
            var query = Q.WhereTrue(nameof(ExamTmAi.Stocked));
            query = GetQueryByAuth(query, auth);
            await _repository.DeleteAsync(query);
        }
        public async Task StockedAsync(int id)
        {
            await _repository.UpdateAsync(Q.
                Set(nameof(ExamTmAi.Stocked), true).
                Where(nameof(ExamTmAi.Id), id));
        }
        public async Task StockedAllAsync(AdminAuth auth)
        {
            var query = Q.Set(nameof(ExamTmAi.Stocked), true).WhereNullOrFalse(nameof(ExamTmAi.Stocked));
            query = GetQueryByAuth(query, auth);
            await _repository.UpdateAsync(query);
        }
        public async Task<(int total, List<ExamTmAi>)> GetStockedAllAsync(AdminAuth auth)
        {
            var query = Q.Set(nameof(ExamTmAi.Stocked), true).WhereNullOrFalse(nameof(ExamTmAi.Stocked));
            query = GetQueryByAuth(query, auth);
            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query);
            return (total, list);
        }
    }
}
