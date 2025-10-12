using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin
{
    public partial class SyncDatabaseController
    {
        [HttpPost, Route(Route)]
        public async Task<ActionResult<SubmitResult>> Submit([FromBody] SubmitRequest request)
        {
            var config = await _configRepository.GetAsync();

            var oldVersion = config.DatabaseVersion;

            if (config.DatabaseVersion == _settingsManager.Version)
            {
                if (_settingsManager.SecurityKey != request.SecurityKey)
                {
                    return this.Error("SecurityKey 输入错误！");
                }
            }

            await _databaseManager.SyncDatabaseAsync();

            if (oldVersion != _settingsManager.Version)
            {
                #region 8.2 升级差异
                if (_settingsManager.Version == "8.2")
                {
                    var repositories = _databaseManager.GetAllRepositories();
                    foreach (var repository in repositories)
                    {
                        if (string.IsNullOrEmpty(repository.TableName) || repository.TableColumns == null || repository.TableColumns.Count <= 0) continue;

                        if (await _settingsManager.Database.IsTableExistsAsync(repository.TableName))
                        {
                            try
                            {
                                if (repository.TableName == _databaseManager.AdministratorRepository.TableName)
                                {
                                    var admins = await _databaseManager.AdministratorRepository.GetAllAsync();
                                    if (admins != null && admins.Count > 0)
                                    {
                                        foreach (var admin in admins)
                                        {
                                            admin.AuthDataShowAll = true;
                                            admin.AuthDataCurrentOrganId = 1;

                                            await _databaseManager.AdministratorRepository.UpdateAuthDataShowAllAsync(admin);
                                            await _databaseManager.AdministratorRepository.UpdateCurrentOragnAsync(admin);
                                            await _databaseManager.AdministratorRepository.UpdateAuthAsync(admin);
                                            await _databaseManager.AdministratorRepository.UpdateAuthDataAsync(admin);
                                        }
                                    }
                                }
                                if (repository.TableName == _databaseManager.ExamPaperTreeRepository.TableName)
                                {
                                    var allTree = await _databaseManager.ExamPaperTreeRepository.GetAllAsync();
                                    foreach (var tree in allTree)
                                    {
                                        tree.ParentPath = await _databaseManager.ExamPaperTreeRepository.GetParentPathAsync(tree.Id);
                                        await _databaseManager.ExamPaperTreeRepository.UpdateAsync(tree);
                                    }
                                }
                                if (repository.TableName == _databaseManager.ExamPaperRepository.TableName)
                                {
                                    var allPaper = await _databaseManager.ExamPaperRepository.GetAllAsync();
                                    foreach (var paper in allPaper)
                                    {
                                        var tree = await _databaseManager.ExamPaperTreeRepository.GetAsync(paper.TreeId);
                                        if (tree != null)
                                        {
                                            paper.TreeParentPath = tree.ParentPath;
                                            await _databaseManager.ExamPaperRepository.UpdateAsync(paper);
                                        }
                                    }
                                }
                                if (repository.TableName == _databaseManager.ExamTmTreeRepository.TableName)
                                {
                                    var allTree = await _databaseManager.ExamTmTreeRepository.GetAllAsync();
                                    foreach (var tree in allTree)
                                    {
                                        tree.ParentPath = await _databaseManager.ExamTmTreeRepository.GetParentPathAsync(tree.Id);
                                        await _databaseManager.ExamTmTreeRepository.UpdateAsync(tree);
                                    }
                                }
                                if (repository.TableName == _databaseManager.ExamTmRepository.TableName)
                                {
                                    var allTm = await _databaseManager.ExamTmRepository.GetAllAsync();
                                    foreach (var tm in allTm)
                                    {
                                        var tree = await _databaseManager.ExamTmTreeRepository.GetAsync(tm.TreeId);
                                        if (tree != null)
                                        {
                                            tm.TreeParentPath = tree.ParentPath;
                                            await _databaseManager.ExamTmRepository.UpdateAsync(tm);
                                        }
                                    }
                                }
                            }
                            catch { continue; }

                            try
                            {
                                await _databaseManager.ExecuteAsync($"UPDATE {repository.TableName} SET CompanyId=1,CompanyParentPath='''1'''");
                            }
                            catch { continue; }
                        }
                    }
                }
                #endregion
            }

            return new SubmitResult
            {
                Version = _settingsManager.Version
            };
        }
    }
}
