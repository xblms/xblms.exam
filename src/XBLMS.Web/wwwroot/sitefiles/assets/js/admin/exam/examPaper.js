var $url = '/exam/examPaper';
var $urlDelete = $url + '/del';
var $urlLock = $url + '/lock';
var $urlUnLock = $url + '/unLock';

var $treeUrl = '/exam/examPaperTree';
var $treeUrlAdd = $treeUrl + '/add';
var $treeUrlUpdate = $treeUrl + '/update';
var $treeUrlDelete = $treeUrl + '/del';

var data = utils.init({
  formInline: {
    treeIsChildren: true,
    treeId: 0,
    keyword: '',
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  paperList: null,
  paperTotal: 0,


  //tree
  treeItems: null,
  treePopoverVisibles: [],
  treeDefaultExpandedKeys: [],
  treeDefaultExpandedSetKeys: [],
  treeName: '试卷分类',
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

      $this.paperList = res.items;
      $this.paperTotal = res.total;

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
      title: '删除试卷',
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
      $this.apiGetTree();
      $this.btnSearchClick();
    });
  },
  btnUnLockClick: function (id) {
    var $this = this;
    top.utils.alertWarning({
      title: '解锁试卷',
      text: '确定解锁试卷吗？',
      callback: function () {
        $this.apiUnLock(id);
      }
    });
  },
  btnLockClick: function (id) {
    var $this = this;
    top.utils.alertWarning({
      title: '锁定试卷',
      text: '锁定后考生无法参加考试，确定吗？',
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
  switchLocked(row) {
    var $this = this;
    utils.loading(this, true);
    $api.post($urlLock, { id: row.id, locked: row.isStop }).then(function (response) {
      var res = response.data;
      if (row.isStop) {
        utils.success("已停用");
      }
      else {
        utils.success("已启用");
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnEditFastClick: function () {
    if (this.treeSelectId > 0) {
      var $this = this;
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getExamUrl('examPaperEditFast', { id: 0, treeId: this.treeSelectId }),
        width: "100%",
        height: "100%",
        end: function () {
          $this.apiGetTree();
          $this.btnSearchClick();
        }
      });
    }
    else {
      utils.error("请选择一个试卷分类")
    }
  },
  btnEditClick: function (id) {
    if (this.treeSelectId > 0 || id > 0) {
      var $this = this;
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getExamUrl('examPaperEdit', { id: id, treeId: this.treeSelectId }),
        width: "98%",
        height: "98%",
        end: function () {
          $this.apiGetTree();
          $this.btnSearchClick();
        }
      });
    }
    else {
      utils.error("请选择一个试卷分类")
    }
  },
  btnCopyClick: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examPaperEdit', { id: id, copyId: id }),
      width: "98%",
      height: "98%",
      end: function () {
        $this.apiGetTree();
        $this.btnSearchClick();
      }
    });
  },
  btnViewClick: function (row) {
    utils.openTopLeft(row.title + '-预览', utils.getCommonUrl("examPaperLayerView", { id: row.id }));
  },
  btnManagerAnalysisClick: function (row) {
    utils.openTopLeft(row.title + '-综合统计', utils.getExamUrl("examPaperManagerAnalysis", { id: row.id }));
  },
  btnManagerUserClick: function (row) {
    utils.openTopLeft(row.title + '-考生管理', utils.getExamUrl("examPaperManagerUser", { id: row.id }));
  },
  btnManagerMarkClick: function (row) {
    utils.openTopLeft(row.title + '-阅卷管理', utils.getExamUrl("examPaperManagerMark", { id: row.id }));
  },
  btnManagerScoreClick: function (row) {
    utils.openTopLeft(row.title + '-考试成绩', utils.getExamUrl("examPaperManagerScore", { id: row.id }));
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
      utils.error("该分类下有试卷，无法删除分类");
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
    this.apiGetTree();
    this.apiGet();
    document.addEventListener('click', this.treeClearPopover);
  }
});
