using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;


namespace XBLMS.Core.Repositories
{
    public class ExamTmSmallRepository : IExamTmSmallRepository
    {
        private readonly Repository<ExamTmSmall> _repository;

        public ExamTmSmallRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamTmSmall>(settingsManager.Database, settingsManager.Redis);
        }
        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<ExamTmSmall> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task<int> InsertAsync(ExamTmSmall item)
        {
            return await _repository.InsertAsync(item);
        }

        public async Task<bool> UpdateAsync(ExamTmSmall item)
        {
            return await _repository.UpdateAsync(item);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> DeleteByParentIdAsync(int parentId)
        {
            return await _repository.DeleteAsync(Q.Where(nameof(ExamTmSmall.ParentId), parentId)) > 0;
        }


        public async Task<List<ExamTmSmall>> GetListAsync(int parentId)
        {
            return await _repository.GetAllAsync(Q.Where(nameof(ExamTmSmall.ParentId), parentId));
        }

    }
}
