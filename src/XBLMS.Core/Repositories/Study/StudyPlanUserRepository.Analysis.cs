using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Core.Repositories
{
    public partial class StudyPlanUserRepository
    {
        public async Task<(int total, List<StudyPlanUser> list)> Analysis_GetListAsync(int userId, string dateFrom, string dateTo, int pageIndex, int pageSize)
        {
            var query = Q.
                Where(nameof(StudyPlanUser.UserId), userId).
                WhereNullOrFalse(nameof(StudyPlanUser.Locked));

            if (!string.IsNullOrEmpty(dateFrom))
            {
                query.Where(nameof(StudyPlanUser.CreatedDate), ">=", dateFrom);
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                query.Where(nameof(StudyPlanUser.CreatedDate), "<=", dateTo);
            }

            query.OrderByDesc(nameof(StudyPlanUser.Id));

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));

            return (total, list);
        }
        public async Task<(int total, int overTotal,int dabiaoTotal)> Analysis_GetTotalAsync(int userId, string dateFrom, string dateTo)
        {
            var query = Q.
                 Where(nameof(StudyPlanUser.UserId), userId).
                 WhereNullOrFalse(nameof(StudyPlanUser.Locked));

            if (!string.IsNullOrEmpty(dateFrom))
            {
                query.Where(nameof(StudyPlanUser.CreatedDate), ">=", dateFrom);
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                query.Where(nameof(StudyPlanUser.CreatedDate), "<=", dateTo);
            }

            var overQuey = query;
            var dabiaoQuery = query;
            var total = await _repository.CountAsync(query);
            var overTotal = await _repository.CountAsync(overQuey.Where(q => {
                q.Where(nameof(StudyPlanUser.State),StudyStatType.Yiwancheng.GetValue()).OrWhere(nameof(StudyPlanUser.State), StudyStatType.Yidabiao.GetValue());
                return q;
            }));

            var datbiaoTotal = await _repository.CountAsync(dabiaoQuery.Where(nameof(StudyPlanUser.State), StudyStatType.Yidabiao.GetValue())); 

            return (total, overTotal, datbiaoTotal);
        }
    }
}
