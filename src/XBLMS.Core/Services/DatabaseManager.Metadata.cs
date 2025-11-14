using Datory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public partial class DatabaseManager
    {
        public async Task<(bool success, string errorMessage)> InstallAsync(string companyName, string userName, string password, string email, string mobile)
        {
            try
            {
                await SyncDatabaseAsync();


                var company = new OrganCompany
                {
                    Name = companyName,
                    ParentId = 0,
                    CreatorId = 1,
                    CompanyParentPath = ["'1'"],
                    ParentNames = [companyName]
                };
                await OrganCompanyRepository.InsertAsync(company);

                var administrator = new Administrator
                {
                    CompanyId = 1,
                    Auth = AuthorityType.Admin,
                    AuthData = AuthorityDataType.DataAll,
                    UserName = userName,
                    DisplayName = "超级管理员",
                    Email = email,
                    Mobile = mobile,
                    CompanyParentPath = company.CompanyParentPath,
                    AuthDataCurrentOrganId = 1,
                };

                await AdministratorRepository.InsertAsync(administrator, password);
                await ExamTxRepository.ResetAsync();

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, ex.ToString());
            }
        }

        public async Task SyncDatabaseAsync()
        {
            var repositories = GetAllRepositories();

            foreach (var repository in repositories)
            {
                if (string.IsNullOrEmpty(repository.TableName) || repository.TableColumns == null || repository.TableColumns.Count <= 0) continue;

                if (!await _settingsManager.Database.IsTableExistsAsync(repository.TableName))
                {
                    await CreateTableAsync(repository.TableName, repository.TableColumns);
                }
                else
                {
                    await AlterTableAsync(repository.TableName, repository.TableColumns);
                }
            }

            var config = await ConfigRepository.GetAsync();
            if (config.Id == 0)
            {
                await ConfigRepository.InsertAsync(config);
            }

            await UpdateConfigVersionAsync();
        }
        public async Task ClearDatabaseAsync()
        {
            var repositories = GetAllRepositories();

            foreach (var repository in repositories)
            {
                if (repository.TableName == OrganCompanyRepository.TableName)
                {
                    continue;
                }
                if (repository.TableName == AdministratorRepository.TableName)
                {
                    continue;
                }
                if (repository.TableName == ConfigRepository.TableName)
                {
                    continue;
                }

                await repository.Database.DropTableAsync(repository.TableName);
            }

            await SyncDatabaseAsync();

        }

        private async Task UpdateConfigVersionAsync()
        {
            var configInfo = await ConfigRepository.GetAsync();

            if (configInfo.Id > 0)
            {
                configInfo.DatabaseVersion = _settingsManager.Version;
                configInfo.UpdateDate = DateTime.UtcNow;
                await ConfigRepository.UpdateAsync(configInfo);
            }
        }

        private async Task<(bool IsSuccess, Exception Ex)> CreateTableAsync(string tableName, IList<TableColumn> tableColumns)
        {
            try
            {
                await _settingsManager.Database.CreateTableAsync(tableName, tableColumns);

                await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_General", "CreatorId", "CompanyId", "DepartmentId");
                await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_CompanyParentPath", "CompanyParentPath");
                await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_DepartmentParentPath", "DepartmentParentPath");
                await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_CreatedDate", "CreatedDate");

                if (tableName == ExamCerUserRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamCerUser.UserId)}", $"{nameof(ExamCerUser.UserId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamCerUser.CerId)}", $"{nameof(ExamCerUser.CerId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamCerUser.PlanId)}", $"{nameof(ExamCerUser.PlanId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamCerUser.CourseId)}", $"{nameof(ExamCerUser.CourseId)}");
                }
                else if (tableName == ExamPaperAnswerRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamPaperAnswer.UserId)}", $"{nameof(ExamPaperAnswer.UserId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamPaperAnswer.ExamPaperId)}", $"{nameof(ExamPaperAnswer.ExamPaperId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamPaperAnswer.ExamStartId)}", $"{nameof(ExamPaperAnswer.ExamStartId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamPaperAnswer.RandomTmId)}", $"{nameof(ExamPaperAnswer.RandomTmId)}");
                }
                else if (tableName == ExamPaperRandomRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamPaperRandom.ExamPaperId)}", $"{nameof(ExamPaperRandom.ExamPaperId)}");
                }
                else if (tableName == ExamPaperRandomConfigRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamPaperRandomConfig.ExamPaperId)}", $"{nameof(ExamPaperRandomConfig.ExamPaperId)}");
                }
                else if (tableName == ExamPaperRandomTmRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamPaperRandomTm.ExamPaperId)}", $"{nameof(ExamPaperRandomTm.ExamPaperId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamPaperRandomTm.TxId)}", $"{nameof(ExamPaperRandomTm.TxId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamPaperRandomTm.SourceTmId)}", $"{nameof(ExamPaperRandomTm.SourceTmId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamPaperRandomTm.ExamPaperRandomId)}", $"{nameof(ExamPaperRandomTm.ExamPaperRandomId)}");
                }
                else if (tableName == ExamPaperStartRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamPaperStart.ExamPaperId)}", $"{nameof(ExamPaperStart.ExamPaperId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamPaperStart.ExamPaperRandomId)}", $"{nameof(ExamPaperStart.ExamPaperRandomId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamPaperStart.UserId)}", $"{nameof(ExamPaperStart.UserId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamPaperStart.PlanId)}", $"{nameof(ExamPaperStart.PlanId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamPaperStart.CourseId)}", $"{nameof(ExamPaperStart.CourseId)}");
                }
                else if (tableName == ExamPaperUserRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamPaperUser.UserId)}", $"{nameof(ExamPaperUser.UserId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamPaperUser.ExamPaperId)}", $"{nameof(ExamPaperUser.ExamPaperId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamPaperUser.PlanId)}", $"{nameof(ExamPaperUser.PlanId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamPaperUser.CourseId)}", $"{nameof(ExamPaperUser.CourseId)}");
                }
                else if (tableName == ExamPracticeRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamPractice.UserId)}", $"{nameof(ExamPractice.UserId)}");
                }
                else if (tableName == ExamPracticeAnswerRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamPracticeAnswer.UserId)}", $"{nameof(ExamPracticeAnswer.UserId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamPracticeAnswer.TmId)}", $"{nameof(ExamPracticeAnswer.TmId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamPracticeAnswer.PracticeId)}", $"{nameof(ExamPracticeAnswer.PracticeId)}");
                }
                else if (tableName == ExamPracticeCollectRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamPracticeCollect.UserId)}", $"{nameof(ExamPracticeCollect.UserId)}");
                }
                else if (tableName == ExamPracticeWrongRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamPracticeWrong.UserId)}", $"{nameof(ExamPracticeWrong.UserId)}");
                }
                else if (tableName == ExamQuestionnaireTmRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamQuestionnaireTm.ExamPaperId)}", $"{nameof(ExamQuestionnaireTm.ExamPaperId)}");
                }
                else if (tableName == ExamQuestionnaireUserRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamQuestionnaireUser.ExamPaperId)}", $"{nameof(ExamQuestionnaireUser.ExamPaperId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamQuestionnaireUser.UserId)}", $"{nameof(ExamQuestionnaireUser.UserId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamQuestionnaireUser.PlanId)}", $"{nameof(ExamQuestionnaireUser.PlanId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamQuestionnaireUser.CourseId)}", $"{nameof(ExamQuestionnaireUser.CourseId)}");
                }
                else if (tableName == ExamQuestionnaireAnswerRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamQuestionnaireAnswer.ExamPaperId)}", $"{nameof(ExamQuestionnaireAnswer.ExamPaperId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamQuestionnaireAnswer.UserId)}", $"{nameof(ExamQuestionnaireAnswer.UserId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamQuestionnaireAnswer.TmId)}", $"{nameof(ExamQuestionnaireAnswer.TmId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamQuestionnaireAnswer.PlanId)}", $"{nameof(ExamQuestionnaireAnswer.PlanId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamQuestionnaireAnswer.CourseId)}", $"{nameof(ExamQuestionnaireAnswer.CourseId)}");
                }
                else if (tableName == OrganCompanyRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(OrganCompany.ParentId)}", $"{nameof(OrganCompany.ParentId)}");
                }
                else if (tableName == OrganDepartmentRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(OrganDepartment.ParentId)}", $"{nameof(OrganDepartment.ParentId)}");
                }
                else if (tableName == UserRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(User.UserName)}", $"{nameof(User.UserName)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(User.UserGroupIds)}", $"{nameof(User.UserGroupIds)}");
                }
                else if (tableName == AdministratorRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(Administrator.UserName)}", $"{nameof(Administrator.UserName)}");
                }
                else if (tableName == StatLogRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(StatLog.AdminId)}", $"{nameof(StatLog.AdminId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(StatLog.StatType)}", $"{nameof(StatLog.StatType)}");
                }
                else if (tableName == StatRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(Stat.StatType)}", $"{nameof(Stat.StatType)}");
                }
                else if (tableName == ExamTmAnalysisTmRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamTmAnalysisTm.AnalysisId)}", $"{nameof(ExamTmAnalysisTm.AnalysisId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamTmAnalysisTm.TmId)}", $"{nameof(ExamTmAnalysisTm.TmId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamTmAnalysisTm.TxId)}", $"{nameof(ExamTmAnalysisTm.TxId)}");
                }
                else if (tableName == ExamTmRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamTm.TreeParentPath)}", $"{nameof(ExamTm.TreeParentPath)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamTm.Zhishidian)}", $"{nameof(ExamTm.Zhishidian)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamTm.Nandu)}", $"{nameof(ExamTm.Nandu)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamTm.TxId)}", $"{nameof(ExamTm.TxId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamTm.TreeId)}", $"{nameof(ExamTm.TreeId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamTm.TmGroupIds)}", $"{nameof(ExamTm.TmGroupIds)}");
                }
                else if (tableName == ExamTmCorrectionRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamTmCorrection.AuditAdminId)}", $"{nameof(ExamTmCorrection.AuditAdminId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamTmCorrection.TmId)}", $"{nameof(ExamTmCorrection.TmId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamTmCorrection.ExamPaperId)}", $"{nameof(ExamTmCorrection.ExamPaperId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(ExamTmCorrection.UserId)}", $"{nameof(ExamTmCorrection.UserId)}");
                }
                else if (tableName == StudyCourseFilesRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(StudyCourseFiles.GroupId)}", $"{nameof(StudyCourseFiles.GroupId)}");
                }
                else if (tableName == StudyCourseFilesGroupRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(StudyCourseFilesGroup.ParentId)}", $"{nameof(StudyCourseFilesGroup.ParentId)}");
                }
                else if (tableName == StudyCourseEvaluationItemRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(StudyCourseEvaluationItem.EvaluationId)}", $"{nameof(StudyCourseEvaluationItem.EvaluationId)}");
                }
                else if (tableName == StudyCourseEvaluationItemUserRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(StudyCourseEvaluationItemUser.EvaluationId)}", $"{nameof(StudyCourseEvaluationItemUser.EvaluationId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(StudyCourseEvaluationItemUser.UserId)}", $"{nameof(StudyCourseEvaluationItemUser.UserId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(StudyCourseEvaluationItemUser.PlanId)}", $"{nameof(StudyCourseEvaluationItemUser.PlanId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(StudyCourseEvaluationItemUser.CourseId)}", $"{nameof(StudyCourseEvaluationItemUser.CourseId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(StudyCourseEvaluationItemUser.EvaluationItemId)}", $"{nameof(StudyCourseEvaluationItemUser.EvaluationItemId)}");
                }
                else if (tableName == StudyCourseEvaluationUserRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(StudyCourseEvaluationUser.PlanId)}", $"{nameof(StudyCourseEvaluationUser.PlanId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(StudyCourseEvaluationUser.CourseId)}", $"{nameof(StudyCourseEvaluationUser.CourseId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(StudyCourseEvaluationUser.UserId)}", $"{nameof(StudyCourseEvaluationUser.UserId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(StudyCourseEvaluationUser.EvaluationId)}", $"{nameof(StudyCourseEvaluationUser.EvaluationId)}");
                }
                else if (tableName == StudyCourseUserRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(StudyCourseUser.PlanId)}", $"{nameof(StudyCourseUser.PlanId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(StudyCourseUser.CourseId)}", $"{nameof(StudyCourseUser.CourseId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(StudyCourseUser.UserId)}", $"{nameof(StudyCourseUser.UserId)}");
                }
                else if (tableName == StudyPlanUserRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(StudyPlanUser.PlanId)}", $"{nameof(StudyPlanUser.PlanId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(StudyPlanUser.UserId)}", $"{nameof(StudyPlanUser.UserId)}");
                }
                else if (tableName == StudyCourseWareRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(StudyCourseWare.CourseId)}", $"{nameof(StudyCourseWare.CourseId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(StudyCourseWare.CourseFileId)}", $"{nameof(StudyCourseWare.CourseFileId)}");
                }
                else if (tableName == StudyCourseWareUserRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(StudyCourseWareUser.PlanId)}", $"{nameof(StudyCourseWareUser.PlanId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(StudyCourseWareUser.CourseId)}", $"{nameof(StudyCourseWareUser.CourseId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(StudyCourseWareUser.UserId)}", $"{nameof(StudyCourseWareUser.UserId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(StudyCourseWareUser.CourseWareId)}", $"{nameof(StudyCourseWareUser.CourseWareId)}");
                }
                else if (tableName == StudyCourseRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(StudyCourse.StudyCourseEvaluationId)}", $"{nameof(StudyCourse.StudyCourseEvaluationId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(StudyCourse.ExamId)}", $"{nameof(StudyCourse.ExamId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(StudyCourse.ExamQuestionnaireId)}", $"{nameof(StudyCourse.ExamQuestionnaireId)}");
                }
                else if (tableName == StudyPlanRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(StudyPlan.ExamId)}", $"{nameof(StudyPlan.ExamId)}");
                }
                else if (tableName == PointLogRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(PointLog.UserId)}", $"{nameof(PointLog.UserId)}");
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_{nameof(PointLog.PointType)}", $"{nameof(PointLog.PointType)}");
                }
            }
            catch (Exception ex)
            {
                await ErrorLogRepository.AddErrorLogAsync(ex, string.Empty);
                return (false, ex);
            }

            return (true, null);
        }

        private async Task AlterTableAsync(string tableName, IList<TableColumn> tableColumns, IList<string> dropColumnNames = null)
        {
            try
            {
                await _settingsManager.Database.AlterTableAsync(tableName,
                    GetRealTableColumns(tableColumns), dropColumnNames);
            }
            catch (Exception ex)
            {
                await ErrorLogRepository.AddErrorLogAsync(ex, string.Empty);
            }
        }


        private IList<TableColumn> GetRealTableColumns(IEnumerable<TableColumn> tableColumns)
        {
            var realTableColumns = new List<TableColumn>();
            foreach (var tableColumn in tableColumns)
            {
                if (string.IsNullOrEmpty(tableColumn.AttributeName) ||
                StringUtils.EqualsIgnoreCase(tableColumn.AttributeName, nameof(Entity.Id)) ||
                StringUtils.EqualsIgnoreCase(tableColumn.AttributeName, nameof(Entity.Guid)) ||
                StringUtils.EqualsIgnoreCase(tableColumn.AttributeName, "ExtendValues") ||
                StringUtils.EqualsIgnoreCase(tableColumn.AttributeName, nameof(Entity.CreatedDate)) ||
                StringUtils.EqualsIgnoreCase(tableColumn.AttributeName, nameof(Entity.LastModifiedDate)))
                {
                    continue;
                }

                if (tableColumn.DataType == DataType.VarChar && tableColumn.DataLength == 0)
                {
                    tableColumn.DataLength = 2000;
                }
                realTableColumns.Add(tableColumn);
            }

            realTableColumns.InsertRange(0, new List<TableColumn>
            {
                new TableColumn
                {
                    AttributeName = nameof(Entity.Id),
                    DataType = DataType.Integer,
                    IsIdentity = true,
                    IsPrimaryKey = true
                },
                new TableColumn
                {
                    AttributeName = nameof(Entity.Guid),
                    DataType = DataType.VarChar,
                    DataLength = 50
                },
                new TableColumn
                {
                    AttributeName = "ExtendValues",
                    DataType = DataType.Text,
                },
                new TableColumn
                {
                    AttributeName = nameof(Entity.CreatedDate),
                    DataType = DataType.DateTime
                },
                new TableColumn
                {
                    AttributeName = nameof(Entity.LastModifiedDate),
                    DataType = DataType.DateTime
                }
            });

            return realTableColumns;
        }
    }
}
