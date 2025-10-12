using Dapper;
using Datory;
using Datory.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public partial class DatabaseManager : IDatabaseManager
    {
        private readonly ISettingsManager _settingsManager;

        public ITableStyleRepository TableStyleRepository { get; }
        public IAdministratorRepository AdministratorRepository { get; }
        public IAdministratorsInRolesRepository AdministratorsInRolesRepository { get; }
        public IConfigRepository ConfigRepository { get; }
        public IDbCacheRepository DbCacheRepository { get; }
        public IErrorLogRepository ErrorLogRepository { get; }
        public ILogRepository LogRepository { get; }
        public IRoleRepository RoleRepository { get; }
        public IStatRepository StatRepository { get; }
        public IStatLogRepository StatLogRepository { get; }
        public IUserGroupRepository UserGroupRepository { get; }
        public IUserMenuRepository UserMenuRepository { get; }
        public IUserRepository UserRepository { get; }
        public IOrganCompanyRepository OrganCompanyRepository { get; }
        public IOrganDepartmentRepository OrganDepartmentRepository { get; }
        public IBlockRuleRepository BlockRuleRepository { get; }
        public IBlockAnalysisRepository BlockAnalysisRepository { get; }
        public IDbBackupRepository DbBackupRepository { get; }
        public IDbRecoverRepository DbRecoverRepository { get; }
        public IScheduledTaskRepository ScheduledTaskRepository { get; }

        public IExamTxRepository ExamTxRepository { get; }
        public IExamTmTreeRepository ExamTmTreeRepository { get; }
        public IExamTmRepository ExamTmRepository { get; }
        public IExamTmAnalysisRepository ExamTmAnalysisRepository { get; }
        public IExamTmAnalysisTmRepository ExamTmAnalysisTmRepository { get; }
        public IExamCerUserRepository ExamCerUserRepository { get; }
        public IExamCerRepository ExamCerRepository { get; }
        public IExamPaperTreeRepository ExamPaperTreeRepository { get; }
        public IExamPaperRepository ExamPaperRepository { get; }
        public IExamTmGroupRepository ExamTmGroupRepository { get; }
        public IExamPaperRandomRepository ExamPaperRandomRepository { get; }
        public IExamPaperRandomTmRepository ExamPaperRandomTmRepository { get; }
        public IExamPaperRandomConfigRepository ExamPaperRandomConfigRepository { get; }
        public IExamPaperUserRepository ExamPaperUserRepository { get; }
        public IExamPaperStartRepository ExamPaperStartRepository { get; }
        public IExamPaperAnswerRepository ExamPaperAnswerRepository { get; }


        public IExamPracticeRepository ExamPracticeRepository { get; }
        public IExamPracticeAnswerRepository ExamPracticeAnswerRepository { get; }
        public IExamPracticeCollectRepository ExamPracticeCollectRepository { get; }
        public IExamPracticeWrongRepository ExamPracticeWrongRepository { get; }

        public IExamQuestionnaireAnswerRepository ExamQuestionnaireAnswerRepository { get; }
        public IExamQuestionnaireRepository ExamQuestionnaireRepository { get; }
        public IExamQuestionnaireTmRepository ExamQuestionnaireTmRepository { get; }
        public IExamQuestionnaireUserRepository ExamQuestionnaireUserRepository { get; }

        public IExamPkRepository ExamPkRepository { get; }
        public IExamPkRoomRepository ExamPkRoomRepository { get; }
        public IExamPkRoomAnswerRepository ExamPkRoomAnswerRepository { get; }
        public IExamPkUserRepository ExamPkUserRepository { get; }

        public IExamAssessmentRepository ExamAssessmentRepository { get; }
        public IExamAssessmentUserRepository ExamAssessmentUserRepository { get; }
        public IExamAssessmentTmRepository ExamAssessmentTmRepository { get; }
        public IExamAssessmentAnswerRepository ExamAssessmentAnswerRepository { get; }
        public IExamAssessmentConfigRepository ExamAssessmentConfigRepository { get; }
        public IExamAssessmentConfigSetRepository ExamAssessmentConfigSetRepository { get; }

        public IExamPaperAnswerSmallRepository ExamPaperAnswerSmallRepository { get; }
        public IExamPaperRandomTmSmallRepository ExamPaperRandomTmSmallRepository { get; }
        public IExamPracticeAnswerSmallRepository ExamPracticeAnswerSmallRepository { get; }
        public IExamTmSmallRepository ExamTmSmallRepository { get; }

        public IKnowlegesRepository KnowlegesRepository { get; }
        public IKnowlegesTreeRepository KnowlegesTreeRepository { get; }

        public IStudyCourseFilesRepository StudyCourseFilesRepository { get; }
        public IStudyCourseFilesGroupRepository StudyCourseFilesGroupRepository { get; }
        public IStudyCourseEvaluationItemRepository StudyCourseEvaluationItemRepository { get; }
        public IStudyCourseEvaluationItemUserRepository StudyCourseEvaluationItemUserRepository { get; }
        public IStudyCourseEvaluationRepository StudyCourseEvaluationRepository { get; }
        public IStudyCourseEvaluationUserRepository StudyCourseEvaluationUserRepository { get; }

        public IStudyPlanRepository StudyPlanRepository { get; }
        public IStudyPlanCourseRepository StudyPlanCourseRepository { get; }
        public IStudyPlanUserRepository StudyPlanUserRepository { get; }
        public IStudyCourseUserRepository StudyCourseUserRepository { get; }
        public IStudyCourseWareUserRepository StudyCourseWareUserRepository { get; }
        public IStudyCourseTreeRepository StudyCourseTreeRepository { get; }
        public IStudyCourseRepository StudyCourseRepository { get; }
        public IStudyCourseWareRepository StudyCourseWareRepository { get; }
        public IPointShopRepository PointShopRepository { get; }
        public IPointShopUserRepository PointShopUserRepository { get; }
        public IPointLogRepository PointLogRepository { get; }

        public DatabaseManager(
            ISettingsManager settingsManager,
            ITableStyleRepository tableStyleRepository,
            IAdministratorRepository administratorRepository,
            IAdministratorsInRolesRepository administratorsInRolesRepository,
            IConfigRepository configRepository,
            IDbCacheRepository dbCacheRepository,
            IErrorLogRepository errorLogRepository,
            ILogRepository logRepository,
            IRoleRepository roleRepository,
            IStatRepository statRepository,
            IStatLogRepository statLogRepository,
            IUserGroupRepository userGroupRepository,
            IUserMenuRepository userMenuRepository,
            IUserRepository userRepository,
            IOrganCompanyRepository organCompanyRepository,
            IOrganDepartmentRepository organDepartmentRepository,
            IBlockAnalysisRepository blockAnalysisRepository,
            IBlockRuleRepository blockRuleRepository,
            IDbRecoverRepository dbRecoverRepository,
            IDbBackupRepository dbBackupRepository,
            IScheduledTaskRepository scheduledTaskRepository,
            IExamTxRepository examTxRepository,
            IExamTmTreeRepository examTmTreeRepository,
            IExamTmRepository examTmRepository,
            IExamTmAnalysisRepository examTmAnalysisRepository,
            IExamTmAnalysisTmRepository examTmAnalysisTmRepository,
            IExamCerUserRepository examCerUserRepository,
            IExamCerRepository examCerRepository,
            IExamPaperTreeRepository examPaperTreeRepository,
            IExamPaperRepository examPaperRepository,
            IExamTmGroupRepository examTmGroupRepository,
            IExamPaperRandomRepository examPaperRandomRepository,
            IExamPaperRandomTmRepository examPaperRandomTmRepository,
            IExamPaperRandomConfigRepository examPaperRandomConfigRepository,
            IExamPaperUserRepository examPaperUserRepository,
            IExamPaperStartRepository examPaperStartRepository,
            IExamPaperAnswerRepository examPaperAnswerRepository,
            IExamPracticeRepository examPracticeRepository,
            IExamPracticeAnswerRepository examPracticeAnswerRepository,
            IExamPracticeCollectRepository examPracticeCollectRepository,
            IExamPracticeWrongRepository examPracticeWrongRepository,
            IExamQuestionnaireAnswerRepository examQuestionnaireAnswerRepository,
            IExamQuestionnaireRepository examQuestionnaireRepository,
            IExamQuestionnaireTmRepository examQuestionnaireTmRepository,
            IExamQuestionnaireUserRepository examQuestionnaireUserRepository,
            IExamPkRepository examPkRepository,
            IExamPkRoomRepository examPkRoomRepository,
            IExamPkRoomAnswerRepository examPkRoomAnswerRepository,
            IExamPkUserRepository examPkUserRepository,
            IExamAssessmentRepository examAssessmentRepository,
            IExamAssessmentUserRepository examAssessmentUserRepository,
            IExamAssessmentTmRepository examAssessmentTmRepository,
            IExamAssessmentAnswerRepository examAssessmentAnswerRepository,
            IExamAssessmentConfigRepository examAssessmentConfigRepository,
            IExamAssessmentConfigSetRepository examAssessmentConfigSetRepository,
            IExamPaperAnswerSmallRepository examPaperAnswerSmallRepository,
            IExamPaperRandomTmSmallRepository examPaperRandomTmSmallRepository,
            IExamPracticeAnswerSmallRepository examPracticeAnswerSmallRepository,
            IExamTmSmallRepository examTmSmallRepository,
            IKnowlegesRepository knowlegesRepository,
            IKnowlegesTreeRepository knowlegesTreeRepository,
            IStudyCourseFilesRepository studyCourseFilesRepository,
            IStudyCourseFilesGroupRepository studyCourseFilesGroupRepository,
            IStudyCourseEvaluationItemRepository studyCourseEvaluationItemRepository,
            IStudyCourseEvaluationItemUserRepository studyCourseEvaluationItemUserRepository,
            IStudyCourseEvaluationRepository studyCourseEvaluationRepository,
            IStudyCourseEvaluationUserRepository studyCourseEvaluationUserRepository,
            IStudyPlanRepository studyPlanRepository,
            IStudyPlanCourseRepository studyPlanCourseRepository,
            IStudyPlanUserRepository studyPlanUserRepository,
            IStudyCourseUserRepository studyCourseUserRepository,
            IStudyCourseWareUserRepository studyCourseWareUserRepository,
            IStudyCourseTreeRepository studyCourseTreeRepository,
            IStudyCourseRepository studyCourseRepository,
            IStudyCourseWareRepository studyCourseWareRepository,
            IPointShopRepository pointShopRepository,
            IPointShopUserRepository pointShopUserRepository,
            IPointLogRepository pointLogRepository)
        {
            _settingsManager = settingsManager;
            TableStyleRepository = tableStyleRepository;
            AdministratorRepository = administratorRepository;
            AdministratorsInRolesRepository = administratorsInRolesRepository;
            ConfigRepository = configRepository;
            DbCacheRepository = dbCacheRepository;
            ErrorLogRepository = errorLogRepository;
            LogRepository = logRepository;
            RoleRepository = roleRepository;
            StatRepository = statRepository;
            StatLogRepository = statLogRepository;
            UserGroupRepository = userGroupRepository;
            UserMenuRepository = userMenuRepository;
            UserRepository = userRepository;
            OrganCompanyRepository = organCompanyRepository;
            OrganDepartmentRepository = organDepartmentRepository;
            BlockAnalysisRepository = blockAnalysisRepository;
            BlockRuleRepository = blockRuleRepository;
            DbRecoverRepository = dbRecoverRepository;
            DbBackupRepository = dbBackupRepository;
            ScheduledTaskRepository = scheduledTaskRepository;
            ExamTxRepository = examTxRepository;
            ExamTmTreeRepository = examTmTreeRepository;
            ExamTmRepository = examTmRepository;
            ExamTmAnalysisRepository = examTmAnalysisRepository;
            ExamTmAnalysisTmRepository = examTmAnalysisTmRepository;
            ExamCerUserRepository = examCerUserRepository;
            ExamCerRepository = examCerRepository;
            ExamPaperTreeRepository = examPaperTreeRepository;
            ExamPaperRepository = examPaperRepository;
            ExamTmGroupRepository = examTmGroupRepository;
            ExamPaperRandomRepository = examPaperRandomRepository;
            ExamPaperRandomTmRepository = examPaperRandomTmRepository;
            ExamPaperRandomConfigRepository = examPaperRandomConfigRepository;
            ExamPaperUserRepository = examPaperUserRepository;
            ExamPaperStartRepository = examPaperStartRepository;
            ExamPaperAnswerRepository = examPaperAnswerRepository;
            ExamPracticeRepository = examPracticeRepository;
            ExamPracticeAnswerRepository = examPracticeAnswerRepository;
            ExamPracticeCollectRepository = examPracticeCollectRepository;
            ExamPracticeWrongRepository = examPracticeWrongRepository;
            ExamQuestionnaireAnswerRepository = examQuestionnaireAnswerRepository;
            ExamQuestionnaireRepository = examQuestionnaireRepository;
            ExamQuestionnaireTmRepository = examQuestionnaireTmRepository;
            ExamQuestionnaireUserRepository = examQuestionnaireUserRepository;
            ExamPkRepository = examPkRepository;
            ExamPkRoomRepository = examPkRoomRepository;
            ExamPkRoomAnswerRepository = examPkRoomAnswerRepository;
            ExamPkUserRepository = examPkUserRepository;
            ExamAssessmentRepository = examAssessmentRepository;
            ExamAssessmentUserRepository = examAssessmentUserRepository;
            ExamAssessmentTmRepository = examAssessmentTmRepository;
            ExamAssessmentAnswerRepository = examAssessmentAnswerRepository;
            ExamAssessmentConfigRepository = examAssessmentConfigRepository;
            ExamAssessmentConfigSetRepository = examAssessmentConfigSetRepository;
            ExamPaperAnswerSmallRepository = examPaperAnswerSmallRepository;
            ExamPaperRandomTmSmallRepository = examPaperRandomTmSmallRepository;
            ExamPracticeAnswerSmallRepository = examPracticeAnswerSmallRepository;
            ExamTmSmallRepository = examTmSmallRepository;
            KnowlegesRepository = knowlegesRepository;
            KnowlegesTreeRepository = knowlegesTreeRepository;
            StudyCourseFilesRepository = studyCourseFilesRepository;
            StudyCourseFilesGroupRepository = studyCourseFilesGroupRepository;
            StudyCourseEvaluationItemRepository = studyCourseEvaluationItemRepository;
            StudyCourseEvaluationItemUserRepository = studyCourseEvaluationItemUserRepository;
            StudyCourseEvaluationRepository = studyCourseEvaluationRepository;
            StudyCourseEvaluationUserRepository = studyCourseEvaluationUserRepository;
            StudyPlanRepository = studyPlanRepository;
            StudyPlanCourseRepository = studyPlanCourseRepository;
            StudyPlanUserRepository = studyPlanUserRepository;
            StudyCourseUserRepository = studyCourseUserRepository;
            StudyCourseWareUserRepository = studyCourseWareUserRepository;
            StudyCourseTreeRepository = studyCourseTreeRepository;
            StudyCourseRepository = studyCourseRepository;
            StudyCourseWareRepository = studyCourseWareRepository;
            PointShopRepository = pointShopRepository;
            PointShopUserRepository = pointShopUserRepository;
            PointLogRepository = pointLogRepository;
        }

        public List<IRepository> GetAllRepositories()
        {
            var list = new List<IRepository>
            {
                ConfigRepository,
                TableStyleRepository,
                AdministratorRepository,
                AdministratorsInRolesRepository,
                RoleRepository,
                OrganCompanyRepository,
                OrganDepartmentRepository,
                DbCacheRepository,
                ErrorLogRepository,
                LogRepository,
                StatRepository,
                StatLogRepository,
                UserGroupRepository,
                UserRepository,
                UserMenuRepository,
                BlockRuleRepository,
                BlockAnalysisRepository,
                DbBackupRepository,
                DbRecoverRepository,
                ScheduledTaskRepository,
                ExamTxRepository,
                ExamTmTreeRepository,
                ExamTmRepository,
                ExamTmAnalysisRepository,
                ExamTmAnalysisTmRepository,
                ExamCerRepository,
                ExamCerUserRepository,
                ExamPaperTreeRepository,
                ExamPaperRepository,
                ExamTmGroupRepository,
                ExamPaperRandomRepository,
                ExamPaperRandomTmRepository,
                ExamPaperRandomConfigRepository,
                ExamPaperUserRepository,
                ExamPaperStartRepository,
                ExamPaperAnswerRepository,
                ExamPracticeRepository,
                ExamPracticeWrongRepository,
                ExamPracticeAnswerRepository,
                ExamPracticeCollectRepository,
                ExamQuestionnaireUserRepository,
                ExamQuestionnaireTmRepository,
                ExamQuestionnaireRepository,
                ExamQuestionnaireAnswerRepository,
                ExamPkRepository,
                ExamPkRoomRepository,
                ExamPkRoomAnswerRepository,
                ExamPkUserRepository,
                ExamAssessmentRepository,
                ExamAssessmentUserRepository,
                ExamAssessmentTmRepository,
                ExamAssessmentAnswerRepository,
                ExamAssessmentConfigRepository,
                ExamAssessmentConfigSetRepository,
                ExamPaperAnswerSmallRepository,
                ExamPracticeAnswerSmallRepository,
                ExamPaperRandomTmSmallRepository,
                ExamTmSmallRepository,
                KnowlegesRepository,
                KnowlegesTreeRepository,
                StudyCourseFilesRepository,
                StudyCourseFilesGroupRepository,
                StudyCourseEvaluationItemRepository,
                StudyCourseEvaluationItemUserRepository,
                StudyCourseEvaluationRepository,
                StudyCourseEvaluationUserRepository,
                StudyPlanRepository,
                StudyPlanUserRepository,
                StudyPlanCourseRepository,
                StudyCourseUserRepository,
                StudyCourseWareUserRepository,
                StudyCourseTreeRepository,
                StudyCourseRepository,
                StudyCourseWareRepository,
                PointShopRepository,
                PointShopUserRepository,
                PointLogRepository
            };

            return list;
        }

        public Database GetDatabase(string connectionString = null)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = _settingsManager.Database.ConnectionString;
            }

            return new Database(_settingsManager.Database.DatabaseType, connectionString);
        }

        private IDbConnection GetConnection(string connectionString = null)
        {
            var database = GetDatabase(connectionString);
            return database.GetConnection();
        }

        private IDbConnection GetConnection(DatabaseType databaseType, string connectionString = null)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = _settingsManager.Database.ConnectionString;
            }
            var database = new Database(databaseType, connectionString);
            return database.GetConnection();
        }

        public async Task DeleteDbLogAsync()
        {
            if (_settingsManager.Database.DatabaseType == DatabaseType.MySql)
            {
                using var connection = _settingsManager.Database.GetConnection();
                await connection.ExecuteAsync("PURGE MASTER LOGS BEFORE DATE_SUB( NOW( ), INTERVAL 3 DAY)");
            }
            else if (_settingsManager.Database.DatabaseType == DatabaseType.SqlServer)
            {
                var databaseName = await _settingsManager.Database.GetDatabaseNamesAsync();

                using var connection = _settingsManager.Database.GetConnection();
                var versions = await connection.QueryFirstAsync<string>("SELECT SERVERPROPERTY('productversion')");

                var version = 8;
                var arr = versions.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                if (arr.Length > 0)
                {
                    version = TranslateUtils.ToInt(arr[0], 8);
                }
                if (version < 10)
                {
                    await connection.ExecuteAsync($"BACKUP LOG [{databaseName}] WITH NO_LOG");
                }
                else
                {
                    await connection.ExecuteAsync($@"ALTER DATABASE [{databaseName}] SET RECOVERY SIMPLE;DBCC shrinkfile ([{databaseName}_log], 1); ALTER DATABASE [{databaseName}] SET RECOVERY FULL; ");
                }
            }
        }

        public int GetIntResult(string connectionString, string sqlString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = _settingsManager.Database.ConnectionString;
            }

            int count;

            var database = new Database(_settingsManager.Database.DatabaseType, connectionString);
            using (var conn = database.GetConnection())
            {
                count = conn.ExecuteScalar<int>(sqlString);
                //conn.Open();
                //using (var rdr = ExecuteReader(conn, sqlString))
                //{
                //    if (rdr.Read())
                //    {
                //        count = GetInt(rdr, 0);
                //    }
                //    rdr.Close();
                //}
            }
            return count;
        }

        public int GetIntResult(string sqlString)
        {
            return GetIntResult(_settingsManager.Database.ConnectionString, sqlString);
        }

        public string GetString(string connectionString, string sqlString)
        {
            string value;

            using (var connection = GetConnection(connectionString))
            {
                value = connection.ExecuteScalar<string>(sqlString);
            }

            return value;
        }

        private string GetString(string sqlString)
        {
            string value;

            using (var connection = GetConnection())
            {
                value = connection.ExecuteScalar<string>(sqlString);
            }

            return value;
        }

        public IEnumerable<IDictionary<string, object>> GetRows(DatabaseType databaseType, string connectionString, string sqlString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = _settingsManager.Database.ConnectionString;
            }

            if (string.IsNullOrEmpty(sqlString)) return null;

            IEnumerable<IDictionary<string, object>> rows;

            using (var connection = GetConnection(databaseType, connectionString))
            {
                rows = connection.Query(sqlString).Cast<IDictionary<string, object>>();
            }

            return rows;
        }

        public int GetPageTotalCount(string sqlString)
        {
            var temp = StringUtils.ToLower(sqlString);
            var pos = temp.LastIndexOf("order by", StringComparison.OrdinalIgnoreCase);
            if (pos > -1)
                sqlString = sqlString.Substring(0, pos);

            // Add new ORDER BY info if SortKeyField is specified
            //if (!string.IsNullOrEmpty(sortField) && addCustomSortInfo)
            //    SelectCommand += " ORDER BY " + SortField;

            return GetIntResult($"SELECT COUNT(*) FROM ({sqlString}) AS T0");
        }

        public string GetSelectSqlString(string tableName, int totalNum, string columns, string whereString, string orderByString)
        {
            return GetSelectSqlString(tableName, totalNum, columns, whereString, orderByString, string.Empty);
        }

        private string GetSelectSqlString(string tableName, int totalNum, string columns, string whereString, string orderByString, string joinString)
        {
            if (!string.IsNullOrEmpty(whereString))
            {
                whereString = StringUtils.ReplaceStartsWith(whereString.Trim(), "AND", string.Empty);
                if (!StringUtils.StartsWithIgnoreCase(whereString, "WHERE "))
                {
                    whereString = "WHERE " + whereString;
                }
            }

            if (!string.IsNullOrEmpty(joinString))
            {
                whereString = joinString + " " + whereString;
            }

            return DbUtils.ToTopSqlString(_settingsManager.Database, tableName, columns, whereString, orderByString, totalNum);
        }

        public int GetCount(string tableName)
        {
            int count;

            using (var conn = _settingsManager.Database.GetConnection())
            {
                count = conn.ExecuteScalar<int>($"SELECT COUNT(*) FROM {Quote(tableName)}");
            }
            return count;

            //return GetIntResult();
        }

        public async Task<List<IDictionary<string, object>>> GetObjectsAsync(string tableName)
        {
            List<IDictionary<string, object>> objects;
            var sqlString = $"select * from {tableName}";

            await using (var connection = _settingsManager.Database.GetConnection())
            {
                objects = (from row in await connection.QueryAsync(sqlString)
                           select (IDictionary<string, object>)row).AsList();
            }

            return objects;
        }

        public async Task<List<IDictionary<string, object>>> GetPageObjectsAsync(string tableName, string identityColumnName, int offset, int limit)
        {
            List<IDictionary<string, object>> objects;
            var sqlString = GetPageSqlString(tableName, "*", string.Empty, $"ORDER BY {identityColumnName} ASC", offset, limit);

            await using (var connection = _settingsManager.Database.GetConnection())
            {
                objects = (from row in await connection.QueryAsync(sqlString)
                           select (IDictionary<string, object>)row).AsList();
            }

            return objects;
        }

        private decimal? _sqlServerVersion;

        private decimal SqlServerVersion
        {
            get
            {
                if (_settingsManager.Database.DatabaseType != DatabaseType.SqlServer)
                {
                    return 0;
                }

                if (_sqlServerVersion == null)
                {
                    try
                    {
                        _sqlServerVersion =
                            TranslateUtils.ToDecimal(
                                GetString("select left(cast(serverproperty('productversion') as varchar), 4)"));
                    }
                    catch
                    {
                        _sqlServerVersion = 0;
                    }
                }

                return _sqlServerVersion.Value;
            }
        }

        private bool IsSqlServer2012 => SqlServerVersion >= 11;

        public string GetPageSqlString(string tableName, string columnNames, string whereSqlString, string orderSqlString, int offset, int limit)
        {
            var retVal = string.Empty;

            if (string.IsNullOrEmpty(orderSqlString))
            {
                orderSqlString = "ORDER BY Id DESC";
            }

            if (offset == 0 && limit == 0)
            {
                return $@"SELECT {columnNames} FROM {tableName} {whereSqlString} {orderSqlString}";
            }

            if (_settingsManager.Database.DatabaseType == DatabaseType.MySql)
            {
                if (limit == 0)
                {
                    limit = int.MaxValue;
                }
                retVal = $@"SELECT {columnNames} FROM {tableName} {whereSqlString} {orderSqlString} LIMIT {limit} OFFSET {offset}";
            }
            else if (_settingsManager.Database.DatabaseType == DatabaseType.SqlServer && IsSqlServer2012)
            {
                retVal = limit == 0
                    ? $"SELECT {columnNames} FROM {tableName} {whereSqlString} {orderSqlString} OFFSET {offset} ROWS"
                    : $"SELECT {columnNames} FROM {tableName} {whereSqlString} {orderSqlString} OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY";
            }
            else if (_settingsManager.Database.DatabaseType == DatabaseType.SqlServer && !IsSqlServer2012)
            {
                if (offset == 0)
                {
                    retVal = $"SELECT TOP {limit} {columnNames} FROM {tableName} {whereSqlString} {orderSqlString}";
                }
                else
                {
                    var rowWhere = limit == 0
                        ? $@"WHERE [row_num] > {offset}"
                        : $@"WHERE [row_num] BETWEEN {offset + 1} AND {offset + limit}";

                    retVal = $@"SELECT * FROM (
    SELECT {columnNames}, ROW_NUMBER() OVER ({orderSqlString}) AS [row_num] FROM [{tableName}] {whereSqlString}
) as T {rowWhere}";
                }
            }
            else
            {
                retVal = limit == 0
                    ? $@"SELECT {columnNames} FROM {tableName} {whereSqlString} {orderSqlString} OFFSET {offset}"
                    : $@"SELECT {columnNames} FROM {tableName} {whereSqlString} {orderSqlString} LIMIT {limit} OFFSET {offset}";
            }

            return retVal;
        }

        public string GetDatabaseNameFormConnectionString(string connectionString)
        {
            var name = GetValueFromConnectionString(connectionString, "Database");
            if (string.IsNullOrEmpty(name))
            {
                name = GetValueFromConnectionString(connectionString, "Initial Catalog");
            }
            return name;
        }

        private string GetValueFromConnectionString(string connectionString, string attribute)
        {
            var retVal = string.Empty;
            if (!string.IsNullOrEmpty(connectionString) && !string.IsNullOrEmpty(attribute))
            {
                var pairs = connectionString.Split(';');
                foreach (var pair in pairs)
                {
                    if (pair.IndexOf("=", StringComparison.Ordinal) != -1)
                    {
                        if (StringUtils.EqualsIgnoreCase(attribute, pair.Trim().Split('=')[0]))
                        {
                            retVal = pair.Trim().Split('=')[1];
                            break;
                        }
                    }
                }
            }
            return retVal;
        }
    }
}

