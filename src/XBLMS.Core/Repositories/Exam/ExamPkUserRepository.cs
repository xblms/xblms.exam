using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public class ExamPkUserRepository : IExamPkUserRepository
    {
        private readonly Repository<ExamPkUser> _repository;

        public ExamPkUserRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamPkUser>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;
        public async Task<ExamPkUser> GetAsync(int pkId,int userId)
        {
            return await _repository.GetAsync(Q.Where(nameof(ExamPkUser.PkId), pkId).Where(nameof(ExamPkUser.UserId), userId));
        }

        public async Task<int> InsertAsync(ExamPkUser examPkUser)
        {
            return await _repository.InsertAsync(examPkUser);
        }

        public async Task UpdateAsync(ExamPkUser examPkUser)
        {
            await _repository.UpdateAsync(examPkUser);
        }

        public async Task DeleteByPkIdAsync(int pkId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamPkUser.PkId), pkId));
        }

        public async Task<int> CountAsync(int pkId)
        {
            var query = Q.Where(nameof(ExamPkUser.PkId), pkId);
            var total = await _repository.CountAsync(query);
            return total;
        }

        public async Task<List<int>> GetUserIdsAsync(int pkId)
        {
            var query = Q.Select(nameof(ExamPkUser.UserId)).Where(nameof(ExamPkUser.PkId), pkId);
            return await _repository.GetAllAsync<int>(query);
        }

        public async Task<(int total, List<ExamPkUser> list)> GetListAsync(int pkId, string keyWords)
        {
            var query = Q.Where(nameof(ExamPkUser.PkId), pkId);

            if (!string.IsNullOrEmpty(keyWords))
            {
                query.WhereLike(nameof(ExamPkUser.KeyWordsAdmin), $"%{keyWords}%");
            }

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.OrderByDesc(nameof(ExamPkUser.RightTotal)));
            return (total, list);
        }
    }
}
