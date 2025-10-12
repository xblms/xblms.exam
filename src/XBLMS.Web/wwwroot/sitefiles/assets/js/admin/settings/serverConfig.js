var $url = '/settings/serverConfig';

var data = utils.init({
  systemCodeOld: null,
  form: {
    bushuFilesServer: null,
    bushuFilesServerUrl: null,
    systemCode: null,
    systemCodeName: null
  }
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url).then(function (response) {
      var res = response.data;

      $this.form.bushuFilesServer = res.bushuFilesServer;
      $this.form.bushuFilesServerUrl = res.bushuFilesServerUrl;
      $this.form.systemCode = $this.form.systemCodeOld = res.systemCode;
      $this.form.systemCodeName = res.systemCodeName;

    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  apiSubmit: function () {
    var $this = this;

    utils.loading(this, true);
    $api.post($url, {
      bushuFilesServer: this.form.bushuFilesServer,
      bushuFilesServerUrl: this.form.bushuFilesServerUrl,
      systemCode: this.form.systemCode,
      systemCodeName: this.form.systemCodeName
    }).then(function (response) {
      var res = response.data;

      utils.success('操作成功！');

      if ($this.systemCode !== $this.form.systemCodeOld) {
        top.location.href = utils.getIndexUrl();
      }

    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  btnSubmitClick: function () {
    var $this = this;

    this.$refs.form.validate(function (valid) {
      if (valid) {
        $this.apiSubmit();
      }
    });
  },

  btnCloseClick: function () {
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
