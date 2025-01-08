using System.Threading.Tasks;
using XBLMS.Core.Repositories;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Services
{
    public partial class OrganManager : IOrganManager
    {
        private readonly IAdministratorRepository _administratorRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrganCompanyRepository _companyRepository;
        private readonly IOrganDepartmentRepository _departmentRepository;
        private readonly IOrganDutyRepository _dutyRepository;
        private readonly IExamAssessmentRepository _examAssessmentRepository;
        private readonly IExamPkRepository _examPkRepository;
        private readonly IExamQuestionnaireRepository _examQuestionnaireRepository;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IExamQuestionnaireUserRepository _examQuestionnaireUserRepository;
        private readonly IExamQuestionnaireAnswerRepository _examQuestionnaireAnswerRepository;
        private readonly IExamPracticeWrongRepository _examPracticeWrongRepository;
        private readonly IExamPracticeRepository _examPracticeRepository;
        private readonly IExamPracticeCollectRepository _examPracticeCollectRepository;
        private readonly IExamPracticeAnswerRepository _examPracticeAnswerRepository;
        private readonly IExamPkUserRepository _examPkUserRepository;
        private readonly IExamPkRoomRepository _examPkRoomRepository;
        private readonly IExamPkRoomAnswerRepository _examPkRoomAnswerRepository;
        private readonly IExamPaperUserRepository _examPaperUserRepository;
        private readonly IExamPaperStartRepository _examPaperStartRepository;
        private readonly IExamPaperAnswerRepository _examPaperAnswerRepository;
        private readonly IExamCerUserRepository _examCerUserRepository;
        private readonly IExamAssessmentUserRepository _examAssessmentUserRepository;
        private readonly IUserGroupRepository _userGroupRepository;

        public OrganManager(IOrganCompanyRepository companyRepository,
            IOrganDepartmentRepository departmentRepository,
            IAdministratorRepository administratorRepository,
            IUserRepository userRepository,
            IOrganDutyRepository dutyRepository,
            IExamAssessmentRepository examAssessmentRepository,
            IExamPkRepository examPkRepository,
            IExamQuestionnaireRepository examQuestionnaireRepository,
            IExamPaperRepository examPaperRepository,
            IExamQuestionnaireUserRepository examQuestionnaireUserRepository,
            IExamQuestionnaireAnswerRepository examQuestionnaireAnswerRepository,
            IExamPracticeWrongRepository examPracticeWrongRepository,
            IExamPracticeRepository examPracticeRepository,
            IExamPracticeCollectRepository examPracticeCollectRepository,
            IExamPracticeAnswerRepository examPracticeAnswerRepository,
            IExamPkUserRepository examPkUserRepository,
            IExamPkRoomRepository examPkRoomRepository,
            IExamPkRoomAnswerRepository examPkRoomAnswerRepository,
            IExamPaperUserRepository examPaperUserRepository,
            IExamPaperStartRepository examPaperStartRepository,
            IExamPaperAnswerRepository examPaperAnswerRepository,
            IExamCerUserRepository examCerUserRepository,
            IExamAssessmentUserRepository examAssessmentUserRepository,
            IUserGroupRepository userGroupRepository)
        {
            _companyRepository = companyRepository;
            _departmentRepository = departmentRepository;
            _administratorRepository = administratorRepository;
            _userRepository = userRepository;
            _dutyRepository = dutyRepository;
            _examAssessmentRepository = examAssessmentRepository;
            _examPkRepository = examPkRepository;
            _examQuestionnaireRepository = examQuestionnaireRepository;
            _examPaperRepository = examPaperRepository;
            _examQuestionnaireUserRepository = examQuestionnaireUserRepository;
            _examQuestionnaireAnswerRepository = examQuestionnaireAnswerRepository;
            _examPracticeWrongRepository = examPracticeWrongRepository;
            _examPracticeRepository = examPracticeRepository;
            _examPracticeCollectRepository = examPracticeCollectRepository;
            _examPracticeAnswerRepository = examPracticeAnswerRepository;
            _examPkUserRepository = examPkUserRepository;
            _examPkRoomRepository = examPkRoomRepository;
            _examPkRoomAnswerRepository = examPkRoomAnswerRepository;
            _examPaperUserRepository = examPaperUserRepository;
            _examPaperStartRepository = examPaperStartRepository;
            _examPaperAnswerRepository = examPaperAnswerRepository;
            _examCerUserRepository = examCerUserRepository;
            _examAssessmentUserRepository = examAssessmentUserRepository;
            _userGroupRepository = userGroupRepository;

        }

        public async Task<int> GetGroupCount(int groupId)
        {
            return await _examAssessmentRepository.GetGroupCount(groupId)
                + await _examPkRepository.GetGroupCount(groupId)
                + await _examQuestionnaireRepository.GetGroupCount(groupId)
                + await _examPaperRepository.GetGroupCount(groupId);
        }

        public async Task DeleteUser(int userId)
        {
            await _userRepository.DeleteAsync(userId);
            await _userGroupRepository.DeleteAsync(userId);

            await _examQuestionnaireUserRepository.DeleteByUserId(userId);
            await _examQuestionnaireAnswerRepository.DeleteByUserId(userId);
            await _examPracticeWrongRepository.DeleteByUserId(userId);
            await _examPracticeRepository.DeleteAsync(userId);
            await _examPracticeCollectRepository.DeleteByUserId(userId);
            await _examPracticeAnswerRepository.DeleteByUserId(userId);
            await _examPkUserRepository.DeleteByUserId(userId);
            await _examPkRoomRepository.DeleteByUserId(userId);
            await _examPkRoomAnswerRepository.DeleteByUserId(userId);
            await _examPaperUserRepository.DeleteByUserId(userId);
            await _examPaperStartRepository.DeleteByUserId(userId);
            await _examPaperAnswerRepository.DeleteByUserId(userId);
            await _examCerUserRepository.DeleteByUserId(userId);
            await _examAssessmentUserRepository.DeleteByUserId(userId);
        }
    }
}
