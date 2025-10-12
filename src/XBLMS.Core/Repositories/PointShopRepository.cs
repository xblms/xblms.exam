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
    public class PointShopRepository : IPointShopRepository
    {
        private readonly Repository<PointShop> _repository;

        public PointShopRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<PointShop>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<PointShop> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _repository.ExistsAsync(id);
        }


        public async Task<(int total, List<PointShop> list)> GetListAsync(AdminAuth auth, string keyWords, int pageIndex, int pageSize)
        {
            var query = Q.OrderByDesc(nameof(PointShop.Id));

            if (!string.IsNullOrEmpty(keyWords))
            {
                query.WhereLike(nameof(PointShop.Name), $"%{keyWords}%");
            }

            query = GetQueryByAuth(query, auth);

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }

        public async Task<(int total, List<PointShop> list)> GetListAsync(int companyId, int point, string keyWords, int pageIndex, int pageSize)
        {
            var query = Q.WhereNullOrFalse(nameof(PointShop.Locked));

            query.Where(nameof(PointShop.Point), ">=", point);

            query.Where(q =>
            {
                q.WhereNullOrFalse(nameof(PointShop.OnlyCompany)).OrWhere(qq =>
                {
                    qq.WhereTrue(nameof(PointShop.OnlyCompany)).Where(nameof(PointShop.CompanyId), companyId);
                    return qq;
                });
                return q;
            });

            if (!string.IsNullOrEmpty(keyWords))
            {
                query.WhereLike(nameof(PointShop.Name), $"%{keyWords}%");
            }

            query.OrderByDesc(nameof(PointShop.Id));

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }
        public async Task<int> InsertAsync(PointShop item)
        {
            return await _repository.InsertAsync(item);
        }

        public async Task UpdateAsync(PointShop item)
        {
            await _repository.UpdateAsync(item);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount(AdminAuth auth)
        {
            var countQuery = Q.NewQuery();
            var lockedCountQuery = Q.WhereTrue(nameof(PointShop.Locked));
            var unLockedCountQuery = Q.WhereNullOrFalse(nameof(PointShop.Locked));

            countQuery = GetQueryByAuth(countQuery, auth);
            lockedCountQuery = GetQueryByAuth(lockedCountQuery, auth);
            unLockedCountQuery = GetQueryByAuth(unLockedCountQuery, auth);

            var count = await _repository.CountAsync(countQuery);
            var lockedCount = await _repository.CountAsync(lockedCountQuery);
            var unLockedCount = await _repository.CountAsync(unLockedCountQuery);
            return (count, 0, 0, lockedCount, unLockedCount);
        }

        private Query GetQueryByAuth(Query query, AdminAuth auth)
        {
            if (auth.AuthDataType == AuthorityDataType.DataCreator)
            {
                query.Where(nameof(PointShop.CreatorId), auth.AdminId);
            }
            else
            {
                if (auth.AuthDataShowAll)
                {
                    if (auth.CurCompanyId != 1)
                    {
                        query.WhereLike(nameof(PointShop.CompanyParentPath), $"%'{auth.CurCompanyId}'%");
                    }
                }
                else
                {
                    query.Where(nameof(PointShop.CompanyId), auth.CurCompanyId);
                }
            }

            return query;
        }
    }
}
