var $url = '/clearDatabase';

var data = utils.init({
  securityKey: null,
});

var methods = {
  apiGet: function () {
    var $this = this;

    $this.$prompt('请进入系统根目录，打开 xblms.json 获取 SecurityKey的值', 'SecurityKey验证', {
      confirmButtonText: '确定',
      cancelButtonText: '取消',
    }).then(function (val) {
      $this.securityKey = val.value;
      $this.apiSubmit();
    }).catch(() => {
      top.location.href = utils.getIndexUrl();
    });;
  },

  apiSubmit: function () {
    var $this = this;
    utils.loading(this, true, "正在清理数据");
    $api.post($url, {
      securityKey: this.securityKey
    }).then(function (response) {
      top.location.href = utils.getIndexUrl();
    }).catch(function (error) {
      utils.loading(this, false);
      $this.securityKey = null;
      utils.error(error);
    });
  },

};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    document.title = "清空数据";
    utils.loading(this, false);
    this.apiGet();
  }
});
