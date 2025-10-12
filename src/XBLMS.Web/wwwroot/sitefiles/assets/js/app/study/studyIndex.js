var data = utils.init({
  winHeight: $(window).height(),
  studyCurrent: null,
  studyUrl: null,
  studyMenuPop: false
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
        this.studyUrl = utils.getStudyUrl('studyCourseLog');
      }
      this.studyMenuPop = false;
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
