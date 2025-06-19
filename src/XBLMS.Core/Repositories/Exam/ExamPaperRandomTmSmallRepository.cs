using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public class ExamPaperRandomTmSmallRepository : IExamPaperRandomTmSmallRepository
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly Repository<ExamPaperRandomTmSmall> _repository;

        public ExamPaperRandomTmSmallRepository(ISettingsManager settingsManager, IExamPaperRepository examPaperRepository)
        {
            _settingsManager = settingsManager;
            _examPaperRepository = examPaperRepository;
            _repository = new Repository<ExamPaperRandomTmSmall>(settingsManager.Database, settingsManager.Redis);
            
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;


        public async Task<ExamPaperRandomTmSmall> GetAsync(int Id)
        {
            return await _repository.GetAsync(Id);
        }


        public async Task<int> InsertAsync(ExamPaperRandomTmSmall item)
        {
            return await _repository.InsertAsync(item);
        }
        public async Task<int> DeleteByRandomIdAsync(int examPaperRandomId,int examPaperId)
        {
            return await _repository.DeleteAsync(Q.Where(nameof(ExamPaperRandomTmSmall.ExamPaperRandomId), examPaperRandomId));
        }
        public async Task<int> DeleteByPaperAsync(int examPaperId)
        {
            return await _repository.DeleteAsync(Q.Where(nameof(ExamPaperRandomTmSmall.ExamPaperId), examPaperId));
        }
        public async Task<List<ExamPaperRandomTmSmall>> GetListAsync(int randomTmId)
        {
            var infoList = await _repository.GetAllAsync(Q.
                Where(nameof(ExamPaperRandomTmSmall.ParentId), randomTmId).
                OrderBy(nameof(ExamPaperRandomTmSmall.Id)));
            return infoList;
        }
        public async Task<List<ExamPaperRandomTmSmall>> GetListAsync(int examPaperRandomId, int txId,int examPaperId)
        {
            var infoList = await _repository.GetAllAsync(Q.
                Where(nameof(ExamPaperRandomTmSmall.ExamPaperRandomId), examPaperRandomId).
                Where(nameof(ExamPaperRandomTmSmall.TxId), txId).
                OrderBy(nameof(ExamPaperRandomTmSmall.Id)));
            return infoList;
        }
        public async Task<List<int>> GetIdsAsync(int tmId,int examPaperId)
        {
            var idsList = await _repository.GetAllAsync<int>(Q.
                Select(nameof(ExamPaperRandomTmSmall.Id)).
                Where(nameof(ExamPaperRandomTmSmall.SourceTmId), tmId));
            return idsList;
        }
    }
}
