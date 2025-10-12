using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Core.Utils;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;
using TableColumn = Datory.TableColumn;

namespace XBLMS.Core.Repositories
{
    public partial class TableStyleRepository : ITableStyleRepository
    {
        private readonly Repository<TableStyle> _repository;
        private readonly IUserRepository _userRepository;
        private readonly IExamTmRepository _examTmRepository;

        public TableStyleRepository(ISettingsManager settingsManager, IUserRepository userRepository, IExamTmRepository examTmRepository)
        {
            _repository = new Repository<TableStyle>(settingsManager.Database, settingsManager.Redis);
            _userRepository = userRepository;
            _examTmRepository = examTmRepository;
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        private string GetCacheKey(string tableName)
        {
            return CacheUtils.GetListKey(_repository.TableName, tableName);
        }

        private void Sync(TableStyle style)
        {
            if (style?.Items != null)
            {
                style.ItemValues = TranslateUtils.JsonSerialize(style.Items);
            }

            if (style?.Rules != null)
            {
                style.RuleValues = TranslateUtils.JsonSerialize(style.Rules);
            }
            if (style?.TableName != null)
            {
                if (style.TableName == _examTmRepository.TableName)
                {
                    style.Type = Enums.TableStyleType.Tm;
                }
                if (style.TableName == _userRepository.TableName)
                {
                    style.Type = Enums.TableStyleType.User;
                }
            }
        }

        public async Task<int> InsertAsync(List<int> relatedIdentities, TableStyle style)
        {
            Sync(style);

            if (style.Taxis == 0)
            {
                style.Taxis =
                    await GetMaxTaxisAsync(style.TableName,
                        relatedIdentities) + 1;
            }

            var styleId = await _repository.InsertAsync(style, Q
                .CachingRemove(GetCacheKey(style.TableName))
            );

            return styleId;
        }

        public async Task UpdateAsync(TableStyle style)
        {
            Sync(style);

            await _repository.UpdateAsync(style, Q
                .CachingRemove(GetCacheKey(style.TableName))
            );
        }

        public async Task DeleteAllAsync(string tableName)
        {
            await _repository.DeleteAsync(Q
                .Where(nameof(TableStyle.TableName), tableName)
                .CachingRemove(GetCacheKey(tableName))
            );
        }

        public async Task DeleteAllAsync(string tableName, List<int> relatedIdentities)
        {
            if (relatedIdentities == null || relatedIdentities.Count <= 0) return;

            await _repository.DeleteAsync(Q
                .WhereIn(nameof(TableStyle.RelatedIdentity), relatedIdentities)
                .Where(nameof(TableStyle.TableName), tableName)
                .CachingRemove(GetCacheKey(tableName))
            );
        }

        public async Task DeleteAsync(string tableName, int relatedIdentity, string attributeName)
        {
            await _repository.DeleteAsync(Q
                .Where(nameof(TableStyle.RelatedIdentity), relatedIdentity)
                .Where(nameof(TableStyle.TableName), tableName)
                .Where(nameof(TableStyle.AttributeName), attributeName)
                .CachingRemove(GetCacheKey(tableName))
            );
        }

        private async Task<List<TableStyle>> GetAllAsync(string tableName)
        {
            var styles = await _repository.GetAllAsync(Q
                .Where(nameof(TableStyle.TableName), tableName)
                .OrderByDesc(nameof(TableStyle.Taxis), nameof(TableStyle.Id))
                .CachingGet(GetCacheKey(tableName))
            );

            foreach (var style in styles)
            {
                style.Items = TranslateUtils.JsonDeserialize<List<InputStyleItem>>(style.ItemValues);
                style.Rules = TranslateUtils.JsonDeserialize<List<InputStyleRule>>(style.RuleValues);
            }

            return styles;
        }
    }
}
