var $url = 'settings/database/backup';

var data = utils.init({
  formInline: {
    pageIndex: 1,
    pageSize: 10
  },
  formRecover: {
    pageIndex: 1,
    pageSize: 10,
  },
  list: [],
  total: 0,
  recoverList: [],
  recoverTotal: 0,
  uploadBackupUrl: null,
  fileList: [],
  showFileList: true
});


var methods = {
  apiGet: function () {
    var $this = this;
    utils.loading(this, true);
    $api.post($url, $this.formInline).then(function (response) {
      var res = response.data;

      $this.list = res.list;
      $this.total = res.total;

    }).catch(function (error) {
      utils.loading($this, false);
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.apiGetRecover();
    });
  },
  apiGetRecover: function () {
    var $this = this;
    utils.loading(this, true);
    $api.post($url + '/recoverlog', $this.formRecover).then(function (response) {
      var res = response.data;
      $this.recoverList = res.list;
      $this.recoverTotal = res.total;
    }).catch(function (error) {
      utils.loading($this, false);
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  searchList: function () {
    this.formInline.pageIndex = 1;
    this.apiGet();
  },
  handleSizeChange(val) {
    this.formInline.pageSize = val;
    this.apiGet();
  },
  handleCurrentChange: function (val) {
    this.formInline.pageIndex = val;
    this.apiGet();
  },
  handleSizeChangeRecover(val) {
    this.formRecover.pageSize = val;
    this.apiGetRecover();
  },
  handleCurrentChangeRecover: function (val) {
    this.formRecover.pageIndex = val;
    this.apiGetRecover();
  },
  btnAsyncClick: function () {
    var $this = this;
    utils.loading(this, true, "正在备份，请稍等...");
    $api.get($url + '/excution').then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success("备份成功");
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.apiGet();
    });
  },
  handleCommand: function (type, row) {
    var $this = this;
    if (type === 'recover') {

      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getSettingsUrl('databaseBackupRecover', { id: row.id }),
        width: "38%",
        height: "58%",
        end: function () {
          $this.apiGet();
        }
      });
    }
    if (type === 'delete') {
      top.utils.alertDelete({
        title: '删除备份文件',
        text: '确认删除吗？',
        callback: function () {
          $this.apiDelete(row.id);
        }
      });
    }
    if (type === 'download') {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getSettingsUrl('databaseBackupDownload', { id: row.id }),
        width: "38%",
        height: "58%"
      });
    }
  },
  btnUpload: function () {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getSettingsUrl('databaseBackupUpload'),
      width: "58%",
      height: "88%",
      end: function () {
        $this.apiGet();
      }
    });
  },
  apiDelete: function (id) {
    var $this = this;
    utils.loading($this, true);
    $api.post($url + '/delbackup', { id: id }).then(function (response) {
      var res = response.data;
      utils.success("删除成功");
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.apiGet();
    });
  },
  btnClearLogClick: function () {
    var $this = this;
    top.utils.alertDelete({
      title: '删除数据库恢复日志',
      text: '确认删除吗？',
      callback: function () {
        utils.loading($this, true);
        $api.post($url + '/recoverlogdel').then(function (response) {
          var res = response.data;
          utils.success("操作成功");
        }).catch(function (error) {
          utils.error(error);
        }).then(function () {
          utils.loading($this, false);
          $this.apiGetRecover();
        });
      }
    });
  }
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
    this.uploadBackupUrl = $apiUrl + '/' + $url + '/upload';
  }
});

