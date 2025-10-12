using Datory;
using SqlKata;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public class StatRepository : IStatRepository
    {
        private readonly Repository<Stat> _repository;

        public StatRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<Stat>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task AddCountAsync(Administrator admin, StatType statType)
        {
            var lowerDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var higherDate = lowerDate.AddDays(1);

            var query = Q
                .Where(nameof(Stat.StatType), statType.GetValue())
                .WhereBetween(nameof(Stat.CreatedDate), lowerDate, higherDate);

            if (admin != null)
            {
                query.Where(nameof(Stat.AdminId), admin.Id);
            }
            else
            {
                admin = new Administrator();
            }

            if (await _repository.ExistsAsync(query))
            {
                await _repository.IncrementAsync(nameof(Stat.Count), query);
            }
            else
            {
                await _repository.InsertAsync(new Stat
                {
                    StatType = statType,
                    AdminId = admin.Id,
                    CreatorId = admin.Id,
                    CompanyId = admin.CompanyId,
                    DepartmentId = admin.DepartmentId,
                    CompanyParentPath = admin.CompanyParentPath,
                    DepartmentParentPath = admin.DepartmentParentPath,
                    Count = 1
                });
            }
        }
        public async Task AddUserCountAsync(User user, StatType statType)
        {
            var lowerDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var higherDate = lowerDate.AddDays(1);

            var query = Q
                .Where(nameof(Stat.StatType), statType.GetValue())
                .WhereBetween(nameof(Stat.CreatedDate), lowerDate, higherDate);

            if (user != null)
            {
                query.Where(nameof(Stat.AdminId), user.Id);
            }
            else
            {
                user = new User();
            }

            if (await _repository.ExistsAsync(query))
            {
                await _repository.IncrementAsync(nameof(Stat.Count), query);
            }
            else
            {
                await _repository.InsertAsync(new Stat
                {
                    StatType = statType,
                    AdminId = user.Id,
                    CreatorId = user.Id,
                    CompanyId = user.CompanyId,
                    DepartmentId = user.DepartmentId,
                    CompanyParentPath = user.CompanyParentPath,
                    DepartmentParentPath = user.DepartmentParentPath,
                    Count = 1
                });
            }
        }

        public async Task<List<Stat>> GetStatsAsync(DateTime lowerDate, DateTime higherDate, StatType statType)
        {
            var query = Q
                .Where(nameof(Stat.StatType), statType.GetValue())
                .WhereBetween(nameof(Stat.CreatedDate), lowerDate, higherDate.AddDays(1));

            return await _repository.GetAllAsync(query);
        }
        public async Task<List<Stat>> GetStatsAsync(AdminAuth auth, DateTime lowerDate, DateTime higherDate, StatType statType)
        {
            var query = Q
                .Where(nameof(Stat.StatType), statType.GetValue())
               .WhereBetween(nameof(Stat.CreatedDate), lowerDate, higherDate.AddDays(1));

            query = GetQueryByAuth(query, auth);

            return await _repository.GetAllAsync(query);
        }

        public async Task<List<Stat>> GetStatsWithAdminIdAsync(DateTime lowerDate, DateTime higherDate, StatType statType)
        {
            var query = Q
                .Where(nameof(Stat.StatType), statType.GetValue())
                .WhereNot(nameof(Stat.AdminId), 0)
                .WhereBetween(nameof(Stat.CreatedDate), lowerDate, higherDate.AddDays(1));

            return await _repository.GetAllAsync(query);
        }

        public async Task<int> SumAsync(StatType statType)
        {
            return await _repository.SumAsync(nameof(Stat.Count), Q.Where(nameof(Stat.StatType), statType.GetValue()));
        }
        public async Task<int> SumAsync(StatType statType, AdminAuth auth)
        {
            var query = Q.Where(nameof(Stat.StatType), statType.GetValue());

            query = GetQueryByAuth(query, auth);

            return await _repository.SumAsync(nameof(Stat.Count), query);
        }

        private Query GetQueryByAuth(Query query, AdminAuth auth)
        {
            if (auth.AuthDataType == AuthorityDataType.DataCreator)
            {
                query.Where(nameof(Stat.AdminId), auth.AdminId);
            }
            else
            {
                if (auth.AuthDataShowAll)
                {
                    if (auth.CurCompanyId != 1)
                    {
                        query.WhereLike(nameof(Stat.CompanyParentPath), $"%'{auth.CurCompanyId}'%");
                    }
                }
                else
                {
                    query.Where(nameof(Stat.CompanyId), auth.CurCompanyId);
                }
            }

            return query;
        }
    }
}
