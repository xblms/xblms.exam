using System.Threading.Tasks;

namespace XBLMS.Repositories
{
    public partial interface IExamPaperAnswerRepository
    {
        Task CreateSeparateStorageAsync(int examPaperId);
        string GetNewTableNameAsync(int examPaperId);
    }
}
