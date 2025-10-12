using Datory;
using System;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Core.Repositories
{
    public partial class ConfigRepository
    {
        public async Task<Config> GetAsync()
        {
            Config config = null;

            if (!string.IsNullOrEmpty(_settingsManager.DatabaseConnectionString))
            {
                try
                {
                    config = await _repository.GetAsync(Q
                        .OrderBy(nameof(Config.Id))
                        .CachingGet(_cacheKey)
                    );
                }
                catch
                {
                    // ignored
                }
            }

            return config ?? new Config
            {
                Id = 0,
                DatabaseVersion = string.Empty,
                UpdateDate = DateTime.Now
            };
        }

        public async Task<(int value,int maxValue)> GetPointValueByPointType(PointType type)
        {
            var config = await GetAsync();
            if (type == PointType.PointLogin) return (config.PointLogin, config.PointLoginDayMax);
            if (type == PointType.PointPlanOver) return (config.PointPlanOver, config.PointPlanOverDayMax);
            if (type == PointType.PointCourseOver) return (config.PointCourseOver, config.PointCourseOverDayMax);
            if (type == PointType.PointVideo) return (config.PointVideo, config.PointVideoDayMax);
            if (type == PointType.PointDocument) return (config.PointDocument, config.PointDocumentDayMax);
            if (type == PointType.PointExam) return (config.PointExam, config.PointExamDayMax);
            if (type == PointType.PointExamFull) return (config.PointExamFull, config.PointExamFullDayMax);
            if (type == PointType.PointExamPass) return (config.PointExamPass, config.PointExamPassDayMax);
            if (type == PointType.PointExamQ) return (config.PointExamQ, config.PointExamQDayMax);
            if (type == PointType.PointExamAss) return (config.PointExamAss, config.PointExamAssDayMax);
            if (type == PointType.PointExamPractice) return (config.PointExamPractice, config.PointExamPracticeDayMax);
            if (type == PointType.PointExamPracticeRight) return (config.PointExamPracticeRight, config.PointExamPracticeRightDayMax);
            if (type == PointType.PointEvaluation) return (config.PointEvaluation, config.PointEvaluationDayMax);

            return (0, 0);
        }
    }
}
