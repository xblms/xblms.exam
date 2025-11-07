var $url = '/settings/usersGroup';
var $urlDelete = $url + '/actions/delete';

var data = utils.init({
  groups: null,
  search: ''
});

var methods = {
  apiGet: function (message) {
    var $this = this;

    utils.loading(this, true);
    $api.get($url, { params: { search: this.search } }).then(function (response) {
      var res = response.data;
      $this.groups = res.groups;
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      if (message) {
        utils.success(message);
      }
    });
  },
  btnSearch: function () {
    this.apiGet();
  },
  btnRangeClick: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getSettingsUrl('usersGroupRange', { groupId: id }),
      width: "99%",
      height: "99%",
      end: function () { $this.apiGet() }
    });
  },
  btnEditClick: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getSettingsUrl('usersGroupEdit', { id: id }),
      width: "60%",
      height: "88%",
      end: function () { $this.apiGet() }
    });
  },
  apiDelete: function (id) {
    var $this = this;
    utils.loading(this, true);
    $api.post($urlDelete, {
      id: id
    }).then(function (response) {
      utils.success('操作成功！');
      $this.apiGet();
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnAdminViewClick: function (id) {
    utils.openAdminView(id);
  },
  btnListClick: function (group) {
    utils.openTopLeft('用户列表：' + group.groupName, utils.getSettingsUrl("usersGroupUserList", { groupId: group.id }));
  },
  btnDeleteClick: function (group) {
    var $this = this;
    if (group.useCount > 0) {
      utils.error("不能删除被使用的组");
    }
    else {
      top.utils.alertDelete({
        title: '删除用户组',
        text: '此操作将删除用户组 ' + group.groupName + '，确定吗？',
        callback: function () {
          $this.apiDelete(group.id);
        }
      });
    }
  },
  selectable: function (row) {
    return row.userTotal > 0;
  },
  btnSelectClick: function () {
    var nodes = this.$refs.userGroupSelectTable.selection;
    if (nodes && nodes.length > 0) {
      var parentFrameName = utils.getQueryString("pf");
      var parentLayer = top.frames[parentFrameName];
      parentLayer.$vue.btnUserGroupSelectCallback(nodes);
      utils.closeLayerSelf();
    }
    else {
      utils.error("请至少选中一个用户组", { layer: true });
    }
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
