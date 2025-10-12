var $url = '/syncDatabase';
var $urlVerify = '/syncDatabase/actions/verify';

var data = utils.init({
  pageType: 'prepare',
  databaseVersion: null,
  version: null,
  securityKey: null,
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url).then(function (response) {
      var res = response.data;

      $this.databaseVersion = res.databaseVersion;
      $this.version = res.version;
      if ($this.databaseVersion === $this.version) {
        $this.$prompt('请进入系统根目录，打开 xblms.json 获取 SecurityKey的值', 'SecurityKey验证', {
          confirmButtonText: '确定',
          cancelButtonText: '取消',
        }).then(function(val) {
          $this.securityKey = val.value;
          $this.apiSubmit();
        }).catch(() => {
          top.location.href ="/xblms-admin/"
        });
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  apiSubmit: function () {
    var $this = this;

    this.pageType = 'update';
    $api.post($url, {
      securityKey: this.securityKey
    }).then(function (response) {
      $this.pageType = 'done';
    }).catch(function (error) {
      $this.securityKey = null;
      $this.pageType = 'prepare';
      utils.error(error);
    });
  },

  btnStartClick: function (e) {
    var $this = this;
    e.preventDefault();

    if (this.databaseVersion === this.version && !this.securityKey) {
      this.$prompt('请进入系统根目录，打开 xblms.json 获取 SecurityKey的值', 'SecurityKey验证', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
      }).then(function(val) {
        $this.securityKey = val.value;
        $this.apiSubmit();
      });
    } else {
      this.apiSubmit();
    }
  },
  btnBackClick: function () {
    top.location.href = "/xblms-admin/";
  }
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    document.title = "系统升级";
    this.apiGet();
  }
});
