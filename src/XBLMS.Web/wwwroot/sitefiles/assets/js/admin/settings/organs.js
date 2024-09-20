var $url = '/settings/organs';
var $urlDelete = $url + '/info/del';

var data = utils.init({
  companys: null,
  expandRowKeys: [],
  defaultExpandAll:false,
  search: ''
});

var methods = {
  apiGet: function () {
    var $this = this;
    $api.get($url, { params: { keyWord: this.search } }).then(function (response) {
      var res = response.data;
      $this.companys = res.organs;

    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
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
      width: "38%",
      height: "38%",
      end: function () {
        $this.apiGet();
      }
    });
  },
  btnSearch: function () {
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

      $this.apiGet();
    }).catch(function (error) {
      top.utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  rowClick: function (row, column, event) {
    this.$refs.companysTable.toggleRowExpansion(row);
  },

  //  树形表格过滤
  handleTreeData(treeData, searchValue) {
    if (!treeData || treeData.length === 0) {
      return [];
    }
    const array = [];
    for (let i = 0; i < treeData.length; i += 1) {
      let match = false;
      for (let pro in treeData[i]) {
        if (typeof (treeData[i][pro]) == 'string') {
          match |= treeData[i].name.includes(searchValue);
          if (match) break;
        }
      }
      if (this.handleTreeData(treeData[i].children, searchValue).length > 0 || match) {
        array.push({
          ...treeData[i],
          children: this.handleTreeData(treeData[i].children, searchValue),
        });
      }
    }
    return array;
  },
  // 将过滤好的树形数据展开
  setExpandRow(handleTreeData) {
    if (handleTreeData.length) {
      for (let i of handleTreeData) {
        this.expandRowKeys.push(i.guid)
        if (i.children.length) {
          this.setExpandRow(i.children)
        }
      }
    }
  },
  btnExpand: function () {
    var $this = this;
    this.defaultExpandAll = !this.defaultExpandAll;
    this.companys.forEach(c => {
      $this.treeExpand(c,$this.defaultExpandAll);
    });
  },
  treeExpand: function (row,status) {
    var $this = this;
    this.$refs.companysTable.toggleRowExpansion(row, status);
    if (row.children && row.children.length > 0) {
      row.children.forEach(c => {
        $this.$refs.companysTable.toggleRowExpansion(c, status);
        $this.treeExpand(c, status);
      });
    }
  }
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  computed: {
    treeTable: function () {
      var searchValue = this.search;
      if (searchValue) {
        let treeData = this.companys;
        let handleTreeData = this.handleTreeData(treeData, searchValue);
        this.defaultExpandAll = true;
        this.setExpandRow(handleTreeData);
        return handleTreeData;
      }
      this.expandRowKeys.push(this.companys[0].guid);
      return this.companys;
    }
  },
  created: function () {
    this.apiGet();
  }
});
