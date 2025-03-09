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
    public partial class ExamPaperRandomRepository : IExamPaperRandomRepository
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly Repository<ExamPaperRandom> _repository;

        public ExamPaperRandomRepository(ISettingsManager settingsManager,IExamPaperRepository examPaperRepository)
        {
            _settingsManager = settingsManager;
            _examPaperRepository = examPaperRepository;
            _repository = new Repository<ExamPaperRandom>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<List<ExamPaperRandom>> GetListByPaperAsync(int examPaperId)
        {
            var tableName = await GetTableNameAsync(examPaperId);
            var repository = await GetRepositoryAsync(tableName);

            var infoList = await repository.GetAllAsync(Q.
                Where(nameof(ExamPaperRandom.ExamPaperId), examPaperId).
                OrderBy(nameof(ExamPaperRandom.Id)));
            return infoList;
        }
        public async Task<int> GetOneIdByPaperAsync(int examPaperId)
        {
            var tableName = await GetTableNameAsync(examPaperId);
            var repository = await GetRepositoryAsync(tableName);

            var infoList = await repository.GetAllAsync<int>(Q.
                Select(nameof(ExamPaperRandom.Id)).
                Where(nameof(ExamPaperRandom.ExamPaperId), examPaperId).
                OrderByRandom(StringUtils.Guid()));
            infoList = infoList.OrderBy(o => StringUtils.Guid()).ToList();
            return infoList.FirstOrDefault();
        }

        public async Task<List<int>> GetIdsByPaperAsync(int examPaperId)
        {
            var tableName = await GetTableNameAsync(examPaperId);
            var repository = await GetRepositoryAsync(tableName);

            var infoList = await repository.GetAllAsync<int>(Q.
                Where(nameof(ExamPaperRandom.ExamPaperId), examPaperId).
                OrderBy(nameof(ExamPaperRandom.Id)));
            return infoList;
        }
        public async Task<int> InsertAsync(ExamPaperRandom item)
        {
            var tableName = await GetTableNameAsync(item.ExamPaperId);
            var repository = await GetRepositoryAsync(tableName);

            return await repository.InsertAsync(item);
        }
        public async Task<bool> UpdateAsync(ExamPaperRandom item)
        {
            var tableName = await GetTableNameAsync(item.ExamPaperId);
            var repository = await GetRepositoryAsync(tableName);

            return await repository.UpdateAsync(item);
        }
        public async Task<int> DeleteByPaperAsync(int examPaperId)
        {
            var tableName = await GetTableNameAsync(examPaperId);
            var repository = await GetRepositoryAsync(tableName);

            return await repository.DeleteAsync(Q.Where(nameof(ExamPaperRandom.ExamPaperId), examPaperId));
        }
        public async Task DeleteAsync(int id,int examPaperId)
        {
            var tableName = await GetTableNameAsync(examPaperId);
            var repository = await GetRepositoryAsync(tableName);

            await repository.DeleteAsync(id);
        }
    }

}
