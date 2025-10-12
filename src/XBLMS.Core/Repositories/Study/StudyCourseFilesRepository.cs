using Datory;
using SqlKata;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Core.Repositories
{
    public class StudyCourseFilesRepository : IStudyCourseFilesRepository
    {
        private readonly ISettingsManager _settingsManager;
        private readonly Repository<StudyCourseFiles> _repository;

        public StudyCourseFilesRepository(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            _repository = new Repository<StudyCourseFiles>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;
        public async Task<bool> ExistsAsync(int id)
        {
            return await _repository.ExistsAsync(id);
        }
        public async Task<int> InsertAsync(StudyCourseFiles file)
        {
            return await _repository.InsertAsync(file);
        }

        public async Task<bool> UpdateAsync(StudyCourseFiles file)
        {
            return await _repository.UpdateAsync(file);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var file = await GetAsync(id);
            if (file != null && !string.IsNullOrEmpty(file.Url))
            {
                var filePath = PathUtils.Combine(_settingsManager.WebRootPath, file.Url);
                FileUtils.DeleteFileIfExists(filePath);
            }

            return await _repository.DeleteAsync(id);
        }
        public static void GetSeletQuery(Query query, bool isVideo)
        {
            List<string> officeType = ["PDF", "PPT", "XLSX", "DOC", "XLS", "PPTX", "DOCX"];
            if (isVideo)
            {
                query.Where(q =>
                {
                    foreach (var item in officeType)
                    {
                        q.WhereNot(nameof(StudyCourseFiles.FileType), item);
                    }

                    return q;
                });
            }
            else
            {
                query.Where(q =>
                {
                    q.Where(nameof(StudyCourseFiles.FileType), "PDF");
                    foreach (var item in officeType)
                    {
                        q.OrWhere(nameof(StudyCourseFiles.FileType), item);
                    }

                    return q;
                });
            }
        }

        public async Task<List<StudyCourseFiles>> GetAllAsync(AdminAuth auth, string keyWords, string fileType)
        {
            var query = Q.OrderBy(nameof(StudyCourseFiles.Id));

            if (!string.IsNullOrEmpty(keyWords))
            {
                query.WhereLike(nameof(StudyCourseFiles.FileName), $"%{keyWords}%");
            }
            if (!string.IsNullOrEmpty(fileType))
            {
                GetSeletQuery(query, FileUtils.IsPlayer(fileType));
            }

            query = GetQueryByAuth(query, auth);

            return await _repository.GetAllAsync(query);
        }
        public async Task<List<StudyCourseFiles>> GetAllAsync(AdminAuth auth, int groupId, string fileType)
        {
            var query = Q.Where(nameof(StudyCourseFiles.GroupId), groupId).OrderBy(nameof(StudyCourseFiles.Id));
            if (!string.IsNullOrEmpty(fileType))
            {
                GetSeletQuery(query, FileUtils.IsPlayer(fileType));
            }
            query = GetQueryByAuth(query, auth);

            return await _repository.GetAllAsync(query);
        }

        public async Task<List<StudyCourseFiles>> GetAllAsync(AdminAuth auth, int groupId, string keyword, int organId)
        {
            var query = Q.Where(nameof(StudyCourseFiles.GroupId), groupId);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query.WhereLike(nameof(StudyCourseFiles.FileName), $"%{keyword}%");
            }

            query = GetQueryByAuth(query, auth);

            return await _repository.GetAllAsync(query);
        }


        public async Task<List<int>> GetIdsAsync(int groupId, string keyword, int organId)
        {
            var query = Q.Select(nameof(StudyCourseFiles.Id));
            query.Where(nameof(StudyCourseFiles.GroupId), groupId);
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query.WhereLike(nameof(StudyCourseFiles.FileName), $"%{keyword}%");
            }

            return await _repository.GetAllAsync<int>(query);
        }

        public async Task<StudyCourseFiles> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task<bool> IsExistsAsync(string fileName, int companyId, int groupId)
        {
            return await _repository.ExistsAsync(Q
                .Where(nameof(StudyCourseFiles.CompanyId), companyId)
                .Where(nameof(StudyCourseFiles.GroupId), groupId)
                .Where(nameof(StudyCourseFiles.FileName), fileName));
        }



        public async Task<int> CountAsync()
        {
            var query = Q.NewQuery();
            var total = await _repository.CountAsync(query);
            return total;
        }

        public async Task<long> SumFileSizeAsync(List<int> groupIds)
        {
            long result = 0;
            if (groupIds != null && groupIds.Count > 0)
            {
                var list = await _repository.GetAllAsync(Q.WhereIn(nameof(StudyCourseFiles.GroupId), groupIds));
                if (list != null && list.Count > 0)
                {
                    list.ForEach((file) =>
                    {
                        result += file.FileSize;
                    });
                }
            }

            return result;
        }


        public async Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount(AdminAuth auth)
        {
            var total = 0;
            var lockedTotal = 0;
            var unLockedTotal = 0;

            var query = Q.NewQuery();
            query = GetQueryByAuth(query, auth);

            total = await _repository.CountAsync(query);

            return (total, 0, 0, lockedTotal, unLockedTotal);
        }

        private Query GetQueryByAuth(Query query, AdminAuth auth)
        {
            if (auth.AuthDataType == AuthorityDataType.DataCreator)
            {
                query.Where(nameof(Knowledges.CreatorId), auth.AdminId);
            }
            else
            {
                if (auth.AuthDataShowAll)
                {
                    if (auth.CurCompanyId != 1)
                    {
                        query.WhereLike(nameof(Knowledges.CompanyParentPath), $"%'{auth.CurCompanyId}'%");
                    }
                }
                else
                {
                    query.Where(nameof(Knowledges.CompanyId), auth.CurCompanyId);
                }
            }

            return query;
        }

    }
}
