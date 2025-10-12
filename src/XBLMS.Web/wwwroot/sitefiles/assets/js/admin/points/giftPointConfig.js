var $url = '/points/pointshop/config';

var data = utils.init({
  systemCode: null,
  form: {
    pointLogin: 0,
    pointLoginDayMax: 0,
    pointPlanOver: 0,
    pointPlanOverDayMax: 0,
    pointVideo: 0,
    pointVideoDayMax: 0,
    pointDocument: 0,
    pointDocumentDayMax: 0,
    pointCourseOver: 0,
    pointCourseOverDayMax: 0,
    pointEvaluation: 0,
    pointEvaluationDayMax: 0,
    pointExam: 0,
    pointExamDayMax: 0,
    pointExamPass: 0,
    pointExamPassDayMax: 0,
    pointExamFull: 0,
    pointExamFullDayMax: 0,
    pointExamQ: 0,
    pointExamQDayMax: 0,
    pointExamAss: 0,
    pointExamAssDayMax: 0,
    pointExamPractice: 0,
    pointExamPracticeDayMax: 0,
    pointExamPracticeRight: 0,
    pointExamPracticeRightDayMax: 0
  }
});

var methods = {
  apiGet: function () {
    var $this = this;

    $api.get($url).then(function (response) {
      var res = response.data;
      $this.systemCode = res.systemCode;
      $this.form.pointLogin = res.pointLogin;
      $this.form.pointLoginDayMax = res.pointLoginDayMax;
      $this.form.pointPlanOver = res.pointPlanOver;
      $this.form.pointPlanOverDayMax = res.pointPlanOverDayMax;
      $this.form.pointVideo = res.pointVideo;
      $this.form.pointVideoDayMax = res.pointVideoDayMax;
      $this.form.pointDocument = res.pointDocument;
      $this.form.pointDocumentDayMax = res.pointDocumentDayMax;
      $this.form.pointCourseOver = res.pointCourseOver;
      $this.form.pointCourseOverDayMax = res.pointCourseOverDayMax;
      $this.form.pointEvaluation = res.pointEvaluation;
      $this.form.pointEvaluationDayMax = res.pointEvaluationDayMax;
      $this.form.pointExam = res.pointExam;
      $this.form.pointExamDayMax = res.pointExamDayMax;
      $this.form.pointExamPass = res.pointExamPass;
      $this.form.pointExamPassDayMax = res.pointExamPassDayMax;
      $this.form.pointExamFull = res.pointExamFull;
      $this.form.pointExamFullDayMax = res.pointExamFullDayMax;
      $this.form.pointExamQ = res.pointExamQ;
      $this.form.pointExamQDayMax = res.pointExamQDayMax;
      $this.form.pointExamAss = res.pointExamAss;
      $this.form.pointExamAssDayMax = res.pointExamAssDayMax;
      $this.form.pointExamPractice = res.pointExamPractice;
      $this.form.pointExamPracticeDayMax = res.pointExamPracticeDayMax;
      $this.form.pointExamPracticeRight = res.pointExamPracticeRight;
      $this.form.pointExamPracticeRightDayMax = res.pointExamPracticeRightDayMax;
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  apiSubmit: function () {
    var $this = this;

    utils.loading(this, true);
    $api.post($url, {
      pointLogin: this.form.pointLogin,
      pointLoginDayMax: this.form.pointLoginDayMax,
      pointPlanOver: this.form.pointPlanOver,
      pointPlanOverDayMax: this.form.pointPlanOverDayMax,
      pointVideo: this.form.pointVideo,
      pointVideoDayMax: this.form.pointVideoDayMax,
      pointDocument: this.form.pointDocument,
      pointDocumentDayMax: this.form.pointDocumentDayMax,
      pointCourseOver: this.form.pointCourseOver,
      pointCourseOverDayMax: this.form.pointCourseOverDayMax,
      pointEvaluation: this.form.pointEvaluation,
      pointEvaluationDayMax: this.form.pointEvaluationDayMax,
      pointExam: this.form.pointExam,
      pointExamDayMax: this.form.pointExamDayMax,
      pointExamPass: this.form.pointExamPass,
      pointExamPassDayMax: this.form.pointExamPassDayMax,
      pointExamFull: this.form.pointExamFull,
      pointExamFullDayMax: this.form.pointExamFullDayMax,
      pointExamQ: this.form.pointExamQ,
      pointExamQDayMax: this.form.pointExamQDayMax,
      pointExamAss: this.form.pointExamAss,
      pointExamAssDayMax: this.form.pointExamAssDayMax,
      pointExamPractice: this.form.pointExamPractice,
      pointExamPracticeDayMax: this.form.pointExamPracticeDayMax,
      pointExamPracticeRight: this.form.pointExamPracticeRight,
      pointExamPracticeRightDayMax: this.form.pointExamPracticeRightDayMax
    }).then(function (response) {
      var res = response.data;

      utils.success('操作成功！');
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  btnSubmitClick: function () {
    var $this = this;
    this.$refs.form.validate(function (valid) {
      if (valid) {
        $this.apiSubmit();
      }
    });
  }
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  }
});
