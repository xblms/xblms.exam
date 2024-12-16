using Datory;
using System.Threading.Tasks;

namespace XBLMS.Repositories
{
    public interface IExamPkRoomAnswerRepository : IRepository
    {
        Task DeleteByRoomIdAsync(int roomId);
    }
}
