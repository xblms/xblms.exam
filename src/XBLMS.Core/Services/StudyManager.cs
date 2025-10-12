using System.Threading.Tasks;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Services
{
    public partial class StudyManager : IStudyManager
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IPathManager _pathManager;
        private readonly IOrganManager _organManager;
        private readonly IUserRepository _userRepository;
        private readonly IUserGroupRepository _userGroupRepository;

        private readonly IStudyCourseTreeRepository _studyCourseTreeRepository;
        private readonly IStudyCourseRepository _studyCourseRepository;
        private readonly IStudyPlanUserRepository _studyPlanUserRepository;
        private readonly IStudyCourseUserRepository _studyCourseUserRepository;
        private readonly IStudyPlanCourseRepository _studyPlanCourseRepository;
        private readonly IStudyPlanRepository _studyPlanRepository;
        private readonly IExamPaperStartRepository _examPaperStartRepository;
        private readonly IExamPaperRepository _examPaperRepository;


        public StudyManager(ISettingsManager settingsManager,
            IOrganManager organManager,
            IPathManager pathManager,
            IUserRepository userRepository,
            IUserGroupRepository userGroupRepository,
            IStudyCourseTreeRepository studyCourseTreeRepository,
            IStudyCourseRepository studyCourseRepository,
            IStudyPlanUserRepository studyPlanUserRepository,
            IStudyCourseUserRepository studyCourseUserRepository,
            IStudyPlanCourseRepository studyPlanCourseRepository,
            IStudyPlanRepository studyPlanRepository,
            IExamPaperStartRepository examPaperStartRepository,
            IExamPaperRepository examPaperRepository)
        {
            _settingsManager = settingsManager;
            _organManager = organManager;
            _pathManager = pathManager;
            _userRepository = userRepository;
            _userGroupRepository = userGroupRepository;
            _studyCourseTreeRepository = studyCourseTreeRepository;
            _studyCourseRepository = studyCourseRepository;
            _studyPlanUserRepository = studyPlanUserRepository;
            _studyCourseUserRepository = studyCourseUserRepository;
            _studyPlanCourseRepository = studyPlanCourseRepository;
            _studyPlanRepository = studyPlanRepository;
            _examPaperStartRepository = examPaperStartRepository;
            _examPaperRepository = examPaperRepository;
        }

        public async Task<int> GetUseCountByPaperId(int paperId)
        {
            return
                await _studyPlanRepository.GetPaperUseCount(paperId) +
                await _studyCourseRepository.GetPaperUseCount(paperId) +
                await _studyPlanCourseRepository.GetPaperUseCount(paperId);
        }
        public async Task<int> GetUseCountByPaperQId(int paperId)
        {
            return
                await _studyCourseRepository.GetPaperQUseCount(paperId) +
                await _studyPlanCourseRepository.GetPaperQUseCount(paperId);
        }
        public async Task<int> GetUseCountByEvaluationId(int evaluationId)
        {
            return
                await _studyCourseRepository.GetEvaluationUseCount(evaluationId) +
                await _studyPlanCourseRepository.GetEvaluationUseCount(evaluationId);
        }
    }
}
