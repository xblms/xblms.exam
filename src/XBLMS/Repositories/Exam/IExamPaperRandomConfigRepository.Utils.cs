using System.Threading.Tasks;

namespace XBLMS.Repositories
{
    public partial interface IExamPaperRandomConfigRepository
    {
        Task CreateSeparateStorageAsync(int examPaperId);
    }
}
