using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Core.Repositories
{
    public partial class StudyCourseRepository
    {
        public async Task<(int total, List<StudyCourse> list)> Select_GetListAsync(AdminAuth auth, string keyWords, string type, int pageIndex, int pageSize)
        {
            var query = Q.
                WhereNullOrFalse(nameof(StudyCourse.Public)).
                WhereNullOrFalse(nameof(StudyCourse.Locked));

            query = GetQueryByAuth(query, auth);

            if (!string.IsNullOrEmpty(type))
            {
                if (type == "online")
                {
                    query.WhereNullOrFalse(nameof(StudyCourse.OffLine));
                }
                if (type == "offline")
                {
                    query.WhereTrue(nameof(StudyCourse.OffLine));
                }
            }
            if (!string.IsNullOrEmpty(keyWords))
            {
                query.Where(q => q.WhereLike(nameof(StudyCourse.Name), $"%{keyWords}%").OrWhereLike(nameof(StudyCourse.Mark), $"%{keyWords}%"));
            }

            query.OrderByDesc(nameof(StudyCourse.Id));
            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }

    }
}
