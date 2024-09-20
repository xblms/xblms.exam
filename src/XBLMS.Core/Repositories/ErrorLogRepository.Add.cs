using System;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Models;

namespace XBLMS.Core.Repositories
{
    public partial class ErrorLogRepository
    {
        public async Task<int> AddErrorLogAsync(ErrorLog log)
        {
            try
            {
                var config = await _configRepository.GetAsync();
                if (!config.IsLogError) return 0;

                await DeleteIfThresholdAsync();

                return await InsertAsync(log);
            }
            catch
            {
                // ignored
            }

            return 0;
        }

        public async Task<int> AddErrorLogAsync(Exception ex, string summary = "")
        {
            return await AddErrorLogAsync(new ErrorLog
            {
                Id = 0,
                Category = LogUtils.CategoryAdmin,
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                Summary = summary,
            });
        }
    }
}
