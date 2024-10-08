var $url = '/exam/examPaperMark';

var data = utils.init({
  form: {
    keywords: '',
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  list: null,
  total: 0,
});

var methods = {
  apiGet: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($url, { params: $this.form }).then(function (response) {
      var res = response.data;
      $this.list = res.list;
      $this.total = res.total;

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },

  btnMarkSearchClick: function () {
    this.form.pageIndex = 1;
    this.apiGet();
  },
  btnPaperMarkView: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('examPaperUserMark', { id: id }),
      width: "99%",
      height: "99%",
      end: function () {
        $this.btnMarkSearchClick();
      }
    });
  },
  markHandleCurrentChange: function (val) {
    this.form.pageIndex = val;
    this.apiGet();
  },
};
var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  }
});
