var $url = '/index';


var data = utils.init({
  appMenuActive: "index",
  rightUrl: utils.getRootUrl('dashboard')
});

var methods = {
  btnAppMenuClick: function (common) {

    var $this = this;

    $this.appMenuActive = common;

    if (common === 'index') {
      document.title = '首页';
      $this.rightUrl = utils.getRootUrl("dashboard");
    }
    if (common === 'exam') {
      document.title = '考试中心';
      $this.rightUrl = utils.getExamUrl("examPaper");
    }
    if (common === 'moni') {
      document.title = '模拟考试中心';
      $this.rightUrl = utils.getExamUrl("examPaperMoni");
    }
    if (common === 'shuati') {
      document.title = '刷题练习';
      $this.rightUrl = utils.getExamUrl("examPractice");
    }
    if (common === 'mine') {
      document.title = '用户中心';
      $this.rightUrl = utils.getRootUrl('mine');
    }

    $this.$nextTick(() => {
      $this.$refs.homeRightIframe.src = $this.rightUrl;
    })

  }
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    utils.loading(this, false);
    document.title = '首页';
    this.btnAppMenuClick("index");
  }
});
