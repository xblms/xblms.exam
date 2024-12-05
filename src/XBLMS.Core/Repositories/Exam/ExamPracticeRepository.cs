using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Core.Repositories
{
    public class ExamPracticeRepository : IExamPracticeRepository
    {
        private readonly Repository<ExamPractice> _repository;

        public ExamPracticeRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamPractice>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;
        public async Task<ExamPractice> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task<(int total, List<ExamPractice> list)> GetListAsync(int userId, string dateFrom, string dateTo, int pageIndex, int pageSize)
        {
            var query = Q.Where(nameof(ExamPractice.UserId), userId);

            if (!string.IsNullOrWhiteSpace(dateFrom))
            {
                query.Where(nameof(ExamPractice.CreatedDate), ">=", TranslateUtils.ToDateTime(dateFrom));
            }

            if (!string.IsNullOrWhiteSpace(dateTo))
            {
                query.Where(nameof(ExamPractice.CreatedDate), "<=", TranslateUtils.ToDateTime(dateTo));
            }

            var count = await _repository.CountAsync(query);

            var list = await _repository.GetAllAsync(query.OrderByDesc(nameof(ExamCer.Id)).ForPage(pageIndex, pageSize));
            return (count, list);
        }

        public async Task<int> InsertAsync(ExamPractice item)
        {
            return await _repository.InsertAsync(item);
        }

        public async Task IncrementAnswerCountAsync(int id)
        {
            await _repository.IncrementAsync(nameof(ExamPractice.AnswerCount), Q.Where(nameof(ExamPractice.Id), id));
        }
        public async Task IncrementRightCountAsync(int id)
        {
            await _repository.IncrementAsync(nameof(ExamPractice.RightCount), Q.Where(nameof(ExamPractice.Id), id));
        }

        public async Task DeleteAsync(int userId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(ExamPractice.UserId), userId));
        }


        public async Task<(int answerTotal, int rightTotal, int allAnswerTotal, int allRightTotal, int collectAnswerTotal, int collectRightTotal, int wrongAnswerTotal, int wrongRightTotal)> SumAsync(int userId)
        {
            var answerTotal = await _repository.SumAsync(nameof(ExamPractice.AnswerCount), Q.Where(nameof(ExamPractice.UserId), userId));
            var rightTtoal = await _repository.SumAsync(nameof(ExamPractice.RightCount), Q.Where(nameof(ExamPractice.UserId), userId));


            var allAnswerTotal = await _repository.SumAsync(nameof(ExamPractice.AnswerCount), Q.Where(nameof(ExamPractice.PracticeType), PracticeType.All.GetValue()).Where(nameof(ExamPractice.UserId), userId));
            var allRightTotal = await _repository.SumAsync(nameof(ExamPractice.RightCount), Q.Where(nameof(ExamPractice.PracticeType), PracticeType.All.GetValue()).Where(nameof(ExamPractice.UserId), userId));

            var collectAnswerTotal = await _repository.SumAsync(nameof(ExamPractice.AnswerCount), Q.Where(nameof(ExamPractice.PracticeType), PracticeType.Collect.GetValue()).Where(nameof(ExamPractice.UserId), userId));
            var collectRightTotal = await _repository.SumAsync(nameof(ExamPractice.RightCount), Q.Where(nameof(ExamPractice.PracticeType), PracticeType.Collect.GetValue()).Where(nameof(ExamPractice.UserId), userId));

            var wrongAnswerTotal = await _repository.SumAsync(nameof(ExamPractice.AnswerCount), Q.Where(nameof(ExamPractice.PracticeType), PracticeType.Wrong.GetValue()).Where(nameof(ExamPractice.UserId), userId));
            var wrongRightTotal = await _repository.SumAsync(nameof(ExamPractice.RightCount), Q.Where(nameof(ExamPractice.PracticeType), PracticeType.Wrong.GetValue()).Where(nameof(ExamPractice.UserId), userId));

            return (answerTotal, rightTtoal, allAnswerTotal, allRightTotal, collectAnswerTotal, collectRightTotal, wrongAnswerTotal, wrongRightTotal);
        }
    }
}
