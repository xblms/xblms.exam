using Datory;
using System;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public partial class AuthManager
    {

        public async Task AddAdminLogAsync(string action, string summary)
        {
            var admin = await GetAdminAsync();
            if (admin != null)
            {
                var ipAddress = PageUtils.GetIpAddress(_context.HttpContext.Request);
                await _databaseManager.LogRepository.AddAdminLogAsync(admin, ipAddress, action, summary);
            }
        }

        public async Task AddAdminLogAsync(string action)
        {
            var admin = await GetAdminAsync();
            if (admin != null)
            {
                var ipAddress = PageUtils.GetIpAddress(_context.HttpContext.Request);
                await _databaseManager.LogRepository.AddAdminLogAsync(admin, ipAddress, action);
            }
        }

        public async Task AddUserLogAsync(string action, string summary)
        {
            var user = await GetUserAsync();
            if (user != null)
            {
                var ipAddress = PageUtils.GetIpAddress(_context.HttpContext.Request);
                await _databaseManager.LogRepository.AddUserLogAsync(user, ipAddress, action, summary);
            }
        }

        public async Task AddUserLogAsync(string action)
        {
            var user = await GetUserAsync();
            if (user != null)
            {
                var ipAddress = PageUtils.GetIpAddress(_context.HttpContext.Request);
                await _databaseManager.LogRepository.AddUserLogAsync(user, ipAddress, action);
            }
        }


        public async Task AddStatLogAsync(StatType statType, string statTypeStr, int objectId = 0, string objectName = "", object entity = null)
        {
            var admin = await GetAdminAsync();
            if (admin != null)
            {
                var ipAddress = PageUtils.GetIpAddress(_context.HttpContext.Request);
                var entityStr = string.Empty;
                if (entity != null)
                {
                    entityStr = TranslateUtils.JsonSerialize(entity);
                }
                statTypeStr = StringUtils.SubString(statTypeStr, 300);
                objectName = StringUtils.SubString(objectName, 300);
                await _databaseManager.StatLogRepository.InsertAsync(admin, statType, statTypeStr, ipAddress, objectId, objectName, entityStr);
            }
        }

        public async Task AddStatCount(StatType statType, Administrator admin = null)
        {
            if (admin == null)
            {
                admin = await GetAdminAsync();
            }
            await _databaseManager.StatRepository.AddCountAsync(admin, statType);
        }
        public async Task AddUserStatCount(StatType statType, User user = null)
        {
            if (user == null)
            {
                user = await GetUserAsync();
            }
            await _databaseManager.StatRepository.AddUserCountAsync(user, statType);
        }

        public async Task AddPointsLogAsync(PointType pointType, User user, int objectId = 0, string objectName = "", bool isNotice = false)
        {
            var dateStr = DateTime.Now.ToString("yyyy-MM-dd");
            var (value, maxvalue) = await _databaseManager.ConfigRepository.GetPointValueByPointType(pointType);
            var todayMaxvalue = await _databaseManager.PointLogRepository.GetSumPoint(pointType, user.Id, dateStr);

            if (!isNotice)
            {
                isNotice = pointType == PointType.PointExamFull || pointType == PointType.PointExamPass;
            }
            if (maxvalue > todayMaxvalue)
            {
                var log = new PointLog()
                {
                    IsNotice = isNotice,
                    UserId = user.Id,
                    CompanyId = user.CompanyId,
                    DepartmentId = user.DepartmentId,
                    CompanyParentPath = user.CompanyParentPath,
                    DepartmentParentPath = user.DepartmentParentPath,
                    CreatorId = user.Id,
                    ObjectId = objectId,
                    ObjectName = objectName,
                    PointType = pointType,
                    DateStr = dateStr,
                    Point = value,
                    Subject = $"{pointType.GetDisplayName()}奖励积分"
                };
                await _databaseManager.PointLogRepository.InsertAsync(log);
                await _databaseManager.UserRepository.UpdatePointsAsync(value, user.Id);
            }
        }

        public async Task<PointNotice> PointNotice(PointType type, int userId)
        {
            var (notice, value, msg) = await _databaseManager.PointLogRepository.GetNotice(type, userId);
            return new PointNotice
            {
                Success = notice,
                Value = value,
                Msg = msg
            };
        }
        public async Task<PointNotice> PointNotice(int userId)
        {
            var (notice, value, msg) = await _databaseManager.PointLogRepository.GetNotice(userId);
            return new PointNotice
            {
                Success = notice,
                Value = value,
                Msg = msg
            };
        }
    }
}
