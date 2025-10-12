var $url = '/study/studyPlanManager';

var $urlCourse = $url + '/course';
var $urlCourseExport = $urlCourse + '/export';


var data = utils.init({
  id: 0,
  planName: null,
  formCourse: {
    id: 0,
    keyWords: '',
  },

  frameDrawer: false,
  frameTitle: null,
  frameSrc: '',
});

var methods = {
  apiGet: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($urlCourse, { params: $this.formCourse }).then(function (response) {
      var res = response.data;
      $this.courseList = res.list;
      $this.planName = res.planName;

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnCourseSearchClick: function () {
    this.apiGet();
  },
  btnCourseExportClick: function () {
    var $this = this;

    utils.loading(this, true);
    $api.post($urlCourseExport, this.formCourse).then(function (response) {
      var res = response.data;

      window.open(res.value);
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnCourseManagerAnalysisClick: function (row) {
    this.openLeft(row.courseName + '-综合统计', utils.getStudyUrl("studyCourseManagerAnalysis", { id: row.courseId, planId: row.planId }));
  },
  btnManagerScoreClick: function (row) {
    this.openLeft(row.courseName + '-考试成绩', utils.getExamUrl("examPaperManagerScore", { id: row.examId, courseId: row.courseId, planId: row.planId }));
  },
  btnManagerQClick: function (row) {
    this.openLeft(row.courseName + '-调查结果', utils.getExamUrl("examQuestionnaireAnalysis", { id: row.examQuestionnaireId, courseId: row.courseId, planId: row.planId }));
  },
  btnManagerEvaluationClick: function (row) {
    this.openLeft(row.courseName + '-课程评价', utils.getStudyUrl("studyCourseManagerEvaluation", { id: row.courseId, planId: row.planId }));
  },
  btnManagerUserClick: function (row) {
    this.openLeft(row.courseName + '-学习情况', utils.getStudyUrl("studyCourseManagerUser", { id: row.courseId, planId: row.planId }));
  },

  openLeft: function (title, src) {
    this.frameTitle = title;
    this.frameSrc = src;
    this.frameDrawer = true;
  },
  closeLeft: function () {
    this.frameDrawer = false;
  },
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.id = this.formCourse.id = utils.getQueryInt("id");
    this.apiGet();
  }
});
