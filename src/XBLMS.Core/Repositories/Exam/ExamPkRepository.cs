using Datory;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public class ExamPkRepository : IExamPkRepository
    {
        private readonly Repository<ExamPk> _repository;

        public ExamPkRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamPk>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<ExamPk> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }
        public async Task<ExamPk> GetNextAsync(int parentId,int current)
        {
            return await _repository.GetAsync(Q.Where(nameof(ExamPk.ParentId), parentId).Where(nameof(ExamPk.Current), current));
        }
        public async Task<ExamPk> GetNextAsync(int parentId, int current,int vs)
        {
            return await _repository.GetAsync(Q.Where(nameof(ExamPk.ParentId), parentId).Where(nameof(ExamPk.Current), current).Where(nameof(ExamPk.Vs), vs));
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _repository.ExistsAsync(id);
        }

        public async Task<int> InsertAsync(ExamPk examPk)
        {
            return await _repository.InsertAsync(examPk);
        }

        public async Task UpdateAsync(ExamPk examPk)
        {
            await _repository.UpdateAsync(examPk);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamPk.Id), id).OrWhere(nameof(ExamPk.ParentId), id));
        }
        public async Task<List<ExamPk>> GetChildList(int id)
        {
            return await _repository.GetAllAsync(Q.Where(nameof(ExamPk.ParentId), id).OrderByDesc(nameof(ExamPk.Current)));
        }

        public async Task<(int total,List<ExamPk> list)> GetListAsync(string name,int pageIndex,int pageSize)
        {
            var query = Q.Where(nameof(ExamPk.ParentId), 0);
            if (!string.IsNullOrEmpty(name))
            {
                query.WhereLike(nameof(ExamPk.Name), $"%{name}%");
            }
            var total=await _repository.CountAsync(query);

            query.OrderByDesc(nameof(ExamPk.Id));
            var list= await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }
        public async Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount()
        {
            var count = await _repository.CountAsync();
            return (count, 0, 0, 0, count);
        }
    }
}
