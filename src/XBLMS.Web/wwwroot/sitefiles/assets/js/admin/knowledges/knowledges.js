var $url = '/knowledges';
var $urlDelete = $url + "/del";
var $urlLock = $url + "/lock";
var $urlUnLock = $url + "/unLock";

var $treeUrl = '/knowledgesTree';
var $treeUrlAdd = $treeUrl + '/add';
var $treeUrlUpdate = $treeUrl + '/update';
var $treeUrlDelete = $treeUrl + '/del';

var data = utils.init({
  formInline: {
    treeIsChildren: true,
    treeId: 0,
    keywords: '',
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  list: null,
  total: 0,


  //tree
  treeItems: null,
  treePopoverVisibles: [],
  treeDefaultExpandedKeys: [],
  treeDefaultExpandedSetKeys: [],
  treeName: '知识库分类',
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

      $this.list = res.list;
      $this.total = res.total;

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
  btnDeleteClick: function (id) {
    var $this = this;
    top.utils.alertDelete({
      title: '删除知识库',
      text: '确定删除吗？',
      callback: function () {
        $this.apiDelete(id);
      }
    });
  },
  apiDelete: function (id) {
    var $this = this;
    utils.loading(this, true);
    $api.post($urlDelete, { id: id }).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success("操作成功")
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.btnSearchClick();
    });
  },
  btnUnLockClick: function (id) {
    var $this = this;
    top.utils.alertWarning({
      title: '上架知识库',
      text: '确定上架吗？',
      callback: function () {
        $this.apiUnLock(id);
      }
    });
  },
  btnLockClick: function (id) {
    var $this = this;
    top.utils.alertWarning({
      title: '下架知识库',
      text: '确定下架吗？',
      callback: function () {
        $this.apiLock(id);
      }
    });
  },
  apiLock: function (id) {
    var $this = this;
    utils.loading(this, true);
    $api.post($urlLock, { id: id }).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success("操作成功")
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.btnSearchClick();
    });
  },
  apiUnLock: function (id) {
    var $this = this;
    utils.loading(this, true);
    $api.post($urlUnLock, { id: id }).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success("操作成功")
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.btnSearchClick();
    });
  },
  btnAddClick: function () {
    if (this.treeSelectId > 0) {
      var $this = this;
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getKnowledgesUrl('knowledgesUpload', { treeId: this.treeSelectId }),
        width: "99%",
        height: "99%",
        end: function () {
          $this.apiGet();
        }
      });
    }
    else {
      utils.error("请选择一个分类")
    }
  },
  btnViewClick: function (row) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getKnowledgesUrl("knowledgesView", { url: row.url }),
      width: "88%",
      height: "99%"
    });
  },


  //tree
  apiGetTree: function () {
    var $this = this;
    $api.get($treeUrl).then(function (response) {
      var res = response.data;
      $this.treeItems = res.items;

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
      utils.error("该分类下有数据，无法删除分类");
    }
    else {
      var $this = this;
      this.treeClearPopover();
      top.utils.alertDelete({
        title: '删除分类',
        text: '此操作将删除分类及所有下级: ' + data.label + '，确定删除吗？',
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
        utils.success('操作成功');
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
        utils.success("操作成功");
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
        utils.success("操作成功");
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
    this.apiGet();
    this.apiGetTree();
    document.addEventListener('click', this.treeClearPopover);
  }
});
