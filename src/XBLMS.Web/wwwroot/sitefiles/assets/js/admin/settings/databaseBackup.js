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
    utils.loading(this, true);
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


      this.$prompt('请输入秘钥 SecurityKey', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
      }).then(({ value }) => {


        utils.loading($this, true, '正在恢复备份，请稍等...');
        $api.post($url + '/recover', { id: row.id, securityKey: value }).then(function (response) {
          var res = response.data;
          if (res.value) {
            top.utils.alertSuccess({
              title: '已成功恢复',
              callback: function () {
                window.top.location.href = window.top.location.href;
              }
            });
          }
          else {
            utils.error('失败的恢复，详细请查看恢复日志。');
          }
        }).catch(function (error) {
          utils.error(error);
        }).then(function () {
          utils.loading($this, false);
          $this.apiGet();
        });


      }).catch(() => {
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
  }
});

