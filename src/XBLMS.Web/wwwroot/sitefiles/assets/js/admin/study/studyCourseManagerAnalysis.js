var $url = '/study/studyCourseManager';

var data = utils.init({
  id: 0,
  planId:0,
  course: null,

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
    labels: ['完成率'],
  },
  pieChartColors: ['#67c23a', '#1989fa', '#5cb87a', '#e6a23c'],
});

var methods = {
  apiGet: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($url, { params: { id: this.id,planId:this.planId } }).then(function (response) {
      var res = response.data;
      $this.course = res.item;

      $this.passSeries = [utils.formatPercentFloat($this.course.totalPassUser, $this.course.totalUser)];

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
    this.planId = utils.getQueryInt("planId");

    this.apiGet();
  }
});
