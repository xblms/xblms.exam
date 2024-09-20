using Datory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Core.Repositories
{
    public class ExamPaperRandomRepository : IExamPaperRandomRepository
    {
        private readonly Repository<ExamPaperRandom> _repository;

        public ExamPaperRandomRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamPaperRandom>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<List<ExamPaperRandom>> GetListByPaperAsync(int PaperId)
        {
            var infoList = await _repository.GetAllAsync(Q.
                Where(nameof(ExamPaperRandom.ExamPaperId), PaperId).
                OrderBy(nameof(ExamPaperRandom.Id)));
            return infoList;
        }
        public async Task<ExamPaperRandom> GetAsync(int id)
        {
            var info = await _repository.GetAsync(id);
            return info;
        }
        public async Task<ExamPaperRandom> GetOneByPaperAsync(int PaperId)
        {
            var infoList = await _repository.GetAllAsync(Q.
                Where(nameof(ExamPaperRandom.ExamPaperId), PaperId).
                OrderByRandom(StringUtils.Guid()));
            infoList = infoList.OrderBy(o => StringUtils.Guid()).ToList();
            return infoList.FirstOrDefault();
        }
        public async Task<int> GetOneIdByPaperAsync(int PaperId)
        {
            var infoList = await _repository.GetAllAsync<int>(Q.
                Select(nameof(ExamPaperRandom.Id)).
                Where(nameof(ExamPaperRandom.ExamPaperId), PaperId).
                OrderByRandom(StringUtils.Guid()));
            infoList = infoList.OrderBy(o => StringUtils.Guid()).ToList();
            return infoList.FirstOrDefault();
        }

        public async Task<List<int>> GetIdsByPaperAsync(int PaperId)
        {
            var infoList = await _repository.GetAllAsync<int>(Q.
                Where(nameof(ExamPaperRandom.ExamPaperId), PaperId).
                OrderBy(nameof(ExamPaperRandom.Id)));
            return infoList;
        }
        public async Task<int> InsertAsync(ExamPaperRandom item)
        {
            return await _repository.InsertAsync(item);
        }
        public async Task<bool> UpdateAsync(ExamPaperRandom item)
        {
            return await _repository.UpdateAsync(item);
        }
        public async Task<bool> DeleteAsync(int Id)
        {
            return await _repository.DeleteAsync(Id);
        }
        public async Task<int> DeleteByPaperAsync(int paperId)
        {
            return await _repository.DeleteAsync(Q.Where(nameof(ExamPaperRandom.ExamPaperId), paperId));
        }
    }
}
