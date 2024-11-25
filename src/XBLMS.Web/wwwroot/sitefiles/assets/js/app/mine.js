var $url = '/index';


var data = utils.init({
  user: null,
  appMenuActive: "mine"
});

var methods = {
  apiGet: function () {
    var $this = this;

    $api.get($url).then(function (response) {
      var res = response.data;
      if (res.user) {
        $this.user = res.user;
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnAppMenuClick: function (common) {
    if (common === 'index') {
      location.href = utils.getIndexUrl();
    }
    if (common === 'exam') {
      location.href = utils.getExamUrl("examPaper");
    }
    if (common === 'wenjuan') {
      location.href = utils.getExamUrl("examQuestionnaire");
    }
    if (common === 'mine') {
      location.href = utils.getRootUrl('mine');
    }
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
    document.title = "用户中心";
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
