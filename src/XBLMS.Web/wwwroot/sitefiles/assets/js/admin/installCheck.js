var data = utils.init({
  xblmscheck: utils.getQueryBoolean('xblmscheck'),
  par: utils.getQueryString("par")
});

var $vue = new Vue({
  el: "#main",
  data: data,
  created: function () {
    utils.loading(this, false);
  },
});
