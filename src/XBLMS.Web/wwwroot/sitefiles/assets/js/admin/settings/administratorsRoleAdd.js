var $url = '/settings/administratorsRoleAdd';
var $urlUpdate = $url + '/actions/update';

var data = utils.init({
  roleId: utils.getQueryInt('roleId'),
  allMenus: [],
  checkdKeys: [],
  expandedKeys:[],
  form: {
    roleId: 0,
    roleName: '',
    description: '',
  }
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url, {
      params: {
        roleId: this.roleId
      }
    }).then(function (response) {
      var res = response.data;

      if (res.role) {
        $this.form.roleName = res.role.roleName;
        $this.form.description = res.role.description;
        $this.checkdKeys = res.role.selectIds;
      }
      $this.allMenus = res.menus;
      $this.allMenus[0].disabled = true;
      $this.checkdKeys.push("home");

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },

  apiAdd: function () {
    var $this = this;

    utils.loading(this, true);
    $api.post($url, {
      roleName: this.form.roleName,
      menus: this.$refs.roleTree.getCheckedNodes(false, true),
      selectIds: this.$refs.roleTree.getCheckedKeys(),
      description: this.form.description
    }).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.closeLayerSelf();
      }
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },

  apiEdit: function () {
    var $this = this;

    utils.loading(this, true);
    $api.post($urlUpdate, {
      roleId: this.roleId,
      roleName: this.form.roleName,
      menus: this.$refs.roleTree.getCheckedNodes(false, true),
      selectIds: this.$refs.roleTree.getCheckedKeys(),
      description: this.form.description
    }).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.closeLayerSelf();
      }
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },

  btnSubmitClick: function () {
    var $this = this;
    this.$refs.form.validate(function(valid) {
      if (valid) {
        if ($this.roleId > 0) {
          $this.apiEdit();
        } else {
          $this.apiAdd();
        }
      }
    });
  },

  btnCancelClick: function () {
    utils.closeLayerSelf();
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
