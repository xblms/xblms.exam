var $url = "/gifts";

var data = utils.init({
  form: {
    point:0,
    keywords: '',
    pageIndex: 1,
    pageSize: 24
  },
  list: [],
  total: 0,
  pushTotal: 0,
  loadMoreLoading: false,
});

var methods = {
  apiGet: function () {
    var $this = this;

    if (this.total === 0) {
      utils.loading(this, true);
    }
    $api.get($url, { params: this.form }).then(function (response) {
      var res = response.data;

      if (res.list && res.list.length > 0) {
        res.list.forEach(gift => {
          $this.list.push(gift);
          $this.pushTotal++;
        });
      }
      $this.total = res.total;

    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.loadMoreLoading = false;
    });
  },
  btnSearchClick: function () {
    this.pushTotal = 0;
    this.form.pageIndex = 1;
    this.list = [];
    this.apiGet();
  },
  btnLoadMoreClick: function () {
    this.loadMoreLoading = true;
    this.form.pageIndex++;
    this.apiGet();
  },
  btnViewClick: function (row) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getGiftUrl("giftView", { id: row.id }),
      width: "100%",
      height: "100%"
    });
  }
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  },
});
