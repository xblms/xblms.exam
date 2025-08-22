using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Repositories;
namespace XBLMS.Services
{
    public partial interface IDatabaseManager
    {
        IAdministratorRepository AdministratorRepository { get; }
        IAdministratorsInRolesRepository AdministratorsInRolesRepository { get; }
        IConfigRepository ConfigRepository { get; }
        IDbCacheRepository DbCacheRepository { get; }
        IErrorLogRepository ErrorLogRepository { get; }
        ILogRepository LogRepository { get; }
        IRoleRepository RoleRepository { get; }
        IStatRepository StatRepository { get; }
        IStatLogRepository StatLogRepository { get; }
        IUserGroupRepository UserGroupRepository { get; }
        IUserMenuRepository UserMenuRepository { get; }
        IUserRepository UserRepository { get; }
        IOrganCompanyRepository OrganCompanyRepository { get; }
        IOrganDepartmentRepository OrganDepartmentRepository { get; }
        IOrganDutyRepository OrganDutyRepository { get; }
        IBlockAnalysisRepository BlockAnalysisRepository { get; }
        IBlockRuleRepository BlockRuleRepository { get; }
        IDbBackupRepository DbBackupRepository { get; }
        IDbRecoverRepository DbRecoverRepository { get; }
        ICrudDemoRepository CrudDemoRepository { get; }

        IExamTxRepository ExamTxRepository { get; }
        IExamTmTreeRepository ExamTmTreeRepository { get; }
        IExamTmRepository ExamTmRepository { get; }
        IExamTmSmallRepository ExamTmSmallRepository { get; }
        IExamTmAnalysisRepository ExamTmAnalysisRepository { get; }
        IExamTmAnalysisTmRepository ExamTmAnalysisTmRepository { get; }
        IExamCerRepository ExamCerRepository { get; }
        IExamCerUserRepository ExamCerUserRepository { get; }

        IExamPaperTreeRepository ExamPaperTreeRepository { get; }
        IExamPaperRepository ExamPaperRepository { get; }
        IExamTmGroupRepository ExamTmGroupRepository { get; }
        IExamPaperRandomRepository ExamPaperRandomRepository { get; }
        IExamPaperRandomTmRepository ExamPaperRandomTmRepository { get; }
        IExamPaperRandomTmSmallRepository ExamPaperRandomTmSmallRepository { get; }
        IExamPaperRandomConfigRepository ExamPaperRandomConfigRepository { get; }

        IExamPaperUserRepository ExamPaperUserRepository { get; }
        IExamPaperStartRepository ExamPaperStartRepository { get; }
        IExamPaperAnswerRepository ExamPaperAnswerRepository { get; }
        IExamPaperAnswerSmallRepository ExamPaperAnswerSmallRepository { get; }

        IExamPracticeRepository ExamPracticeRepository { get; }
        IExamPracticeWrongRepository ExamPracticeWrongRepository { get; }
        IExamPracticeCollectRepository ExamPracticeCollectRepository { get; }
        IExamPracticeAnswerRepository ExamPracticeAnswerRepository { get; }
        IExamPracticeAnswerSmallRepository ExamPracticeAnswerSmallRepository { get; }

        IExamQuestionnaireAnswerRepository ExamQuestionnaireAnswerRepository { get; }
        IExamQuestionnaireRepository ExamQuestionnaireRepository { get; }
        IExamQuestionnaireTmRepository ExamQuestionnaireTmRepository { get; }
        IExamQuestionnaireUserRepository ExamQuestionnaireUserRepository { get; }

        IExamPkRepository ExamPkRepository { get; }
        IExamPkRoomRepository ExamPkRoomRepository { get; }
        IExamPkUserRepository ExamPkUserRepository { get; }
        IExamPkRoomAnswerRepository ExamPkRoomAnswerRepository { get; }

        IExamAssessmentRepository ExamAssessmentRepository { get; }
        IExamAssessmentUserRepository ExamAssessmentUserRepository { get; }
        IExamAssessmentTmRepository ExamAssessmentTmRepository { get; }
        IExamAssessmentAnswerRepository ExamAssessmentAnswerRepository { get; }
        IExamAssessmentConfigRepository ExamAssessmentConfigRepository { get; }
        IExamAssessmentConfigSetRepository ExamAssessmentConfigSetRepository { get; }

        IKnowlegesRepository KnowlegesRepository { get; }
        IKnowlegesTreeRepository KnowlegesTreeRepository { get; }

        List<IRepository> GetAllRepositories();

        Database GetDatabase(string connectionString = null);

        int GetIntResult(string connectionString, string sqlString);

        int GetIntResult(string sqlString);

        string GetString(string connectionString, string sqlString);

        IEnumerable<IDictionary<string, object>> GetRows(DatabaseType databaseType, string connectionString, string sqlString);

        int GetPageTotalCount(string sqlString);

        string GetSelectSqlString(string tableName, int totalNum, string columns, string whereString,
            string orderByString);

        int GetCount(string tableName);

        Task<List<IDictionary<string, object>>> GetObjectsAsync(string tableName);

        Task<List<IDictionary<string, object>>> GetPageObjectsAsync(string tableName, string identityColumnName, int offset, int limit);

        string GetPageSqlString(string tableName, string columnNames, string whereSqlString, string orderSqlString,
            int offset, int limit);

        string GetDatabaseNameFormConnectionString(string connectionString);
    }
}
