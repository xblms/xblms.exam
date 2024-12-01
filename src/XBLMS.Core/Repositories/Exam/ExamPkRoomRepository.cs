using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public class ExamPkRoomRepository : IExamPkRoomRepository
    {
        private readonly Repository<ExamPkRoom> _repository;

        public ExamPkRoomRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamPkRoom>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;


        public async Task<ExamPkRoom> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task<int> InsertAsync(ExamPkRoom examPkRoom)
        {
            return await _repository.InsertAsync(examPkRoom);
        }

        public async Task UpdateAsync(ExamPkRoom examPkRoom)
        {
            await _repository.UpdateAsync(examPkRoom);
        }
        public async Task RandomPositonAsync(int pkId, int userId)
        {
            var random = await _repository.GetAsync(Q.
                  Where(nameof(ExamPkRoom.UserId_A), 0).
                  Where(nameof(ExamPkRoom.PkId), pkId).Take(1));
            if (random != null)
            {
                random.UserId_A = userId;
                await _repository.UpdateAsync(random);
            }
            else
            {
                random = await _repository.GetAsync(Q.
                 Where(nameof(ExamPkRoom.UserId_B), 0).
                 Where(nameof(ExamPkRoom.PkId), pkId).Take(1));
                if (random != null)
                {
                    random.UserId_B = userId;
                    await _repository.UpdateAsync(random);
                }
            }
        }
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
        public async Task DeleteByPkIdAsync(int pkId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamPkRoom.PkId), pkId));
        }
        public async Task<(int total, List<ExamPkRoom> list)> GetListAsync(int pkId)
        {
            var query = Q.Where(nameof(ExamPkRoom.PkId), pkId);

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query);
            return (total, list);
        }

        public async Task<(int total, List<ExamPkRoom> list)> GetUserListAsync(int userId, int pageIndex, int pageSize)
        {
            var query = Q.Where(nameof(ExamPkRoom.UserId_A), userId).OrWhere(nameof(ExamPkRoom.UserId_B), userId);

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.OrderByDesc(nameof(ExamPkRoom.Id)).ForPage(pageIndex, pageSize));
            return (total, list);
        }
    }
}
