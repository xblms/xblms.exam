var data = utils.init({
  leftWidth: 238,
  winHeight: $(window).height(),
  rightIndex: 0,
  rightUrl: null,
  studyMenuPop: false
});

var methods = {
  btnMenu: function (value) {
    this.rightIndex = value;
    if (value === 0) {
      this.rightUrl = utils.getExamUrl("examPaperMoni");
    }
    if (value === 1) {
      this.rightUrl = utils.getExamUrl("examPractice");
    }
    if (value === 2) {
      this.rightUrl = utils.getExamUrl("examQuestionnaire");
    }
    if (value === 3) {
      this.rightUrl = utils.getExamUrl("examAssessment");
    }
    if (value === 4) {
      this.rightUrl = utils.getExamUrl("examPk");
    }
    if (value === 5) {
      this.rightUrl = utils.getKnowledgesUrl("knowledges");
    }
    this.studyMenuPop = false;
  },
  winResize: function () {
    this.winHeight = $(window).height();
  },
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.rightUrl = utils.getExamUrl("examPaperMoni");
    window.onresize = this.winResize;
    window.onresize();
    utils.loading(this, false);
  }
});
