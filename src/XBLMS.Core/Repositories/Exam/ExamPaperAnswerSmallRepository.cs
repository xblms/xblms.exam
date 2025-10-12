using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public partial class ExamPaperAnswerSmallRepository : IExamPaperAnswerSmallRepository
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly Repository<ExamPaperAnswerSmall> _repository;

        public ExamPaperAnswerSmallRepository(ISettingsManager settingsManager, IExamPaperRepository examPaperRepository)
        {
            _settingsManager = settingsManager;
            _examPaperRepository = examPaperRepository;
            _repository = new Repository<ExamPaperAnswerSmall>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;


        public async Task<int> InsertAsync(ExamPaperAnswerSmall item)
        {
            return await _repository.InsertAsync(item);
        }
        public async Task<bool> UpdateAsync(ExamPaperAnswerSmall item)
        {
            return await _repository.UpdateAsync(item);
        }
        public async Task<ExamPaperAnswerSmall> GetAsync(int tmId, int startId, int examPaperId)
        {
            return await _repository.GetAsync(Q.
                Where(nameof(ExamPaperAnswerSmall.ExamPaperId), examPaperId).
                Where(nameof(ExamPaperAnswerSmall.ExamStartId), startId).
                Where(nameof(ExamPaperAnswerSmall.RandomTmId), tmId));
        }
        public async Task<ExamPaperAnswerSmall> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task<List<ExamPaperAnswerSmall>> GetListAsync(int answerId)
        {
            return await _repository.GetAllAsync(Q.Where(nameof(ExamPaperAnswerSmall.AnswerId), answerId));
        }
    }
}
