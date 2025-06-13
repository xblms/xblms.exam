using Datory;
using Datory.Annotations;
using System;
using System.Collections.Generic;
using XBLMS.Enums;
namespace XBLMS.Models
{
    [DataTable("exam_Paper")]
    public class ExamPaper : Entity
    {
        [DataColumn]
        public int TreeId { get; set; }
        [DataColumn]
        public string Title { get; set; }
        /// <summary>
        /// 考试须知
        /// </summary>
        [DataColumn(Text = true)]
        public string Subject { get; set; }
        [DataColumn]
        public DateTime? ExamBeginDateTime { get; set; } = DateTime.Now;
        [DataColumn]
        public DateTime? ExamEndDateTime { get; set; } = DateTime.Now.AddDays(3);
        /// <summary>
        /// 考试次数
        /// </summary>
        [DataColumn]
        public int ExamTimes { get; set; } = 999;//考试次数
        /// <summary>
        /// 是否计时考试
        /// </summary>
        [DataColumn]
        public bool IsTiming { get; set; } = true;
        /// <summary>
        /// 计时分钟
        /// </summary>
        [DataColumn]
        public int TimingMinute { get; set; } = 60;
        [DataColumn]
        public int TotalScore { get; set; } = 100;
        [DataColumn]
        public int PassScore { get; set; } = 60;
        [DataColumn]
        public int CerId { get; set; }

        /// <summary>
        /// 自动判分
        /// </summary>
        [DataColumn]
        public bool IsAutoScore { get; set; } = true;//自动判分
        [DataColumn]
        public bool IsAutoScoreTiankongti { get; set; } = true;
        [DataColumn]
        public bool IsAutoScoreJiandati { get; set; } = true;
        [DataColumn]
        public bool IsAutoScoreDuoxuanti { get; set; } = true;

        [DataColumn(Text = true)]
        public List<int> UserGroupIds { get; set; }
        [DataColumn(Text = true)]
        public List<int> TmGroupIds { get; set; }
        /// <summary>
        /// 固定还是随机
        /// </summary>
        [DataColumn]
        public ExamPaperTmRandomType TmRandomType { get; set; } = ExamPaperTmRandomType.RandomExaming;//固定题目还是随机题目
        [DataColumn]
        public bool Moni { get; set; } = false;
        [DataColumn]
        public int RandomCount { get; set; } = 1;
        /// <summary>
        /// 题目分数计算类型
        /// </summary>
        [DataColumn]
        public ExamPaperTmScoreType TmScoreType { get; set; } = ExamPaperTmScoreType.ScoreTypeRate;
        [DataColumn(Text = true)]
        public List<int> TxIds { get; set; }
        [DataColumn(Text = true)]
        public List<int> TmIds { get; set; }//固定题目的时候用

       /// <summary>
       /// 允许查看成绩
       /// </summary>
        [DataColumn]
        public bool SecrecyScore { get; set; } = true;
        /// <summary>
        /// 允许查看答卷
        /// </summary>
        [DataColumn]
        public bool SecrecyPaperContent { get; set; } = true;
        /// <summary>
        /// 允许查看正确答案
        /// </summary>
        [DataColumn]
        public bool SecrecyPaperAnswer { get; set; } = true;
        [DataColumn]
        public int TmCount { get; set; } = 100;
        [DataColumn]
        public bool IsClientExam { get; set; } = false;//普通考试 客户端考试
        /// <summary>
        /// 是否显示退出考试按钮
        /// </summary>
        [DataColumn]
        public bool OpenExist { get; set; } = true;
        /// <summary>
        /// 退出次数，切屏次数，>= 后自动交卷 0不限制
        /// </summary>
        [DataColumn]
        public int ExistCount { get; set; } = 0;
        /// <summary>
        /// 题目随机显示
        /// </summary>
        [DataColumn]
        public bool IsExamingTmRandomView { get; set; } = false;
        /// <summary>
        /// 候选项随机显示
        /// </summary>
        [DataColumn]
        public bool IsExamingTmOptionRandomView { get; set; } = false;
        [DataColumn]
        public SubmitType SubmitType { get; set; } = SubmitType.Save;
        [DataColumn]
        public bool Locked { get; set; } = false;
        [DataColumn]
        public bool LockedApp { get; set; } = false;
        [DataColumn]
        public bool FullScreen { get; set; } = false;
        [DataColumn]
        public bool SeparateStorage { get; set; }
    }
}
