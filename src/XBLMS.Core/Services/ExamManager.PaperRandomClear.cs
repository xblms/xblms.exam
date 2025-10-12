using System.Threading.Tasks;

namespace XBLMS.Core.Services
{
    public partial class ExamManager
    {
        public async Task ClearRandom(int examPaperId, bool isClear)
        {
            if (isClear)
            {
                await _databaseManager.ExamPaperRandomRepository.DeleteByPaperAsync(examPaperId);
                await _databaseManager.ExamPaperRandomTmRepository.DeleteByPaperAsync(examPaperId);
                await _databaseManager.ExamPaperRandomConfigRepository.DeleteByPaperAsync(examPaperId);
                await _databaseManager.ExamPaperUserRepository.ClearByPaperAsync(examPaperId);
                await _databaseManager.ExamPaperStartRepository.ClearByPaperAsync(examPaperId);
                await _databaseManager.ExamPaperAnswerRepository.ClearByPaperAsync(examPaperId);
                await _databaseManager.ExamCerUserRepository.DeleteByPaperId(examPaperId);
            }
            else
            {
                var randomList = await _databaseManager.ExamPaperRandomRepository.GetListByPaperAsync(examPaperId);
                if (randomList != null && randomList.Count > 0)
                {
                    foreach (var random in randomList)
                    {
                        random.Locked = true;
                        await _databaseManager.ExamPaperRandomRepository.UpdateAsync(random);
                    }
                }

            }
        }

        public async Task ClearRandomUser(int examPaperId, int userId)
        {
            var startList = await _databaseManager.ExamPaperStartRepository.GetNoSubmitListAsync(examPaperId, userId);
            if (startList != null && startList.Count > 0)
            {
                foreach (var start in startList)
                {
                    var paper = await _databaseManager.ExamPaperRepository.GetAsync(examPaperId);
                    if (paper.TmRandomType == Enums.ExamPaperTmRandomType.RandomExaming)
                    {
                        await _databaseManager.ExamPaperRandomRepository.DeleteAsync(start.ExamPaperRandomId, examPaperId);
                        await _databaseManager.ExamPaperRandomTmRepository.DeleteByRandomIdAsync(start.ExamPaperRandomId, examPaperId);
                    }
                    await _databaseManager.ExamPaperStartRepository.DeleteAsync(start.Id);
                }
            }
        }
    }
}
