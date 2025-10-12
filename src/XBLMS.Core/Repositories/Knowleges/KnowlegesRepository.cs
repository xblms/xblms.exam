using Datory;
using SqlKata;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public partial class KnowlegesRepository : IKnowlegesRepository
    {
        private readonly Repository<Knowledges> _repository;

        public KnowlegesRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<Knowledges>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<int> InsertAsync(Knowledges item)
        {
            return await _repository.InsertAsync(item);
        }
        public async Task<bool> UpdateAsync(Knowledges item)
        {
            return await _repository.UpdateAsync(item);
        }
        public async Task<List<Knowledges>> GetNewListAsync(int companyId)
        {
            var query = Q.WhereNullOrFalse(nameof(Knowledges.Locked));
            query.Where(q => {
                q.WhereNullOrFalse(nameof(Knowledges.OnlyCompany)).OrWhere(qq => {
                    qq.WhereTrue(nameof(Knowledges.OnlyCompany)).Where(nameof(Knowledges.CompanyId), companyId);
                    return qq;
                });
                return q;
            });

            return await _repository.GetAllAsync(query.OrderByDesc(nameof(Knowledges.Id)).ForPage(1, 12));
        }
        public async Task<(int total, List<Knowledges> list)> GetListAsync(int companyId, int userId, int treeId, bool isTreeWithChild, bool like, bool collect, string keyword, int pageIndex, int pageSize)
        {
            var query = Q.WhereNullOrFalse(nameof(Knowledges.Locked));
            query.Where(q => {
                q.WhereNullOrFalse(nameof(Knowledges.OnlyCompany)).OrWhere(qq => {
                    qq.WhereTrue(nameof(Knowledges.OnlyCompany)).Where(nameof(Knowledges.CompanyId), companyId);
                    return qq;
                });
                return q;
            });
            if (treeId > 0)
            {
                if (isTreeWithChild)
                {
                    query.WhereLike(nameof(Knowledges.TreeParentPath), $"%'{treeId}'%");
                }
                else
                {
                    query.Where(nameof(Knowledges.TreeId), treeId);
                }
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var likeQ = $"%{keyword}%";
                query.Where(q => q
                    .WhereLike(nameof(Knowledges.Name), likeQ)
                );
            }
            if (like)
            {
                query.WhereLike(nameof(Knowledges.LikeUserIds), $"'{userId}'");
            }
            if (collect)
            {
                query.WhereLike(nameof(Knowledges.CollectUserIds), $"'{userId}'");

            }

            query.OrderByDesc(nameof(Knowledges.Likes), nameof(Knowledges.Collects), nameof(Knowledges.Id));

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }
        public async Task<(int total, List<Knowledges> list)> GetListAsync(AdminAuth auth, int treeId, bool isTreeWithChild, string keyword, int pageIndex, int pageSize)
        {
            var query = Q.NewQuery();

            query = GetQueryByAuth(query, auth);

            if (treeId > 0)
            {
                if (isTreeWithChild)
                {
                    query.WhereLike(nameof(Knowledges.TreeParentPath), $"%'{treeId}'%");
                }
                else
                {
                    query.Where(nameof(Knowledges.TreeId), treeId);
                }
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var like = $"%{keyword}%";
                query.Where(q => q
                    .WhereLike(nameof(Knowledges.Name), like)
                );
            }

            query.OrderByDesc(nameof(Knowledges.Likes), nameof(Knowledges.Collects), nameof(Knowledges.Id));

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _repository.ExistsAsync(id);
        }
        public async Task<Knowledges> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }
        public async Task<bool> DeleteAsync(int Id)
        {
            var result = await _repository.DeleteAsync(Id);
            return result;
        }
        public async Task<int> CountAsync(int treeId, bool withChild = true)
        {
            var query = new Query();
            if (treeId > 0)
            {
                if (withChild)
                {
                    query.WhereLike(nameof(Knowledges.TreeParentPath), $"%'{treeId}'%");
                }
                else
                {
                    query.Where(nameof(Knowledges.TreeId), treeId);
                }
            }
            return await _repository.CountAsync(query);
        }

        public async Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount(AdminAuth auth)
        {
            var countQuery = Q.NewQuery();
            var lockedCountQuery = Q.WhereTrue(nameof(Knowledges.Locked));
            var unLockedCountQuery = Q.WhereNullOrFalse(nameof(Knowledges.Locked));

            countQuery = GetQueryByAuth(countQuery, auth);
            lockedCountQuery = GetQueryByAuth(lockedCountQuery, auth);
            unLockedCountQuery = GetQueryByAuth(unLockedCountQuery, auth);

            var count = await _repository.CountAsync(countQuery);
            var lockedCount = await _repository.CountAsync(lockedCountQuery);
            var unLockedCount = await _repository.CountAsync(unLockedCountQuery);
            return (count, 0, 0, lockedCount, unLockedCount);
        }

        public async Task<(int count, int total)> GetTotalAndCountByTreeIdAsync(AdminAuth auth, int treeId)
        {
            var countquery = Q.NewQuery();
            var totalquery = Q.NewQuery();

            countquery = GetQueryByAuth(countquery, auth);
            totalquery = GetQueryByAuth(totalquery, auth);

            var count = await _repository.CountAsync(countquery.Where(nameof(Knowledges.TreeId), treeId));
            var total = await _repository.CountAsync(totalquery.WhereLike(nameof(Knowledges.TreeParentPath), $"%'{treeId}'%"));

            return (count, total);
        }


        private Query GetQueryByAuth(Query query, AdminAuth auth)
        {
            if (auth.AuthDataType == AuthorityDataType.DataCreator)
            {
                query.Where(nameof(Knowledges.CreatorId), auth.AdminId);
            }
            else
            {
                if (auth.AuthDataShowAll)
                {
                    if (auth.CurCompanyId != 1)
                    {
                        query.WhereLike(nameof(Knowledges.CompanyParentPath), $"%'{auth.CurCompanyId}'%");
                    }
                }
                else
                {
                    query.Where(nameof(Knowledges.CompanyId), auth.CurCompanyId);
                }
            }

            return query;
        }

    }
}
