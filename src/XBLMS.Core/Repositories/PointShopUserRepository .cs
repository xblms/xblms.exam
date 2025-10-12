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
    public partial class PointShopUserRepository : IPointShopUserRepository
    {
        private readonly Repository<PointShopUser> _repository;

        public PointShopUserRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<PointShopUser>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<PointShopUser> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task<(int total, List<PointShopUser> list)> GetListAsync(AdminAuth auth, int id, string keyWords, int pageIndex, int pageSize)
        {
            var query = Q.OrderByDesc(nameof(PointShopUser.Id));

            if (id > 0)
            {
                query.Where(nameof(PointShopUser.ShopId), id);
            }

            if (!string.IsNullOrEmpty(keyWords))
            {
                query.WhereLike(nameof(PointShopUser.KeyWordsAdmin), $"%{keyWords}%");
            }

            query = GetQueryByAuth(query, auth);

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }
        public async Task<(int total, List<PointShopUser> list)> GetListAsync(int userId, string dateFrom, string dateTo, string keyWords, int pageIndex, int pageSize)
        {
            var query = Q.OrderByDesc(nameof(PointShopUser.Id));

            if (!string.IsNullOrEmpty(keyWords))
            {
                query.WhereLike(nameof(PointShopUser.KeyWords), $"%{keyWords}%");
            }

            if (!string.IsNullOrWhiteSpace(dateFrom))
            {
                query.Where(nameof(PointShopUser.CreatedDate), ">=", dateFrom);
            }
            if (!string.IsNullOrWhiteSpace(dateTo))
            {
                query.Where(nameof(PointShopUser.CreatedDate), "<=", dateTo);
            }

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }

        public async Task<int> InsertAsync(PointShopUser item)
        {
            return await _repository.InsertAsync(item);
        }

        public async Task UpdateAsync(PointShopUser item)
        {
            await _repository.UpdateAsync(item);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        private Query GetQueryByAuth(Query query, AdminAuth auth)
        {
            if (auth.AuthDataType == AuthorityDataType.DataCreator)
            {
                query.Where(nameof(PointShopUser.CreatorId), auth.AdminId);
            }
            else
            {
                if (auth.AuthDataShowAll)
                {
                    if (auth.CurCompanyId != 1)
                    {
                        query.WhereLike(nameof(PointShopUser.CompanyParentPath), $"%'{auth.CurCompanyId}'%");
                    }
                }
                else
                {
                    query.Where(nameof(PointShopUser.CompanyId), auth.CurCompanyId);
                }
            }

            return query;
        }
    }
}
