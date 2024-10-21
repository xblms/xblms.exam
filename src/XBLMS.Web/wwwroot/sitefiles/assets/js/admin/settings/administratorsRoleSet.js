var $url = '/settings/administratorsRoleAdd/actions/setRole';

var data = utils.init({
  permissionInfo: {},
  adminIds: utils.getQueryIntList("adminIds")
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    var pid = this.adminIds.length == 1 ? this.adminIds[0] : 0;
    $api.get($url, { params: { id: pid } }).then(function (response) {
      var res = response.data;

      $this.permissionInfo = {
        allRoles: res.roles,
        checkedRoles: res.checkedRoles,
        loading: false
      };
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },

  apiSet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.post($url, {
      adminIds: this.adminIds,
      roleIds: this.permissionInfo.checkedRoles
    }).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success("操作成功");
        utils.closeLayer(false);
      }
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },

  btnSubmitClick: function () {
    this.apiSet();
  },
  filterMethod(query, item) {
    return item.label.indexOf(query) > -1;
  },
  btnCancelClick: function () {
    utils.closeLayer(false);
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
