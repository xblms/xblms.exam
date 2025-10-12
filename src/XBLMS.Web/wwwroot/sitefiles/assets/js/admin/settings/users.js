var $url = '/settings/users';
var $urlOtherData = $url + '/actions/otherData';
var $urlDelete = $url + '/actions/delete';
var $urlDeletes = $url + '/actions/deletes';
var $urlExport = $url + '/actions/export';
var $urlUpload = $apiUrl + '/settings/users/actions/import';

var $urlTreeLazy = '/settings/organs/lazy';
var $urlTreeLazyCount = $urlTreeLazy + '/count';

var data = utils.init({
  items: null,
  count: null,
  groups: null,
  organs: null,
  selectOrganId: 0,
  selectOrganType: '',
  selectOrganName: '',
  formInline: {
    state: '',
    groupId: utils.getQueryInt("groupId") || 0,
    order: '',
    lastActivityDate: 0,
    keyword: '',
    currentPage: 1,
    organId: 0,
    organType: '',
    offset: 0,
    limit: 30
  },
  uploadPanel: false,
  uploadLoading: false,
  uploadList: [],
  filterText: '',
  curOrganId: '',
  multipleSelection: [],
  treeParentId: 0,
  treeOrganType: 'company',
  treeLoading: false,
  topNode: null,
  topResolve: null,
  loadNodeGuidList: []
});

var methods = {
  apiGetOtherData: function () {
    var $this = this;

    $api.get($urlOtherData).then(function (response) {
      var res = response.data;
      $this.groups = res.groups;
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  loadTree(node, resolve) {

    if (node.level !== 0) {
      let tree = node.data;
      this.treeParentId = tree.id;
      this.treeOrganType = tree.organType;
      this.loadNodeGuidList.push(tree.guid);
    }
    else {
      this.topNode = node;
      this.topResolve = resolve;
    }
    var $this = this;
    $this.treeLoading = true;

    var organParams = {
      keyWords: this.filterText,
      parentId: this.treeParentId,
      organType: this.treeOrganType,
      showAdminTotal: false,
      showUserTotal: true
    };

    $api.get($urlTreeLazy, { params: organParams }).then(function (response) {
      var res = response.data;
      resolve(res.organs)


    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      $this.treeLoading = false;
    });

  },
  loadTreeRefreshTotal: function () {
    let loadedIdList = [];
    if (this.loadNodeGuidList.length > 0) {
      this.loadNodeGuidList.forEach(letGuid => {
        let letNode = this.$refs.tree.store.getNode(letGuid);
        if (letNode) {
          if (!utils.contains(loadedIdList, letGuid)) {
            loadedIdList.push(letGuid)
            this.apiGetTreeCount(letNode);
          }

          if (letNode.childNodes && letNode.childNodes.length > 0) {
            letNode.childNodes.forEach(letcnode => {

              let letcGuid = letcnode.data.guid;
              if (!utils.contains(loadedIdList, letcGuid)) {
                loadedIdList.push(letcGuid)
                this.apiGetTreeCount(letcnode);
              }

            });
          }
        }
      });
    }
    else {
      this.loadTreeSearch();
    }
  },
  loadTreeSearch: function () {
    var $this = this;
    this.treeParentId = 0;
    this.treeOrganType = "company";
    $this.topNode.childNodes = [];
    $this.loadTree($this.topNode, $this.topResolve);
  },
  apiGetTreeCount: function (node) {
    var $this = this;

    $api.get($urlTreeLazyCount, {
      params: { id: node.data.id, type: node.data.organType, userType: 'user' }
    }).then(function (response) {
      var res = response.data;
      $this.$set(node.data, 'userCount', res.count);
      $this.$set(node.data, 'userAllCount', res.total);
    }).catch(function () {
    }).then(function () {
    });
  },
  apiGet: function () {
    var $this = this;

    $api.get($url, {
      params: this.formInline
    }).then(function (response) {
      var res = response.data;

      $this.items = res.users;
      $this.count = res.count;
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  btnViewClick: function (user) {
    utils.openUserView(user.id);
  },

  btnAvatarCerUploadClick: function () {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getSettingsUrl('usersCerAvatarUpload'),
      width: "88%",
      height: "88%",
      end: function () { $this.apiGet() }
    });
  },

  btnAddClick: function () {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getSettingsUrl('usersLayerProfile', { organId: $this.selectOrganId, organType: $this.selectOrganType, organName: $this.selectOrganName }),
      width: "60%",
      height: "88%",
      end: function () { $this.apiGet(); $this.loadTreeRefreshTotal(); }
    });
  },
  btnImportClick: function () {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getSettingsUrl('usersImport'),
      width: "50%",
      height: "88%",
      end: function () { $this.apiGet(); $this.loadTreeRefreshTotal(); }
    });
  },
  btnEditClick: function (row) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getSettingsUrl('usersLayerProfile', { userId: row.id }),
      width: "60%",
      height: "88%",
      end: function () { $this.apiGet() }
    });
  },
  btnPasswordClick: function (row) {
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getSettingsUrl('usersLayerPassword', { userId: row.id }),
      width: "38%",
      height: "58%",
    });
  },

  btnExportClick: function () {
    var $this = this;

    utils.loading(this, true);
    $api.post($urlExport, $this.formInline).then(function (response) {
      var res = response.data;

      window.open(res.value);
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
      utils.success("操作成功");
      $this.items.splice($this.items.indexOf(item), 1);
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.loadTreeRefreshTotal();
    });
  },

  btnDeleteClick: function (item) {
    var $this = this;

    top.utils.alertDelete({
      title: '删除用户',
      text: '此操作将删除用户 ' + item.userName + ' 及其关联的所有数据，确定吗？',
      callback: function () {
        $this.apiDelete(item);
      }
    });
  },
  btnDeletesClick: function () {
    var selectedUsers = this.multipleSelection;
    var ids = [];
    if (selectedUsers.length > 0) {
      selectedUsers.forEach(user => {
        ids.push(user.id);
      })

      var $this = this;

      top.utils.alertDelete({
        title: '删除用户',
        text: '此操作将删除选中的用户及其关联的所有数据，确定吗？',
        callback: function () {

          utils.loading($this, true);
          $api.post($urlDeletes, {
            ids: ids
          }).then(function (response) {
            var res = response.data;
            utils.success("操作成功");
            $this.apiGet();
            $this.loadTreeRefreshTotal();

          }).catch(function (error) {
            utils.error(error);
          }).then(function () {
            utils.loading($this, false);
          });
        }
      });
    }
    else {
      utils.error("请选择需要删除的用户");
    }
  },
  handleSelectionChange(val) {
    this.multipleSelection = val;
  },
  apiLock: function (item) {
    var $this = this;

    utils.loading(this, true);
    $api.post($url + '/actions/lock', {
      id: item.id
    }).then(function (response) {
      var res = response.data;
      utils.success("操作成功");
      item.locked = true;
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  btnLockClick: function (item) {
    var $this = this;

    top.utils.alertDelete({
      title: '锁定用户',
      text: '此操作将锁定用户 ' + item.userName + '，确定吗？',
      button: '确 定',
      callback: function () {
        $this.apiLock(item);
      }
    });
  },

  apiUnLock: function (item) {
    var $this = this;

    utils.loading(this, true);
    $api.post($url + '/actions/unLock', {
      id: item.id
    }).then(function (response) {
      var res = response.data;
      utils.success("操作成功");
      item.locked = false;
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  btnUnLockClick: function (item) {
    var $this = this;

    top.utils.alertDelete({
      title: '解锁用户',
      text: '此操作将解锁用户 ' + item.userName + '，确定吗？',
      button: '确 定',
      callback: function () {
        $this.apiUnLock(item);
      }
    });
  },

  btnSearchClick() {
    this.apiGet();
  },

  handleCurrentChange: function (val) {
    this.formInline.currentValue = val;
    this.formInline.offset = this.formInline.limit * (val - 1);

    this.btnSearchClick();
  },

  btnTreeClick: function (data, node, e) {
    this.formInline.organId = data.id;
    this.formInline.organType = data.organType;

    this.selectOrganId = data.id;
    this.selectOrganName = data.name;
    this.selectOrganType = data.organType;

    this.btnSearchClick();
  },
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.apiGetOtherData();
    this.apiGet();
  }
});
