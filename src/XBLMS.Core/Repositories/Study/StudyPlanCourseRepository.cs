using Datory;
using SqlKata;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public class StudyPlanCourseRepository : IStudyPlanCourseRepository
    {
        private readonly ISettingsManager _settingsManager;
        private readonly Repository<StudyPlanCourse> _repository;

        public StudyPlanCourseRepository(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            _repository = new Repository<StudyPlanCourse>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task<int> InsertAsync(StudyPlanCourse item)
        {
            return await _repository.InsertAsync(item);
        }

        public async Task<bool> UpdateAsync(StudyPlanCourse item)
        {
            return await _repository.UpdateAsync(item);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> DeleteByPlanAsync(int planId)
        {
            return await _repository.DeleteAsync(Q.Where(nameof(StudyPlanCourse.PlanId), planId)) > 0;
        }

        public async Task<bool> DeleteByNotIdsAsync(List<int> notIds, int planId)
        {
            return await _repository.DeleteAsync(Q.
                Where(nameof(StudyPlanCourse.PlanId), planId).
                WhereNotIn(nameof(StudyPlanCourse.Id), notIds)) > 0;
        }

        public async Task<StudyPlanCourse> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }
        public async Task<StudyPlanCourse> GetAsync(int planId, int courseId)
        {
            var query = Q.Where(nameof(StudyPlanCourse.PlanId), planId).Where(nameof(StudyPlanCourse.CourseId), courseId);
            return await _repository.GetAsync(query);
        }
        public async Task<(int total, List<StudyPlanCourse> list)> GetListByTeacherAsync(int teacherId, string keyWords, int pageIndex, int pageSize)
        {
            var query = Q.Where(nameof(StudyPlanCourse.TeacherId), teacherId);

            if (!string.IsNullOrEmpty(keyWords))
            {
                query.Where(q => q.WhereLike(nameof(StudyPlanCourse.CourseName), $"%{keyWords}%"));
            }

            query.OrderByDesc(nameof(StudyPlanCourse.Id));
            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }
        public async Task<List<StudyPlanCourse>> GetListAsync(bool isSelect, int planId)
        {
            var query = Q.Where(nameof(StudyPlanCourse.PlanId), planId);

            if (isSelect)
            {
                query.WhereTrue(nameof(StudyPlanCourse.IsSelectCourse));
            }
            else
            {
                query.WhereNullOrFalse(nameof(StudyPlanCourse.IsSelectCourse));
            }

            query.OrderByDesc(nameof(StudyPlanCourse.Taxis));
            return await _repository.GetAllAsync(query);
        }
        public async Task<List<StudyPlanCourse>> GetListAsync(int planId)
        {
            var query = Q.Where(nameof(StudyPlanCourse.PlanId), planId);

            query.OrderByDesc(nameof(StudyPlanCourse.IsSelectCourse), nameof(StudyPlanCourse.Taxis));
            return await _repository.GetAllAsync(query);
        }
        public async Task<int> GetPaperUseCount(int paperId)
        {
            return await _repository.CountAsync(Q.Where(nameof(StudyPlanCourse.ExamId), paperId));
        }
        public async Task<int> GetPaperQUseCount(int paperId)
        {
            return await _repository.CountAsync(Q.Where(nameof(StudyPlanCourse.ExamQuestionnaireId), paperId));
        }
        public async Task<int> GetEvaluationUseCount(int eId)
        {
            return await _repository.CountAsync(Q.Where(nameof(StudyPlanCourse.StudyCourseEvaluationId), eId));
        }
        public async Task<int> GetCourseUseCount(int courseId)
        {
            return await _repository.CountAsync(Q.Where(nameof(StudyPlanCourse.CourseId), courseId));
        }
        public async Task<int> CountAsync(int planId, bool isSelect)
        {
            var query = Q.Where(nameof(StudyPlanCourse.PlanId), planId);

            if (isSelect)
            {
                query.WhereTrue(nameof(StudyPlanCourse.IsSelectCourse));
            }
            else
            {
                query.WhereNullOrFalse(nameof(StudyPlanCourse.IsSelectCourse));
            }

            return await _repository.CountAsync(query);
        }

        public async Task<decimal> GetTotalCreditAsync(int planId, bool isSelect)
        {
            decimal totalCredit = 0;
            var query = Q.Where(nameof(StudyPlanCourse.PlanId), planId);

            if (isSelect)
            {
                query.WhereTrue(nameof(StudyPlanCourse.IsSelectCourse));
            }
            else
            {
                query.WhereNullOrFalse(nameof(StudyPlanCourse.IsSelectCourse));
            }
            var list = await _repository.GetAllAsync<decimal>(query.Select(nameof(StudyPlanCourse.Credit)));
            if (list != null && list.Count > 0)
            {
                totalCredit = list.Sum(x => x);
            }
            return totalCredit;
        }


        public async Task<(int total, List<StudyPlanCourse> list)> GetOffTrinListByWeekAsync(AdminAuth auth)
        {
            var query = Q.
                WhereTrue(nameof(StudyPlanCourse.OffLine));

            query = GetQueryByAuth(query, auth);

            query.OrderByDesc(nameof(StudyCourse.Id));

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query);
            return (total, list);
        }

        private Query GetQueryByAuth(Query query, AdminAuth auth)
        {
            if (auth.AuthDataType == AuthorityDataType.DataCreator)
            {
                query.Where(nameof(StudyPlanCourse.CreatorId), auth.AdminId);
            }
            else
            {
                if (auth.AuthDataShowAll)
                {
                    if (auth.CurCompanyId != 1)
                    {
                        query.WhereLike(nameof(StudyPlanCourse.CompanyParentPath), $"%'{auth.CurCompanyId}'%");
                    }
                }
                else
                {
                    query.Where(nameof(StudyPlanCourse.CompanyId), auth.CurCompanyId);
                }
            }

            return query;
        }

    }
}
