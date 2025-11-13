var $url = '/logout';

var data = utils.init({});

var methods = {
  apiSubmit: function () {
    var $this = this;
    utils.loading(this, true);
    $api.post($url).then(function (response) {
      sessionStorage.removeItem(ACCESS_TOKEN_NAME);
      localStorage.removeItem(ACCESS_TOKEN_NAME);
      $this.redirect();
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  redirect: function () {
    window.top.location.href = utils.getRootUrl("login");
  }
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.apiSubmit();
  }
});
