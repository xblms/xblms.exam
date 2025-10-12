var data = utils.init({
  leftWidth: 238,
  winHeight: $(window).height(),
  winWidth: $(window).width(),
  studyCurrent: null,
  studyUrl: null
});

var methods = {
  btnStudyClick: function (value) {
    if (this.studyCurrent !== value) {
      this.studyCurrent = value;
      if (value === 'plan') {
        this.studyUrl = utils.getStudyUrl('studyPlan');
      }
      if (value === 'course') {
        this.studyUrl = utils.getStudyUrl('studyCourse');
      }
      if (value === 'last') {
        this.studyUrl = utils.getStudyUrl('studyCourseLast');
      }
      if (value === 'collect') {
        this.studyUrl = utils.getStudyUrl('studyCourseCollect');
      }
    }
  }
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    utils.loading(this, false);
    this.btnStudyClick("plan")
  }
});
