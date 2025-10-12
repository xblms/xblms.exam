var $url = '/settings/organs';
var $urlLazy = $url + '/lazy';
var $urlDelete = $url + '/info/del';

var data = utils.init({
  organs: null,
  expandRowKeys: [],
  defaultExpandAll: false,
  treeMaps: new Map(),
  search: '',
  parentId: 0,
  organType: 'company',
  searchLoading: false,
  isSearch: false,
  operate: true
});

var methods = {
  apiGet: function () {
    var $this = this;

    this.searchLoading = true;
    utils.loading(this, true);
    this.organs = null;
    this.parentId = 0;
    this.organType = 'company';

    var organParams = {
      keyWords: this.search,
      parentId: this.parentId,
      organType: this.organType,
      showAdminTotal: true,
      showUserTotal: true
    };

    $api.get($urlLazy, { params: organParams }).then(function (response) {
      var res = response.data;
      $this.organs = res.organs;
      $this.operate = res.operate;

    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.searchLoading = false;
    });
  },
  loadTree(tree, treeNode, resolve) {
    var $this = this;
    this.parentId = tree.id;
    this.organType = tree.organType;
    this.treeMaps.set(tree.guid, { tree, treeNode, resolve });

    var organParams = {
      keyWords: this.search,
      parentId: this.parentId,
      organType: this.organType,
      showAdminTotal: true,
      showUserTotal: true
    };

    $api.get($urlLazy, { params: organParams }).then(function (response) {
      var res = response.data;

      if (res.organs && res.organs.length > 0) {
        resolve(res.organs)
      }
      else {
        tree.hasChildren = false;
        $this.$set($this.$refs.organTable.store.states.lazyTreeNodeMap, tree.guid, []);
      }

    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });

  },
  reLoadTree: function (row, isAdd) {
    if (row.parentGuid === '') {
      this.apiGet();
    }
    else {
      let lazyGuid = row.parentGuid;
      if (isAdd && row.hasChildren) {
        lazyGuid = row.guid;
      }
      let currentTree = this.treeMaps.get(lazyGuid);
      if (currentTree) {
        utils.loading(this, true);
        this.loadTree(currentTree.tree, currentTree.treeNode, currentTree.resolve);
      }
    }
  },
  btnEdit: function (row, isAdd, type) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getSettingsUrl('organsEdit', {
        id: isAdd ? 0 : row.id,
        type: (isAdd ? type : row.organType),
        parentId: row.id,
        parentType: row.organType,
      }),
      width: "48%",
      height: "68%",
      end: function () {
        $this.reLoadTree(row, isAdd);
      }
    });
  },
  btnSearchClick: function () {
    if (this.search.length > 0) {
      this.isSearch = true;
    }
    else {
      this.isSearch = false;
    }
    this.apiGet();
  },
  btnDelete: function (row) {

    var noDelete = row.adminAllCount > 0 || row.userAllCount > 0;
    if (noDelete) {
      utils.error("该组织下面存在账号数据，不能做删除操作");
    }
    else {
      var $this = this;
      var typeName = row.organType === 'company' ? "单位" : '部门';
      top.utils.alertDelete({
        title: '删除' + typeName,
        text: '确定删除' + typeName + '及所有下级吗？',
        callback: function () {
          $this.apiDelete(row);
        }
      });
    }

  },
  apiDelete: function (item) {
    var $this = this;

    utils.loading(this, true);
    $api.post($urlDelete, {
      organs: item
    }).then(function (response) {
      var res = response.data;
      $this.reLoadTree(item);
      utils.success("操作成功");
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  rowClick(row, column, event) {
    const expandBtn = event.currentTarget.querySelector('.el-table__expand-icon');
    if (expandBtn) {
      expandBtn.click();
    }
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
