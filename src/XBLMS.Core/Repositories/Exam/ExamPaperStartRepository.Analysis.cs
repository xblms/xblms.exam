using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Core.Repositories
{
    public partial class ExamPaperStartRepository
    {
        public async Task<(int total, List<ExamPaperStart> list)> Analysis_GetListAsync(int userId, bool isMoni, string dateFrom, string dateTo, int pageIndex, int pageSize)
        {
            var query = Q.
                WhereTrue(nameof(ExamPaperStart.IsSubmit)).
                WhereTrue(nameof(ExamPaperStart.IsMark)).
                Where(nameof(ExamPaperStart.UserId), userId).
                WhereNullOrFalse(nameof(ExamPaperStart.Locked));

            if (isMoni)
            {
                query.WhereTrue(nameof(ExamPaperStart.Moni));
            }
            else
            {
                query.WhereNullOrFalse(nameof(ExamPaperStart.Moni));
            }

            if (!string.IsNullOrEmpty(dateFrom))
            {
                query.Where(nameof(ExamPaperStart.EndDateTime), ">=", dateFrom);
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                query.Where(nameof(ExamPaperStart.EndDateTime), "<=", dateTo);
            }

            query.OrderByDesc(nameof(ExamPaperStart.Id));

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));

            return (total, list);
        }

        public async Task<(int total, int passTotal)> Analysis_GetTotalAsync(int userId, bool isMoni, string dateFrom, string dateTo)
        {
            var query = Q.
             WhereTrue(nameof(ExamPaperStart.IsSubmit)).
             WhereTrue(nameof(ExamPaperStart.IsMark)).
             Where(nameof(ExamPaperStart.UserId), userId).
             WhereNullOrFalse(nameof(ExamPaperStart.Locked));

            if (isMoni)
            {
                query.WhereTrue(nameof(ExamPaperStart.Moni));
            }
            else
            {
                query.WhereNullOrFalse(nameof(ExamPaperStart.Moni));
            }

            if (!string.IsNullOrEmpty(dateFrom))
            {
                query.Where(nameof(ExamPaperStart.EndDateTime), ">=", dateFrom);
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                query.Where(nameof(ExamPaperStart.EndDateTime), "<=", dateTo);
            }

            var total = await _repository.CountAsync(query);
            var passTotal = await _repository.CountAsync(query.WhereTrue(nameof(ExamPaperStart.IsPass)));

            return (total, passTotal);
        }
    }
}
