var $url = '/common/userDocQ';

var data = utils.init({
  form: {
    id: utils.getQueryInt('id'),
    dateFrom: '',
    dateTo: '',
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  total: 0,
  list:[]
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url, {
      params: this.form
    }).then(function (response) {
      var res = response.data;
      $this.total = res.total;
      $this.list = res.list;
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  handleCurrentChange: function (val) {
    this.form.pageIndex = val;
    this.apiGet();
  },
  btnSearchClick: function () {
    this.pageIndex = 1;
    this.apiGet();
  }
};
var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  }
});

