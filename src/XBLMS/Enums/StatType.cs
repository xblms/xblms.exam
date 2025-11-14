using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StatType
    {
        [DataEnum(DisplayName = "其他操作")]
        None,
        [DataEnum(DisplayName = "导出")]
        Export,
        [DataEnum(DisplayName = "单位新增")]
        CompanyAdd,
        [DataEnum(DisplayName = "单位修改")]
        CompanyUpdate,
        [DataEnum(DisplayName = "单位删除")]
        CompanyDelete,
        [DataEnum(DisplayName = "部门新增")]
        DepartmentAdd,
        [DataEnum(DisplayName = "部门修改")]
        DepartmentUpdate,
        [DataEnum(DisplayName = "部门删除")]
        DepartmentDelete,
        [DataEnum(DisplayName = "岗位新增")]
        DutyAdd,
        [DataEnum(DisplayName = "岗位修改")]
        DutyUpdate,
        [DataEnum(DisplayName = "岗位删除")]
        DutyDelete,
        [DataEnum(DisplayName = "管理员登录")]
        AdminLoginSuccess,
        [DataEnum(DisplayName = "管理员登录失败")]
        AdminLoginFailure,
        [DataEnum(DisplayName = "管理员账号新增")]
        AdminAdd,
        [DataEnum(DisplayName = "管理员账号修改")]
        AdminUpdate,
        [DataEnum(DisplayName = "管理员账号删除")]
        AdminDelete,
        [DataEnum(DisplayName = "管理员角色新增")]
        AdminAuthAdd,
        [DataEnum(DisplayName = "管理员角色修改")]
        AdminAuthUpdate,
        [DataEnum(DisplayName = "管理员角色删除")]
        AdminAuthDelete,
        [DataEnum(DisplayName = "用户登录")]
        UserLogin,
        [DataEnum(DisplayName = "用户登录APP")]
        UserLoginApp,
        [DataEnum(DisplayName = "用户登录失败")]
        UserLoginFailure,
        [DataEnum(DisplayName = "用户账号新增")]
        UserAdd,
        [DataEnum(DisplayName = "用户账号修改")]
        UserUpdate,
        [DataEnum(DisplayName = "用户账号删除")]
        UserDelete,
        [DataEnum(DisplayName = "用户组新增")]
        UserGroupAdd,
        [DataEnum(DisplayName = "用户组修改")]
        UserGroupUpdate,
        [DataEnum(DisplayName = "用户组删除")]
        UserGroupDelete,
        [DataEnum(DisplayName = "题型新增")]
        ExamTxAdd,
        [DataEnum(DisplayName = "题型修改")]
        ExamTxUpdate,
        [DataEnum(DisplayName = "题型删除")]
        ExamTxDelete,
        [DataEnum(DisplayName = "题目新增")]
        ExamTmAdd,
        [DataEnum(DisplayName = "题目修改")]
        ExamTmUpdate,
        [DataEnum(DisplayName = "题目删除")]
        ExamTmDelete,
        [DataEnum(DisplayName = "题目组新增")]
        ExamTmGroupAdd,
        [DataEnum(DisplayName = "题目组修改")]
        ExamTmGroupUpdate,
        [DataEnum(DisplayName = "题目组删除")]
        ExamTmGroupDelete,
        [DataEnum(DisplayName = "证书模板新增")]
        ExamCerAdd,
        [DataEnum(DisplayName = "证书模板修改")]
        ExamCerUpdate,
        [DataEnum(DisplayName = "证书模板删除")]
        ExamCerDelete,
        [DataEnum(DisplayName = "考试新增")]
        ExamAdd,
        [DataEnum(DisplayName = "考试修改")]
        ExamUpdate,
        [DataEnum(DisplayName = "考试删除")]
        ExamDelete,
        [DataEnum(DisplayName = "竞赛新增")]
        ExamPkAdd,
        [DataEnum(DisplayName = "竞赛修改")]
        ExamPkUpdate,
        [DataEnum(DisplayName = "竞赛删除")]
        ExamPkDelete,
        [DataEnum(DisplayName = "测评新增")]
        ExamAssAdd,
        [DataEnum(DisplayName = "测评修改")]
        ExamAssUpdate,
        [DataEnum(DisplayName = "测评删除")]
        ExamAssDelete,
        [DataEnum(DisplayName = "问卷调查新增")]
        ExamQAdd,
        [DataEnum(DisplayName = "问卷调查修改")]
        ExamQUpdate,
        [DataEnum(DisplayName = "问卷调查删除")]
        ExamQDelete,
        [DataEnum(DisplayName = "知识库新增")]
        KnowledgesAdd,
        [DataEnum(DisplayName = "知识库修改")]
        KnowledgesUpdate,
        [DataEnum(DisplayName = "知识库删除")]
        KnowledgesDelete,
        [DataEnum(DisplayName = "培训课件新增")]
        StudyFileAdd,
        [DataEnum(DisplayName = "培训课件修改")]
        StudyFileUpdate,
        [DataEnum(DisplayName = "培训课件删除")]
        StudyFileDelete,
        [DataEnum(DisplayName = "培训课程新增")]
        StudyCourseAdd,
        [DataEnum(DisplayName = "培训课程修改")]
        StudyCourseUpdate,
        [DataEnum(DisplayName = "培训课程删除")]
        StudyCourseDelete,
        [DataEnum(DisplayName = "培训课程评价新增")]
        StudyEvaluationAdd,
        [DataEnum(DisplayName = "培训课程评价修改")]
        StudyEvaluationUpdate,
        [DataEnum(DisplayName = "培训课程评价删除")]
        StudyEvaluationDelete,
        [DataEnum(DisplayName = "学习任务新增")]
        StudyPlanAdd,
        [DataEnum(DisplayName = "学习任务修改")]
        StudyPlanUpdate,
        [DataEnum(DisplayName = "学习任务删除")]
        StudyPlanDelete,
        [DataEnum(DisplayName = "积分商品新增")]
        GiftAdd,
        [DataEnum(DisplayName = "积分商品修改")]
        GiftUpdate,
        [DataEnum(DisplayName = "积分商品删除")]
        GiftDelete,
        [DataEnum(DisplayName = "题目纠错审核")]
        ExamTmCorrectionAudit,
    }
}
