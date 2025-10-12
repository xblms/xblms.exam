using Datory;
using SqlKata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Core.Repositories
{
    public partial class LogRepository : ILogRepository
    {
        private readonly Repository<Log> _repository;
        private readonly IConfigRepository _configRepository;
        private readonly IAdministratorRepository _administratorRepository;
        private readonly IUserRepository _userRepository;
        private readonly IErrorLogRepository _errorLogRepository;

        public LogRepository(ISettingsManager settingsManager, IConfigRepository configRepository, IAdministratorRepository administratorRepository, IUserRepository userRepository, IErrorLogRepository errorLogRepository)
        {
            _repository = new Repository<Log>(settingsManager.Database, settingsManager.Redis);
            _configRepository = configRepository;
            _administratorRepository = administratorRepository;
            _userRepository = userRepository;
            _errorLogRepository = errorLogRepository;
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task AddAdminLogAsync(Administrator admin, string ipAddress, string action, string summary = "")
        {
            var config = await _configRepository.GetAsync();
            if (!config.IsLogAdmin) return;

            try
            {
                await DeleteIfThresholdAsync();

                if (!string.IsNullOrEmpty(action))
                {
                    action = StringUtils.MaxLengthText(action, 250);
                }
                if (!string.IsNullOrEmpty(summary))
                {
                    summary = StringUtils.MaxLengthText(summary, 250);
                }

                var log = new Log
                {
                    Id = 0,
                    AdminId = admin.Id,
                    IpAddress = ipAddress,
                    Action = action,
                    Summary = summary,
                    CompanyId = admin.CompanyId,
                    DepartmentId = admin.DepartmentId,
                    CreatorId = admin.Id,
                    CompanyParentPath = admin.CompanyParentPath,
                    DepartmentParentPath = admin.DepartmentParentPath,
                };

                await _repository.InsertAsync(log);

                await _administratorRepository.UpdateLastActivityDateAsync(admin);
            }
            catch (Exception ex)
            {
                await _errorLogRepository.AddErrorLogAsync(ex);
            }
        }

        public async Task AddUserLogAsync(User user, string ipAddress, string action, string summary = "")
        {
            var config = await _configRepository.GetAsync();
            if (!config.IsLogAdmin) return;

            try
            {
                await DeleteIfThresholdAsync();

                if (!string.IsNullOrEmpty(action))
                {
                    action = StringUtils.MaxLengthText(action, 250);
                }
                if (!string.IsNullOrEmpty(summary))
                {
                    summary = StringUtils.MaxLengthText(summary, 250);
                }

                var log = new Log
                {
                    Id = 0,
                    UserId = user.Id,
                    IpAddress = ipAddress,
                    Action = action,
                    Summary = summary,
                    CompanyId = user.CompanyId,
                    DepartmentId = user.DepartmentId,
                    CreatorId = user.Id,
                    CompanyParentPath = user.CompanyParentPath,
                    DepartmentParentPath = user.DepartmentParentPath,
                };

                await _repository.InsertAsync(log);

                await _userRepository.UpdateLastActivityDateAsync(user);
            }
            catch (Exception ex)
            {
                await _errorLogRepository.AddErrorLogAsync(ex);
            }
        }

        public async Task DeleteIfThresholdAsync()
        {
            var config = await _configRepository.GetAsync();
            if (!config.IsTimeThreshold) return;

            var days = config.TimeThreshold;
            if (days <= 0) return;

            await _repository.DeleteAsync(Q.Where(nameof(Log.CreatedDate), "<", DateTime.Now.AddDays(-days)));
        }

        public async Task DeleteAllAdminLogsAsync(AdminAuth auth)
        {
            var query = Q.Where(nameof(Log.AdminId), ">", 0);
            query = GetQueryByAuth(query, auth);
            await _repository.DeleteAsync(query);
        }

        public async Task DeleteAllUserLogsAsync(AdminAuth auth)
        {
            var query = Q.Where(nameof(Log.UserId), ">", 0);
            query = GetQueryByAuth(query, auth);
            await _repository.DeleteAsync(query);
        }

        private Query GetAdminQuery(List<int> adminIds, string keyword, string dateFrom, string dateTo)
        {
            var query = Q
                .Where(nameof(Log.AdminId), ">", 0)
                .OrderByDesc(nameof(Log.Id));

            if (!adminIds.Any() && string.IsNullOrEmpty(keyword) && string.IsNullOrEmpty(dateFrom) && string.IsNullOrEmpty(dateTo))
            {
                return query;
            }

            if (adminIds.Any())
            {
                query.WhereIn(nameof(Log.AdminId), adminIds);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                var like = $"%{keyword}%";
                query.Where(q =>
                    q.WhereLike(nameof(Log.Action), like).OrWhereLike(nameof(Log.Summary), like)
                );
            }

            if (!string.IsNullOrEmpty(dateFrom))
            {
                query.Where(nameof(Log.CreatedDate), ">=", DateUtils.ToString(dateFrom));
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                query.Where(nameof(Log.CreatedDate), "<=", DateUtils.ToString(dateTo));
            }

            return query;
        }

        public async Task<(int total, List<Log> list)> GetAdminLogsAsync(AdminAuth auth, List<int> adminIds, string keyword, string dateFrom, string dateTo, int offset, int limit)
        {
            var query = GetAdminQuery(adminIds, keyword, dateFrom, dateTo);

            query = GetQueryByAuth(query, auth);
            var total = await _repository.CountAsync(query);
            query.Offset(offset).Limit(limit);
            var list = await _repository.GetAllAsync(query);
            return (total, list);
        }

        private Query GetUserQuery(int userId, string keyword, string dateFrom, string dateTo)
        {
            var query = Q
                .Where(nameof(Log.UserId), ">", 0)
                .OrderByDesc(nameof(Log.Id));

            if (userId == 0 && string.IsNullOrEmpty(keyword) && string.IsNullOrEmpty(dateFrom) && string.IsNullOrEmpty(dateTo))
            {
                return query;
            }

            if (userId > 0)
            {
                query.Where(nameof(Log.UserId), userId);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                var like = $"%{keyword}%";
                query.Where(q =>
                    q.WhereLike(nameof(Log.Action), like).OrWhereLike(nameof(Log.Summary), like)
                );
            }

            if (!string.IsNullOrEmpty(dateFrom))
            {
                query.Where(nameof(Log.CreatedDate), ">=", DateUtils.ToString(dateFrom));
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                query.Where(nameof(Log.CreatedDate), "<=", DateUtils.ToString(dateTo));
            }

            return query;
        }

        public async Task<(int total, List<Log> list)> GetUserLogsAsync(AdminAuth auth, int userId, string keyword, string dateFrom, string dateTo, int offset, int limit)
        {
            var query = GetUserQuery(userId, keyword, dateFrom, dateTo);

            query = GetQueryByAuth(query, auth);

            var total = await _repository.CountAsync(query);

            query.Offset(offset).Limit(limit);
            var list = await _repository.GetAllAsync(query);
            return (total, list);
        }
        private Query GetQueryByAuth(Query query, AdminAuth auth)
        {
            if (auth.AuthDataType == Enums.AuthorityDataType.DataCreator)
            {
                query.Where(nameof(Log.CreatorId), auth.AdminId);
            }
            else
            {
                if (auth.AuthDataShowAll)
                {
                    if (auth.CurCompanyId != 1)
                    {
                        query.WhereLike(nameof(Log.CompanyParentPath), $"%'{auth.CurCompanyId}'%");
                    }
                }
                else
                {
                    query.Where(nameof(Log.CompanyId), auth.CurCompanyId);
                }
            }


            return query;
        }
    }
}
