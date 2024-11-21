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
    }
}
