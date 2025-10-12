var $url = '/common/userDocIndex';

var data = utils.init({
  id: utils.getQueryInt('id'),
  form: {
    id: utils.getQueryInt('id'),
    dateFrom: '',
    dateTo: ''
  },
  systemCode: null,
  totalCers: "",
  totalCredit: "",
  totalDuration: "",
  totalLogins: "",
  totalPoints: "",
  planTotal: 0,
  planDabiaoTotal: 0,
  planOverTotal: 0,
  courseOverTotal: 0,
  courseTotal: 0,
  examPracticeAnswerTotal: 0,
  examPracticeRightTotal: 0,
  examAssTotal: 0,
  examAssSubmitTotal: 0,
  examQTotal: 0,
  examQSubmitTotal: 0,
  examTotal: 0,
  examPassTotal: 0,
  examMoinPasaTotal: 0,
  examMoniTotal: 0,
  planOverSeries: [0],
  planOverChartOptions: {
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
    labels: ['完成率'],
  },
  planDabiaoSeries: [0],
  planDabiaoChartOptions: {
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
    labels: ['达标率'],
  },
  courseOverSeries: [0],
  courseOverChartOptions: {
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
    labels: ['完成率'],
  },
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url, {
      params: this.form
    }).then(function (response) {
      var res = response.data;

      $this.systemCode = res.systemCode;

      $this.totalCers = res.totalCers;
      $this.totalCredit = res.totalCredit;
      $this.totalDuration = res.totalDuration;
      $this.totalLogins = res.totalLogins;
      $this.totalPoints = res.totalPoints;

      $this.planTotal = res.planTotal;
      $this.planDabiaoTotal = res.planDabiaoTotal;
      $this.planOverTotal = res.planOverTotal;
      $this.planOverSeries = [utils.formatPercentFloat($this.planOverTotal, $this.planTotal)];
      $this.planDabiaoSeries = [utils.formatPercentFloat($this.planDabiaoTotal, $this.planTotal)];

      $this.courseOverTotal = res.courseOverTotal;
      $this.courseTotal = res.courseTotal;
      $this.courseOverSeries = [utils.formatPercentFloat($this.courseOverTotal, $this.courseTotal)];

      $this.examPracticeAnswerTotal = res.examPracticeAnswerTotal;
      $this.examPracticeRightTotal = res.examPracticeRightTotal;
      $this.examAssTotal = res.examAssTotal;
      $this.examAssSubmitTotal = res.examAssSubmitTotal;
      $this.examQTotal = res.examQTotal;
      $this.examQSubmitTotal = res.examQSubmitTotal;
      $this.examTotal = res.examTotal;
      $this.examPassTotal = res.examPassTotal;
      $this.examMoinPasaTotal = res.examMoinPasaTotal;
      $this.examMoniTotal = res.examMoniTotal;

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnSearchClick: function () {
    this.apiGet();
  }
};
Vue.component("apexchart", {
  extends: VueApexCharts
});
var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  }
});

