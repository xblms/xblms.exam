var $url = '/study/studyOffCourseWeek';

var data = utils.init({
  list: null,
  total: 0,
  curMouseoverId: 0,
});

var methods = {
  apiGet: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($url, { params: $this.formInline }).then(function (response) {
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
  btnCourseManagerAnalysisClick: function (row) {
    utils.openTopLeft(row.name + '-综合统计', utils.getStudyUrl("studyCourseManagerAnalysis", { id: row.id, planId: row.planId }));
  },
  btnManagerScoreClick: function (row) {
    utils.openTopLeft(row.name + '-考试成绩', utils.getExamUrl("examPaperManagerScore", { id: row.examId, courseId: row.id, planId: row.planId }));
  },
  btnManagerQClick: function (row) {
    utils.openTopLeft(row.name + '-调查结果', utils.getExamUrl("examQuestionnaireAnalysis", { id: row.examQuestionnaireId, courseId: row.id, planId: row.planId }));
  },
  btnManagerEvaluationClick: function (row) {
    utils.openTopLeft(row.name + '-课程评价', utils.getStudyUrl("studyCourseManagerEvaluation", { id: row.id, planId: row.planId }));
  },
  btnManagerUserClick: function (row) {
    utils.openTopLeft(row.name + '-学习情况', utils.getStudyUrl("studyCourseManagerUser", { id: row.id, planId: row.planId }));
  },
  mouseoverShowIn: function (row, column, cell, event) {
    this.curMouseoverId = row.guid;
  },
  mouseoverShowOut: function (row, column, cell, event) {
    this.curMouseoverId = '';
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
