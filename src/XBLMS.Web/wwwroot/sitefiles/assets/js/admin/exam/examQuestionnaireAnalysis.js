var $url = 'exam/examQuestionnaireAnalysis';

var data = utils.init({
  id: utils.getQueryInt('id'),
  paper:null,
  list: null,
  pieChartColors: ['#67c23a', '#1989fa', '#5cb87a', '#e6a23c'],
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url, {
      params: {
        id: this.id
      }
    }).then(function (response) {
      var res = response.data;

      $this.paper = res.item;
      $this.list = res.tmList;

    }).catch(function (error) {
      utils.error(error, { layer:true });
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
