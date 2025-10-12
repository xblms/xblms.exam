using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Core.Repositories
{
    public class StudyCourseEvaluationItemUserRepository : IStudyCourseEvaluationItemUserRepository
    {
        private readonly ISettingsManager _settingsManager;
        private readonly Repository<StudyCourseEvaluationItemUser> _repository;

        public StudyCourseEvaluationItemUserRepository(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            _repository = new Repository<StudyCourseEvaluationItemUser>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<int> InsertAsync(StudyCourseEvaluationItemUser item)
        {
            return await _repository.InsertAsync(item);
        }
        public async Task DeleteByUserId(int userId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(StudyCourseEvaluationItemUser.UserId), userId));
        }
        public async Task<StudyCourseEvaluationItemUser> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }
        public async Task<string> GetTextAsync(int courseId, int userId)
        {
            var textList = await _repository.GetAllAsync<string>(Q.Select(nameof(StudyCourseEvaluationItemUser.TextContent)).
                Where(nameof(StudyCourseEvaluationItemUser.CourseId), courseId).
                Where(nameof(StudyCourseEvaluationItemUser.UserId), userId));
            if (textList != null && textList.Count > 0)
            {
                return ListUtils.ToString(textList, "");
            }
            return "";
        }


        public async Task<StudyCourseEvaluationItemUser> GetAsync(int planId,int courseId,int userId,int evaluationId,int itemId)
        {
            return await _repository.GetAsync(Q.
                Where(nameof(StudyCourseEvaluationItemUser.PlanId), planId).
                Where(nameof(StudyCourseEvaluationItemUser.CourseId), courseId).
                Where(nameof(StudyCourseEvaluationItemUser.EvaluationId), evaluationId).
                Where(nameof(StudyCourseEvaluationItemUser.EvaluationItemId), itemId).
                Where(nameof(StudyCourseEvaluationItemUser.UserId), userId));
        }
    }
}
