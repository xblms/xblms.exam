using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public class ExamPkRoomAnswerRepository : IExamPkRoomAnswerRepository
    {
        private readonly Repository<ExamPkRoomAnswer> _repository;

        public ExamPkRoomAnswerRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamPkRoomAnswer>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task DeleteByRoomIdAsync(int roomId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamPkRoomAnswer.RoomId), roomId));
        }
        public async Task<int> InsertAsync(ExamPkRoomAnswer examPkRoomAnswer)
        {
            return await _repository.InsertAsync(examPkRoomAnswer);
        }

    }
}
