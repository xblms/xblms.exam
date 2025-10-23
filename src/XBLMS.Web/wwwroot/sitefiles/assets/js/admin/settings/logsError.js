var $url = '/settings/logsError';
var $urlExport = $url + '/actions/export';
var $urlDelete = $url + '/actions/delete';

var data = utils.init({
  items: null,
  count: null,
  formInline: {
    dateFrom: '',
    dateTo: '',
    keyword: '',
    pageIndex: 1,
    pageSize: PER_PAGE
  }
});

var methods = {
  apiGet: function () {
    var $this = this;
    $api.post($url, this.formInline).then(function (response) {
      var res = response.data;
      $this.items = res.items;
      $this.count = res.count;
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  apiDelete: function () {
    var $this = this;
    utils.loading(this, true);
    $api.post($urlDelete).then(function (response) {
      $this.btnSearchClick();
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnDeleteClick: function () {
    var $this = this;
    top.utils.alertDelete({
      title: '清空系统错误日志',
      text: '此操作将会清空系统错误日志，确定吗？',
      button: '清 空',
      callback: function () {
        $this.apiDelete();
      }
    });
  },
  btnLogView: function(logId) {
    top.utils.openLayer({
      title: false,
      closebtn:0,
      url: utils.getRootUrl('errorView', { logId: logId }),
      width: '80%',
      height: '80%'
    });
  },
  btnExportClick() {
    var $this = this;
    this.formInline.pageIndex = 1;
    utils.loading(this, true);
    $api.post($urlExport, this.formInline).then(function (response) {
      var res = response.data;
      window.open(res.value);
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnSearchClick() {
    this.formInline.pageIndex = 1;
    this.apiGet();
  },
  handleCurrentChange: function(val) {
    this.formInline.pageIndex = val;
    this.apiGet();
  },
  btnCloseClick: function() {
    utils.removeTab();
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
