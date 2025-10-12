var $url = "/dashboard";
var $practiceUrl = "/exam/examPractice/submit";

var data = utils.init({
  user: null,
  passSeries: [0],
  passChartOptions: {
    chart: {
      type: 'radialBar',
      toolbar: {
        show: false
      }
    },
    plotOptions: {
      radialBar: {
        startAngle: -135,
        endAngle: 225,
        hollow: {
          margin: 0,
          size: '70%',
          background: '#fff',
          image: undefined,
          imageOffsetX: 0,
          imageOffsetY: 0,
          position: 'front',
          dropShadow: {
            enabled: true,
            top: 3,
            left: 0,
            blur: 4,
            opacity: 0.24
          }
        },
        track: {
          background: '#fff',
          strokeWidth: '88%',
          margin: 0, // margin is in pixels
          dropShadow: {
            enabled: true,
            top: -3,
            left: 0,
            blur: 4,
            opacity: 0.35
          }
        },

        dataLabels: {
          show: true,
          name: {
            offsetY: -20,
            show: true,
            color: '#ff6a00',
            fontSize: '16px'
          },
          value: {
            formatter: function (val) {
              return parseInt(val) + '%';
            },
            color: '#000',
            fontSize: '36px',
            show: true,
          }
        }
      }
    },
    fill: {
      colors: ['#19cb98']
    },
    stroke: {
      lineCap: 'round'
    },
    labels: ['及格率'],
  },

  allPercent: 0,
  examTotal: 0,
  examPercent: 0,
  examMoniTotal: 0,
  examMoniPercent: 0,

  studyPlan: null,
  studyPlanTotalCredit: 0,
  studyPlanTotalOverCredit: 0,
  totalCourse: 0,
  totalOverCourse: 0,
  totalDuration: 0,

  practiceAnswerTmTotal: 0,
  practiceAnswerPercent: 0,
  practiceAllTmTotal: 0,
  practiceAllPercent: 0,
  practiceCollectTmTotal: 0,
  practiceCollectPercent: 0,
  practiceWrongTmTotal: 0,
  practiceWrongPercent: 0,

  examPaper: null,
  examMoni: null,

  taskPaperTotal: 0,
  taskQTotal: 0,
  taskDialogVisible: false,
  taskAssTotal: 0,
  knowList: null,
  topCer: null,
  dateStr: '',

  openMenus: [],

  taskPlanList: null,
  taskPaperList: null,
  taskQList: null,
  taskAssList: null,
  taskTotal: 0,
  todayExam: null,
  todayExamTotal: 0,
  todayExamOverTotal: 0,
  version: null
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url, { params: { isApp: true } }).then(function (response) {
      var res = response.data;

      $this.user = res.user;

      $this.studyPlan = res.studyPlan;
      $this.studyPlanTotalCredit = res.studyPlanTotalCredit;
      $this.studyPlanTotalOverCredit = res.studyPlanTotalOverCredit;
      $this.totalCourse = res.totalCourse;
      $this.totalOverCourse = res.totalOverCourse;
      $this.totalDuration = res.totalDuration;

      $this.examTotal = res.examTotal;
      $this.examPercent = res.examPercent;
      $this.examMoniTotal = res.examMoniTotal;
      $this.examMoniPercent = res.examMoniPercent;

      $this.practiceAllPercent = res.practiceAllPercent;
      $this.practiceAllTmTotal = res.practiceAllTmTotal;

      $this.practiceAnswerTmTotal = res.practiceAnswerTmTotal;
      $this.practiceAnswerPercent = res.practiceAnswerPercent;

      $this.practiceWrongTmTotal = res.practiceWrongTmTotal;
      $this.practiceWrongPercent = res.practiceWrongPercent;

      $this.practiceCollectTmTotal = res.practiceCollectTmTotal;
      $this.practiceCollectPercent = res.practiceCollectPercent;

      $this.examPaper = res.examPaper;
      $this.examMoni = res.examMoni;
      $this.topCer = res.topCer;

      $this.dateStr = res.dateStr;

      $this.taskPaperTotal = res.taskPaperTotal;
      $this.taskQTotal = res.taskQTotal;
      $this.taskAssTotal = res.taskAssTotal;

      $this.taskPlanList = res.taskPlanList;
      $this.taskPaperList = res.taskPaperList;
      $this.taskQList = res.taskQList;
      $this.taskAssList = res.taskAssList;
      $this.taskTotal = res.taskTotal;

      $this.todayExam = res.todayExam;
      $this.todayExamTotal = res.todayExamTotal;
      $this.todayExamOverTotal = res.todayExamOverTotal;
      $this.knowList = res.knowList;

      $this.openMenus = res.openMenus;

      $this.version = res.version;

      top.utils.pointNotice(res.pointNotice);
      setTimeout(function () {
        $this.passSeries = [100];
      }, 1000);

      setTimeout(function () {
        $this.passSeries = [res.allPercent];
      }, 2000);

    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnTaskClick: function () {
    var eid = '#divTask';
    var scrollTop = ($(eid).offset().top);
    document.documentElement.scrollTop = document.body.scrollTop = scrollTop;
  },
  btnViewPlanClick: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getStudyUrl('studyPlanInfo', { id: id }),
      width: "100%",
      height: "100%",
      end: function () {
        $this.apiGet(id);
      }
    });
  },
  btnMoreMoniClick: function () {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examPaperMoni'),
      width: "100%",
      height: "100%",
      end: function () {
        $this.setDocumentTitle();
      }
    });
  },
  btnMoreShuatiClick: function () {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examPractice'),
      width: "100%",
      height: "100%",
      end: function () {
        $this.setDocumentTitle();
      }
    });
  },
  btnPkMoreMenuClick: function () {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examPk'),
      width: "100%",
      height: "100%",
      end: function () {
        $this.setDocumentTitle();
      }
    });
  },
  btnAssMoreMenuClick: function () {
    location.href = utils.getExamUrl("examAssessment");
  },
  btnMoreMenuClick: function (command) {
    top.$vue.btnAppMenuClick(command);
  },
  btnCreatePracticeClick: function (practiceType) {
    var $this = this;
    if (practiceType === 'Create') {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getExamUrl('examPracticeReady'),
        width: "100%",
        height: "100%"
      });
    }
    else {
      top.utils.alertWarning({
        title: '准备进入刷题模式',
        callback: function () {
          if (practiceType === 'Collect') {
            if ($this.practiceCollectTmTotal > 0) {
              $this.apiCreatePractice(practiceType);
            }
            else {
              utils.error("没有题目可以练习");
            }
          }
          else if (practiceType === 'Wrong') {
            if ($this.practiceWrongTmTotal > 0) {
              $this.apiCreatePractice(practiceType);
            }
            else {
              utils.error("没有题目可以练习");
            }
          }
        }
      });
    }
  },
  apiCreatePractice: function (practiceType) {
    var $this = this;

    utils.loading(this, true, "正在创建练习...");

    $api.post($practiceUrl, { practiceType: practiceType }).then(function (response) {
      var res = response.data;

      if (res.success) {
        $this.goPractice(res.id);
      }
      else {
        utils.error(res.error);
      }

    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  goPractice: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examPracticing', { id: id }),
      width: "100%",
      height: "100%"
    });
  },
  goKnowledges: function () {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getKnowledgesUrl('knowledges'),
      width: "100%",
      height: "100%",
      end: function () {
        $this.setDocumentTitle();
      }
    });
  },
  btnViewKnowClick: function (row) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getKnowledgesUrl("knowledgesView", { id: row.id }),
      width: "100%",
      height: "100%",
      end: function () {
        $this.setDocumentTitle();
      }
    });
  },
  btnViewAssClick: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examAssessmenting', { id: id }),
      width: "100%",
      height: "100%",
      end: function () {
        $this.apiGet();
      }
    });
  },
  btnViewQClick: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examQuestionnairing', { id: id }),
      width: "100%",
      height: "100%",
      end: function () {
        $this.apiGet();
      }
    });
  },
  btnViewPaperClick: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examPaperInfo', { id: id }),
      width: "100%",
      height: "100%",
      end: function () {
        $this.apiGet();
      }
    });
  },
  btnViewCer: function (row) {
    top.utils.openLayerPhoto({
      title: row.name,
      id: row.id,
      src: row.cerImg + '?r=' + Math.random()
    })
  },
  btnEventClick: function () {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getRootUrl('event'),
      width: "100%",
      height: "100%",
      end: function () {
        $this.setDocumentTitle();
      }
    })
  },
  setDocumentTitle: function () {
    top.document.title = "首页";
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
    this.setDocumentTitle();
    this.apiGet();
  },
});
