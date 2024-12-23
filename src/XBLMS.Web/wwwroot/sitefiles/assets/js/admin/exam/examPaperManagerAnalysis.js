var $url = '/exam/examPaperManager';

var data = utils.init({
  id: 0,
  formScore: {
    id: 0,
    keywords: '',
    dateFrom: '',
    dateTo: '',
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  scoreList: null,
  scoreTotal: 0,
  passSeries: [100],
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
              return val + '%';
            },
            color: '#000',
            fontSize: '36px',
            show: true,
          }
        }
      }
    },
    fill: {
      colors: ['#67C23A']
    },
    stroke: {
      lineCap: 'round'
    },
    labels: ['及格率'],
  },
  nopassSeries: [0],
  nopassChartOptions: {
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
              return val + '%';
            },
            color: '#000',
            fontSize: '36px',
            show: true,
          }
        }
      }
    },
    fill: {
      colors: ['#F56C6C']
    },
    stroke: {
      lineCap: 'round'
    },
    labels: ['不及格'],
  },
  totalScore: 0,
  passScore: 0,
  totalUser: 0,
  maxScore: 0,
  minScore: 0,
  totalPass: 0,
  totalPassDistinct: 0,
  totalUserScore: 0,
  totalExamTimes: 0,
  totalExamTimesDistinct: 0,
});

var methods = {
  apiGet: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($url, { params: { id: this.id } }).then(function (response) {
      var res = response.data;
      $this.totalScore = res.totalScore;
      $this.passScore = res.passScore;
      $this.totalUser = res.totalUser;
      $this.maxScore = res.maxScore;
      $this.minScore = res.minScore;
      $this.totalPass = res.totalPass;
      $this.totalPassDistinct = res.totalPassDistinct;
      $this.totalUserScore = res.totalUserScore;
      $this.totalExamTimes = res.totalExamTimes;
      $this.totalExamTimesDistinct = res.totalExamTimesDistinct;

      setTimeout(function () {
        if ($this.totalExamTimes > 0) {
          var passS = utils.formatPercentFloat($this.totalPass, $this.totalExamTimes);
          $this.passSeries = [passS];
          $this.nopassSeries = [100 - passS];
        }
        else {
          $this.passSeries = [0];
          $this.nopassSeries = [0];
        }
      }, 1000);

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false, { layer: true });
    });
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
    this.id = utils.getQueryInt("id");
    this.apiGet();
  }
});
