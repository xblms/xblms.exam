using Datory;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IKnowlegesTreeRepository : IRepository
    {
        Task<int> InsertAsync(KnowledgesTree item);

        Task<bool> UpdateAsync(KnowledgesTree item);
        Task<bool> DeleteAsync(int id);
    }
}
