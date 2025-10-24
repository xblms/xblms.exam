using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Core.Repositories
{
    public partial class ExamPaperRepository
    {
        public async Task<(int total, List<ExamPaper> list)> Select_GetListAsync(AdminAuth auth, string keyWords, int pageIndex, int pageSize)
        {
            var query = Q.
                WhereNullOrFalse(nameof(ExamPaper.Locked)).
                WhereTrue(nameof(ExamPaper.IsCourseUse)).
                WhereNullOrFalse(nameof(ExamPaper.Moni));

            query = GetQueryByAuth(query, auth);


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
        public async Task<List<int>> Select_GetSeparateStorageIdList()
        {
            return await _repository.GetAllAsync<int>(Q.Select(nameof(ExamPaper.Id)).WhereTrue(nameof(ExamPaper.SeparateStorage)));
        }

    }
}
