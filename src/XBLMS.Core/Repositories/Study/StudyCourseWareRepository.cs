using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public class StudyCourseWareRepository : IStudyCourseWareRepository
    {
        private readonly ISettingsManager _settingsManager;
        private readonly Repository<StudyCourseWare> _repository;

        public StudyCourseWareRepository(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            _repository = new Repository<StudyCourseWare>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<int> InsertAsync(StudyCourseWare item)
        {
            return await _repository.InsertAsync(item);
        }

        public async Task<bool> UpdateAsync(StudyCourseWare item)
        {
            return await _repository.UpdateAsync(item);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
        public async Task<bool> DeleteByNotIdsAsync(List<int> notIds, int courseId)
        {
            return await _repository.DeleteAsync(Q.
                WhereNotIn(nameof(StudyCourseWare.Id), notIds).
                Where(nameof(StudyCourseWare.CourseId), courseId)) > 0;
        }
        public async Task<bool> DeleteByCourseIdAsync(int courseId)
        {
            return await _repository.DeleteAsync(Q.Where(nameof(StudyCourseWare.CourseId), courseId)) > 0;
        }
        public async Task<StudyCourseWare> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task<List<StudyCourseWare>> GetListAsync(int courseId)
        {
            var query = Q.Where(nameof(StudyCourseWare.CourseId), courseId).OrderBy(nameof(StudyCourseWare.Taxis));
            return await _repository.GetAllAsync(query);
        }

    }
}
