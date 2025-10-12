using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Core.Utils.Office
{
    public class ExcelObject
    {
        private readonly IDatabaseManager _databaseManager;
        private readonly IPathManager _pathManager;
        private readonly IOrganManager _organManager;
        private readonly IExamManager _examManager;

        public ExcelObject(IDatabaseManager databaseManager, IPathManager pathManager, IOrganManager organManager, IExamManager examManager)
        {
            _databaseManager = databaseManager;
            _pathManager = pathManager;
            _organManager = organManager;
            _examManager = examManager;
        }

        public async Task CreateExcelFileForUsersAsync(AdminAuth auth, int organId, string organType, UserGroup group, int dayOfLastActivity, string keyword, string order, string filePath)
        {
            DirectoryUtils.CreateDirectoryIfNotExists(DirectoryUtils.GetDirectoryPath(filePath));
            FileUtils.DeleteFileIfExists(filePath);

            var head = new List<string>
            {
                "组织",
                "用户名",
                "姓名",
                "岗位",
                "邮箱",
                "手机",
                "注册时间",
                "最后一次活动时间",
                "登录次数"
            };

            var rows = new List<List<string>>();
            var (total, userList) = await _databaseManager.UserRepository.GetListAsync(auth, organId, organType, group, dayOfLastActivity, keyword, order, 0, int.MaxValue);
            foreach (var userInfo in userList)
            {
                await _organManager.GetUser(userInfo);
                rows.Add(
                [
                    userInfo.Get("OrganNames").ToString(),
                    userInfo.UserName,
                    userInfo.DisplayName,
                    userInfo.DutyName,
                    userInfo.Email,
                    userInfo.Mobile,
                    DateUtils.GetDateAndTimeString(userInfo.CreatedDate),
                    DateUtils.GetDateAndTimeString(userInfo.LastActivityDate),
                    userInfo.CountOfLogin.ToString()
                ]);
            }

            ExcelUtils.Write(filePath, head, rows);
        }


        public async Task CreateExcelFileForTmAsync(List<ExamTm> tmList, string filePath)
        {
            DirectoryUtils.CreateDirectoryIfNotExists(DirectoryUtils.GetDirectoryPath(filePath));
            FileUtils.DeleteFileIfExists(filePath);

            var head = new List<string>
            {
                "所属分类",
                "题型",
                "难度",
                "知识点",
                "分数",
                "解析",
                "创建时间",
                "上次修改时间",
                "题目内容",
                "答案",
                "候选项"
            };


            var rows = new List<List<string>>();

            foreach (var tm in tmList)
            {
                await _examManager.GetTmInfo(tm);
                var tx = await _databaseManager.ExamTxRepository.GetAsync(tm.TxId);

                if (tx.ExamTxBase == ExamTxBase.Zuheti)
                {
                    var smallList = await _databaseManager.ExamTmSmallRepository.GetListAsync(tm.Id);
                    var excelList = new List<string>
                    {
                        tm.Get("TreeName").ToString(),
                        tm.Get("TxName").ToString(),
                        tm.Nandu.ToString(),
                        tm.Zhishidian,
                        tm.Score.ToString(),
                        tm.Jiexi,
                        DateUtils.GetDateAndTimeString(tm.CreatedDate),
                        DateUtils.GetDateAndTimeString(tm.LastModifiedDate),
                        tm.Get("TitleHtml").ToString(),
                        (smallList?.Count ?? 0).ToString()
                    };
                    rows.Add(excelList);
                    if (smallList != null && smallList.Count > 0)
                    {
                        foreach (var small in smallList)
                        {
                            var smallTx = await _databaseManager.ExamTxRepository.GetAsync(small.TxId);
                            var smallexcelList = new List<string>
                            {
                                tm.Get("TreeName").ToString(),
                                smallTx.Name,
                                small.Nandu.ToString(),
                                small.Zhishidian,
                                small.Score.ToString(),
                                small.Jiexi,
                                DateUtils.GetDateAndTimeString(tm.CreatedDate),
                                DateUtils.GetDateAndTimeString(tm.LastModifiedDate),
                                small.Title,
                                small.Answer
                            };
                            var options = ListUtils.ToList(small.Get("options"));
                            if (options != null)
                            {
                                if (options.Count > 0)
                                {
                                    foreach (var option in options)
                                    {
                                        if (!string.IsNullOrWhiteSpace(option))
                                        {
                                            smallexcelList.Add(option);
                                        }
                                    }
                                }
                            }
                            rows.Add(smallexcelList);
                        }
                    }
                }
                else
                {
                    var excelList = new List<string>
                    {
                        tm.Get("TreeName").ToString(),
                        tm.Get("TxName").ToString(),
                        tm.Nandu.ToString(),
                        tm.Zhishidian,
                        tm.Score.ToString(),
                        tm.Jiexi,
                        DateUtils.GetDateAndTimeString(tm.CreatedDate),
                        DateUtils.GetDateAndTimeString(tm.LastModifiedDate),
                        tm.Get("TitleHtml").ToString(),
                        tm.Answer
                    };

                    var options = ListUtils.ToList(tm.Get("options"));
                    if (options != null)
                    {
                        if (options.Count > 0)
                        {
                            foreach (var option in options)
                            {
                                if (!string.IsNullOrWhiteSpace(option))
                                {
                                    excelList.Add(option);
                                }
                            }
                        }
                    }
                    rows.Add(excelList);
                }
            }
            ExcelUtils.Write(filePath, head, rows);
        }
    }
}
