using XBLMS.Core.Utils;

namespace XBLMS.Core.Repositories
{
    public partial class ExamPaperRepository
    {
        private string GetCacheKey(int Id)
        {
            return CacheUtils.GetEntityKey(TableName, "id", Id.ToString());
        }
    }
}
