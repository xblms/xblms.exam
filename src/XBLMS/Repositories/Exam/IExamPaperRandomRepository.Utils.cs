using System.Threading.Tasks;

namespace XBLMS.Repositories
{
    public partial interface IExamPaperRandomRepository
    {
        Task CreateSeparateStorageAsync(int examPaperId);
    }
}
