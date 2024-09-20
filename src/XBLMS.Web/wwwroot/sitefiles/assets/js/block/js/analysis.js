var $url = '/settings/block/analysis';

var data = utils.init({
  chartData: null
});

var methods = {
  apiGet: function () {
    var $this = this;

    $api.get($url).then(function (response) {
      var res = response.data;

      $this.chartData = {
        labels: res.days,
        datasets: [
          {
            label: "管理端访问拦截次数",
            backgroundColor: "#67C23A",
            data: res.adminCount
          },
          {
            label: "用户端访问拦截次数",
            backgroundColor: "#ff6a00",
            data: res.userCount
          }

        ]
      };
    })
      .catch(function (error) {
        utils.error(error);
      })
      .then(function () {
        utils.loading($this, false);
      });
  }
};
Vue.component("line-chart", {
  extends: VueChartJs.Line,
  mounted: function () {
    this.renderChart(
      this.$root.chartData,
      {
        responsive: true,
        maintainAspectRatio: false,
        scales: {
          yAxes: [
            {
              ticks: {
                beginAtZero: true
              }
            }
          ]
        }
      }
    );
  }
});

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  }
});
