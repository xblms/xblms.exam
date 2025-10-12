using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public class StatLogRepository : IStatLogRepository
    {
        private readonly Repository<StatLog> _repository;

        public StatLogRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<StatLog>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;
        public async Task<StatLog> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task InsertAsync(Administrator admin, StatType statType, string statTypeStr, string ip, int ohjectId, string objectName, string entity)
        {
            await _repository.InsertAsync(new StatLog
            {
                IpAddress = ip,
                StatType = statType,
                StatTypeStr = statTypeStr,
                AdminId = admin.Id,
                ObjectId = ohjectId,
                ObjectName = objectName,
                LastEntity = entity,
                CreatorId = admin.Id,
                CompanyParentPath = admin.CompanyParentPath,
                DepartmentParentPath = admin.DepartmentParentPath,
                DepartmentId = admin.DepartmentId,
                CompanyId = admin.AuthDataCurrentOrganId
            });
        }


        public async Task<(int total, List<StatLog> list)> GetListAsync(AdminAuth auth, int pageIndex, int pageSize)
        {
            var query = Q.NewQuery();

            if (auth.AuthDataType == AuthorityDataType.DataCreator)
            {
                query.Where(nameof(StatLog.CreatorId), auth.AdminId);
            }
            else
            {
                if (auth.AuthDataShowAll)
                {
                    if (auth.CurCompanyId != 1)
                    {
                        query.WhereLike(nameof(StatLog.CompanyParentPath), $"%'{auth.CurCompanyId}'%");
                    }
                }
                else
                {
                    query.Where(nameof(StatLog.CompanyId), auth.CurCompanyId);
                }
            }
            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.OrderByDesc(nameof(Stat.Id)).ForPage(pageIndex, pageSize));
            return (total, list);
        }

    }
}
