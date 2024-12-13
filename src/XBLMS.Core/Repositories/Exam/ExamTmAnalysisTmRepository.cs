using Datory;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public class ExamTmAnalysisTmRepository : IExamTmAnalysisTmRepository
    {
        private readonly Repository<ExamTmAnalysisTm> _repository;

        public ExamTmAnalysisTmRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamTmAnalysisTm>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<(int total, List<ExamTmAnalysisTm> list)> GetListAsync(string orderType, string keyWords, int analysisId, int pageIndex, int pageSize)
        {
            var query = Q.Where(nameof(ExamTmAnalysisTm.AnalysisId), analysisId);
            if (!string.IsNullOrEmpty(keyWords))
            {
                var like = $"%{keyWords}%";
                query.Where(q => q
                    .WhereLike(nameof(ExamTm.Title), like)
                    .OrWhereLike(nameof(ExamTm.Zhishidian), like)
                    .OrWhereLike(nameof(ExamTm.Jiexi), like)
                    .OrWhereLike(nameof(ExamTm.Answer), like)
                );
            }
            var total = await _repository.CountAsync(query);
            if (orderType == "percent")
            {
                query.OrderByDesc(nameof(ExamTmAnalysisTm.WrongPercent));
            }
            else
            {
                query.OrderByDesc(nameof(ExamTmAnalysisTm.WrongRate));
            }
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }

        public async Task<int> InsertAsync(ExamTmAnalysisTm item)
        {
            return await _repository.InsertAsync(item);
        }

        public async Task DeleteAsync(int analysisId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamTmAnalysisTm.AnalysisId), analysisId));
        }
        public async Task DeleteByTmIdAsync(int tmId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamTmAnalysisTm.TmId), tmId));
        }

        public async Task<List<KeyValuePair<int, int>>> GetChatByTxList(int analysisId)
        {
            var result = new List<KeyValuePair<int, int>>();
            var txIds = await _repository.GetAllAsync<int>(Q.Select(nameof(ExamTmAnalysisTm.TxId)).Where(nameof(ExamTmAnalysisTm.AnalysisId), analysisId).GroupBy(nameof(ExamTmAnalysisTm.TxId)));
            if (txIds != null && txIds.Count > 0)
            {
                foreach (var txId in txIds)
                {
                    var txWrongTotal = await _repository.SumAsync(nameof(ExamTmAnalysisTm.WrongCount), Q.Where(nameof(ExamTmAnalysisTm.TxId), txId).Where(nameof(ExamTmAnalysisTm.AnalysisId), analysisId));
                    result.Add(new KeyValuePair<int, int>(txId, txWrongTotal));
                }
            }
            return result;
        }

        public async Task<List<KeyValuePair<int, int>>> GetChatByNdList(int analysisId)
        {
            var result = new List<KeyValuePair<int, int>>();
            for (var i = 1; i < 6; i++)
            {
                var ndWrongTotal = await _repository.SumAsync(nameof(ExamTmAnalysisTm.WrongCount), Q.Where(nameof(ExamTmAnalysisTm.Nandu), i).Where(nameof(ExamTmAnalysisTm.AnalysisId), analysisId));
                result.Add(new KeyValuePair<int, int>(i, ndWrongTotal));
            }
            return result;
        }


        public async Task<List<KeyValuePair<string, int>>> GetChatByZsdList(int analysisId)
        {
            var result = new List<KeyValuePair<string, int>>();
            var zsds = await _repository.GetAllAsync<string>(Q.Select(nameof(ExamTmAnalysisTm.Zhishidian)).Where(nameof(ExamTmAnalysisTm.AnalysisId), analysisId).GroupBy(nameof(ExamTmAnalysisTm.Zhishidian)));
            if (zsds != null && zsds.Count > 0)
            {
                foreach (var zsd in zsds)
                {
                    if (!string.IsNullOrEmpty(zsd))
                    {
                        var zsdWrongTotal = await _repository.SumAsync(nameof(ExamTmAnalysisTm.WrongCount), Q.Where(nameof(ExamTmAnalysisTm.Zhishidian), zsd).Where(nameof(ExamTmAnalysisTm.AnalysisId), analysisId));
                        if (zsdWrongTotal > 0)
                        {
                            result.Add(new KeyValuePair<string, int>(zsd, zsdWrongTotal));
                        }
                    }
                }
            }
            return result;
        }
    }
}
