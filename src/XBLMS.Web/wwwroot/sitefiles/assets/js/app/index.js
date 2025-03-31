var $url = '/index';


var data = utils.init({
  openMenus: [],
  appMenuActive: "index",
  rightUrl: utils.getRootUrl('dashboard')
});

var methods = {
  apiGet: function () {
    var $this = this;
    $api.get($url).then(function (response) {
      var res = response.data;
      if (res.user) {
        $this.openMenus = res.openMenus;

        $this.btnAppMenuClick("index");
      } else {
        location.href = utils.getRootUrl('login');
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
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
      document.title = '首页';
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
    document.title = '首页';
    this.apiGet();
  }
});
