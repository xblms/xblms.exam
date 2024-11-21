using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Users
{
    public partial class UsersController
    {
        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(RouteImportCheck)]
        public async Task<ActionResult<ImportCheckResult>> ImportCheck([FromForm] IFormFile file)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Import))
            {
                return this.NoAuth();
            }
            if (file == null)
            {
                return this.Error(Constants.ErrorUpload);
            }

            var fileName = _pathManager.GetUploadFileName(file.FileName);

            var sExt = PathUtils.GetExtension(fileName);
            if (!StringUtils.EqualsIgnoreCase(sExt, ".xlsx"))
            {
                return this.Error("导入文件为xlsx格式，请选择有效的文件上传");
            }

            var filePath = _pathManager.GetImportFilesPath(fileName);
            await _pathManager.UploadAsync(file, filePath);

            var msgList = new List<KeyValuePair<int, string>>();

            var success = 0;
            var failure = 0;

            var sheet = ExcelUtils.Read(filePath);
            if (sheet != null)
            {
                for (var i = 1; i < sheet.Rows.Count; i++) //行
                {
                    if (i == 1) continue;

                    var row = sheet.Rows[i];

                    var companyName = row[0].ToString().Trim();
                    var departmentName = row[1].ToString().Trim();
                    var dutyName = row[2].ToString().Trim();

                    if (!string.IsNullOrEmpty(companyName))
                    {
                        var company = await _organManager.GetCompanyAsync(companyName);
                        if (company == null)
                        {
                            msgList.Add(new KeyValuePair<int, string>(i, $"【{companyName}】单位不存在"));
                            failure++;
                            continue;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(departmentName))
                            {
                                var department = await _organManager.GetDepartmentAsync(company.Id, departmentName);
                                if (department == null)
                                {
                                    msgList.Add(new KeyValuePair<int, string>(i, $"【{companyName}】单位下没有【{departmentName}】部门"));
                                    failure++;
                                    continue;
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(dutyName))
                                    {
                                        var duty = await _organManager.GetDutyAsync(company.Id, department.Id, dutyName);
                                        if (duty == null)
                                        {
                                            msgList.Add(new KeyValuePair<int, string>(i, $"【{companyName}】单位下【{departmentName}】部门下没有【{dutyName}】岗位"));
                                            failure++;
                                            continue;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        msgList.Add(new KeyValuePair<int, string>(i, "单位不能为空"));
                        failure++;
                        continue;
                    }


                    var userName = row[3].ToString().Trim();
                    var password = row[4].ToString().Trim();
                    var displayName = row[5].ToString().Trim();
                    var mobile = row[6].ToString().Trim();
                    var email = row[7].ToString().Trim();

                    var (validSuccess, validMsg) = await _userRepository.ValidateAsync(userName, email, mobile, password);
                    if (!validSuccess)
                    {
                        msgList.Add(new KeyValuePair<int, string>(i, validMsg));
                        failure++;
                        continue;
                    }

                    if (string.IsNullOrEmpty(displayName))
                    {
                        msgList.Add(new KeyValuePair<int, string>(i, "姓名不能为空"));
                        failure++;
                        continue;
                    }

                    success++;

                }
            }
            var resultValue = msgList == null || msgList.Count == 0;
            var resultFilePath = TranslateUtils.EncryptStringBySecretKey(filePath, "userimport");
            return new ImportCheckResult
            {
                Value = resultValue,
                Msgs = msgList,
                FilePath = resultFilePath,
                Success = success,
                Failure = failure,
            };
        }



        [HttpPost, Route(RouteImport)]
        public async Task<ActionResult<ImportResult>> Import([FromBody] ImportRequest request)
        {
            var adminId = _authManager.AdminId;
            var filePath = TranslateUtils.DecryptStringBySecretKey(request.FilePath, "userimport");
            var sheet = ExcelUtils.Read(filePath);

            var success = 0;
            var failure = 0;
            if (sheet != null)
            {
                for (var i = 1; i < sheet.Rows.Count; i++) //行
                {
                    if (i == 1) continue;
                    if (request.RowNumber != null && request.RowNumber.Contains(i))
                    {
                        failure++;
                        continue;
                    }

                    var row = sheet.Rows[i];

                    var companName = row[0].ToString().Trim();
                    var departmentName = row[1].ToString().Trim();
                    var dutyName = row[2].ToString().Trim();

                    var companyId = 0;
                    var departmentId = 0;
                    var dutyId = 0;

                    var company = await _organManager.GetCompanyAsync(companName);
                    companyId = company.Id;
                    if (!string.IsNullOrEmpty(departmentName))
                    {
                        var department = await _organManager.GetDepartmentAsync(company.Id, departmentName);
                        departmentId = department.Id;
                        if (!string.IsNullOrEmpty(dutyName))
                        {
                            var duty = await _organManager.GetDutyAsync(company.Id, department.Id, dutyName);
                            dutyId = duty.Id;
                        }
                    }
              
                    var userName = row[3].ToString().Trim();
                    var password = row[4].ToString().Trim();
                    var displayName = row[5].ToString().Trim();
                    var mobile = row[6].ToString().Trim();
                    var email = row[7].ToString().Trim();

                    var (user, message) = await _userRepository.InsertAsync(new User
                    {
                        UserName = userName,
                        DisplayName = displayName,
                        Mobile = mobile,
                        Email = email,
                        CompanyId = companyId,
                        DepartmentId = departmentId,
                        DutyId = dutyId,
                        CreatorId = adminId
                    }, password, true, string.Empty);

                    success++;
                }
            }
            FileUtils.DeleteFileIfExists(filePath);
            return new ImportResult
            {
                Success = success,
                Failure = failure
            };
        }
    }
}
