using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Core.Repositories
{
    public partial class ExamAssessmentUserRepository
    {
        public async Task<(int total, List<ExamAssessmentUser> list)> Analysis_GetListAsync(int userId, string dateFrom, string dateTo, int pageIndex, int pageSize)
        {
            var query = Q.
                Where(nameof(ExamAssessmentUser.SubmitType), SubmitType.Submit.GetValue()).
                Where(nameof(ExamAssessmentUser.UserId), userId).
                WhereNullOrFalse(nameof(ExamAssessmentUser.Locked));

            if (!string.IsNullOrEmpty(dateFrom))
            {
                query.Where(nameof(ExamAssessmentUser.ExamEndDateTime), ">=", dateFrom);
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                query.Where(nameof(ExamAssessmentUser.ExamEndDateTime), "<=", dateTo);
            }

            query.OrderByDesc(nameof(ExamAssessmentUser.Id));

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));

            return (total, list);
        }

        public async Task<(int total, int submitTotal)> Analysis_GetTotalAsync(int userId, string dateFrom, string dateTo)
        {
            var query = Q.
              Where(nameof(ExamAssessmentUser.UserId), userId).
              WhereNullOrFalse(nameof(ExamAssessmentUser.Locked));

            if (!string.IsNullOrEmpty(dateFrom))
            {
                query.Where(nameof(ExamAssessmentUser.ExamEndDateTime), ">=", dateFrom);
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                query.Where(nameof(ExamAssessmentUser.ExamEndDateTime), "<=", dateTo);
            }

            var total = await _repository.CountAsync(query);
            var submitTotal = await _repository.CountAsync(query.Where(nameof(ExamAssessmentUser.SubmitType), SubmitType.Submit.GetValue()));

            return (total, submitTotal);
        }
    }
}
