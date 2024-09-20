var $url = "/entryExamClient";

var data = utils.init({
  token: utils.getQueryString('token'),
  id: utils.getQueryInt('id'),
});

var methods = {
  apiGet: function() {
    this.setEntryToken();
    var examUrl = utils.getExamUrl("examPaperInfo", { isCientIn: true, id: this.id });
    location.href = examUrl;
  },
  setEntryToken: function () {
    sessionStorage.removeItem(ACCESS_TOKEN_NAME);
    localStorage.removeItem(ACCESS_TOKEN_NAME);
    localStorage.setItem(ACCESS_TOKEN_NAME, this.token);
    sessionStorage.setItem(ACCESS_TOKEN_NAME, this.token);
  },
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    document.title = "正在授权跳转...";
    utils.loading(this, false);
    //utils.loading(this, true, '正在授权跳转...');
    this.apiGet();
  },
});
