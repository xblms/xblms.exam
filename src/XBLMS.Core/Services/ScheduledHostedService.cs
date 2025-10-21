using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Services;

namespace XBLMS.Core.Services
{
    public partial class ScheduledHostedService : BackgroundService
    {
        private readonly ILogger<ScheduledHostedService> _logger;
        private static readonly TimeSpan FREQUENCY = TimeSpan.FromSeconds(5);
        private readonly ISettingsManager _settingsManager;
        private readonly IDatabaseManager _databaseManager;

        public ScheduledHostedService(ILogger<ScheduledHostedService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            serviceProvider = serviceProvider.CreateScope().ServiceProvider;
            _settingsManager = serviceProvider.GetRequiredService<ISettingsManager>();
            _databaseManager = serviceProvider.GetRequiredService<IDatabaseManager>();
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(FREQUENCY, stoppingToken);

                if (string.IsNullOrEmpty(_settingsManager.DatabaseConnectionString)) continue;

                ScheduledTask task = null;
                try
                {
                    task = await _databaseManager.ScheduledTaskRepository.GetNextAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }

                if (task == null) continue;
                
                await ExecuteTaskAsync(task, stoppingToken);
            }
        }

        public async Task ExecuteTaskAsync(ScheduledTask task, CancellationToken stoppingToken)
        {
            try
            {
                task.IsRunning = true;
                task.LatestStartDate = DateTime.Now;
                await _databaseManager.ScheduledTaskRepository.UpdateAsync(task);

                var running = RunTaskAsync(task);
                var cancelTask = Task.Delay(TimeSpan.FromMinutes(task.Timeout), stoppingToken);

                //double await so exceptions from either task will bubble up
                await await Task.WhenAny(running, cancelTask);

                task.IsRunning = false;
                task.LatestEndDate = DateTime.Now;

                if (running.IsCompletedSuccessfully)
                {
                    task.IsLatestSuccess = true;
                    task.LatestFailureCount = 0;
                    task.LatestErrorMessage = string.Empty;
                    await _databaseManager.ScheduledTaskRepository.UpdateAsync(task);
                }
                else
                {
                    task.IsLatestSuccess = false;
                    task.LatestEndDate = DateTime.Now;
                    task.LatestFailureCount++;
                    task.LatestErrorMessage = $"任务执行超时（{task.Timeout}分钟）";
                    await _databaseManager.ScheduledTaskRepository.UpdateAsync(task);
                }
            }
            catch (Exception ex)
            {
                task.IsRunning = false;
                task.IsLatestSuccess = false;
                task.LatestEndDate = DateTime.Now;
                task.LatestFailureCount++;
                task.LatestErrorMessage = ex.Message;
                await _databaseManager.ScheduledTaskRepository.UpdateAsync(task);

                await _databaseManager.ErrorLogRepository.AddErrorLogAsync(ex);
                _logger.LogError(ex.Message);
            }
        }

        private async Task RunTaskAsync(ScheduledTask task)
        {
            if (task.TaskType == TaskType.Ping)
            {
                await PingAsync(task);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Scheduled Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }

    }
}
