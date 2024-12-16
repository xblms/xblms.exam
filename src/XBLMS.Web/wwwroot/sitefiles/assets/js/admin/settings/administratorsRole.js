var $url = '/settings/administratorsRole';
var $urlDelete = $url + '/actions/delete';

var data = utils.init({
  roles: null
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url).then(function (response) {
      var res = response.data;

      $this.roles = res.roles;
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
      id: item.id
    }).then(function (response) {
      var res = response.data;
      $this.apiGet();
      utils.success('操作成功！');
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  btnAdminViewClick: function (id) {
    utils.openAdminView(id);
  },

  btnAddClick: function () {

    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getSettingsUrl('administratorsRoleAdd'),
      width: "68%",
      height: "78%",
      end: function () {
        $this.apiGet();
      }
    });
  },

  btnEditClick: function(row) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getSettingsUrl('administratorsRoleAdd', {roleId:row.id}),
      width: "68%",
      height: "78%",
      end: function () {
        $this.apiGet();
      }
    });
  },

  btnDeleteClick: function (item) {
    var $this = this;
    if (item.adminCount > 0) {
      utils.error("不能删除被使用的角色");
    }
    else {
      top.utils.alertDelete({
        title: '删除角色',
        text: '此操作将删除角色 ' + item.roleName + '，确定删除吗？',
        callback: function () {
          $this.apiDelete(item);
        }
      });
    }

  },

  btnCloseClick: function() {
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
