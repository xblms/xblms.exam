var $url = 'settings/database/backup';

var data = utils.init({
  id: utils.getQueryInt('id'),
  form: {
    securityKey: ''
  }
});

var methods = {
  apiRecover: function () {
    var $this = this;

    top.utils.loading($this, true, '正在恢复备份，请稍等...');
    $api.post($url + '/recover', { id: this.id, securityKey: this.form.securityKey }).then(function (response) {
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
        utils.error('失败的恢复，详细请查看恢复日志。', { layer: true });
      }
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      top.utils.loading($this, false);
    });
  },
  btnRecoverClick: function () {
    var $this = this;
    this.$refs.form.validate(function (valid) {
      if (valid) {
        $this.apiRecover();
      }
    });
  }
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    utils.loading(this, false);
  }
});
