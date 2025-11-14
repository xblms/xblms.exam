using Datory;
using SqlKata;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public class ExamTmCorrectionRepository : IExamTmCorrectionRepository
    {
        private readonly Repository<ExamTmCorrection> _repository;

        public ExamTmCorrectionRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamTmCorrection>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<ExamTmCorrection> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }
        public async Task<int> InsertAsync(ExamTmCorrection info)
        {
            return await _repository.InsertAsync(info);
        }
        public async Task<bool> UpdateAsync(ExamTmCorrection info)
        {
            return await _repository.UpdateAsync(info);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
        public async Task<(int total, List<ExamTmCorrection> list)> GetListAsync(int userId, string keyWords, string dateFrom, string dateTo, int pageIndex, int pageSize)
        {
            var query = Q
                .Where(nameof(ExamTmCorrection.UserId), userId);

            if (!string.IsNullOrWhiteSpace(dateFrom))
            {
                query.Where(nameof(ExamTmCorrection.CreatedDate), ">=", dateFrom);
            }
            if (!string.IsNullOrWhiteSpace(dateTo))
            {
                query.Where(nameof(ExamTmCorrection.CreatedDate), "<=", dateTo);
            }


            if (!string.IsNullOrWhiteSpace(keyWords))
            {
                keyWords = $"%{keyWords}%";
                query.Where(w =>
                {
                    w
                    .WhereLike(nameof(ExamTmCorrection.Reason), keyWords)
                    .OrWhereLike(nameof(ExamTmCorrection.TmTitle), keyWords);
                    return w;
                });
            }
            var total = await _repository.CountAsync(query);
            query.OrderByDesc(nameof(ExamTmCorrection.Id));
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }
        public async Task<(int total, List<ExamTmCorrection> list)> GetListAsync(int userId, int tmId)
        {
            var query = Q
                .Where(nameof(ExamTmCorrection.UserId), userId)
                .Where(nameof(ExamTmCorrection.TmId), tmId);
            var total = await _repository.CountAsync(query);
            query.OrderByDesc(nameof(ExamTmCorrection.Id));
            var list = await _repository.GetAllAsync(query);
            return (total, list);
        }

        public async Task<(int total, List<ExamTmCorrection> list)> GetListAsync(AdminAuth auth, string status, string keyWords, int pageIndex, int pageSize)
        {
            var query = Q.NewQuery();

            query = GetQueryByAuth(query, auth);

            if (!string.IsNullOrEmpty(status))
            {
                query.Where(nameof(ExamTmCorrection.AuditStatus), status);
            }
            if (!string.IsNullOrWhiteSpace(keyWords))
            {
                keyWords = $"%{keyWords}%";
                query.Where(w =>
                {
                    w.WhereLike(nameof(ExamTmCorrection.Reason), keyWords)
                    .OrWhereLike(nameof(ExamTmCorrection.TmTitle), keyWords);
                    return w;
                });
            }

            var total = await _repository.CountAsync(query);
            query.OrderByDesc(nameof(ExamTmCorrection.Id));
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }

        public async Task<int> CountAsync()
        {
            return await _repository.CountAsync();
        }

        private Query GetQueryByAuth(Query query, AdminAuth auth)
        {
            if (auth.AuthDataType == AuthorityDataType.DataCreator)
            {
                query.Where(nameof(ExamAssessment.CreatorId), auth.AdminId);
            }
            else
            {
                if (auth.AuthDataShowAll)
                {
                    if (auth.CurCompanyId != 1)
                    {
                        query.WhereLike(nameof(ExamAssessment.CompanyParentPath), $"%'{auth.CurCompanyId}'%");
                    }
                }
                else
                {
                    query.Where(nameof(ExamAssessment.CompanyId), auth.CurCompanyId);
                }
            }

            return query;
        }
    }
}
