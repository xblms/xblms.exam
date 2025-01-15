var $url = 'settings/database/backup';

var data = utils.init({
  id: utils.getQueryInt('id'),
  form: {
    securityKey: ''
  }
});

var methods = {
  apiDownload: function () {
    var $this = this;

    top.utils.loading($this, true, '正在下载，请稍等...');
    $api.get($url + '/download', { params: { id: this.id, securityKey: this.form.securityKey } }).then(function (response) {
      var res = response.data;
      window.open(res.value);
      utils.closeLayerSelf();
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      top.utils.loading($this, false);
    });
  },
  btnDownloadClick: function () {
    var $this = this;
    this.$refs.form.validate(function (valid) {
      if (valid) {
        $this.apiDownload();
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
