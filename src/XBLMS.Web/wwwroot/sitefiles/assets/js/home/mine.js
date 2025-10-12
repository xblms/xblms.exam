var data = utils.init({
  leftWidth: 238,
  winHeight: $(window).height(),
  winWidth: $(window).width(),
  webHomeUserCenterMenuIndex: 1,
  mineUrl: null
});

var methods = {
  btnMenu: function (value) {
    if (this.webHomeUserCenterMenuIndex !== value) {
      this.webHomeUserCenterMenuIndex = value;
      if (value === 11) {
        this.mineUrl = utils.getRootUrl('dashboard');
      }
      if (value === 1) {
        this.mineUrl = utils.getExamUrl('examPaperScore');
      }
      if (value === 2) {
        this.mineUrl = utils.getExamUrl('examPaperCer');
      }
      if (value === 3) {
        this.mineUrl = utils.getGiftUrl('pointsLog');
      }
      if (value === 4) {
        this.mineUrl = utils.getGiftUrl('pointShopLog');
      }
      if (value === 5) {
        this.mineUrl = utils.getRootUrl('profile');
      }
      if (value === 6) {
        this.mineUrl = utils.getRootUrl('password');
      }
    }
  },
  winResize: function () {
    this.winHeight = $(window).height();
    this.btnMenu(11);
  },
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    window.onresize = this.winResize;
    window.onresize();
    utils.loading(this, false);
  }
});
