using System.Threading.Tasks;
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
        public OrganManager(IOrganCompanyRepository companyRepository, IOrganDepartmentRepository departmentRepository, IAdministratorRepository administratorRepository, IUserRepository userRepository, IOrganDutyRepository dutyRepository,
            IExamAssessmentRepository examAssessmentRepository, IExamPkRepository examPkRepository, IExamQuestionnaireRepository examQuestionnaireRepository, IExamPaperRepository examPaperRepository)
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
        }

        public async Task<int> GetGroupCount(int groupId)
        {
            return await _examAssessmentRepository.GetGroupCount(groupId)
                + await _examPkRepository.GetGroupCount(groupId)
                + await _examQuestionnaireRepository.GetGroupCount(groupId)
                + await _examPaperRepository.GetGroupCount(groupId);
        }
    }
}
