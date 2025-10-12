var $url = "/exam/examPractice/readyZsd";

var data = utils.init({
  windowName: utils.getQueryString("windowName"),
  zsdSelect: [],
  order:"total",
  zsdList: null,
});

var methods = {
  apiGet: function () {
    var $this = this;

    var parentLayer = top.frames[this.windowName];
    var tmGroupIds = parentLayer.$vue.tmGroupIds;
    var txIds = parentLayer.$vue.form.txIds;
    var nds = parentLayer.$vue.form.nds;

    $api.post($url, { tmGroupIds: tmGroupIds, txIds: txIds, nds: nds, order: this.order }).then(function (response) {
      var res = response.data;
      $this.zsdList = res.zsdList;
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },

  btnSubmitClick: function () {
    if (this.zsdSelect.length > 0) {
      if (this.zsdSelect.length > 100) {
        utils.error("不要选这么多，没什么意义！", { layer: true });
      }
      else {
        var parentLayer = top.frames[this.windowName];
        parentLayer.$vue.selectZsdsCallback(this.zsdSelect);
        utils.closeLayerSelf();
      }
    }
    else {
      utils.error("请至少选择一个知识点", { layer: true });
    }
  }
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  },
});
