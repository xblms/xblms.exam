var $url = '/logout';

var data = utils.init({
  returnUrl: utils.getQueryString('returnUrl')
});

var methods = {
  logout: function () {
    var $this = this;
    top.utils.alertWarning({
      title: '安全退出',
      text: '确定要退出登录吗？',
      callback: function () {
        localStorage.removeItem(ACCESS_TOKEN_NAME);
        $this.redirect();
      }
    })

  },

  redirect: function () {
    if (this.returnUrl) {
      window.top.location.href = this.returnUrl;
    } else {
      window.top.location.href = utils.getIndexUrl();
    }
  }
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    top.document.title = "退出登录";
    utils.loading(this, false);
  }
});
