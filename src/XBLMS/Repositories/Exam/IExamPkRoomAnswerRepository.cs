using Datory;
using System.Threading.Tasks;

namespace XBLMS.Repositories
{
    public interface IExamPkRoomAnswerRepository : IRepository
    {
        Task DeleteByUserId(int userId);
        Task DeleteByRoomIdAsync(int roomId);
    }
}
