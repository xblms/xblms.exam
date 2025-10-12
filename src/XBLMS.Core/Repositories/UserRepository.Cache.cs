﻿using Datory;
using SqlKata;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Core.Repositories
{
    public partial class UserRepository
    {
        private string GetCacheKeyByUserId(int userId)
        {
            return CacheUtils.GetEntityKey(TableName, "userId", userId.ToString());
        }

        private string GetCacheKeyByGuid(string guid)
        {
            return CacheUtils.GetEntityKey(TableName, "guid", guid);
        }

        private string GetCacheKeyByUserName(string userName)
        {
            return CacheUtils.GetEntityKey(TableName, "userName", userName);
        }

        private string GetCacheKeyByMobile(string mobile)
        {
            return CacheUtils.GetEntityKey(TableName, "mobile", mobile);
        }

        private string GetCacheKeyByEmail(string email)
        {
            return CacheUtils.GetEntityKey(TableName, "email", email);
        }

        private string[] GetCacheKeysToRemove(User user)
        {
            if (user == null) return null;

            var list = new List<string>
            {
                GetCacheKeyByUserId(user.Id),
                GetCacheKeyByGuid(user.Guid),
                GetCacheKeyByUserName(user.UserName),
            };

            if (!string.IsNullOrEmpty(user.Mobile))
            {
                list.Add(GetCacheKeyByMobile(user.Mobile));
            }

            if (!string.IsNullOrEmpty(user.Email))
            {
                list.Add(GetCacheKeyByEmail(user.Email));
            }

            return list.ToArray();
        }

        public async Task<User> GetByAccountAsync(string account)
        {
            if (string.IsNullOrEmpty(account)) return null;

            var user = await GetByUserNameAsync(account);
            if (user != null)
            {
                return user;
            }
            if (StringUtils.IsMobile(account))
            {
                return await GetByMobileAsync(account);
            }
            if (StringUtils.IsEmail(account))
            {
                return await GetByEmailAsync(account);
            }
            return null;
        }
        
        private async Task<User> GetAsync(Query query)
        {
            var user = await _repository.GetAsync(query);

            if (user != null && string.IsNullOrEmpty(user.DisplayName))
            {
                user.DisplayName = user.UserName;
            }

            return user;
        }

        public async Task<User> GetByUserIdAsync(int userId)
        {
            if (userId <= 0) return null;

            return await GetAsync(Q
                .Where(nameof(User.Id), userId)
                .CachingGet(GetCacheKeyByUserId(userId))
            );
        }

        public async Task<User> GetByUserNameAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) return null;

            return await GetAsync(Q
                .Where(nameof(User.UserName), userName)
                .CachingGet(GetCacheKeyByUserName(userName))
            );
        }

        public async Task<User> GetByMobileAsync(string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile)) return null;

            return await GetAsync(Q
                .Where(nameof(User.Mobile), mobile)
                .CachingGet(GetCacheKeyByMobile(mobile))
            );
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;

            return await GetAsync(Q
                .Where(nameof(User.Email), email)
                .CachingGet(GetCacheKeyByEmail(email))
            );
        }

        public string GetDisplay(User user)
        {
            if (user == null) return string.Empty;

            return string.IsNullOrEmpty(user.DisplayName) || user.UserName == user.DisplayName ? user.UserName : $"{user.DisplayName}({user.UserName})";
        }

        private Query GetQueryByAuth(Query query, AdminAuth auth)
        {
            if (auth.AuthDataType == AuthorityDataType.DataCreator)
            {
                query.Where(nameof(User.CreatorId), auth.AdminId);
            }
            else
            {
                if (auth.AuthDataShowAll)
                {
                    if (auth.CurCompanyId != 1)
                    {
                        query.WhereLike(nameof(User.CompanyParentPath), $"%'{auth.CurCompanyId}'%");
                    }
                }
                else
                {
                    query.Where(nameof(User.CompanyId), auth.CurCompanyId);
                }
            }

            return query;
        }

        public async Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount(AdminAuth auth)
        {
            var queryCount = Q.NewQuery();
            var queryLocked = Q.WhereTrue(nameof(User.Locked));
            var queryUnLocked = Q.WhereNullOrFalse(nameof(User.Locked));

            queryCount = GetQueryByAuth(queryCount, auth);
            queryLocked = GetQueryByAuth(queryLocked, auth);
            queryUnLocked = GetQueryByAuth(queryUnLocked, auth);


            var count = await _repository.CountAsync(queryCount);
            var lockedCount = await _repository.CountAsync(queryLocked);
            var unLockedCount = await _repository.CountAsync(queryUnLocked);
            return (count, 0, 0, lockedCount, unLockedCount);
        }

  

        public async Task<int> CountTestMaxInAsync()
        {
            var userIds = new List<int>();
            for(var i = 0; i < 100000; i++)
            {
                userIds.Add(i);
            }
            return await _repository.CountAsync(Q.WhereIn(nameof(User.Id), userIds));
        }
    }
}

