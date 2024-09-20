using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Datory;
using Datory.Annotations;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Newtonsoft.Json.Converters;
using NPOI.POIFS.Properties;
using NPOI.SS.Formula.Functions;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;
using XBLMS.Utils;

namespace XBLMS.Core.Services
{
    public partial class ExamManager
    {
        public async Task ClearQuestionnaire(int examPaperId)
        {
            await _examQuestionnaireAnswerRepository.ClearByPaperAsync(examPaperId);
            await _examQuestionnaireTmRepository.DeleteByPaperAsync(examPaperId);
            await _examQuestionnaireUserRepository.ClearByPaperAsync(examPaperId);
        }

        public async Task SetQuestionnairTm(List<ExamQuestionnaireTm> tmList, int paperId)
        {
            if (tmList != null && tmList.Count > 0)
            {
                foreach (var tm in tmList)
                {
                    tm.ExamPaperId = paperId;
                    await _examQuestionnaireTmRepository.InsertAsync(tm);
                }
            }
        }

        public async Task ArrangeQuestionnaire(ExamQuestionnaire paper)
        {
            if (paper.Published)
            {
                return;
            }
            var userIds = new List<int>();

            if (paper.UserGroupIds != null && paper.UserGroupIds.Count > 0)
            {
                foreach (int groupId in paper.UserGroupIds)
                {
                    var group = await _userGroupRepository.GetAsync(groupId);
                    if (group.GroupType == UsersGroupType.Fixed)
                    {
                        userIds = group.UserIds;
                    }
                    if (group.GroupType == Enums.UsersGroupType.Range)
                    {
                        userIds = await _userRepository.GetUserIdsWithOutLockedAsync(group.CompanyIds, group.DepartmentIds, group.DutyIds);
                    }
                    if (group.GroupType == UsersGroupType.All)
                    {
                        userIds = await _userRepository.GetUserIdsWithOutLockedAsync();
                    }
                }
            }

            if (userIds.Count > 0)
            {
                foreach (int userId in userIds)
                {
                    var exist = await _examQuestionnaireUserRepository.ExistsAsync(paper.Id, userId);
                    if (!exist)
                    {
                        await _examQuestionnaireUserRepository.InsertAsync(new ExamQuestionnaireUser
                        {
                            ExamPaperId = paper.Id,
                            UserId = userId,
                        });
                    }
                }
            }
        }
    }
}
