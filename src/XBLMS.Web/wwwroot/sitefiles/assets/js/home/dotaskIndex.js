var data = utils.init({
  leftWidth: 238,
  winHeight: $(window).height(),
  winWidth: $(window).width(),
  rightIndex: 0,
  rightUrl: null
});

var methods = {
  btnMenu: function (value) {
    this.rightIndex = value;
    if (value === 0) {
      this.rightUrl = utils.getRootUrl("dotask");
    }
    if (value === 1) {
      this.rightUrl = utils.getRootUrl("event");
    }
  },
  winResize: function () {
    this.winHeight = $(window).height();
  },
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.rightUrl = utils.getRootUrl("dotask");
    window.onresize = this.winResize;
    window.onresize();
    utils.loading(this, false);
  }
});
