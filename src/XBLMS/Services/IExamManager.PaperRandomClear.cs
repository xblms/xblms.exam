using System.Threading.Tasks;

namespace XBLMS.Services
{
    public partial interface IExamManager
    {
        Task ClearRandom(int examPaperId, bool isClear);
        Task ClearRandomUser(int examPaperId, int userId);
    }

}
