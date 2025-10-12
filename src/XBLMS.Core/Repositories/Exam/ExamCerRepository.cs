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
    public class ExamCerRepository : IExamCerRepository
    {
        private readonly Repository<ExamCer> _repository;

        public ExamCerRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamCer>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;
        public async Task<bool> ExistsAsync(int id)
        {
            return await _repository.ExistsAsync(id);
        }

        public async Task<ExamCer> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }

        private Query GetQueryByAuth(Query query, AdminAuth auth)
        {
            if (auth.AuthDataType == AuthorityDataType.DataCreator)
            {
                query.Where(nameof(ExamCer.CreatorId), auth.AdminId);
            }
            else
            {
                if (auth.AuthDataShowAll)
                {
                    if (auth.CurCompanyId != 1)
                    {
                        query.WhereLike(nameof(ExamCer.CompanyParentPath), $"%'{auth.CurCompanyId}'%");
                    }
                }
                else
                {
                    query.Where(nameof(ExamCer.CompanyId), auth.CurCompanyId);
                }
            }

            return query;
        }
        public async Task<List<ExamCer>> GetListAsync(AdminAuth auth, string keyWords = null)
        {
            var query = Q.NewQuery();
            if (!string.IsNullOrEmpty(keyWords))
            {
                query.WhereLike(nameof(ExamCer.Name), $"%{keyWords}%");
            }
            query = GetQueryByAuth(query, auth);
            query.OrderByDesc(nameof(ExamCer.Id));
            return await _repository.GetAllAsync(query);
        }

        public async Task<int> InsertAsync(ExamCer item)
        {
            return await _repository.InsertAsync(item);
        }

        public async Task UpdateAsync(ExamCer item)
        {
            await _repository.UpdateAsync(item);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount(AdminAuth auth)
        {
            var query = Q.NewQuery();

            query = GetQueryByAuth(query, auth);

            var count = await _repository.CountAsync(query);
            return (count, 0, 0, 0, count);
        }
    }
}
