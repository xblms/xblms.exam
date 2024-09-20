var $url = '/exam/examTm';
var $urlExportExcel = $url + '/export';
var $urlDelete = $url + '/del';
var $urlDeleteSearch = $url + '/delSearch';

var $treeUrl = '/exam/examTmTree';
var $treeUrlAdd = $treeUrl + '/add';
var $treeUrlUpdate = $treeUrl + '/update';
var $treeUrlDelete = $treeUrl + '/del';

var data = utils.init({
  formInline: {
    tmGroupId:null,
    treeIsChildren: true,
    treeId: 0,
    txId: null,
    nandu: null,
    keyword: '',
    order: '',
    orderType: '',
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  tmList: null,
  tmTotal: 0,
  txList: null,
  orderTypeList: null,
  tmGroups:null,

  //tree
  treeItems: null,
  treePopoverVisibles: [],
  treeDefaultExpandedKeys: [],
  treeDefaultExpandedSetKeys: [],
  treeName: '题目分类',
  treeTopAddPopover: false,
  treeAddForm: {
    parentId: 0,
    names: ''
  },
  treeEditValid: false,
  treeAdd: false,
  treeUpdate: false,
  treeUpdateForm: {
    id: 0,
    name: ''
  },
  treeFilterText: '',
  treeSelectId: 0
  //tree
});

var methods = {
  apiGet: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($url, { params: $this.formInline }).then(function (response) {
      var res = response.data;

      $this.tmList = res.items;
      $this.tmTotal = res.total;

    }).catch(function (error) {
      utils.loading($this, false);
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  handleCurrentChange: function (val) {
    this.formInline.pageIndex = val;
    this.apiGet();
  },
  btnSearchClick: function () {
    this.formInline.treeId = this.treeSelectId;
    this.formInline.pageIndex = 1;
    this.apiGet();
  },
  handleCommand(type, row) {
    var $this = this;
    if (type === 'edit') {
      this.btnEditClick(row.id);
    }
    if (type === 'deletes') {
      this.btnDeleteSearchClick();
    }
    if (type === 'delete') {
      top.utils.alertDelete({
        title: '删除题目',
        text: '确认删除吗？',
        callback: function () {
          $this.apiDelete(row.id);
        }
      });
    }
    if (type === 'info') {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getCommonUrl('examTmLayerView', { id: row.id }),
        width: "58%",
        height: "88%"
      });
    }
    if (type === 'importExcel') {
      if (this.treeSelectId > 0) {
        top.utils.openLayer({
          title: false,
          closebtn: 0,
          url: utils.getExamUrl('examTmImportExcel', { treeId: this.treeSelectId }),
          width: "68%",
          height: "88%",
          end: function () {
            $this.btnSearchClick();
          }
        });
      }
      else {
        utils.error("请选择一个分类再进行题目导入");
      }
    }
    if (type === 'exportExcel') {
      this.btnExcelTmExportClick();
    }
  },
  btnExcelTmExportClick: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($urlExportExcel, { params: $this.formInline }).then(function (response) {
      var res = response.data;

      window.open(res.value);
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnEditClick: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examTmEdit', { id: id, treeId: this.treeSelectId }),
      width: "78%",
      height: "98%",
      end: function () {
        $this.btnSearchClick();
      }
    });
  },
  apiDelete: function (id) {
    var $this = this;

    utils.loading(this, true);
    $api.post($urlDelete, { id: id }).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success('题目删除成功！');
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.apiGet();
    });
  },
  btnDeleteSearchClick: function () {
    var nodes = this.$refs.tmTable.selection;
    var ids = _.map(nodes, function (item) {
      return item.id;
    });
    var $this = this;
    if (ids.length > 0) {
      top.utils.alertDelete({
        title: '批量删除题目',
        text: '此操作将删除选中的题目数据，确认删除吗？',
        callback: function () {
          $this.apiDeleteSearch();
        }
      });
    }
    else {
      utils.error("请选择要删除的题目");
    }

  },
  apiDeleteSearch: function () {
    var $this = this;
    var nodes = this.$refs.tmTable.selection;
    var ids = _.map(nodes, function (item) {
      return item.id;
    });
    utils.loading(this, true);
    $api.post($urlDeleteSearch, { ids: ids }).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success('删除成功！');
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.btnSearchClick();
    });
  },




  //tree
  apiGetTree: function () {
    var $this = this;
    $api.get($treeUrl).then(function (response) {
      var res = response.data;
      $this.treeItems = res.items;
      $this.txList = res.txList;
      $this.orderTypeList = res.orderTypeList;
      $this.tmGroups = res.tmGroups;
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.$nextTick(() => {
        $this.treeDefaultExpandedKeys = $this.treeDefaultExpandedSetKeys;
      })
    });
  },
  treeBtnHidePopover: function () {
    this.treePopoverVisibles = this.treePopoverVisibles.map(item => false);
  },
  treeClearPopover: function () {
    this.treeEditValid = false;
    this.treePopoverVisibles = this.treePopoverVisibles.map(item => false);
    this.treeAdd = false;
    this.treeUpdate = false;
  },

  treeBtnEditClick: function (node, isAdd) {
    if (isAdd) {
      this.treeAdd = true;
      this.treeUpdate = false;
    }
    else {
      this.treeAdd = false;
      this.treeUpdate = true;
    }
  },
  treeBtnDeleteClick: function (node, data) {
    if (data.total > 0 || data.selfTotal > 0) {
      utils.error("该分类下有题目数据，请勿删除");
    }
    else {
      var $this = this;
      this.treeClearPopover();
      top.utils.alertDelete({
        title: '删除分类',
        text: '此操作将删除分类及所有下级: ' + data.label + '，确定吗？',
        callback: function () {
          $this.treeApiDelete(node, data);
        }
      });
    }
  },
  treeApiDelete: function (node, data) {
    var $this = this;

    utils.loading(this, true);
    $api.post($treeUrlDelete, { id: data.id }).then(function (response) {
      var res = response.data;
      if (res.value) {
        const parent = node.parent;
        const children = parent.data.children || parent.data;
        const index = children.findIndex(d => d.id === data.id);
        children.splice(index, 1);
        utils.success('删除成功');
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  treeApiAddSubmit: function () {
    var $this = this;
    this.treeClearPopover();
    this.treeTopAddPopover = false;
    utils.loading(this, true);
    $api.post($treeUrlAdd, this.treeAddForm).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success("分类添加成功")
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.treeClearForm();
      $this.apiGetTree();
    });
  },

  treeBtnUpdateSubmitClick: function (id) {
    this.treeUpdateForm.id = id;
    if (this.treeUpdateForm.name != '' && this.treeUpdateForm.name.length > 0) {
      this.treeEditValid = false;
      this.treeApiUpdateSubmit();
    }
    else {
      this.treeEditValid = true;
    }
  },
  treeApiUpdateSubmit: function () {
    var $this = this;
    this.treeClearPopover();
    utils.loading(this, true);
    $api.post($treeUrlUpdate, { item: this.treeUpdateForm }).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success("分类修改成功")
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.treeClearForm();
      $this.apiGetTree();
    });
  },
  treeClearForm: function () {
    this.treeAddForm.parentId = 0;
    this.treeAddForm.names = "";
    this.treeUpdateForm.id = 0;
    this.treeUpdateForm.name = "";
  },

  treeBtnAddSubmitClick: function (parentId) {
    this.treeAddForm.parentId = parentId;
    if (this.treeAddForm.names != '' && this.treeAddForm.names.length > 0) {
      this.treeEditValid = false;
      this.treeApiAddSubmit();
    }
    else {
      this.treeEditValid = true;
    }
  },
  treeFilterNode(value, data, node) {
    if (!value) return true;
    var exist = data.label.indexOf(value) !== -1;

    if (exist) {
      this.treeNodeExpand(data, node);
    }
    else {
      this.treeNodeCollapse(data, node);
    }

    return exist;

  },
  treeNodeClick: function (data, node, e) {
    this.treeSelectId = data.id;
    this.btnSearchClick();
  },
  treeNodeContextmenu: function (event, data, node, self) {
    this.treeClearPopover();
    this.treePopoverVisibles[node.id] = true;
    this.treeUpdateForm.name = data.label;
  },
  treeNodeExpand: function (data, node, self) {
    this.treeDefaultExpandedSetKeys.push(node.key);
  },
  treeNodeCollapse: function (data, node, self) {
    this.treeDefaultExpandedSetKeys = this.treeDefaultExpandedSetKeys.filter(f => f !== node.key);
  },
  //tree
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  watch: {
    treeFilterText(val) {
      this.$refs.tree.filter(val);
    }
  },
  created: function () {
    this.apiGetTree();
    this.apiGet();
    document.addEventListener('click', this.treeClearPopover);
  }
});
