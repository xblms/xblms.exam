using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface ITableStyleRepository
    {
        Task<List<TableStyle>> GetTableStylesAsync(string tableName, List<int> relatedIdentities, List<string> excludeAttributeNames = null);
        Task<List<TableStyle>> GetExamTmStylesAsync(bool showLocked);
        Task<List<TableStyle>> GetUserStylesAsync();

        Task<List<TableStyle>> GetUserStylesAsync(bool locked);

        Task<TableStyle> GetTableStyleAsync(string tableName, string attributeName, List<int> relatedIdentities);
        Task<bool> IsExistsAsync(string tableName, string attributeName);
        Task<bool> IsExistsAsync(int relatedIdentity, string tableName, string attributeName);

        Task<Dictionary<string, List<TableStyle>>> GetTableStyleWithItemsDictionaryAsync(string tableName,
            List<int> allRelatedIdentities);

        List<int> GetRelatedIdentities(int relatedIdentity);


        List<int> EmptyRelatedIdentities { get; }
    }
}
