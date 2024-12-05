using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public class ExamQuestionnaireTmRepository : IExamQuestionnaireTmRepository
    {
        private readonly Repository<ExamQuestionnaireTm> _repository;

        public ExamQuestionnaireTmRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamQuestionnaireTm>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<int> InsertAsync(ExamQuestionnaireTm item)
        {
            return await _repository.InsertAsync(item);
        }

        public async Task<int> DeleteByPaperAsync(int examPaperId)
        {
            return await _repository.DeleteAsync(Q.Where(nameof(ExamQuestionnaireTm.ExamPaperId), examPaperId));
        }

        public async Task<List<ExamQuestionnaireTm>> GetListAsync(int examPaperId)
        {
            var infoList = await _repository.GetAllAsync(Q.
                Where(nameof(ExamQuestionnaireTm.ExamPaperId), examPaperId).
                OrderBy(nameof(ExamQuestionnaireTm.Id)));
            return infoList;
        }

    }
}
