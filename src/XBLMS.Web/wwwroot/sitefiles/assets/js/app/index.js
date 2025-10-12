var $url = '/index';
var $urlResses = $url + '/resses';

var data = utils.init({
  sessionId: sessionStorage.getItem(SESSION_ID_NAME),
  openMenus: [],
  appMenuActive: "index",
  rightUrl: utils.getRootUrl('dashboard'),
  systemCode: null
});

var methods = {
  apiGet: function () {
    var $this = this;
    $api.get($url, {
      params: {
        sessionId: this.sessionId
      }
    }).then(function (response) {
      var res = response.data;
      if (res.value) {

        $this.systemCode = res.systemCode;
        top.utils.pointNotice(res.pointNotice);
        $this.btnAppMenuClick("index");

        setInterval($this.apiReSession, 5000);
      } else {
        location.href = utils.getRootUrl('login');
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  apiReSession: function () {
    $api.get($urlResses, {
      params: {
        sessionId: this.sessionId
      }
    }).then(function () {
    }).catch(function (error) {
      utils.error(error);
    });
  },
  btnAppMenuClick: function (common) {

    var $this = this;

    $this.appMenuActive = common;

    if (common === 'index') {
      document.title = '待办';
      $this.rightUrl = utils.getRootUrl("dotask");
    }
    if (common === 'exam') {
      document.title = '考试中心';
      $this.rightUrl = utils.getExamUrl("examPaper");
    }
    if (common === 'studyPlan') {
      document.title = '学习任务';
      $this.rightUrl = utils.getStudyUrl("studyIndex");
    }
    if (common === 'more') {
      document.title = '更多';
      $this.rightUrl = utils.getRootUrl("moreIndex");
    }
    if (common === 'mine') {
      document.title = '我的';
      $this.rightUrl = utils.getRootUrl('mine');
    }
  }
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  }
});
