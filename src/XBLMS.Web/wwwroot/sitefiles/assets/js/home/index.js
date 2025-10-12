var $url = '/index';
var $urlResses = $url + '/resses';

var data = utils.init({
  sessionId: sessionStorage.getItem(SESSION_ID_NAME),
  defaultActive: null,
  defaultSrc: null,
  leftWidth: 238,
  winHeight: 0,
  winWidth: 0,

  topFrameDrawer: false,
  topFrameTitle: null,
  topFrameSrc: '',
  topFrameWidth: 88,

  displayName: null,
  avatarUrl: null,
  systemCode: null,
  systemCodeName: null
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
        $this.displayName = res.displayName;
        $this.avatarUrl = res.avatarUrl;
        $this.systemCode = res.systemCode;
        $this.systemCodeName = res.systemCodeName;
        document.title = res.systemCodeName;
        top.utils.pointNotice(res.pointNotice);
        setTimeout($this.ready, 100);
      }
      else {
        location.href = res.redirectUrl;
      }

    }).catch(function (error) {
      utils.error(error);
    });
  },
  ready: function () {
    window.onresize = this.winResize;
    window.onresize();
    utils.loading(this, false);
    this.btnTopMenuClick("index");
    setInterval(this.apiReSession, 5000);
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
  winResize: function () {
    this.winHeight = $(window).height();
    this.winWidth = $(window).width();
    this.isDesktop = this.winWidth > 992;
  },
  btnTopMenuClick: function (command) {
    this.defaultActive = command;
    if (command === 'index') {
      this.defaultSrc = utils.getRootUrl("dotaskindex");
    }
    if (command === 'study') {
      this.defaultSrc = utils.getStudyUrl("studyIndex");
    }
    if (command === 'exam') {
      this.defaultSrc = utils.getExamUrl("examPaper");
    }
    if (command === 'more') {
      this.defaultSrc = utils.getRootUrl("moreIndex");
    }
    if (command === 'mine') {
      this.defaultSrc = utils.getRootUrl("mine");
    }
  },
  loginOut: function () {
    top.utils.alertWarning({
      title: '安全退出',
      text: '确定要退出系统吗？',
      callback: function () {
        sessionStorage.removeItem(ACCESS_TOKEN_NAME);
        localStorage.removeItem(ACCESS_TOKEN_NAME);
        window.top.location.href = utils.getRootUrl("login");
      }
    })
  },
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  }
});
