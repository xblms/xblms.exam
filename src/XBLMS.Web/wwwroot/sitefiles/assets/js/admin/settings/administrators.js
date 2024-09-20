var $url = '/settings/administrators';
var $urlOtherData = $url + '/actions/otherData';
var $urlDelete = $url + '/actions/delete';
var $urlExport = $url + '/actions/export';
var $urlUpload = $apiUrl + '/settings/administrators/actions/import';

var data = utils.init({
  drawer: false,
  administrators: null,
  count: null,
  roles: null,
  adminId: null,
  curOrganGuid:'',
  formInline: {
    organId: 0,
    organType:'',
    role: '',
    order: '',
    lastActivityDate: 0,
    keyword: '',
    currentPage: 1,
    offset: 0,
    limit: PER_PAGE
  },
  permissionInfo: {},
  uploadPanel: false,
  uploadLoading: false,
  uploadList: [],
  filterText: '',
  treeWidth: 0,
  organs:null,
  defaultProps: {
    children: 'children',
    label: 'name'
  },
  multipleSelection: []
});

var methods = {
  apiGetOtherData: function () {
    var $this = this;

    $api.get($urlOtherData, {
      params: this.formInline
    }).then(function (response) {
      var res = response.data;
      $this.organs = res.organs;
      $this.roles = res.roles;
      $this.adminId = res.adminId;
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.apiGet();
    });
  },
  apiGet: function () {
    var $this = this;

    $api.get($url, {
      params: this.formInline
    }).then(function (response) {
      var res = response.data;
      $this.count = res.count;
      $this.administrators = res.administrators;
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

      $this.administrators.splice($this.administrators.indexOf(item), 1);
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  apiLock: function (item) {
    var $this = this;

    utils.loading(this, true);
    $api.post($url + '/actions/lock', {
      id: item.id
    }).then(function (response) {
      var res = response.data;

      item.locked = true;
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  apiUnLock: function (item) {
    var $this = this;

    utils.loading(this, true);
    $api.post($url + '/actions/unLock', {
      id: item.id
    }).then(function (response) {
      var res = response.data;

      item.locked = false;
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  btnViewClick: function(row) {
    utils.openAdminView(row.id);
  },

  btnAddClick: function () {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getSettingsUrl('administratorsLayerProfile', { guid: $this.curOrganGuid }),
      width: "60%",
      height: "88%", end: function () { $this.apiGet() }
    });
  },
  btnEditClick: function (row) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn:0,
      url: utils.getSettingsUrl('administratorsLayerProfile', { userName: row.userName }),
      width: "60%",
      height: "88%",
      end: function () { $this.apiGet() }
    });
  },

  btnPasswordClick: function(row) {
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getSettingsUrl('administratorsLayerPassword', {userName: row.userName}),
      width: "38%",
      height: "58%",
    });
  },


  handleSelectionChange(val) {
    this.multipleSelection = val;
  },
  btnRolesClick: function () {
    var selectedAdmins = this.multipleSelection;
    var ids = [];
    if (selectedAdmins.length > 0) {
      selectedAdmins.forEach(admin => {
        ids.push(admin.id);
      })
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getSettingsUrl('administratorsRoleSet', { adminIds:ids }),
        width: "68%",
        height: "68%",
      });
    }
    else {
      utils.error("请选择需要分配的管理员");
    }

  },

  btnDeleteClick: function (item) {
    var $this = this;

    top.utils.alertDelete({
      title: '删除管理员',
      text: '此操作将删除管理员 ' + item.userName + '，确定吗？',
      callback: function () {
        $this.apiDelete(item);
      }
    });
  },

  btnLockClick: function(item) {
    var $this = this;

    top.utils.alertDelete({
      title: '锁定管理员',
      text: '此操作将锁定管理员 ' + item.userName + '，确定吗？',
      button: '确 定',
      callback: function () {
        $this.apiLock(item);
      }
    });
  },

  btnUnLockClick: function(item) {
    var $this = this;

    top.utils.alertDelete({
      title: '解锁管理员',
      text: '此操作将解锁管理员 ' + item.userName + '，确定吗？',
      button: '确 定',
      callback: function () {
        $this.apiUnLock(item);
      }
    });
  },

  btnSearchClick() {
    this.apiGet();
  },

  btnTreeClick: function (data, node, e) {
    this.formInline.organId = data.id;
    this.formInline.organType = data.organType;
    this.curOrganGuid = data.guid;
    this.btnSearchClick();
  },


  handleCurrentChange: function(val) {
    this.formInline.currentValue = val;
    this.formInline.offset = this.formInline.limit * (val - 1);

    this.btnSearchClick();
  },

  filterNode(value, data) {
    if (!value) return true;
    return data.name.indexOf(value) !== -1;
  }
};

var $vue = new Vue({
  el: '#main',
  data: data,
  watch: {
    filterText(val) {
      this.$refs.tree.filter(val);
    }
  },
  methods: methods,
  created: function () {
    this.apiGetOtherData();
  }
});
