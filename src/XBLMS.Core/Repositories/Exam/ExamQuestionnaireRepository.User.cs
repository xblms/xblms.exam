using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Core.Repositories
{
    public partial class ExamQuestionnaireRepository
    {
        public async Task<(int total, List<ExamQuestionnaire> list)> GetListByUserAsync(List<int> paperIds, string keyWords, int pageIndex, int pageSize)
        {
            var query = Q.WhereNullOrFalse(nameof(ExamQuestionnaire.Locked));

            if (paperIds != null && paperIds.Count > 0)
            {
                query.WhereIn(nameof(ExamQuestionnaire.Id), paperIds);
            }
            else
            {
                return (0, null);
            }
            query.WhereNullOrFalse(nameof(ExamQuestionnaire.Published));

            if (!string.IsNullOrWhiteSpace(keyWords))
            {
                keyWords = $"%{keyWords}%";
                query.WhereLike(nameof(ExamQuestionnaire.Title), keyWords);
            }
            query.OrderByDesc(nameof(ExamQuestionnaire.Id));

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }

    }
}
