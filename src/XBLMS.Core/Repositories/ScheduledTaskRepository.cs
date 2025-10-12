using Datory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public class ScheduledTaskRepository : IScheduledTaskRepository
    {
        private readonly Repository<ScheduledTask> _repository;
        private readonly IConfigRepository _configRepository;

        public ScheduledTaskRepository(ISettingsManager settingsManager, IConfigRepository configRepository)
        {
            _repository = new Repository<ScheduledTask>(settingsManager.Database, settingsManager.Redis);
            _configRepository = configRepository;
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        private static readonly string CacheKey = CacheUtils.GetClassKey(typeof(ScheduledTaskRepository));

        public async Task<ScheduledTask> GetAsync(int id)
        {
            var tasks = await GetAllAsync();
            return tasks.FirstOrDefault(t => t.Id == id);
        }

        public async Task<bool> ExistsPingTask()
        {
            return await _repository.ExistsAsync(Q.Where(nameof(ScheduledTask.TaskType), TaskType.Ping.GetValue()));
        }

        public async Task<List<ScheduledTask>> GetAllAsync()
        {
            return await _repository.GetAllAsync(Q
              .OrderBy(nameof(ScheduledTask.Id))
              .CachingGet(CacheKey)
            );
        }

        public async Task<ScheduledTask> GetNextAsync()
        {
            var tasks = await GetAllAsync();
            var nextTask = tasks.Where(x => !x.IsDisabled && x.ScheduledDate.HasValue && x.ScheduledDate.Value < DateTime.Now).OrderBy(x => x.ScheduledDate).FirstOrDefault();
            return nextTask;
        }

        private DateTime? CalcScheduledDate(ScheduledTask task)
        {
            var now = DateTime.Now;
            if (task.IsDisabled) return null;

            DateTime? date = null;
            if (task.TaskInterval == TaskInterval.Once)
            {
                if (task.StartDate < now) return null;
                date = task.StartDate;
            }
            else if (task.TaskInterval == TaskInterval.EveryMinute)
            {
                date = DateTime.Now.AddMinutes(1);
            }
            else if (task.TaskInterval == TaskInterval.EveryHour)
            {
                if (task.LatestStartDate.HasValue)
                {
                    var ts = new TimeSpan(now.Ticks - task.LatestStartDate.Value.Ticks);
                    if (ts.Hours > task.Every)
                    {
                        date = new DateTime(now.Year, now.Month, now.Day, now.Hour, task.StartDate.Minute, task.StartDate.Second);
                        if (date < now)
                        {
                            date = date.Value.AddHours(1);
                        }
                    }
                    else
                    {
                        date = new DateTime(now.Year, now.Month, now.Day, task.LatestStartDate.Value.Hour, task.StartDate.Minute, task.StartDate.Second);
                        date = date.Value.AddHours(task.Every);
                    }
                }
                else
                {
                    if (task.StartDate < now)
                    {
                        date = new DateTime(now.Year, now.Month, now.Day, now.Hour, task.StartDate.Minute, task.StartDate.Second);
                        if (date < now)
                        {
                            date = date.Value.AddHours(1);
                        }
                    }
                    else
                    {
                        date = task.StartDate;
                    }
                }
            }
            else if (task.TaskInterval == TaskInterval.EveryDay)
            {
                if (task.LatestStartDate.HasValue)
                {
                    var ts = new TimeSpan(now.Ticks - task.LatestStartDate.Value.Ticks);
                    if (ts.Days > task.Every)
                    {
                        date = new DateTime(now.Year, now.Month, now.Day, task.StartDate.Hour, task.StartDate.Minute, task.StartDate.Second);
                        if (date < now)
                        {
                            date = date.Value.AddDays(1);
                        }
                    }
                    else
                    {
                        date = new DateTime(now.Year, now.Month, now.Day, task.StartDate.Hour, task.StartDate.Minute, task.StartDate.Second);
                        date = date.Value.AddDays(task.Every);
                    }
                }
                else
                {
                    if (task.StartDate < now)
                    {
                        date = new DateTime(now.Year, now.Month, now.Day, task.StartDate.Hour, task.StartDate.Minute, task.StartDate.Second);
                        if (date < now)
                        {
                            date = date.Value.AddDays(1);
                        }
                    }
                    else
                    {
                        date = task.StartDate;
                    }
                }
            }
            else if (task.TaskInterval == TaskInterval.EveryWeek)
            {
                var nowWeek = (int)now.DayOfWeek;
                if (task.Weeks != null && task.Weeks.Count > 0)
                {
                    date = new DateTime(now.Year, now.Month, now.Day, task.StartDate.Hour, task.StartDate.Minute, task.StartDate.Second);
                    for (var i = nowWeek; i <= 6 + nowWeek; i++)
                    {
                        var theWeek = i % 7;
                        if (task.Weeks.Contains(theWeek))
                        {
                            date = date.Value.AddDays(i - nowWeek);
                            break;
                        }
                    }
                }
            }
            return date;
        }

        public async Task<int> InsertAsync(ScheduledTask task)
        {
            task.ScheduledDate = CalcScheduledDate(task);
            return await _repository.InsertAsync(task, Q.CachingRemove(CacheKey));
        }

        public async Task UpdateAsync(ScheduledTask task)
        {
            task.ScheduledDate = CalcScheduledDate(task);
            await _repository.UpdateAsync(task, Q.CachingRemove(CacheKey));
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id, Q.CachingRemove(CacheKey));
        }
    }
}
