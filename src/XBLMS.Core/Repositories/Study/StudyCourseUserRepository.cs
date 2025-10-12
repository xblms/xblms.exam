using Datory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Core.Repositories
{
    public partial class StudyCourseUserRepository : IStudyCourseUserRepository
    {
        private readonly ISettingsManager _settingsManager;
        private readonly Repository<StudyCourseUser> _repository;

        public StudyCourseUserRepository(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            _repository = new Repository<StudyCourseUser>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<int> InsertAsync(StudyCourseUser item)
        {
            return await _repository.InsertAsync(item);
        }

        public async Task<bool> UpdateAsync(StudyCourseUser item)
        {
            return await _repository.UpdateAsync(item);
        }
        public async Task<bool> UpdateByCourseAsync(StudyCourse courseInfo)
        {
            return await _repository.UpdateAsync(Q.
                Set(nameof(StudyCourseUser.Locked), courseInfo.Locked).
                Set(nameof(StudyCourseUser.Credit), courseInfo.Credit).
                Set(nameof(StudyCourseUser.Mark), courseInfo.Mark).
                Set(nameof(StudyCourseUser.KeyWords), courseInfo.Name).
                Where(nameof(StudyCourseUser.CourseId), courseInfo.Id)) > 0;
        }

        public async Task<bool> DeleteByCourseAsync(int courseId)
        {
            return await _repository.DeleteAsync(Q.Where(nameof(StudyCourseUser.CourseId), courseId)) > 0;
        }
        public async Task DeleteByUserId(int userId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(StudyCourseUser.UserId), userId));
        }
        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<StudyCourseUser> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }
        public async Task<bool> ExistsAsync(int userId, int planId, int courseId)
        {
            return await _repository.ExistsAsync(Q.
                Where(nameof(StudyCourseUser.UserId), userId).
                Where(nameof(StudyCourseUser.PlanId), planId).
                Where(nameof(StudyCourseUser.CourseId), courseId));
        }
        public async Task<StudyCourseUser> GetAsync(int userId, int planId, int courseId)
        {
            return await _repository.GetAsync(Q.
                Where(nameof(StudyCourseUser.UserId), userId).
                Where(nameof(StudyCourseUser.PlanId), planId).
                Where(nameof(StudyCourseUser.CourseId), courseId));
        }
        public async Task<(int total, List<StudyCourseUser> list)> GetLastListAsync(int userId, string state, string keyWords, int pageIndex, int pageSize)
        {
            var query = Q.
                Where(nameof(StudyCourseUser.UserId), userId).
                WhereNullOrFalse(nameof(StudyCourseUser.Locked));
            if (!string.IsNullOrEmpty(keyWords))
            {
                query.WhereLike(nameof(StudyCourseUser.KeyWords), $"%{keyWords}%");
            }
            if (!string.IsNullOrEmpty(state))
            {
                if (state == "stateOver")
                {
                    query.Where(nameof(StudyCourseUser.State), StudyStatType.Yiwancheng.GetValue());
                }
                if (state == "stateStuding")
                {
                    query.Where(nameof(StudyCourseUser.State), StudyStatType.Xuexizhong.GetValue());
                }
            }

            query.OrderByDesc(nameof(StudyCourseUser.LastStudyDateTime));

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }
        public async Task<(int total, List<StudyCourseUser> list)> GetListAsync(int userId, bool collection, string keyWords, string mark, string orderby, string state, int pageIndex, int pageSize)
        {
            var query = Q.
                Where(nameof(StudyCourseUser.UserId), userId).
                WhereNullOrFalse(nameof(StudyCourseUser.Locked));
            if (collection)
            {
                query.WhereTrue(nameof(StudyCourseUser.Collection));
            }
            if (!string.IsNullOrEmpty(keyWords))
            {
                query.WhereLike(nameof(StudyCourseUser.KeyWords), $"%{keyWords}%");
            }
            if (!string.IsNullOrEmpty(mark))
            {
                query.WhereLike(nameof(StudyCourseUser.Mark), $"%{mark}%");
            }
            if (!string.IsNullOrEmpty(state))
            {
                if (state == "stateOver")
                {
                    query.Where(nameof(StudyCourseUser.State), StudyStatType.Yiwancheng.GetValue());
                }
                if (state == "stateStuding")
                {
                    query.Where(nameof(StudyCourseUser.State), StudyStatType.Xuexizhong.GetValue());
                }
            }
            if (!string.IsNullOrEmpty(orderby))
            {
                if (orderby == "orderbyEvaluation")
                {
                    query.OrderByDesc(nameof(StudyCourseUser.TotalEvaluation));
                }
                if (orderby == "orderbyCount")
                {
                    query.OrderByDesc(nameof(StudyCourseUser.TotalEvaluation));
                }
            }
            else
            {
                query.OrderByDesc(nameof(StudyCourseUser.Id));
            }

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }
        public async Task<(int total, List<StudyCourseUser> list)> GetListAsync(int planId, int courseId, string keyWords, string state, int pageIndex, int pageSize)
        {
            var query = Q.
                Where(nameof(StudyCourseUser.PlanId), planId).
                Where(nameof(StudyCourseUser.CourseId), courseId);

            if (!string.IsNullOrEmpty(keyWords))
            {
                query.WhereLike(nameof(StudyCourseUser.KeyWordsAdmin), $"%{keyWords}%");
            }

            if (!string.IsNullOrEmpty(state))
            {
                query.Where(nameof(StudyCourseUser.State), state);
            }

            query.OrderByDesc(nameof(StudyCourseUser.Id));

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }
        public async Task<(int total, List<StudyCourseUser> list)> GetOfflineListByEventAsync(int userId)
        {
            var query = Q.
                Where(nameof(StudyCourseUser.PlanId), ">", 0).
                WhereTrue(nameof(StudyCourseUser.OffLine)).
                WhereNot(nameof(StudyCourseUser.State), StudyStatType.Yiwancheng.GetValue()).
                WhereNot(nameof(StudyCourseUser.State), StudyStatType.Yiguoqi.GetValue()).
                Where(nameof(StudyCourseUser.UserId), userId);

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query);
            return (total, list);
        }
        public async Task<(int total, List<string>)> GetMarkListAsync(int userId)
        {
            var query = Q.
                Select(nameof(StudyCourseUser.Mark)).
                WhereNullOrFalse(nameof(StudyCourseUser.Locked));
            var list = new List<string>();
            var total = 0;
            var markList = await _repository.GetAllAsync<string>(query);
            if (markList != null && markList.Count > 0)
            {
                foreach (var marks in markList)
                {
                    var listMark = ListUtils.ToList(marks);
                    foreach (var mark in listMark)
                    {
                        if (!markList.Contains(mark))
                        {
                            total++;
                            list.Add(mark);
                        }
                    }

                }
                list = list.OrderBy(x => x).ToList();
            }
            return (total, list);

        }
        public async Task<int> GetAvgEvaluationAsync(int courseId, int minStar)
        {
            return await _repository.CountAsync(Q.
                Where(nameof(StudyCourseUser.AvgEvaluation), ">=", minStar).
                Where(nameof(StudyCourseUser.CourseId), courseId));
        }



        public async Task<List<StudyCourseUser>> GetListAsync(int planId, int userId)
        {
            var query = Q.
                Where(nameof(StudyCourseUser.UserId), userId).
                Where(nameof(StudyCourseUser.PlanId), planId);

            var list = await _repository.GetAllAsync(query);
            return list;
        }

        public async Task<(int total, int overTotal)> GetTotalAsync(int userId)
        {
            var query = Q.Where(nameof(StudyCourseUser.UserId), userId).WhereNullOrFalse(nameof(StudyCourseUser.Locked));

            var total = await _repository.CountAsync(query);
            var overTotal = await _repository.CountAsync(query.Where(nameof(StudyCourseUser.State), StudyStatType.Yiwancheng.GetValue()));

            return (total, overTotal);
        }
        public async Task<int> GetTotalDurationAsync(int userId)
        {
            var query = Q.Where(nameof(StudyCourseUser.UserId), userId).WhereNullOrFalse(nameof(StudyCourseUser.Locked));

            var total = await _repository.SumAsync(nameof(StudyCourseUser.TotalDuration), query);
            return total;
        }
        public async Task<int> GetTaskCountAsync(int userId)
        {
            var query = Q.
                Where(nameof(StudyCourseUser.State), StudyStatType.Xuexizhong.GetValue()).
                Where(nameof(StudyCourseUser.UserId), userId).
                WhereNullOrFalse(nameof(StudyCourseUser.Locked));

            return await _repository.CountAsync(query);
        }
        public async Task<int> GetOverCountAsync(int planId, int userId, bool isSelect)
        {
            var query = Q.Where(nameof(StudyCourseUser.PlanId), planId).Where(nameof(StudyCourseUser.UserId), userId).Where(nameof(StudyCourseUser.State), StudyStatType.Yiwancheng.GetValue());

            if (isSelect)
            {
                query.WhereTrue(nameof(StudyCourseUser.IsSelectCourse));
            }
            else
            {
                query.WhereNullOrFalse(nameof(StudyCourseUser.IsSelectCourse));
            }

            return await _repository.CountAsync(query);
        }
        public async Task<int> GetOverCountAsync(int planId, bool isSelect)
        {
            var query = Q.Where(nameof(StudyCourseUser.PlanId), planId).Where(nameof(StudyCourseUser.State), StudyStatType.Yiwancheng.GetValue());

            if (isSelect)
            {
                query.WhereTrue(nameof(StudyCourseUser.IsSelectCourse));
            }
            else
            {
                query.WhereNullOrFalse(nameof(StudyCourseUser.IsSelectCourse));
            }

            return await _repository.CountAsync(query);
        }
        public async Task<int> GetOverCountByAnalysisAsync(int planId, int courseId, bool? isOver)
        {
            var query = Q.Where(nameof(StudyCourseUser.PlanId), planId).Where(nameof(StudyCourseUser.CourseId), courseId);
            if (isOver.HasValue)
            {
                if (isOver.Value)
                {
                    query.Where(nameof(StudyCourseUser.State), StudyStatType.Yiwancheng.GetValue());
                }
                else
                {
                    query.WhereNot(nameof(StudyCourseUser.State), StudyStatType.Yiwancheng.GetValue());
                }
            }
            return await _repository.CountAsync(query);
        }
        public async Task<decimal> GetOverTotalCreditAsync(int planId, bool isSelect)
        {
            decimal totalCredit = 0;
            var query = Q.Where(nameof(StudyCourseUser.PlanId), planId).Where(nameof(StudyCourseUser.State), StudyStatType.Yiwancheng.GetValue());

            if (isSelect)
            {
                query.WhereTrue(nameof(StudyCourseUser.IsSelectCourse));
            }
            else
            {
                query.WhereNullOrFalse(nameof(StudyCourseUser.IsSelectCourse));
            }
            var list = await _repository.GetAllAsync<decimal>(query.Select(nameof(StudyCourseUser.Credit)));
            if (list != null && list.Count > 0)
            {
                totalCredit = list.Sum(x => x);
            }
            return totalCredit;
        }

        public async Task<(int totalUser, int overTotalUser)> GetOverCountByAnalysisAsync(int planId, int courseId)
        {
            var query = Q.Where(nameof(StudyCourseUser.PlanId), planId).Where(nameof(StudyCourseUser.CourseId), courseId);

            var count = await _repository.CountAsync(query);
            var total = await _repository.CountAsync(query.Where(nameof(StudyCourseUser.State), StudyStatType.Yiwancheng.GetValue()));
            return (count, total);
        }
        public async Task<decimal> GetOverTotalCreditAsync(int planId, int userId, bool isSelect)
        {
            decimal totalCredit = 0;
            var query = Q.
                Where(nameof(StudyCourseUser.UserId), userId).
                Where(nameof(StudyCourseUser.PlanId), planId).
                Where(nameof(StudyCourseUser.State), StudyStatType.Yiwancheng.GetValue());

            if (isSelect)
            {
                query.WhereTrue(nameof(StudyCourseUser.IsSelectCourse));
            }
            else
            {
                query.WhereNullOrFalse(nameof(StudyCourseUser.IsSelectCourse));
            }
            var list = await _repository.GetAllAsync<decimal>(query.Select(nameof(StudyCourseUser.Credit)));
            if (list != null && list.Count > 0)
            {
                totalCredit = list.Sum(x => x);
            }
            return totalCredit;
        }

        public async Task<(int starUser, int starTotal)> GetEvaluation(int planId, int courseId)
        {
            var query = Q.
                  Where(nameof(StudyCourseUser.TotalEvaluation), ">", 0).
                  Where(nameof(StudyCourseUser.CourseId), courseId).
                  Where(nameof(StudyCourseUser.PlanId), planId);
            var starUser = await _repository.CountAsync(query);
            var starTotal = await _repository.SumAsync(nameof(StudyCourseUser.AvgEvaluation), query);
            return (starUser, starTotal);
        }
    }
}
