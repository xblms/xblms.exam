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
  knowList:null,
  topCer: null,
  dateStr: '',

  taskPaperList: null,
  taskQList: null,
  taskAssList: null,
  taskTotal: 0,
  todayExam: null,
  version: null

});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url, { params: { isApp: true } }).then(function (response) {
      var res = response.data;

      $this.user = res.user;

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

      $this.taskPaperList = res.taskPaperList;
      $this.taskQList = res.taskQList;
      $this.taskAssList = res.taskAssList;
      $this.taskTotal = res.taskTotal;

      $this.todayExam = res.todayExam;
      $this.knowList = res.knowList;

      $this.version = res.version;
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
  btnPkMoreMenuClick: function () {
    location.href = utils.getExamUrl("examPk");
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
  goKnowledges: function (id) {
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
