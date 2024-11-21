using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Core.Repositories
{
    public partial class ExamPaperRepository
    {
        public async Task<(int total, List<ExamPaper> list)> GetListByUserAsync(List<int> paperIds, string keyWords, int pageIndex, int pageSize, bool isMoni = false)
        {
            var query = Q.WhereNullOrFalse(nameof(ExamPaper.Locked));

            if (paperIds != null && paperIds.Count > 0)
            {
                query.WhereIn(nameof(ExamPaper.Id), paperIds);
            }
            else
            {
                return (0, null);
            }


            if (isMoni)
            {
                query.WhereTrue(nameof(ExamPaper.Moni));
            }
            else
            {
                query.WhereNullOrFalse(nameof(ExamPaper.Moni));
            }
            if (!string.IsNullOrWhiteSpace(keyWords))
            {
                keyWords = $"%{keyWords}%";
                query.WhereLike(nameof(ExamPaper.Title), keyWords);
            }
            query.OrderByDesc(nameof(ExamPaper.Id));

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }

    }
}
