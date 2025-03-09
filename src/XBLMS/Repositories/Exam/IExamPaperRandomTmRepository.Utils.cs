using System.Threading.Tasks;

namespace XBLMS.Repositories
{
    public partial interface IExamPaperRandomTmRepository
    {
        Task CreateSeparateStorageAsync(int examPaperId);
    }
}
