using Datory;
using SqlKata;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Core.Repositories
{
    public partial class ExamTmGroupRepository
    {
        public async Task<bool> ExistsAsync(string name, int companyId)
        {
            return await _repository.ExistsAsync(Q.Where(nameof(ExamTmGroup.GroupName), name).Where(nameof(ExamTmGroup.CompanyId), companyId));
        }

        public async Task<ExamTmGroup> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task<List<ExamTmGroup>> GetListByOpenUserAsync()
        {
            var query = Q.WhereNullOrFalse(nameof(ExamTmGroup.Locked)).WhereTrue(nameof(ExamTmGroup.OpenUser));

            return await _repository.GetAllAsync(query);
        }

        public async Task<List<ExamTmGroup>> GetListAsync(AdminAuth auth, string keyWords = null, bool withoutLocked = false)
        {
            var query = Q.OrderByDesc(nameof(ExamTmGroup.Id));
            if (!string.IsNullOrEmpty(keyWords))
            {
                query.WhereLike(nameof(ExamTmGroup.GroupName), $"%{keyWords}%");
            }
            if (withoutLocked)
            {
                query.WhereNullOrFalse(nameof(ExamTmGroup.Locked));
            }
            query = GetQueryByAuth(query, auth);

            return await _repository.GetAllAsync(query);
        }

        private Query GetQueryByAuth(Query query, AdminAuth auth)
        {
            if (auth.AuthDataType == AuthorityDataType.DataCreator)
            {
                query.Where(nameof(ExamTmGroup.CreatorId), auth.AdminId);
            }
            else
            {
                if (auth.AuthDataShowAll)
                {
                    if (auth.CurCompanyId != 1)
                    {
                        query.WhereLike(nameof(ExamTmGroup.CompanyParentPath), $"%'{auth.CurCompanyId}'%");
                    }
                }
                else
                {
                    query.Where(nameof(ExamTmGroup.CompanyId), auth.CurCompanyId);
                }
            }

            return query;
        }
    }
}
