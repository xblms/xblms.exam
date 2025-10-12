var $url = 'exam/examQuestionnaireAnalysis';
var $urlExportWord = $url +'/exportWord';

var data = utils.init({
  id: utils.getQueryInt('id'),
  planId: utils.getQueryInt('planId'),
  courseId: utils.getQueryInt('courseId'),
  paper: null,
  list: null,
  pieChartColors: ['#67c23a', '#1989fa', '#5cb87a', '#e6a23c'],
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url, {
      params: {
        id: this.id, planId: this.planId, courseId: this.courseId
      }
    }).then(function (response) {
      var res = response.data;

      $this.paper = res.item;
      $this.list = res.tmList;

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnExportWordClick: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($urlExportWord, {
      params: {
        id: this.id, planId: this.planId, courseId: this.courseId
      } }).then(function (response) {
      var res = response.data;

      window.open(res.value);
    }).catch(function (error) {
      utils.error(error, { layer: true });
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
