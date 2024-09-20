var $url = "/exam/examPracticeLog";
var $urlDelete = $url + "/del";

var data = utils.init({
  form: {
    dateFrom: '',
    dateTo: '',
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  list: [],
  total: 0,
  loadMoreLoading: false
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
        res.list.forEach(paper => {
          $this.list.push(paper);
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
    this.form.pageIndex = 1;
    this.list = [];
    this.apiGet();
  },
  btnLoadMoreClick: function () {
    this.loadMoreLoading = true;
    this.form.pageIndex++;
    this.apiGet();
  },
  goBack: function () {
    location.href = utils.getExamUrl('examPractice');
  },
  goPracticeResult: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examPracticeResult', { id: id }),
      width: "68%",
      height: "88%",
    });
  },
  btnClearClick: function () {
    var $this = this;
    top.utils.alertWarning({
      title: '清空练习记录',
      text: '此操作将清空所有练习记录，确定吗？',
      callback: function () {
        $this.apiClear();
      }
    });
  },
  apiClear: function () {
    var $this = this;

    utils.loading(this, true);
    $api.post($urlDelete).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success("操作成功");
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.apiGet();
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
