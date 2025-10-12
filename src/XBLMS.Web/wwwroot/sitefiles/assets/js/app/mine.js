var $url = '/dashboard';

var data = utils.init({
  user: null,
  passSeries: [0],
  passChartOptions: {
    chart: {
      title: 'testament',
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
            fontSize: '18px'
          },
          value: {
            formatter: function (val) {
              return parseInt(val) + '%';
            },
            color: '#000',
            fontSize: '28px',
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
    labels: ['考试及格率'],
  },

  allPercent: 0,
  examTotal: 0,
  examPercent: 0,
  examMoniTotal: 0,
  examMoniPercent: 0,
  examCerTotal: 0,
  examQTotal: 0,
  examAssTotal: 0,

  practiceAnswerTmTotal: 0,
  practiceAnswerPercent: 0,
  practiceAllTmTotal: 0,
  practiceAllPercent: 0,
  practiceCollectTmTotal: 0,
  practiceCollectPercent: 0,
  practiceWrongTmTotal: 0,
  practiceWrongPercent: 0,

  studyPlanTotalCredit: 0,
  studyPlanTotalOverCredit: 0,
  totalCourse: 0,
  totalOverCourse: 0,
  totalDuration: 0,

  version: null,
  versionName: null,
  systemCode: null
});

var methods = {
  apiGet: function () {
    var $this = this;

    $api.get($url, { params: { isApp: true } }).then(function (response) {
      var res = response.data;

      $this.systemCode = res.systemCode;
      $this.user = res.user;

      $this.examTotal = res.examTotal;
      $this.examPercent = res.examPercent;
      $this.examMoniTotal = res.examMoniTotal;
      $this.examMoniPercent = res.examMoniPercent;
      $this.examCerTotal = res.examCerTotal;
      $this.examQTotal = res.examQTotal;
      $this.examAssTotal = res.examAssTotal;

      $this.practiceAllPercent = res.practiceAllPercent;
      $this.practiceAllTmTotal = res.practiceAllTmTotal;

      $this.practiceAnswerTmTotal = res.practiceAnswerTmTotal;
      $this.practiceAnswerPercent = res.practiceAnswerPercent;

      $this.practiceWrongTmTotal = res.practiceWrongTmTotal;
      $this.practiceWrongPercent = res.practiceWrongPercent;

      $this.practiceCollectTmTotal = res.practiceCollectTmTotal;
      $this.practiceCollectPercent = res.practiceCollectPercent;


      $this.studyPlanTotalCredit = res.studyPlanTotalCredit;
      $this.studyPlanTotalOverCredit = res.studyPlanTotalOverCredit;
      $this.totalCourse = res.totalCourse;
      $this.totalOverCourse = res.totalOverCourse;
      $this.totalDuration = res.totalDuration;

      $this.version = res.version;
      $this.versionName = res.versionName;

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
  btnTab: function (common) {

    var $this = this;

    if (common === 'info') {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getRootUrl('profile'),
        width: "100%",
        height: "100%",
        end: function () {
          $this.setDocumentTitle();
          $this.apiGet();
        }
      });
    }
    if (common === 'pwd') {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getRootUrl('password'),
        width: "100%",
        height: "100%",
        end: function () {
          $this.setDocumentTitle();
        }
      });
    }
    if (common === 'gifts') {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getGiftUrl('gifts'),
        width: "100%",
        height: "100%",
        end: function () {
          $this.setDocumentTitle();
          $this.apiGet();
        }
      });
    }
    if (common === 'pointlog') {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getGiftUrl('pointsLog'),
        width: "100%",
        height: "100%",
        end: function () {
          $this.setDocumentTitle();
        }
      });
    }
    if (common === 'shoplog') {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getGiftUrl('pointShopLog'),
        width: "100%",
        height: "100%",
        end: function () {
          $this.setDocumentTitle();
        }
      });
    }
    if (common === 'logout') {
      top.utils.alertWarning({
        title: '安全退出',
        text: '确定要退出系统吗？',
        callback: function () {
          sessionStorage.removeItem(ACCESS_TOKEN_NAME);
          localStorage.removeItem(ACCESS_TOKEN_NAME);
          top.location.href = utils.getRootUrl("login");
        }
      })
    }
    if (common === 'shuati') {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getExamUrl("examPracticeLog"),
        width: "100%",
        height: "100%",
        end: function () {
          $this.setDocumentTitle();
        }
      });
    }
    if (common === 'cer') {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getExamUrl("examPaperCer"),
        width: "100%",
        height: "100%",
        end: function () {
          $this.setDocumentTitle();
        }
      });
    }
    if (common === 'score') {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getExamUrl("examPaperScore"),
        width: "100%",
        height: "100%",
        end: function () {
          $this.setDocumentTitle();
        }
      });
    }
  },
  setDocumentTitle: function () {
    top.document.title = "我的";
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
  }
});
