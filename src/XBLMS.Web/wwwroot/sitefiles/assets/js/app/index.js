var $url = '/index';


var data = utils.init({
});

var methods = {
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    document.title = '首页';
    location.href = utils.getRootUrl("dashboard");
  }
});
