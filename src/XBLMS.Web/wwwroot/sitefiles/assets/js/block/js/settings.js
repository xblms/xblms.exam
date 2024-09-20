var $url = '/settings/block/settings';
var $urlDelete = $url + '/actions/delete';

var data = utils.init({
  rules: null,
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url).then(function (response) {
      var res = response.data;

      $this.rules = res.rules;
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  apiDelete: function (item) {
    var $this = this;

    utils.loading(this, true);
    $api.post($urlDelete, {
      ruleId: item.id
    }).then(function (response) {
      var res = response.data;

      $this.rules = res.rules;
      utils.success('拦截规则删除成功');
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnEdit: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getSettingsUrl('blockAdd', {
        id: id,
      }),
      width: "68%",
      height: "88%",
      end: function () {
        $this.apiGet();
      }
    });
  },

  btnDeleteClick: function (item) {
    var $this = this;

    top.utils.alertDelete({
      title: '删除拦截规则',
      text: '此操作将删除拦截规则' + item.ruleName + '，确定吗？',
      callback: function () {
        $this.apiDelete(item);
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
