using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Datory;
using SqlKata;
using XBLMS.Core.Utils;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Core.Repositories
{
    public partial class UserRepository : IUserRepository
    {
        private readonly Repository<User> _repository;
        private readonly ICacheManager _cacheManager;
        private readonly IConfigRepository _configRepository;

        public UserRepository(ISettingsManager settingsManager, ICacheManager cacheManager, IConfigRepository configRepository)
        {
            _repository = new Repository<User>(settingsManager.Database, settingsManager.Redis);
            _cacheManager = cacheManager;
            _configRepository = configRepository;
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;
        public List<TableColumn> TableColumns => _repository.TableColumns;
        public async Task<(bool success, string errorMessage)> ValidateAsync(string userName, string email, string mobile, string password)
        {
            var config = await _configRepository.GetAsync();

            if (string.IsNullOrEmpty(password))
            {
                return (false, "密码不能为空");
            }
            if (password.Length < config.UserPasswordMinLength)
            {
                return (false, $"密码长度必须大于等于{config.UserPasswordMinLength}");
            }
            if (!PasswordRestrictionUtils.IsValid(password, config.UserPasswordRestriction))
            {
                return (false, $"密码不符合规则，请包含{config.UserPasswordRestriction.GetDisplayName()}");
            }
            if (string.IsNullOrEmpty(userName))
            {
                return (false, "用户名为空，请填写用户名");
            }
            if (!string.IsNullOrEmpty(userName) && await IsUserNameExistsAsync(userName))
            {
                return (false, "用户名已被注册，请更换用户名");
            }
            if (!IsUserNameCompliant(userName.Replace("@", string.Empty).Replace(".", string.Empty)))
            {
                return (false, "用户名包含不规则字符，请更换用户名");
            }

            if (!string.IsNullOrEmpty(email) && await IsEmailExistsAsync(email))
            {
                return (false, "电子邮件地址已被注册，请更换邮箱");
            }
            if (!string.IsNullOrEmpty(mobile) && await IsMobileExistsAsync(mobile))
            {
                return (false, "手机号码已被注册，请更换手机号码");
            }

            return (true, string.Empty);
        }
        private async Task<(bool success, string errorMessage)> InsertValidateAsync(string userName, string email, string mobile, string password, string ipAddress)
        {
            var config = await _configRepository.GetAsync();
            if (string.IsNullOrEmpty(password))
            {
                return (false, "密码不能为空");
            }
            if (password.Length < config.UserPasswordMinLength)
            {
                return (false, $"密码长度必须大于等于{config.UserPasswordMinLength}");
            }
            if (!PasswordRestrictionUtils.IsValid(password, config.UserPasswordRestriction))
            {
                return (false, $"密码不符合规则，请包含{config.UserPasswordRestriction.GetDisplayName()}");
            }
            if (string.IsNullOrEmpty(userName))
            {
                return (false, "用户名为空，请填写用户名");
            }
            if (!string.IsNullOrEmpty(userName) && await IsUserNameExistsAsync(userName))
            {
                return (false, "用户名已被注册，请更换用户名");
            }
            if (!IsUserNameCompliant(userName.Replace("@", string.Empty).Replace(".", string.Empty)))
            {
                return (false, "用户名包含不规则字符，请更换用户名");
            }

            if (!string.IsNullOrEmpty(email) && await IsEmailExistsAsync(email))
            {
                return (false, "电子邮件地址已被注册，请更换邮箱");
            }
            if (!string.IsNullOrEmpty(mobile) && await IsMobileExistsAsync(mobile))
            {
                return (false, "手机号码已被注册，请更换手机号码");
            }

            return (true, string.Empty);
        }

        private async Task<(User user, string errorMessage)> UpdateValidateAsync(User user)
        {
            if (user == null || user.Id <= 0)
            {
                return (null, "用户不存在");
            }

            var entity = await GetByUserIdAsync(user.Id);
            user.UserName = entity.UserName;
            user.Password = entity.Password;
            user.PasswordFormat = entity.PasswordFormat;
            user.PasswordSalt = entity.PasswordSalt;

            if (entity.Mobile != user.Mobile && !string.IsNullOrEmpty(user.Mobile) && await IsMobileExistsAsync(user.Mobile))
            {
                return (null, "手机号码已存在");
            }

            if (entity.Email != user.Email && !string.IsNullOrEmpty(user.Email) && await IsEmailExistsAsync(user.Email))
            {
                return (null, "邮箱地址已存在");
            }

            return (entity, string.Empty);
        }

        public async Task<(User user, string errorMessage)> InsertAsync(User user, string password, bool isChecked, string ipAddress)
        {

            if (StringUtils.IsMobile(user.UserName) && string.IsNullOrEmpty(user.Mobile))
            {
                user.Mobile = user.UserName;
            }

            var (success, errorMessage) = await InsertValidateAsync(user.UserName, user.Email, user.Mobile, password, ipAddress);
            if (!success)
            {
                return (null, errorMessage);
            }

            var passwordSalt = GenerateSalt();
            password = EncodePassword(password, PasswordFormat.Encrypted, passwordSalt);
            user.LastActivityDate = DateTime.Now;
            user.LastResetPasswordDate = DateTime.Now;

            user.Id = await InsertWithoutValidationAsync(user, password, PasswordFormat.Encrypted, passwordSalt);

            return (user, string.Empty);
        }

        private async Task<int> InsertWithoutValidationAsync(User user, string password, PasswordFormat passwordFormat, string passwordSalt)
        {
            user.LastActivityDate = DateTime.Now;
            user.LastResetPasswordDate = DateTime.Now;

            user.Password = password;
            user.PasswordFormat = passwordFormat;
            user.PasswordSalt = passwordSalt;
            user.Set("ConfirmPassword", string.Empty);
            user.Id = await _repository.InsertAsync(user);

            return user.Id;
        }

        public async Task<(bool success, string errorMessage)> UpdateAsync(User user)
        {
            var (entity, errorMessage) = await UpdateValidateAsync(user);
            if (entity == null)
            {
                return (false, errorMessage);
            }

            user.Set("ConfirmPassword", string.Empty);
            await _repository.UpdateAsync(user, Q.CachingRemove(GetCacheKeysToRemove(entity)));
            return (true, string.Empty);
        }

        private async Task UpdateLastActivityDateAndCountOfFailedLoginAsync(User user)
        {
            if (user == null) return;

            user.LastActivityDate = DateTime.Now;
            user.CountOfFailedLogin += 1;

            await _repository.UpdateAsync(Q
                .Set(nameof(User.LastActivityDate), user.LastActivityDate)
                .Set(nameof(User.CountOfFailedLogin), user.CountOfFailedLogin)
                .Where(nameof(User.Id), user.Id)
                .CachingRemove(GetCacheKeysToRemove(user))
            );
        }

        public async Task UpdateLastActivityDateAndCountOfLoginAsync(User user)
        {
            if (user == null) return;

            user.LastActivityDate = DateTime.Now;
            user.CountOfLogin += 1;
            user.CountOfFailedLogin = 0;

            await _repository.UpdateAsync(Q
                .Set(nameof(User.LastActivityDate), user.LastActivityDate)
                .Set(nameof(User.CountOfLogin), user.CountOfLogin)
                .Set(nameof(User.CountOfFailedLogin), user.CountOfFailedLogin)
                .Where(nameof(User.Id), user.Id)
                .CachingRemove(GetCacheKeysToRemove(user))
            );
        }

        public async Task UpdateLastActivityDateAsync(User user)
        {
            if (user == null) return;

            user.LastActivityDate = DateTime.Now;

            await _repository.UpdateAsync(Q
                .Set(nameof(User.LastActivityDate), user.LastActivityDate)
                .Where(nameof(User.Id), user.Id)
                .CachingRemove(GetCacheKeysToRemove(user))
            );
        }

        private static string EncodePassword(string password, PasswordFormat passwordFormat, string passwordSalt)
        {
            var retVal = string.Empty;

            if (passwordFormat == PasswordFormat.Clear)
            {
                retVal = password;
            }
            else if (passwordFormat == PasswordFormat.Hashed)
            {
                var src = Encoding.Unicode.GetBytes(password);
                var buffer2 = Convert.FromBase64String(passwordSalt);
                var dst = new byte[buffer2.Length + src.Length];
                byte[] inArray = null;
                Buffer.BlockCopy(buffer2, 0, dst, 0, buffer2.Length);
                Buffer.BlockCopy(src, 0, dst, buffer2.Length, src.Length);
                var algorithm = SHA1.Create(); // HashAlgorithm.Create("SHA1");
                if (algorithm != null) inArray = algorithm.ComputeHash(dst);

                if (inArray != null) retVal = Convert.ToBase64String(inArray);
            }
            else if (passwordFormat == PasswordFormat.Encrypted)
            {
                retVal = TranslateUtils.EncryptStringBySecretKey(password, passwordSalt);

                //var des = new DesEncryptor
                //{
                //    InputString = password,
                //    EncryptKey = passwordSalt
                //};
                //des.DesEncrypt();

                //retVal = des.OutString;
            }
            return retVal;
        }

        private static string DecodePassword(string password, PasswordFormat passwordFormat, string passwordSalt)
        {
            var retVal = string.Empty;
            if (passwordFormat == PasswordFormat.Clear)
            {
                retVal = password;
            }
            else if (passwordFormat == PasswordFormat.Hashed)
            {
                throw new Exception("can not decode hashed password");
            }
            else if (passwordFormat == PasswordFormat.Encrypted)
            {
                retVal = TranslateUtils.DecryptStringBySecretKey(password, passwordSalt);

                //var des = new DesEncryptor
                //{
                //    InputString = password,
                //    DecryptKey = passwordSalt
                //};
                //des.DesDecrypt();

                //retVal = des.OutString;
            }
            return retVal;
        }

        private static string GenerateSalt()
        {
            var data = new byte[0x10];
            // new RNGCryptoServiceProvider().GetBytes(data);
            var rand = RandomNumberGenerator.Create();
            rand.GetBytes(data);

            return Convert.ToBase64String(data);
        }

        public async Task<(bool success, string errorMessage)> ChangePasswordAsync(int userId, string password)
        {
            var config = await _configRepository.GetAsync();
            if (password.Length < config.UserPasswordMinLength)
            {
                return (false, $"密码长度必须大于等于{config.UserPasswordMinLength}");
            }
            if (!PasswordRestrictionUtils.IsValid(password, config.UserPasswordRestriction))
            {
                return (false, $"密码不符合规则，请包含{config.UserPasswordRestriction.GetDisplayName()}");
            }

            var passwordSalt = GenerateSalt();
            password = EncodePassword(password, PasswordFormat.Encrypted, passwordSalt);
            await ChangePasswordAsync(userId, PasswordFormat.Encrypted, passwordSalt, password);
            return (true, string.Empty);
        }

        private async Task ChangePasswordAsync(int userId, PasswordFormat passwordFormat, string passwordSalt, string password)
        {
            var user = await GetByUserIdAsync(userId);
            if (user == null) return;

            user.LastResetPasswordDate = DateTime.Now;

            await _repository.UpdateAsync(Q
                .Set(nameof(User.Password), password)
                .Set(nameof(User.PasswordFormat), passwordFormat.GetValue())
                .Set(nameof(User.PasswordSalt), passwordSalt)
                .Set(nameof(User.LastResetPasswordDate), user.LastResetPasswordDate)
                .Where(nameof(User.Id), user.Id)
                .CachingRemove(GetCacheKeysToRemove(user))
            );
        }

        public async Task LockAsync(IList<int> userIds)
        {
            var cacheKeys = new List<string>();
            foreach (var userId in userIds)
            {
                var user = await GetByUserIdAsync(userId);
                cacheKeys.AddRange(GetCacheKeysToRemove(user));
            }

            await _repository.UpdateAsync(Q
                .Set(nameof(User.Locked), true)
                .WhereIn(nameof(User.Id), userIds)
                .CachingRemove(cacheKeys.ToArray())
            );
        }

        public async Task UnLockAsync(IList<int> userIds)
        {
            var cacheKeys = new List<string>();
            foreach (var userId in userIds)
            {
                var user = await GetByUserIdAsync(userId);
                cacheKeys.AddRange(GetCacheKeysToRemove(user));
            }

            await _repository.UpdateAsync(Q
                .Set(nameof(User.Locked), false)
                .Set(nameof(User.CountOfFailedLogin), 0)
                .WhereIn(nameof(User.Id), userIds)
                .CachingRemove(cacheKeys.ToArray())
            );
        }

        public async Task<bool> IsUserNameExistsAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName)) return false;

            return await _repository.ExistsAsync(Q.Where(nameof(User.UserName), userName));
        }

        private static bool IsUserNameCompliant(string userName)
        {
            if (userName.IndexOf("　", StringComparison.Ordinal) != -1 || userName.IndexOf(" ", StringComparison.Ordinal) != -1 || userName.IndexOf("'", StringComparison.Ordinal) != -1 || userName.IndexOf(":", StringComparison.Ordinal) != -1 || userName.IndexOf(".", StringComparison.Ordinal) != -1)
            {
                return false;
            }
            return DirectoryUtils.IsDirectoryNameCompliant(userName);
        }

        public async Task<bool> IsMobileExistsAsync(string mobile)
        {
            if (string.IsNullOrEmpty(mobile)) return false;

            var exists = await IsUserNameExistsAsync(mobile);
            if (exists) return true;

            return await _repository.ExistsAsync(Q.Where(nameof(User.Mobile), mobile));
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;

            var exists = await IsUserNameExistsAsync(email);
            if (exists) return true;

            return await _repository.ExistsAsync(Q.Where(nameof(User.Email), email));
        }

        public bool CheckPassword(string password, bool isPasswordMd5, string dbPassword, PasswordFormat passwordFormat, string passwordSalt)
        {
            var decodePassword = DecodePassword(dbPassword, passwordFormat, passwordSalt);
            if (isPasswordMd5)
            {
                return password == AuthUtils.Md5ByString(decodePassword);
            }
            return password == decodePassword;
        }

        public async Task<(User user, string userName, string errorMessage)> ValidateAsync(string account, string password, bool isPasswordMd5)
        {
            if (string.IsNullOrEmpty(account))
            {
                return (null, null, "账号不能为空");
            }
            if (string.IsNullOrEmpty(password))
            {
                return (null, null, "密码不能为空");
            }

            var user = await GetByAccountAsync(account);

            if (string.IsNullOrEmpty(user?.UserName))
            {
                return (null, null, "帐号或密码错误");
            }

            var (success, errorMessage) = await ValidateStateAsync(user);
            if (!success)
            {
                return (null, user.UserName, errorMessage);
            }

            var userEntity = await GetByUserIdAsync(user.Id);

            if (!CheckPassword(password, isPasswordMd5, userEntity.Password, userEntity.PasswordFormat, userEntity.PasswordSalt))
            {
                await UpdateLastActivityDateAndCountOfFailedLoginAsync(user);
                return (null, user.UserName, "帐号或密码错误");
            }

            return (user, user.UserName, string.Empty);
        }

        public async Task<(bool success, string errorMessage)> ValidateStateAsync(User user)
        {

            if (user.Locked)
            {
                return (false, "此账号被锁定，无法登录");
            }

            var config = await _configRepository.GetAsync();

            if (config.IsUserLockLogin)
            {
                if (user.CountOfFailedLogin > 0 && user.CountOfFailedLogin >= config.UserLockLoginCount)
                {
                    var lockType = TranslateUtils.ToEnum(config.UserLockLoginType, LockType.Hours);
                    if (lockType == LockType.Forever)
                    {
                        return (false, "此账号错误登录次数过多，已被永久锁定");
                    }
                    if (lockType == LockType.Hours && user.LastActivityDate.HasValue)
                    {
                        var ts = new TimeSpan(DateTime.Now.Ticks - user.LastActivityDate.Value.Ticks);
                        var hours = Convert.ToInt32(config.UserLockLoginHours - ts.TotalHours);
                        if (hours > 0)
                        {
                            return (false, $"此账号错误登录次数过多，已被锁定，请等待{hours}小时后重试");
                        }
                    }
                }
            }

            return (true, null);
        }

        public async Task<int> GetCountAsync(List<int> companyIds, List<int> departmentIds, List<int> dutyIds, List<int> userIds, int range, int dayOfLastActivity, string keyword)
        {
            var query = GetGroupQuery(companyIds, departmentIds, dutyIds, userIds, range, dayOfLastActivity, keyword, string.Empty);
            return await _repository.CountAsync(query);
        }
        public async Task<List<User>> GetUsersAsync(List<int> companyIds, List<int> departmentIds, List<int> dutyIds, List<int> userIds, int range, int dayOfLastActivity, string keyword, string order, int offset, int limit)
        {
            var query = GetGroupQuery(companyIds, departmentIds, dutyIds, userIds, range, dayOfLastActivity, keyword, order);
            query.Offset(offset).Limit(limit);

            return await _repository.GetAllAsync(query);
        }
        public async Task<List<int>> GetUserIdsAsync(List<int> companyIds, List<int> departmentIds, List<int> dutyIds, List<int> userIds, int range, int dayOfLastActivity, string keyword, string order)
        {
            var query = GetGroupQuery(companyIds, departmentIds, dutyIds, userIds, range, dayOfLastActivity, keyword, order);
            return await _repository.GetAllAsync<int>(query.Select(nameof(User.Id)));
        }
        private static Query GetGroupQuery(List<int> companyIds, List<int> departmentIds, List<int> dutyIds, List<int> userIds, int range, int dayOfLastActivity, string keyword, string order)
        {
            var query = Q.NewQuery();
            if (userIds == null) userIds = new List<int>();
            if (companyIds != null && companyIds.Count > 0)
            {
                query.WhereIn(nameof(User.CompanyId), companyIds);
            }
            if (departmentIds != null && departmentIds.Count > 0)
            {
                query.WhereIn(nameof(User.DepartmentId), departmentIds);
            }
            if (dutyIds != null && dutyIds.Count > 0)
            {
                query.WhereIn(nameof(User.DutyId), dutyIds);
            }
            if (range == 0)
            {
                query.WhereNotIn(nameof(User.Id), userIds);
            }
            else
            {
                query.WhereIn(nameof(User.Id), userIds);
            }
            if (dayOfLastActivity > 0)
            {
                var dateTime = DateTime.Now.AddDays(-dayOfLastActivity);
                query.Where(nameof(User.LastActivityDate), ">=", DateUtils.ToString(dateTime));
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                var like = $"%{keyword}%";
                query.Where(q => q
                    .WhereLike(nameof(User.UserName), like)
                    .OrWhereLike(nameof(User.Email), like)
                    .OrWhereLike(nameof(User.Mobile), like)
                    .OrWhereLike(nameof(User.DisplayName), like)
                );
            }

            if (!string.IsNullOrEmpty(order))
            {
                if (StringUtils.EqualsIgnoreCase(order, nameof(User.UserName)))
                {
                    query.OrderBy(nameof(User.UserName));
                }
                else
                {
                    query.OrderByDesc(order);
                }
            }
            else
            {
                query.OrderByDesc(nameof(User.Id));
            }

            return query;
        }
        private static Query GetQuery(List<int> companyIds, List<int> departmentIds, List<int> dutyIds, List<int> userIds, int dayOfLastActivity, string keyword, string order)
        {
            var query = Q.NewQuery();

            if (companyIds != null && companyIds.Count > 0)
            {
                query.WhereIn(nameof(User.CompanyId), companyIds);
            }
            if (departmentIds != null && departmentIds.Count > 0)
            {
                query.WhereIn(nameof(User.DepartmentId), departmentIds);
            }
            if (dutyIds != null && dutyIds.Count > 0)
            {
                query.WhereIn(nameof(User.DutyId), dutyIds);
            }
            if (userIds != null && userIds.Count > 0)
            {
                query.WhereIn(nameof(User.Id), userIds);
            }
            if (dayOfLastActivity > 0)
            {
                var dateTime = DateTime.Now.AddDays(-dayOfLastActivity);
                query.Where(nameof(User.LastActivityDate), ">=", DateUtils.ToString(dateTime));
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                var like = $"%{keyword}%";
                query.Where(q => q
                    .WhereLike(nameof(User.UserName), like)
                    .OrWhereLike(nameof(User.Email), like)
                    .OrWhereLike(nameof(User.Mobile), like)
                    .OrWhereLike(nameof(User.DisplayName), like)
                );
            }

            if (!string.IsNullOrEmpty(order))
            {
                if (StringUtils.EqualsIgnoreCase(order, nameof(User.UserName)))
                {
                    query.OrderBy(nameof(User.UserName));
                }
                else
                {
                    query.OrderByDesc(order);
                }
            }
            else
            {
                query.OrderByDesc(nameof(User.Id));
            }

            return query;
        }

        public async Task<int> GetCountAsync(List<int> companyIds, List<int> departmentIds, List<int> dutyIds, List<int> userIds, int dayOfLastActivity, string keyword)
        {
            var query = GetQuery(companyIds, departmentIds, dutyIds, userIds, dayOfLastActivity, keyword, string.Empty);
            return await _repository.CountAsync(query);
        }

        public async Task<List<User>> GetUsersAsync(List<int> companyIds, List<int> departmentIds, List<int> dutyIds, List<int> userIds, int dayOfLastActivity, string keyword, string order, int offset, int limit)
        {
            var query = GetQuery(companyIds, departmentIds, dutyIds, userIds, dayOfLastActivity, keyword, order);
            query.Offset(offset).Limit(limit);

            return await _repository.GetAllAsync(query);
        }

        public async Task<List<int>> GetUserIdsAsync(List<int> companyIds, List<int> departmentIds, List<int> dutyIds, List<int> userIds, int dayOfLastActivity, string keyword, string order)
        {
            var query = GetQuery(companyIds, departmentIds, dutyIds, userIds, dayOfLastActivity, keyword, order);

            return await _repository.GetAllAsync<int>(query.Select(nameof(User.Id)));
        }
        public async Task<List<int>> GetUserIdsAsync(List<int> companyIds, List<int> departmentIds, List<int> dutyIds)
        {
            var query = Q.Select(nameof(User.Id));


            if (companyIds != null && companyIds.Count > 0)
            {
                query.WhereIn(nameof(User.CompanyId), companyIds);
            }
            if (departmentIds != null && departmentIds.Count > 0)
            {
                query.WhereIn(nameof(User.DepartmentId), departmentIds);
            }
            if (dutyIds != null && dutyIds.Count > 0)
            {
                query.WhereIn(nameof(User.DutyId), dutyIds);
            }

            return await _repository.GetAllAsync<int>(query);
        }
        public async Task<List<int>> GetUserIdsWithOutLockedAsync(List<int> companyIds, List<int> departmentIds, List<int> dutyIds)
        {
            var query = Q.Select(nameof(User.Id));


            if (companyIds != null && companyIds.Count > 0)
            {
                query.WhereIn(nameof(User.CompanyId), companyIds);
            }
            if (departmentIds != null && departmentIds.Count > 0)
            {
                query.WhereIn(nameof(User.DepartmentId), departmentIds);
            }
            if (dutyIds != null && dutyIds.Count > 0)
            {
                query.WhereIn(nameof(User.DutyId), dutyIds);
            }
            query.WhereNullOrFalse(nameof(User.Locked));
            return await _repository.GetAllAsync<int>(query);
        }
        public async Task<List<int>> GetUserIdsWithOutLockedAsync()
        {
            var query = Q.Select(nameof(User.Id));
            query.WhereNullOrFalse(nameof(User.Locked));
            return await _repository.GetAllAsync<int>(query);
        }
        public async Task<int> GetCountAsync(int companyId, int departmentId, int dutyId)
        {
            var query = Q.NewQuery();
            if (companyId > 0)
            {
                query.Where(nameof(User.CompanyId), companyId);
            }
            if (departmentId > 0)
            {
                query.Where(nameof(User.DepartmentId), departmentId);
            }
            if (dutyId > 0)
            {
                query.Where(nameof(User.DutyId), dutyId);
            }

            return await _repository.CountAsync(query);
        }
        public async Task<int> GetCountAsync(List<int> companyIds, List<int> departmentIds, List<int> dutyIds)
        {
            var query = new Query();


            if (companyIds != null && companyIds.Count > 0)
            {
                query.WhereIn(nameof(User.CompanyId), companyIds);
            }
            if (departmentIds != null && departmentIds.Count > 0)
            {
                query.WhereIn(nameof(User.DepartmentId), departmentIds);
            }
            if (dutyIds != null && dutyIds.Count > 0)
            {
                query.WhereIn(nameof(User.DutyId), dutyIds);
            }

            return await _repository.CountAsync(query);
        }

        public async Task<User> DeleteAsync(int userId)
        {
            var user = await GetByUserIdAsync(userId);

            await _repository.DeleteAsync(userId, Q.CachingRemove(GetCacheKeysToRemove(user)));

            return user;
        }
        public async Task<int> GetCountByUserGroupAsync()
        {
            var query = Q.WhereNullOrFalse(nameof(User.Locked));
            return await _repository.CountAsync(query);
        }
        public async Task<int> GetCountByUserGroupAsync(List<int> userIds)
        {
            if (userIds != null)
            {
                var query = Q.WhereNullOrFalse(nameof(User.Locked)).WhereIn(nameof(User.Id), userIds);
                return await _repository.CountAsync(query);
            }
            return 0;
       
        }
        public async Task<int> GetCountByUserGroupAsync(List<int> companyIds, List<int> departmentIds, List<int> dutyIds)
        {
            var query = Q.WhereNullOrFalse(nameof(User.Locked));


            if (companyIds != null && companyIds.Count > 0)
            {
                query.WhereIn(nameof(User.CompanyId), companyIds);
            }
            if (departmentIds != null && departmentIds.Count > 0)
            {
                query.WhereIn(nameof(User.DepartmentId), departmentIds);
            }
            if (dutyIds != null && dutyIds.Count > 0)
            {
                query.WhereIn(nameof(User.DutyId), dutyIds);
            }

            return await _repository.CountAsync(query);
        }
    }
}

