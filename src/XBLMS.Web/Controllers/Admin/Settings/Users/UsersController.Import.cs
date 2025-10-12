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
                var mobileList = new List<string>();
                var emailList = new List<string>();
                var userNameList = new List<string>();

                for (var i = 1; i < sheet.Rows.Count; i++) //行
                {
                    if (i == 1) continue;

                    var row = sheet.Rows[i];

                    var companyName = row[0].ToString().Trim();
                    var departmentName = row[1].ToString().Trim();

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
                    if (userNameList.Contains(userName))
                    {
                        msgList.Add(new KeyValuePair<int, string>(i, "用户名已存在"));
                        failure++;
                        continue;
                    }

                    if (!string.IsNullOrEmpty(mobile) && mobileList.Contains(mobile))
                    {
                        msgList.Add(new KeyValuePair<int, string>(i, "手机号码已存在"));
                        failure++;
                        continue;
                    }
                    if (!string.IsNullOrEmpty(email) && emailList.Contains(email))
                    {
                        msgList.Add(new KeyValuePair<int, string>(i, "电子邮箱已存在"));
                        failure++;
                        continue;
                    }


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

                    userNameList.Add(userName);
                    mobileList.Add(mobile);
                    emailList.Add(email);

                    success++;

                }
            }
            var resultValue = msgList == null || msgList.Count == 0;
            var resultFilePath = DesEncryptor.EncryptStringBySecretKey(filePath, "userimport");
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
            var filePath = DesEncryptor.DecryptStringBySecretKey(request.FilePath, "userimport");
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

                    var company = await _organManager.GetCompanyAsync(companName);
                    companyId = company.Id;

                    var department = new OrganDepartment();
                    if (!string.IsNullOrEmpty(departmentName))
                    {
                        department = await _organManager.GetDepartmentAsync(company.Id, departmentName);
                        if (department != null && department.Id > 0)
                        {
                            departmentId = department.Id;
                        }

                    }

                    var userName = row[3].ToString().Trim();
                    var password = row[4].ToString().Trim();
                    var displayName = row[5].ToString().Trim();
                    var mobile = row[6].ToString().Trim();
                    var email = row[7].ToString().Trim();

                    var insertUser = new User
                    {
                        UserName = userName,
                        DisplayName = displayName,
                        Mobile = mobile,
                        Email = email,
                        CompanyId = companyId,
                        DepartmentId = departmentId,
                        DutyName = dutyName,
                        CreatorId = adminId
                    };
                    if (insertUser.CompanyId > 0)
                    {
                        insertUser.CompanyParentPath = company.CompanyParentPath;
                    }
                    if (insertUser.DepartmentId > 0)
                    {
                        insertUser.DepartmentParentPath = department.DepartmentParentPath;
                    }

                    var (user, message) = await _userRepository.InsertAsync(insertUser, password, true, string.Empty);

                    if (user != null)
                    {
                        await _authManager.AddAdminLogAsync("新增用户账号-导入", $"{userName}");
                        await _authManager.AddStatLogAsync(StatType.UserAdd, "新增用户账号", user.Id, user.DisplayName);
                        await _authManager.AddStatCount(StatType.UserAdd);
                        success++;
                    }


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
