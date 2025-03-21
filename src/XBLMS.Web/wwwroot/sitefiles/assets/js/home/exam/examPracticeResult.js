var $url = "/exam/examPracticeResult";

var data = utils.init({
  id: utils.getQueryInt("id"),
  title: null,
  total: 0,
  answerTotal: 0,
  rightTotal: 0,
  wrongTotal: 0,
  series: [75],
  chartOptions: {
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
          strokeWidth: '100%',
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
            fontSize: '17px'
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
    labels: ['正确率'],
  },
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);

    $api.get($url, { params: { id: $this.id } }).then(function (response) {
      var res = response.data;

      $this.title = res.title;
      $this.total = res.total;
      $this.wrongTotal = res.wrongTotal;
      $this.rightTotal = res.rightTotal;
      $this.answerTotal = res.answerTotal;


      $this.series = [utils.formatPercentFloat($this.rightTotal, $this.answerTotal)];

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnViewClick: function () {
    utils.closeLayerSelf();
    utils.openTopLeft(this.title + "-答题预览", utils.getExamUrl("examPracticeResultView", { id: this.id }));
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
    this.apiGet();
  },
});
