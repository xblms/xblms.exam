using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public partial class StudyCourseWareUserRepository : IStudyCourseWareUserRepository
    {
        private readonly ISettingsManager _settingsManager;
        private readonly Repository<StudyCourseWareUser> _repository;

        public StudyCourseWareUserRepository(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            _repository = new Repository<StudyCourseWareUser>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<int> InsertAsync(StudyCourseWareUser item)
        {
            return await _repository.InsertAsync(item);
        }

        public async Task<bool> UpdateAsync(StudyCourseWareUser item)
        {
            return await _repository.UpdateAsync(item);
        }
        public async Task DeleteByUserId(int userId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(StudyCourseWareUser.UserId), userId));
        }
        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
        public async Task<bool> DeleteByCourseIdAsync(int courseId)
        {
            return await _repository.DeleteAsync(Q.Where(nameof(StudyCourseWareUser.CourseId), courseId)) > 0;
        }

        public async Task<bool> ClearCureentAsync(int userId, int planId, int courseId)
        {
            var query = Q.
                Set(nameof(StudyCourseWareUser.StudyCurrent), false).
                Where(nameof(StudyCourseWareUser.UserId), userId).
                Where(nameof(StudyCourseWareUser.PlanId), planId).
                Where(nameof(StudyCourseWareUser.CourseId), courseId);
            return await _repository.UpdateAsync(query) > 0;
        }
        public async Task<bool> ExistsAsync(int userId, int planId, int courseId, int wareId)
        {
            var query = Q.
            Where(nameof(StudyCourseWareUser.UserId), userId).
            Where(nameof(StudyCourseWareUser.PlanId), planId).
            Where(nameof(StudyCourseWareUser.CourseWareId), wareId).
            Where(nameof(StudyCourseWareUser.CourseId), courseId);
            return await _repository.ExistsAsync(query);
        }

        public async Task<StudyCourseWareUser> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task<List<StudyCourseWareUser>> GetListAsync(int userId, int planId, int courseId)
        {
            var query = Q.
                 Where(nameof(StudyCourseWareUser.UserId), userId).
                 Where(nameof(StudyCourseWareUser.PlanId), planId).
                 Where(nameof(StudyCourseWareUser.CourseId), courseId);

            query.OrderByDesc(nameof(StudyCourseWareUser.Id));
            var list = await _repository.GetAllAsync(query);
            return list;
        }

        public async Task<(int total, List<StudyCourseWareUser> list)> GetListAsync(string keyWords, int pageIndex, int pageSize)
        {
            var query = Q.NewQuery();

            query.OrderByDesc(nameof(StudyCourseWareUser.Id));
            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }


    }
}
