using Datory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Core.Repositories
{
    public partial class StudyCourseUserRepository
    {
        public async Task<decimal> Analysis_GetTotalCreditAsync(int userId, string dateFrom, string dateTo)
        {
            decimal totalCredit = 0;

            var query = Q.
                Where(nameof(StudyCourseUser.State), StudyStatType.Yiwancheng.GetValue()).
                Where(nameof(StudyCourseUser.UserId), userId).
                WhereNullOrFalse(nameof(StudyCourseUser.Locked));

            if (!string.IsNullOrEmpty(dateFrom))
            {
                query.Where(nameof(StudyCourseUser.OverStudyDateTime), ">=", dateFrom);
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                query.Where(nameof(StudyCourseUser.OverStudyDateTime), "<=", dateTo);
            }

            var list = await _repository.GetAllAsync(query);
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    totalCredit += item.Credit;
                }
            }
            return Math.Round(totalCredit, 2);
        }
        public async Task<(int total, int overTotal)> Analysis_GetTotalAsync(int userId, string dateFrom, string dateTo)
        {
            var query = Q.
               Where(nameof(StudyCourseUser.UserId), userId).
               WhereNullOrFalse(nameof(StudyCourseUser.Locked));

            if (!string.IsNullOrEmpty(dateFrom))
            {
                query.Where(nameof(StudyCourseUser.CreatedDate), ">=", dateFrom);
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                query.Where(nameof(StudyCourseUser.CreatedDate), "<=", dateTo);
            }
            var total = await _repository.CountAsync(query);
            var overTotal = await _repository.CountAsync(query.Where(nameof(StudyCourseUser.State), StudyStatType.Yiwancheng.GetValue()));

            return (total, overTotal);
        }
        public async Task<(int total, List<StudyCourseUser> list)> Analysis_GetListAsync(int userId, string dateFrom, string dateTo, int pageIndex, int pageSize)
        {
            var query = Q.
                Where(nameof(StudyCourseUser.UserId), userId).
                WhereNullOrFalse(nameof(StudyCourseUser.Locked));

            if (!string.IsNullOrEmpty(dateFrom))
            {
                query.Where(nameof(StudyCourseUser.CreatedDate), ">=", dateFrom);
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                query.Where(nameof(StudyCourseUser.CreatedDate), "<=", dateTo);
            }

            query.OrderByDesc(nameof(StudyCourseUser.Id));

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));

            return (total, list);
        }
    }
}
