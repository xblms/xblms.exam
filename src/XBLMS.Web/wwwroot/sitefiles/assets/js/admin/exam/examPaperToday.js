var $url = '/exam/examPaperToday';

var data = utils.init({
  list: null,
  dateType: utils.getQueryString("dateType"),
  curMouseoverId: 0,
});

var methods = {
  apiGet: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($url, { params: { dateType: this.dateType }}).then(function (response) {
      var res = response.data;

      $this.list = res.items;

    }).catch(function (error) {
      utils.loading($this, false);
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnManagerScoreClick: function (row) {
    utils.openTopLeft(row.title + '-考试成绩', utils.getExamUrl("examPaperManagerScore", { id: row.id }));
  },
  btnManagerAnalysisClick: function (row) {
    utils.openTopLeft(row.title + '-综合统计', utils.getExamUrl("examPaperManagerAnalysis", { id: row.id }));
  },
  btnManagerUserClick: function (row) {
    utils.openTopLeft(row.title + '-考生管理', utils.getExamUrl("examPaperManagerUser", { id: row.id }));
  },
  btnViewClick: function (row) {
    utils.openTopLeft(row.title + '-预览', utils.getCommonUrl("examPaperLayerView", { id: row.id }));
  },
  mouseoverShowIn: function (row, column, cell, event) {
    this.curMouseoverId = row.id;
  },
  mouseoverShowOut: function (row, column, cell, event) {
    this.curMouseoverId = 0;
  },
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  }
});
