using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Core.Services
{
    public partial class ExamManager
    {
        public async Task ClearQuestionnaire(int examPaperId)
        {
            await _databaseManager.ExamQuestionnaireAnswerRepository.ClearByPaperAsync(examPaperId);
            await _databaseManager.ExamQuestionnaireTmRepository.DeleteByPaperAsync(examPaperId);
            await _databaseManager.ExamQuestionnaireUserRepository.ClearByPaperAsync(examPaperId);
        }

        public async Task SetQuestionnairTm(List<ExamQuestionnaireTm> tmList, int paperId)
        {
            if (tmList != null && tmList.Count > 0)
            {
                var parentId = 0;
                foreach (var tm in tmList)
                {
                    tm.Remove("TmIndex");
                    if (tm.ParentId > 0)
                    {
                        tm.Remove("Answer");
                        tm.Remove("Answers");
                        tm.ParentId = parentId;
                        tm.ExamPaperId = paperId;
                        var tmId = await _databaseManager.ExamQuestionnaireTmRepository.InsertAsync(tm);
                    }
                    else
                    {
                        tm.Remove("SmallList");
                        tm.ExamPaperId = paperId;
                        var tmId = await _databaseManager.ExamQuestionnaireTmRepository.InsertAsync(tm);
                        if (tm.Tx == ExamQuestionnaireTxType.DanxuantiErwei || tm.Tx == ExamQuestionnaireTxType.DuoxuantiErwei || tm.Tx == ExamQuestionnaireTxType.DanxuantiSanwei || tm.Tx == ExamQuestionnaireTxType.DuoxuantiSanwei)
                        {
                            parentId = tmId;
                        }
                        else
                        {
                            parentId = 0;
                        }
                    }
                }
            }
        }
    }
}
