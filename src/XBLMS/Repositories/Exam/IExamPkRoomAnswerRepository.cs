using Datory;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IExamPkRoomAnswerRepository : IRepository
    {
        Task DeleteByRoomIdAsync(int roomId);
    }
}
