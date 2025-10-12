var $url = '/settings/usersGroupRange';
var $urlOtherData = $url + '/actions/otherData';
var $urlRange = $url + '/actions/range';

var $urlTreeLazy = '/settings/organs/lazy';

var data = utils.init({
  items: null,
  count: null,
  organs: null,
  formInline: {
    groupId: utils.getQueryInt("groupId"),
    range: 0,
    order: '',
    lastActivityDate: 0,
    keyword: '',
    rangeUserIds: [],
    rangeAll: false,
    organId: 0,
    organType:'company',
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  defaultProps: {
    children: 'children',
    label: 'name'
  },
  filterText: '',
  curOrganId: '',
  groupName: null,
  multipleSelection: [],
  pageSizes: [PER_PAGE, 50, 300, 500, 1000],
  treeParentId: 0,
  treeOrganType: 'company',
  treeLoading: false,
  topNode: null,
  topResolve: null
});

var methods = {
  loadTree(node, resolve) {
    if (node.level !== 0) {
      let tree = node.data;
      this.treeParentId = tree.id;
      this.treeOrganType = tree.organType;
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
  loadTreeSearch: function () {
    var $this = this;
    this.treeParentId = 0;
    this.treeOrganType = "company";
    $this.topNode.childNodes = [];
    $this.loadTree($this.topNode, $this.topResolve);
  },
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);

    $api.get($url, {
      params: this.formInline
    }).then(function (response) {
      var res = response.data;

      $this.items = res.users;
      $this.count = res.count;

      $this.groupName = res.groupName;
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },

  handleSelectionChange(val) {
    this.multipleSelection = val;
  },
  btnRangeClick: function (rangeType, id) {
    var $this = this;
    var isRead = false;

    $this.formInline.rangeUserIds = [];
    $this.formInline.rangeAll = false;

    if (rangeType === 'all') {
      if ($this.count && $this.count > 0) {
        isRead = true;
      }
      $this.formInline.rangeAll = true;
    }
    else if (rangeType === 'select') {
      var selectedUsers = $this.multipleSelection;
      if (selectedUsers.length > 0) {
        selectedUsers.forEach(user => {
          $this.formInline.rangeUserIds.push(user.id);
        })
      }
      if ($this.formInline.rangeUserIds.length > 0) {
        isRead = true;
      }
    }
    else {
      $this.formInline.rangeUserIds.push(id);
      isRead = true;
    }
    if (isRead) {
      if ($this.formInline.range == 0) {
        top.utils.alertSuccess({
          title: '安排用户',
          text: '此操作将安排范围内用户到用户组: ' + $this.groupName + '，确定吗？',
          showCancelButton: true,
          callback: function () {
            $this.apiRange();
          }
        });
      }
      else {
        top.utils.alertWarning({
          title: '移出用户',
          text: '此操作将移出范围内用户到用户组: ' + $this.groupName + '，确定吗？',
          showCancelButton: true,
          callback: function () {
            $this.apiRange();
          }
        });
      }
    }
    else {
      utils.error("请选择用户", { layer: true });
    }
  },
  apiRange: function () {
    var $this = this;

    $api.post($urlRange, this.formInline).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success("操作成功");

        $this.formInline.rangeUserIds = [];
        $this.formInline.rangeAll = false;


        $this.btnSearchClick();
      }

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },

  rowClick(row, column, event) {
    this.$refs.userTable.toggleRowSelection(row);
  },
  btnViewClick: function (user) {
    utils.openUserView(user.id);
  },

  btnSearchClick() {
    this.formInline.pageIndex = 1;
    this.apiGet();
  },
  handleSizeChange: function (val) {
    this.formInline.pageIndex = 1;
    this.formInline.pageSize = val;

    this.apiGet();
  },
  handleCurrentChange: function (val) {
    this.formInline.pageIndex = val;
    this.apiGet();
  },

  filterNode(value, data) {
    if (!value) return true;
    return data.name.indexOf(value) !== -1;
  },
  btnTreeClick: function (data, node, e) {
    this.formInline.organId = data.id;
    this.formInline.organType = data.organType;
    this.curOrganId = data.guid;
    this.btnSearchClick();
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
