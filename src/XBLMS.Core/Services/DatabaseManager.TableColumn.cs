﻿using Dapper;
using Datory;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public partial class DatabaseManager
    {
        public async Task<List<TableColumn>> GetTableColumnInfoListAsync(string tableName, List<string> excludeAttributeNameList)
        {
            var list = await _settingsManager.Database.GetTableColumnsAsync(tableName);
            if (excludeAttributeNameList == null || excludeAttributeNameList.Count == 0) return list;

            return list.Where(tableColumnInfo =>
                !ListUtils.ContainsIgnoreCase(excludeAttributeNameList, tableColumnInfo.AttributeName)).ToList();
        }

        public async Task<List<TableColumn>> GetTableColumnInfoListAsync(string tableName, DataType excludeDataType)
        {
            var list = await _settingsManager.Database.GetTableColumnsAsync(tableName);

            return list.Where(tableColumnInfo =>
                tableColumnInfo.DataType != excludeDataType).ToList();
        }

        public async Task<TableColumn> GetTableColumnInfoAsync(string tableName, string attributeName)
        {
            var list = await _settingsManager.Database.GetTableColumnsAsync(tableName);
            return list.FirstOrDefault(tableColumnInfo =>
                StringUtils.EqualsIgnoreCase(tableColumnInfo.AttributeName, attributeName));
        }

        public async Task<bool> IsAttributeNameExistsAsync(string tableName, string attributeName)
        {
            var list = await _settingsManager.Database.GetTableColumnsAsync(tableName);
            return list.Any(tableColumnInfo =>
                StringUtils.EqualsIgnoreCase(tableColumnInfo.AttributeName, attributeName));
        }

        public async Task<List<string>> GetTableColumnNameListAsync(string tableName)
        {
            var allTableColumnInfoList = await _settingsManager.Database.GetTableColumnsAsync(tableName);
            return allTableColumnInfoList.Select(tableColumnInfo => tableColumnInfo.AttributeName).ToList();
        }

        public async Task<List<string>> GetTableColumnNameListAsync(string tableName, List<string> excludeAttributeNameList)
        {
            var allTableColumnInfoList = await GetTableColumnInfoListAsync(tableName, excludeAttributeNameList);
            return allTableColumnInfoList.Select(tableColumnInfo => tableColumnInfo.AttributeName).ToList();
        }

        public async Task<List<string>> GetTableColumnNameListAsync(string tableName, DataType excludeDataType)
        {
            var allTableColumnInfoList = await GetTableColumnInfoListAsync(tableName, excludeDataType);
            return allTableColumnInfoList.Select(tableColumnInfo => tableColumnInfo.AttributeName).ToList();
        }

        public async Task<List<string>> GetTableNamesAsync()
        {
            return await _settingsManager.Database.GetTableNamesAsync();
        }
        public async Task<List<dynamic>> QueryAsync(string query)
        {
            using (var connection = _settingsManager.Database.GetConnection())
            {
                return (await connection.QueryAsync(query)).ToList();
            }
        }
        public List<string> GetPropertyKeysForDynamic(dynamic dynamicToGetPropertiesFor)
        {
            var jObject = (JObject)JToken.FromObject(dynamicToGetPropertiesFor);
            var values = jObject.ToObject<Dictionary<string, object>>();
            var toReturn = new List<string>();
            foreach (var key in values.Keys)
            {
                if (StringUtils.EqualsIgnoreCase(key, "id"))
                {
                    toReturn.Add("id");
                }
                else if (StringUtils.EqualsIgnoreCase(key, "guid"))
                {
                    toReturn.Add("guid");
                }
                else
                {
                    toReturn.Add(StringUtils.ToCamelCase(key));
                }
            }

            return toReturn;
        }

        public async Task<int> ExecuteAsync(string query)
        {
            using (var connection = _settingsManager.Database.GetConnection())
            {
                return await connection.ExecuteAsync(query);
            }
        }
    }

}
