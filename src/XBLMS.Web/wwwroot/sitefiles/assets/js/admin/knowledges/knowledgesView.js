var data = utils.init({
  url: utils.getQueryString("url")
});

var methods = {
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    utils.loading(this, false);
  }
});
