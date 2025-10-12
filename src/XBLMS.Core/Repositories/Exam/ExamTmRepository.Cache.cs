using Datory;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Models;

namespace XBLMS.Core.Repositories
{
    public partial class ExamTmRepository
    {
        private string GetCacheKey(int companyId)
        {
            return CacheUtils.GetEntityKey(TableName, "id", companyId.ToString());
        }

        public List<ExamTm> CacheGetListAsync(int companyId)
        {
            var list = _cacheManager.Get<List<ExamTm>>(GetCacheKey(companyId));
            return list;
        }
        public async Task<List<ExamTm>> CacheSetListAsync(int companyId)
        {
            CacheRemoveListAsync(companyId);

            var allList = new List<ExamTm>();

            await CacheSetListPageAsync(allList, 1, 1000);
 
            _cacheManager.AddOrUpdate(GetCacheKey(companyId), allList);
            return allList;
        }
        public async Task CacheSetListPageAsync(List<ExamTm> allList, int pageIndex, int pageSize)
        {
            var query = Q.WhereNullOrFalse(nameof(ExamTm.Locked)).OrderBy(nameof(ExamTm.Id));
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            if (list != null && list.Count > 0)
            {
                allList.AddRange(list);
                pageIndex++;
                Thread.Sleep(1000);
                await CacheSetListPageAsync(allList, pageIndex, pageSize);
            }
        }
        public void CacheRemoveListAsync(int companyId)
        {
            _cacheManager.Remove(GetCacheKey(companyId));
        }
    }
}
