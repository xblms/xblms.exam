var $url = "/pointShopLog";

var data = utils.init({
  form: {
    dateFrom: '',
    dateTo: '',
    keyWords: '',
    pageIndex: 1,
    pageSize: PER_PAGE
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
  }
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    top.document.title = "兑换记录";
    this.apiGet();
  },
});
