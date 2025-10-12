using Datory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public partial class StudyPlanUserRepository : IStudyPlanUserRepository
    {
        private readonly ISettingsManager _settingsManager;
        private readonly Repository<StudyPlanUser> _repository;

        public StudyPlanUserRepository(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            _repository = new Repository<StudyPlanUser>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<bool> ExistsAsync(int planId, int userId)
        {
            return await _repository.ExistsAsync(Q.
                Where(nameof(StudyPlanUser.UserId), userId).
                Where(nameof(StudyPlanUser.PlanId), planId));
        }

        public async Task<int> InsertAsync(StudyPlanUser item)
        {
            return await _repository.InsertAsync(item);
        }

        public async Task<bool> UpdateAsync(StudyPlanUser item)
        {
            return await _repository.UpdateAsync(item);
        }
        public async Task DeleteByUserId(int userId)
        {
            await _repository.DeleteAsync(Q.Where(nameof(StudyPlanUser.UserId), userId));
        }
        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> DeleteByPlanAsync(int planId)
        {
            return await _repository.DeleteAsync(Q.Where(nameof(StudyPlanUser.PlanId), planId)) > 0;
        }

        public async Task<bool> UpdateByPlanAsync(StudyPlan planInfo)
        {
            return await _repository.UpdateAsync(Q.
                Set(nameof(StudyPlanUser.PlanEndDateTime), planInfo.PlanEndDateTime).
                Set(nameof(StudyPlanUser.PlanBeginDateTime), planInfo.PlanBeginDateTime).
                Set(nameof(StudyPlanUser.Locked), planInfo.Locked).
                Set(nameof(StudyPlanUser.Credit), planInfo.PlanCredit).
                Set(nameof(StudyPlanUser.PlanYear), planInfo.PlanYear).
                Set(nameof(StudyPlanUser.KeyWords), planInfo.PlanName).
                Where(nameof(StudyPlanUser.PlanId), planInfo.Id)) > 0;
        }

        public async Task<StudyPlanUser> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }
        public async Task<StudyPlanUser> GetAsync(int planId, int userId)
        {
            return await _repository.GetAsync(Q.
                Where(nameof(StudyPlanUser.UserId), userId).
                Where(nameof(StudyPlanUser.PlanId), planId));
        }
        public async Task<(int total, List<StudyPlanUser> list)> GetTaskListAsync(int userId)
        {
            var query = Q.Where(nameof(StudyPlanUser.UserId), userId).WhereNullOrFalse(nameof(StudyPlanUser.Locked));

            query.WhereNot(nameof(StudyPlanUser.State), StudyStatType.Yiwancheng.GetValue());
            query.WhereNot(nameof(StudyPlanUser.State), StudyStatType.Yidabiao.GetValue());
            query.WhereNot(nameof(StudyPlanUser.State), StudyStatType.Yiguoqi.GetValue());

            query.OrderByDesc(nameof(StudyPlanUser.Id));
            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query);
            return (total, list);
        }
        public async Task<(int total, List<StudyPlanUser> list)> GetListAsync(int year, string state, string keyWords, int userId, int pageIndex, int pageSize)
        {
            var query = Q.Where(nameof(StudyPlanUser.UserId), userId).WhereNullOrFalse(nameof(StudyPlanUser.Locked));

            if (year > 0)
            {
                query.Where(nameof(StudyPlanUser.PlanYear), year);
            }
            if (!string.IsNullOrEmpty(state))
            {
                query.Where(nameof(StudyPlanUser.State), state);
            }
            if (!string.IsNullOrEmpty(keyWords))
            {
                query.WhereLike(nameof(StudyPlanUser.KeyWords), $"%{keyWords}%");
            }

            query.OrderByDesc(nameof(StudyPlanUser.Id));
            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }
        public async Task<(int total, List<StudyPlanUser> list)> GetListAsync(string state, string keyWords, int planId, int pageIndex, int pageSize)
        {
            var query = Q.Where(nameof(StudyPlanUser.PlanId), planId);

            if (!string.IsNullOrEmpty(state))
            {
                if (state == StudyStatType.Yiwancheng.GetValue())
                {
                    query.Where(q =>
                    {
                        q.
                        Where(nameof(StudyPlanUser.State), StudyStatType.Yiwancheng.GetValue()).
                        OrWhere(nameof(StudyPlanUser.State), StudyStatType.Yidabiao.GetValue());
                        return q;
                    });
                }
                else
                {
                    query.Where(nameof(StudyPlanUser.State), state);
                }
            }
            if (!string.IsNullOrEmpty(keyWords))
            {
                query.WhereLike(nameof(StudyPlanUser.KeyWordsAdmin), $"%{keyWords}%");
            }

            query.OrderByDesc(nameof(StudyPlanUser.Id));
            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }
        public async Task<List<int>> GetUserIdsAsync(int planId)
        {
            var query = Q.Select(nameof(StudyPlanUser.UserId)).Where(nameof(StudyPlanUser.PlanId), planId);
            var list = await _repository.GetAllAsync<int>(query);
            return list;
        }

        public async Task<(decimal totalCredit, decimal totalOverCredit)> GetCreditAsync(int userId)
        {

            decimal totalCredit = 0;
            decimal totalOverCredit = 0;

            var query = Q.Where(nameof(StudyPlanUser.UserId), userId).WhereNullOrFalse(nameof(StudyPlanUser.Locked));
            var list = await _repository.GetAllAsync(query);
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    totalCredit += item.Credit;
                    totalOverCredit += item.TotalCredit;
                }
            }
            return (Math.Round(totalCredit, 2), Math.Round(totalOverCredit, 2));
        }


        public async Task<int> GetTaskCountAsync(int userId)
        {
            var query = Q.
                Where(q =>
                {
                    q.
                    Where(nameof(StudyPlanUser.State), StudyStatType.Xuexizhong.GetValue()).
                    OrWhere(nameof(StudyPlanUser.State), StudyStatType.Weikaishi.GetValue());
                    return q;
                }).
                Where(nameof(StudyPlanUser.PlanBeginDateTime), "<", DateTime.Now).
                Where(nameof(StudyPlanUser.PlanEndDateTime), ">", DateTime.Now).
                Where(nameof(StudyPlanUser.UserId), userId).
                WhereNullOrFalse(nameof(StudyPlanUser.Locked));

            return await _repository.CountAsync(query);
        }
        public async Task<(int count, int overCount)> GetCountAsync(int userId)
        {
            var query = Q.
                Where(nameof(StudyPlanUser.UserId), userId).
                WhereNullOrFalse(nameof(StudyPlanUser.Locked));

            var count = await _repository.CountAsync(query);


            var queryOver = Q.
             Where(q =>
             {
                 q.
                 Where(nameof(StudyPlanUser.State), StudyStatType.Yiwancheng.GetValue()).
                 OrWhere(nameof(StudyPlanUser.State), StudyStatType.Yidabiao.GetValue());
                 return q;
             }).
             Where(nameof(StudyPlanUser.UserId), userId).
             WhereNullOrFalse(nameof(StudyPlanUser.Locked));

            var overCount = await _repository.CountAsync(queryOver);

            return (count, overCount);
        }
        public async Task<int> GetCountAsync(int planId, string state)
        {
            var query = Q.Where(nameof(StudyPlanUser.PlanId), planId);


            if (!string.IsNullOrEmpty(state))
            {
                if (state == StudyStatType.Yiwancheng.GetValue())
                {
                    query.Where(q =>
                    {
                        q.
                        Where(nameof(StudyPlanUser.State), StudyStatType.Yiwancheng.GetValue()).
                        OrWhere(nameof(StudyPlanUser.State), StudyStatType.Yidabiao.GetValue());
                        return q;
                    });
                }
                else
                {
                    query.Where(nameof(StudyPlanUser.State), state);
                }
         
            }

            return await _repository.CountAsync(query);
        }
        public async Task<decimal> GetTotalCreditAsync(int planId)
        {
            decimal totalCredit = 0;
            var list= await _repository.GetAllAsync<decimal>(Q.Select(nameof(StudyPlanUser.TotalCredit)).Where(nameof(StudyPlanUser.PlanId), planId));
            if (list != null && list.Count > 0)
            {
                totalCredit = list.Sum(x => x);
            }
            return totalCredit;
        }
    }
}
