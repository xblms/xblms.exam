var $url = '/index';

var data = utils.init({
  user: null,
  openMenus: [],
  version: null
});

var methods = {
  apiGet: function () {
    var $this = this;

    $api.get($url).then(function (response) {
      var res = response.data;
      if (res.user) {
        $this.user = res.user;
        $this.openMenus = res.openMenus;
        $this.version = res.version;
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnTab: function (common) {

    var $this = this;

    if (common === 'info') {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getRootUrl('profile'),
        width: "100%",
        height: "100%",
        end: function () {
          $this.setDocumentTitle();
          $this.apiGet();
        }
      });
    }
    if (common === 'pwd') {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getRootUrl('password'),
        width: "100%",
        height: "100%",
        end: function () {
          $this.setDocumentTitle();
        }
      });
    }
    if (common === 'event') {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getRootUrl('event'),
        width: "100%",
        height: "100%",
        end: function () {
          $this.setDocumentTitle();
        }
      });
    }
    if (common === 'logout') {
      location.href = utils.getRootUrl("logout");
    }
    if (common === 'shuati') {
      location.href = utils.getExamUrl("examPracticeLog");
    }
    if (common === 'cer') {
      location.href = utils.getExamUrl("examPaperCer");
    }
    if (common === 'score') {
      location.href = utils.getExamUrl("examPaperScore");
    }
  },
  setDocumentTitle: function () {
    top.document.title = "我的";
  }
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.setDocumentTitle();
    this.apiGet();
  }
});
