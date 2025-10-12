var $url = '/study/studyPlanMonth';

var data = utils.init({
  isOver: utils.getQueryBoolean('isOver'),
  list: null,
  total: 0,
  curMouseoverId: 0,
});

var methods = {
  apiGet: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($url, { params: { isOver: this.isOver } }).then(function (response) {
      var res = response.data;

      $this.list = res.list;
      $this.total = res.total;

    }).catch(function (error) {
      utils.loading($this, false);
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnManagerAnalysisClick: function (row) {
    utils.openTopLeft(row.planName + '-综合统计', utils.getStudyUrl("studyPlanManagerAnalysis", { id: row.id }));
  },
  btnManagerUserClick: function (row) {
    utils.openTopLeft(row.planName + '-学习情况', utils.getStudyUrl("studyPlanManagerUser", { id: row.id }));
  },
  btnManagerScoreClick: function (row) {
    utils.openTopLeft(row.planName + '-考试成绩', utils.getExamUrl("examPaperManagerScore", { id: row.examId,planId:row.id }));
  },
  btnManagerCourseClick: function (id) {

    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getStudyUrl('studyPlanManagerCourse', { id: id }),
      width: "100%",
      height: "100%"
    });

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
