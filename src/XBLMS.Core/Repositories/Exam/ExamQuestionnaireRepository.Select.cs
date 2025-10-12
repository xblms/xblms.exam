using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Core.Repositories
{
    public partial class ExamQuestionnaireRepository
    {
        public async Task<(int total, List<ExamQuestionnaire> list)> Select_GetListAsync(AdminAuth auth, string keyword, int pageIndex, int pageSize)
        {
            var query = Q.WhereNullOrFalse(nameof(ExamQuestionnaire.Locked)).WhereTrue(nameof(ExamQuestionnaire.IsCourseUse));

            query = GetQueryByAuth(query, auth);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var like = $"%{keyword}%";
                query.Where(q => q
                    .WhereLike(nameof(ExamQuestionnaire.Title), like)
                );
            }
            query.OrderByDesc(nameof(ExamQuestionnaire.Id));

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }

    }
}
