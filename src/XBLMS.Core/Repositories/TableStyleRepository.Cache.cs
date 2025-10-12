using Datory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Core.Repositories
{
    public partial class TableStyleRepository
    {
        public async Task<List<TableStyle>> GetTableStylesAsync(string tableName, List<int> relatedIdentities, List<string> excludeAttributeNames = null)
        {
            var allAttributeNames = new List<string>();
            var styleList = new List<TableStyle>();

            var styles = await GetAllAsync(tableName);
            relatedIdentities = ParseRelatedIdentities(relatedIdentities);
            foreach (var relatedIdentity in relatedIdentities)
            {
                var list = styles.Where(style =>
                    style.RelatedIdentity == relatedIdentity && style.TableName == tableName);

                foreach (var style in list)
                {
                    if (!allAttributeNames.Contains(style.AttributeName))
                    {
                        allAttributeNames.Add(style.AttributeName);
                        styleList.Add(style);
                    }
                }
            }

            return styleList.OrderBy(style => style.Taxis == 0 ? int.MaxValue : style.Taxis).ToList();
        }

        public async Task<List<TableStyle>> GetExamTmStylesAsync(bool showLocked)
        {
            var relatedIdentities = EmptyRelatedIdentities;
            var tableName = _examTmRepository.TableName;
            var styles = await GetTableStylesAsync(tableName, relatedIdentities);
            if (showLocked)
            {
                return styles;
            }
            else
            {
                return styles.Where(x => x.Locked == false).ToList();
            }

        }

        public async Task<List<TableStyle>> GetUserStylesAsync()
        {
            var relatedIdentities = EmptyRelatedIdentities;
            var userTableName = _userRepository.TableName;
            var styles = await GetTableStylesAsync(userTableName, relatedIdentities);
            return styles;
        }
        public async Task<List<TableStyle>> GetUserStylesAsync(bool locked)
        {
            var relatedIdentities = EmptyRelatedIdentities;
            var userTableName = _userRepository.TableName;
            var styles = await GetTableStylesAsync(userTableName, relatedIdentities);
            return styles;
        }
        //relatedIdentities从大到小，最后是0
        public async Task<TableStyle> GetTableStyleAsync(string tableName, string attributeName, List<int> relatedIdentities)
        {
            if (attributeName == null) attributeName = string.Empty;

            relatedIdentities = ParseRelatedIdentities(relatedIdentities);
            var styles = await GetAllAsync(tableName);
            foreach (var relatedIdentity in relatedIdentities)
            {
                var style = styles.FirstOrDefault(x => x.RelatedIdentity == relatedIdentity && x.TableName == tableName && x.AttributeName == attributeName);
                if (style == null) continue;

                return style;
            }

            return new TableStyle
            {
                Id = 0,
                RelatedIdentity = 0,
                TableName = tableName,
                AttributeName = attributeName,
                Taxis = 0,
                DisplayName = string.Empty,
                HelpText = string.Empty,
                List = false,
                InputType = InputType.Text,
                DefaultValue = string.Empty,
                Horizontal = true,
                Locked = false
            };
        }
        public async Task<bool> IsExistsAsync(string tableName, string attributeName)
        {
            var styles = await GetAllAsync(tableName);
            return styles.Any(x => x.AttributeName == attributeName);
        }
        public async Task<bool> IsExistsAsync(int relatedIdentity, string tableName, string attributeName)
        {
            var styles = await GetAllAsync(tableName);
            return styles.Any(x => x.RelatedIdentity == relatedIdentity && x.AttributeName == attributeName);
        }

        public async Task<Dictionary<string, List<TableStyle>>> GetTableStyleWithItemsDictionaryAsync(string tableName, List<int> allRelatedIdentities)
        {
            var dict = new Dictionary<string, List<TableStyle>>();

            var styles = await GetAllAsync(tableName);
            foreach (var style in styles)
            {
                if (!StringUtils.EqualsIgnoreCase(style.TableName, tableName) ||
                    !allRelatedIdentities.Contains(style.RelatedIdentity)) continue;

                var tableStyleWithItemList = dict.ContainsKey(style.AttributeName) ? dict[style.AttributeName] : new List<TableStyle>();
                tableStyleWithItemList.Add(style);
                dict[style.AttributeName] = tableStyleWithItemList;
            }

            return dict;
        }

        public List<int> GetRelatedIdentities(int relatedIdentity)
        {
            return relatedIdentity == 0 ? EmptyRelatedIdentities : new List<int> { relatedIdentity, 0 };
        }

        public List<int> EmptyRelatedIdentities => new List<int> { 0 };

        private async Task<int> GetMaxTaxisAsync(string tableName, List<int> relatedIdentities)
        {
            var list = await GetTableStylesAsync(tableName, relatedIdentities);
            if (list != null && list.Count > 0)
            {
                return list.Max(x => x.Taxis);
            }

            return 0;
        }

        private List<int> ParseRelatedIdentities(IReadOnlyCollection<int> list)
        {
            var relatedIdentities = new List<int>();

            if (list != null && list.Count > 0)
            {
                foreach (var i in list)
                {
                    if (!relatedIdentities.Contains(i))
                    {
                        relatedIdentities.Add(i);
                    }
                }
            }

            relatedIdentities.Sort();
            relatedIdentities.Reverse();

            if (!relatedIdentities.Contains(0))
            {
                relatedIdentities.Add(0);
            }

            return relatedIdentities;
        }

        private static TableStyle GetDefaultUserTableStyle(string tableName, string attributeName)
        {
            var style = new TableStyle
            {
                Id = 0,
                RelatedIdentity = 0,
                TableName = tableName,
                AttributeName = attributeName,
                Taxis = 0,
                DisplayName = string.Empty,
                HelpText = string.Empty,
                List = false,
                InputType = InputType.Text,
                DefaultValue = string.Empty,
                Horizontal = true
            };

            if (StringUtils.EqualsIgnoreCase(attributeName, nameof(User.DisplayName)))
            {
                style.AttributeName = nameof(User.DisplayName);
                style.DisplayName = "姓名";
                style.RuleValues = TranslateUtils.JsonSerialize(new List<InputStyleRule>
                {
                    new InputStyleRule
                    {
                        Type = ValidateType.Required,
                        Message = ValidateType.Required.GetDisplayName()
                    }
                });
                style.Taxis = 1;
            }
            else if (StringUtils.EqualsIgnoreCase(attributeName, nameof(User.Mobile)))
            {
                style.AttributeName = nameof(User.Mobile);
                style.DisplayName = "手机号";
                style.HelpText = "可用于登录、找回密码等功能";
                style.InputType = InputType.Number;
                style.RuleValues = TranslateUtils.JsonSerialize(new List<InputStyleRule>
                {
                    new InputStyleRule
                    {
                        Type = ValidateType.Mobile,
                        Message = ValidateType.Mobile.GetDisplayName()
                    }
                });
                style.Taxis = 2;
            }
            else if (StringUtils.EqualsIgnoreCase(attributeName, nameof(User.Email)))
            {
                style.AttributeName = nameof(User.Email);
                style.DisplayName = "邮箱";
                style.HelpText = "可用于登录、找回密码等功能";
                style.RuleValues = TranslateUtils.JsonSerialize(new List<InputStyleRule>
                {
                    new InputStyleRule
                    {
                        Type = ValidateType.Email,
                        Message = ValidateType.Email.GetDisplayName()
                    }
                });
                style.Taxis = 3;
            }

            style.Items = TranslateUtils.JsonDeserialize<List<InputStyleItem>>(style.ItemValues);
            style.Rules = TranslateUtils.JsonDeserialize<List<InputStyleRule>>(style.RuleValues);

            return style;
        }
    }
}
