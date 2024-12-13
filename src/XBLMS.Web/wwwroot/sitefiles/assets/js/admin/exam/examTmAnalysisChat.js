var $url = '/exam/examTmAnalysisChat';

var data = utils.init({
  id: utils.getQueryInt("id"),
  seriesTx: [],
  chartOptionsTx: {
    chart: {
      toolbar: {
        show: true
      },
      height: 350,
      type: 'pie',
    },
    colors: [
      '#FF0000',
      '#FF3333',
      '#FF6666',
      '#FF9999',
      '#FFCCCC',
      '#FF3366',
      '#FF6699',
      '#FF99CC',
    ],
    labels: [],
    responsive: [{
      breakpoint: 480,
      options: {
        chart: {
          width: 200
        },
        legend: {
          position: 'bottom'
        }
      }
    }],
    title: {
      text: '按题型'
    }
  },
  seriesNd: [],
  chartOptionsNd: {
    chart: {
      toolbar: {
        show: true
      },
      height: 350,
      type: 'donut',
    },
    colors: [
      '#FF0000',
      '#FF3333',
      '#FF6666',
      '#FF9999',
      '#FFCCCC',
      '#FF3366',
      '#FF6699',
      '#FF99CC',
    ],
    labels: [],
    responsive: [{
      breakpoint: 480,
      options: {
        chart: {
          width: 200
        },
        legend: {
          position: 'bottom'
        }
      }
    }],
    title: {
      text: '按难度'
    }
  },
  seriesZsd: [
    {
      name: "",
      data: [],
    },
  ],
  chartOptionsZsd: {
    chart: {
      type: 'bar',
      height: 380,
      dropShadow: {
        enabled: true,
      },
    },
    plotOptions: {
      bar: {
        borderRadius: 0,
        horizontal: true,
        barHeight: '80%',
        isFunnel: true,
      },
    },
    colors: [
      '#FF0000',
      '#FF3333',
      '#FF6666',
      '#FF9999',
      '#FFCCCC',
      '#FF3366',
      '#FF6699',
      '#FF99CC',
    ],
    dataLabels: {
      enabled: true,
      formatter: function (val, opt) {
        return opt.w.globals.labels[opt.dataPointIndex] + ':  ' + val
      },
      dropShadow: {
        enabled: true,
      },
    },
    title: {
      text: '按知识点',
      align: 'left',
    },
    xaxis: {
      categories: [
      ],
    },
    legend: {
      show: false,
    },
  },
});

var methods = {
  apiGet: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($url, { params: { id: this.id } }).then(function (response) {
      var res = response.data;

      $this.seriesTx = res.txValues;
      $this.seriesNd = res.ndValues;
      $this.seriesZsd[0].data = res.zsdValues;

      $this.chartOptionsZsd.xaxis.categories = res.zsdLabels;
      $this.chartOptionsTx.labels = res.txLabels;
      $this.chartOptionsNd.labels = res.ndLabels;

    }).catch(function (error) {
      utils.loading($this, false);
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
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
    this.apiGet();
  }
});
