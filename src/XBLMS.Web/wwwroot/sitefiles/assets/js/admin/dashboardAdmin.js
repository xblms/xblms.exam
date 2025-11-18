var $url = "/dashboardAdmin";
var $urlData = $url + "/data";

var data = utils.init({
  series: [],
  chartOptions: {},
  total: 0,
  list: [],
  pageIndex: 1,
  pageSize: PER_PAGE,
  loadMoreLoading: false,
  examTotalToday: 0,
  examTotalWeek: 0,
  offTrainTotal: 0,
  planCreateTotal: 0,
  planOverTotal: 0,
  taskExamTotal: 0,
  taskStudyTotal: 0,
  systemCode: null,
  totalCompany: 1,
  totalAdmin: 2,
  totalUser: 3,
  totalTm: 4,
  totalFile: 5
});

var methods = {
  apiGetLog: function () {
    var $this = this;
    $api.get($url, { params: { pageIndex: this.pageIndex, pageSize: this.pageSize } }).then(function (response) {
      var res = response.data;
      $this.total = res.total;

      if (res.list && res.list.length > 0) {
        res.list.forEach(paper => {
          $this.list.push(paper);
        });
      }

    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.loadMoreLoading = false;
    });
  },
  apiGetData: function () {
    var $this = this;

    $api.get($urlData).then(function (response) {
      var res = response.data;

      $this.systemCode = res.systemCode;
      $this.examTotalToday = res.examTotalToday;
      $this.examTotalWeek = res.examTotalWeek;
      $this.planCreateTotal = res.planCreateTotal;
      $this.planOverTotal = res.planOverTotal;
      $this.offTrainTotal = res.offTrainTotal;

      $this.taskExamTotal = $this.examTotalToday + $this.examTotalWeek;
      $this.taskStudyTotal = $this.planCreateTotal + $this.planOverTotal + $this.offTrainTotal;

      $this.totalCompany = res.totalCompany;
      $this.totalAdmin = res.totalAdmin;
      $this.totalUser = res.totalUser;
      $this.totalTm = res.totalTm;
      $this.totalFile = res.totalFile;

      $this.series = res.dataList;
      $this.chartOptions = {
        chart: {
          type: 'bar',
          stacked: true,
        },
        title: {
          text: '',
          align: 'left',
        },
        stroke: {
          width: 1,
          colors: ['#fff']
        },
        plotOptions: {
          bar: {
            horizontal: true
          }
        },
        xaxis: {
          categories: res.dataTitleList
        },
        fill: {
          opacity: 1,
        },
        colors: ['#6600FF', '#FF7F50', '#F56C6C', '#FF0000', '#67C23A'],
        legend: {
          position: 'top',
          horizontalAlign: 'left'
        }
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      $this.apiGetLog();
    });
  },
  btnLoadMoreClick: function () {
    this.loadMoreLoading = true;
    this.pageIndex++;
    this.apiGetLog();
  },
  btnViewClick: function (log) {
    var $this = this;
    if (log.isAdmin) {
      utils.openAdminView(log.objectId);
    }
    else if (log.isUser) {
      utils.openUserView(log.objectId);
    }
    else if (log.isPlan) {
      utils.openTopLeft(log.objectName, utils.getStudyUrl("studyPlanManagerAnalysis", { id: log.objectId }));
    }
    else if (log.isCourse) {
      utils.openTopLeft(log.objectName, utils.getStudyUrl("studyCourseManagerAnalysis", { id: log.objectId }));
    }
    else if (log.isFile) {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getStudyUrl('studyCourseFileLayerView', { id: log.objectId }),
        width: "68%",
        height: "98%"
      });
    }
    else if (log.isDeleteTm) {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getCommonUrl('examTmDeleteLayerView', { id: log.id }),
        width: "58%",
        height: "88%"
      });
    }
    else if (log.isTm) {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getCommonUrl('examTmLayerView', { id: log.objectId }),
        width: "58%",
        height: "88%"
      });
    }
    else if (log.isExam) {
      utils.openTopLeft(log.objectName + '-预览', utils.getCommonUrl("examPaperLayerView", { id: log.objectId }));
    }
    else if (log.isExamQ) {
      utils.openTopLeft(log.objectName + '-结果统计', utils.getExamUrl("examQuestionnaireAnalysis", { id: log.objectId }));
    }
    else if (log.isExamAss) {
      utils.openTopLeft(log.objectName + '-测评结果', utils.getExamUrl("examAssessmentUsers", { id: log.objectId }));
    }
    else if (log.isExamPk) {
      utils.openTopLeft(log.objectName + '-赛程', utils.getExamUrl("examPkRooms", { id: log.objectId }));
    }
    else if (log.isExamCer) {
      utils.openTopLeft(log.objectName + '-预览', utils.getCommonUrl("examCerLayerView", { id: log.objectId }));
    }
    else if (log.isKnowledge) {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getKnowledgesUrl("knowledgesView", { url: log.src }),
        width: "88%",
        height: "99%"
      });
    }
    else if (log.isTmCorrection) {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getCommonUrl('examTmCorrectionLayerView', { id: log.objectId }),
        width: "88%",
        height: "88%"
      });
    }
  },

  btnTaskStudy: function () {
    if (this.planCreateTotal > 0) {
      this.btnCreatePlan();
    }
    else if (this.planOverTotal > 0) {
      this.btnOverPlan();
    }
    else {
      this.btnWeekOfftrain();
    }
  },
  btnTaskExam: function () {
    if (this.examTotalToday > 0) {
      this.btnTodayExam('today');
    }
    else {
      this.btnTodayExam('week');
    }
  },
  btnTodayExam: function (dateType) {
    var rightTitle = dateType === 'today' ? "今日考试安排" : "本周考试安排";
    utils.openTopRight(rightTitle, utils.getExamUrl("examPaperToday", { dateType: dateType }), 40);
  },
  btnCreatePlan: function () {
    var rightTitle = "本月发布计划";
    utils.openTopRight(rightTitle, utils.getStudyUrl("studyPlanMonth", { isOver: false }), 40);
  },
  btnOverPlan: function (dateType) {
    var rightTitle = "本月将完成计划";
    utils.openTopRight(rightTitle, utils.getStudyUrl("studyPlanMonth", { isOver: true }), 40);
  },
  btnWeekOfftrain: function () {
    var rightTitle = "本周面授课";
    utils.openTopRight(rightTitle, utils.getStudyUrl("studyOffCourseWeek"), 40);
  },
  btnDoc: function () {
    utils.openTopLeft(this.systemCode === 'Exam' ? '考试档案' : '学习档案', utils.getSettingsUrl("usersDoc"), 98);
  },
  btnEditClick: function (log) {
    var $this = this;
    if (log.isAdmin) {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getSettingsUrl('administratorsLayerProfile', { userName: log.objectName }),
        width: "60%",
        height: "88%"
      });
    }
    else if (log.isGift) {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getPointsUrl('giftsEdit', { id: log.objectId }),
        width: "60%",
        height: "88%",
      });
    }
    else if (log.isUser) {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getSettingsUrl('usersLayerProfile', { userId: log.objectId }),
        width: "60%",
        height: "88%"
      });
    }
    else if (log.isPlan) {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getStudyUrl('studyPlanEdit', { id: log.objectId }),
        width: "98%",
        height: "98%"
      });
    }
    else if (log.isCourse) {
      var layerWidth = "68%";

      var url = utils.getStudyUrl('studyCourseFaceEdit', { id: log.objectId, face: log.isFace });
      if (!log.isFace) {
        layerWidth = "98%";
        url = utils.getStudyUrl('studyCourseEdit', { id: log.objectId, face: log.isFace });
      }

      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: url,
        width: layerWidth,
        height: "98%"
      });
    }
    else if (log.isTm) {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getExamUrl('examTmEdit', { id: log.objectId }),
        width: "78%",
        height: "98%"
      });
    }
    else if (log.isExam) {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getExamUrl('examPaperEdit', { id: log.objectId }),
        width: "98%",
        height: "98%",
      });
    }
    else if (log.isExamQ) {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getExamUrl('examQuestionnaireEdit', { id: log.objectId }),
        width: "98%",
        height: "98%"
      });
    }
    else if (log.isExamAss) {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getExamUrl('examAssessmentEdit', { id: log.objectId }),
        width: "98%",
        height: "98%"
      });
    }
    else if (log.isExamPk) {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getExamUrl('examPkEdit', { id: log.objectId }),
        width: "68%",
        height: "88%"
      });
    }
    else if (log.isExamCer) {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getExamUrl('examCerEdit', { id: log.objectId }),
        width: "99%",
        height: "99%"
      });
    }
  }
};
Vue.component("apexchart", {
  extends: VueApexCharts
});

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.apiGetData();
  },
});
