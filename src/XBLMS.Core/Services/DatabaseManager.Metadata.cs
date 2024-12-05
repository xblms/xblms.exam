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

                var administrator = new Administrator
                {
                    CompanyId = 1,
                    Auth = AuthorityType.Admin,
                    UserName = userName,
                    DisplayName = "超级管理员",
                    Email = email,
                    Mobile = mobile
                };

                await AdministratorRepository.InsertAsync(administrator, password);

                var company = new OrganCompany
                {
                    Name = companyName,
                    ParentId = 0
                };
                await OrganCompanyRepository.InsertAsync(company);


                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
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
                if (repository.TableName == ConfigRepository.TableName) continue;

                if (repository.TableName == OrganCompanyRepository.TableName)
                {
                    //await OrganCompanyRepository.ClearAsync();
                    continue;
                }
                if (repository.TableName == OrganDepartmentRepository.TableName)
                {
                    continue;
                }
                if (repository.TableName == OrganDutyRepository.TableName)
                {
                    continue;
                }
                if (repository.TableName == AdministratorRepository.TableName)
                {
                    //await AdministratorRepository.ClearAsync();
                    continue;
                }
                if (repository.TableName == UserRepository.TableName)
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

                if (tableName == ExamCerUserRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_General", $"{nameof(ExamCerUser.UserId)} DESC", $"{nameof(ExamCerUser.CerId)} DESC");
                }
                else if (tableName == ExamPaperAnswerRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_General", $"{nameof(ExamPaperAnswer.UserId)} DESC", $"{nameof(ExamPaperAnswer.ExamPaperId)} DESC", $"{nameof(ExamPaperAnswer.ExamStartId)} DESC", $"{nameof(ExamPaperAnswer.RandomTmId)} DESC");
                }
                else if (tableName == ExamPaperRandomRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_General", $"{nameof(ExamPaperRandom.ExamPaperId)} DESC");
                }
                else if (tableName == ExamPaperRandomConfigRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_General", $"{nameof(ExamPaperRandomConfig.ExamPaperId)} DESC");
                }
                else if (tableName == ExamPaperRandomTmRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_General", $"{nameof(ExamPaperRandomTm.ExamPaperId)} DESC", $"{nameof(ExamPaperRandomTm.TxId)} DESC", $"{nameof(ExamPaperRandomTm.SourceTmId)} DESC", $"{nameof(ExamPaperRandomTm.ExamPaperRandomId)} DESC");
                }
                else if (tableName == ExamPaperStartRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_General", $"{nameof(ExamPaperStart.ExamPaperId)} DESC", $"{nameof(ExamPaperStart.ExamPaperRandomId)} DESC", $"{nameof(ExamPaperStart.UserId)} DESC", $"{nameof(ExamPaperStart.MarkTeacherId)} DESC");
                }
                else if (tableName == ExamPaperUserRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_General", $"{nameof(ExamPaperUser.ExamPaperId)} DESC", $"{nameof(ExamPaperUser.UserId)} DESC");
                }
                else if (tableName == ExamPracticeRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_General", $"{nameof(ExamPractice.UserId)} DESC");
                }
                else if (tableName == ExamPracticeAnswerRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_General", $"{nameof(ExamPracticeAnswer.UserId)} DESC", $"{nameof(ExamPracticeAnswer.PracticeId)} DESC", $"{nameof(ExamPracticeAnswer.TmId)} DESC");
                }
                else if (tableName == ExamPracticeCollectRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_General", $"{nameof(ExamPracticeCollect.UserId)} DESC");
                }
                else if (tableName == ExamPracticeWrongRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_General", $"{nameof(ExamPracticeWrong.UserId)} DESC");
                }
                else if (tableName == ExamQuestionnaireTmRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_General", $"{nameof(ExamQuestionnaireTm.ExamPaperId)} DESC");
                }
                else if (tableName == ExamQuestionnaireUserRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_General", $"{nameof(ExamQuestionnaireUser.ExamPaperId)} DESC", $"{nameof(ExamQuestionnaireUser.UserId)} DESC");
                }
                else if (tableName == ExamQuestionnaireAnswerRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_General", $"{nameof(ExamQuestionnaireAnswer.ExamPaperId)} DESC", $"{nameof(ExamQuestionnaireAnswer.UserId)} DESC", $"{nameof(ExamQuestionnaireAnswer.TmId)} DESC");
                }
                else if (tableName == OrganDepartmentRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_General", $"{nameof(OrganDepartment.CompanyId)} DESC");
                }
                else if (tableName == OrganDutyRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_General", $"{nameof(OrganDuty.CompanyId)} DESC", $"{nameof(OrganDuty.DepartmentId)} DESC");
                }
                else if (tableName == UserRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_General", $"{nameof(User.CompanyId)} DESC", $"{nameof(User.DepartmentId)} DESC", $"{nameof(User.DutyId)} DESC");
                }
                else if (tableName == StatLogRepository.TableName)
                {
                    await _settingsManager.Database.CreateIndexAsync(tableName, $"IX_{tableName}_General", $"{nameof(StatLog.AdminId)} DESC", $"{nameof(StatLog.ObjectId)} DESC");
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
