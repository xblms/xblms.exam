using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public class StudyCourseEvaluationUserRepository : IStudyCourseEvaluationUserRepository
    {
        private readonly ISettingsManager _settingsManager;
        private readonly Repository<StudyCourseEvaluationUser> _repository;

        public StudyCourseEvaluationUserRepository(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            _repository = new Repository<StudyCourseEvaluationUser>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<int> InsertAsync(StudyCourseEvaluationUser item)
        {
            return await _repository.InsertAsync(item);
        }
        public async Task DeleteByUserId(int userId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(StudyCourseEvaluationUser.UserId), userId));
        }
        public async Task<StudyCourseEvaluationUser> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }
        public async Task<StudyCourseEvaluationUser> GetAsync(int planId, int courseId, int evaluationId, int userId)
        {
            return await _repository.GetAsync(Q.
                Where(nameof(StudyCourseEvaluationUser.PlanId), planId).
                Where(nameof(StudyCourseEvaluationUser.CourseId), courseId).
                Where(nameof(StudyCourseEvaluationUser.EvaluationId), evaluationId).
                Where(nameof(StudyCourseEvaluationUser.UserId), userId));
        }
        public async Task<(int total, List<StudyCourseEvaluationUser> list)> GetListAsync(int courseId, int pageIndex, int pageSize)
        {
            var query = Q.
                Where(nameof(StudyCourseEvaluationUser.CourseId), courseId).
                OrderByDesc(nameof(StudyCourseEvaluationUser.Id));

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }
        public async Task<(int total, List<StudyCourseEvaluationUser> list)> GetListAsync(int planId, int courseId,string keyWords, int pageIndex, int pageSize)
        {
            var query = Q.
                Where(nameof(StudyCourseEvaluationUser.PlanId), planId).
                Where(nameof(StudyCourseEvaluationUser.CourseId), courseId).
                OrderByDesc(nameof(StudyCourseEvaluationUser.Id));

            if (!string.IsNullOrEmpty(keyWords))
            {
                query.WhereLike(nameof(StudyCourseEvaluationUser.KeyWordsAdmin), $"%{keyWords}%");
            }

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }
    }
}
