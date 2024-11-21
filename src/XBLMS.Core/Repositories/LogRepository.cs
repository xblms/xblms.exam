using Datory;
using SqlKata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Core.Repositories
{
    public class LogRepository : ILogRepository
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
                    CompanyId=user.CompanyId,
                    DepartmentId=user.DepartmentId,
                    CreatorId=user.Id,
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

            await _repository.DeleteAsync(Q
                .Where(nameof(Log.CreatedDate), "<", DateTime.Now.AddDays(-days))
            );
        }

        public async Task DeleteAllAdminLogsAsync()
        {
            await _repository.DeleteAsync(Q.Where(nameof(Log.AdminId), ">", 0));
        }

        public async Task DeleteAllUserLogsAsync()
        {
            await _repository.DeleteAsync(Q.Where(nameof(Log.UserId), ">", 0));
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

        public async Task<int> GetAdminLogsCountAsync(List<int> adminIds, string keyword, string dateFrom, string dateTo)
        {
            return await _repository.CountAsync(GetAdminQuery(adminIds, keyword, dateFrom, dateTo));
        }

        public async Task<List<Log>> GetAdminLogsAsync(List<int> adminIds, string keyword, string dateFrom, string dateTo, int offset, int limit)
        {
            var query = GetAdminQuery(adminIds, keyword, dateFrom, dateTo);
            query.Offset(offset).Limit(limit);
            return await _repository.GetAllAsync(query);
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

        public async Task<int> GetUserLogsCountAsync(int userId, string keyword, string dateFrom, string dateTo)
        {
            return await _repository.CountAsync(GetUserQuery(userId, keyword, dateFrom, dateTo));
        }

        public async Task<List<Log>> GetUserLogsAsync(int userId, string keyword, string dateFrom, string dateTo, int offset, int limit)
        {
            var query = GetUserQuery(userId, keyword, dateFrom, dateTo);
            query.Offset(offset).Limit(limit);
            return await _repository.GetAllAsync(query);
        }

        public async Task<DateTimeOffset> GetLastRemoveLogDateAsync()
        {
            var addDate = await _repository.GetAsync<DateTime?>(Q
                .Select(nameof(Log.CreatedDate))
                .Where(nameof(Log.Action), "清空数据库日志")
                .OrderByDesc(nameof(Log.Id))
            );

            return addDate ?? DateTime.MinValue;
        }

        public async Task<List<Log>> GetUserLogsAsync(int userId, int offset, int limit)
        {
            return await _repository.GetAllAsync(Q
                .Where(nameof(Log.UserId), userId)
                .Offset(offset)
                .Limit(limit)
                .OrderByDesc(nameof(Log.Id))
            );
        }

        public async Task<List<Log>> GetAdminLogsAsync(int adminId, int offset, int limit)
        {
            return await _repository.GetAllAsync(Q
                .Where(nameof(Log.AdminId), adminId)
                .Offset(offset)
                .Limit(limit)
                .OrderByDesc(nameof(Log.Id))
            );
        }
    }
}
