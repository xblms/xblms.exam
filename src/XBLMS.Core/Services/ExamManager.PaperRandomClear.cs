using System.Threading.Tasks;

namespace XBLMS.Core.Services
{
    public partial class ExamManager
    {
        public async Task ClearRandom(int examPaperId, bool isClear)
        {
            if (isClear)
            {
                await _examPaperRandomRepository.DeleteByPaperAsync(examPaperId);
                await _examPaperRandomTmRepository.DeleteByPaperAsync(examPaperId);
                await _examPaperRandomConfigRepository.DeleteByPaperAsync(examPaperId);
                await _examPaperUserRepository.ClearByPaperAsync(examPaperId);
                await _examPaperStartRepository.ClearByPaperAsync(examPaperId);
                await _examPaperAnswerRepository.ClearByPaperAsync(examPaperId);
                await _examCerUserRepository.DeleteByPaperId(examPaperId);

            }
            else
            {
                var randomList = await _examPaperRandomRepository.GetListByPaperAsync(examPaperId);
                if (randomList != null && randomList.Count > 0)
                {
                    foreach (var random in randomList)
                    {
                        random.Locked = true;
                        await _examPaperRandomRepository.UpdateAsync(random);
                    }
                }

            }
        }
        public async Task ClearRandomUser(int examPaperId, int userId)
        {
            var startList = await _examPaperStartRepository.GetNoSubmitListAsync(examPaperId, userId);
            if (startList != null && startList.Count > 0)
            {
                foreach (var start in startList)
                {
                    var paper = await _examPaperRepository.GetAsync(examPaperId);
                    if (paper.TmRandomType == Enums.ExamPaperTmRandomType.RandomExaming)
                    {
                        await _examPaperRandomRepository.DeleteAsync(start.ExamPaperRandomId, examPaperId);
                        await _examPaperRandomTmRepository.DeleteByRandomIdAsync(start.ExamPaperRandomId, examPaperId);
                    }
                    await _examPaperStartRepository.DeleteAsync(start.Id);
                }
            }
        }
    }
}
