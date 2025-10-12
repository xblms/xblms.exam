var $url = '/exam/examTmSelect';
var $urlGetSelect = $url + '/getIn';
var $urlRemove = $url + '/delGroupTm';
var $urlAdd = $url + '/setGroupTm';

var $treeUrl = '/exam/examTmTree';
var $treeUrlTmTotal = $treeUrl + '/tmTotal';

var data = utils.init({
  id: utils.getQueryInt("id"),
  formInline: {
    id: utils.getQueryInt("id"),
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
  formSInline: {
    id: utils.getQueryInt("id"),
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
  tmSelectList: null,
  tmSelectTotal: 0,
  txList: null,
  orderTypeList: null,

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
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  apiGetSelect: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($urlGetSelect, { params: $this.formSInline }).then(function (response) {
      var res = response.data;

      $this.tmSelectList = res.items;
      $this.tmSelectTotal = res.total;

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnSelects: function () {
    var nodes = this.$refs.tmTable.selection;
    var ids = _.map(nodes, function (item) {
      return item.id;
    });

    if (ids.length > 0) {
      var $this = this;
      top.utils.alertInfo({
        title: '加入题目',
        text: '确定加入选择的题目到题目组吗?',
        callback: function () {
          $this.apiSelects(ids);
        }
      });
    }
    else {
      utils.error("请选择题目", { layer: true });
    }

  },
  btnRemoves: function () {
    var nodes = this.$refs.tmSelectTable.selection;
    var ids = _.map(nodes, function (item) {
      return item.id;
    });

    if (ids.length > 0) {
      var $this = this;
      top.utils.alertInfo({
        title: '移出题目',
        text: '确定将选择的题目移出题目组吗？',
        callback: function () {
          $this.apiRemoves(ids);
        }
      });
    }
    else {
      utils.error("请选择题目", { layer: true });
    }
  },
  btnSelect: function (id) {
    var $this = this;
    top.utils.alertInfo({
      title: '确定加入题目组吗？',
      text: '',
      callback: function () {
        $this.apiSelect(id);
      }
    });
  },
  btnRemove: function (id) {
    var $this = this;
    top.utils.alertInfo({
      title: '确定移出吗？',
      text: '',
      callback: function () {
        $this.apiRemove(id);
      }
    });
  },
  apiRemove: function (id) {
    var $this = this;
    utils.loading(this, true);
    $api.post($urlRemove, { id: this.id, ids: [id] }).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success('操作成功！', { layer: true });
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.btnSearchClick();
    });
  },
  apiRemoves: function (ids) {
    var $this = this;
    utils.loading(this, true);
    $api.post($urlRemove, { id: this.id, ids: ids }).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success('操作成功！', { layer: true });
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.btnSearchClick();
    });
  },
  apiSelect: function (id) {
    var $this = this;
    utils.loading(this, true);
    $api.post($urlAdd, { id: this.id, ids: [id] }).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success('操作成功！', { layer: true });
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.btnSearchClick();
    });
  },
  apiSelects: function (ids) {
    var $this = this;
    utils.loading(this, true);
    $api.post($urlAdd, { id: this.id, ids: ids }).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success('操作成功！', { layer: true });
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.btnSearchClick();
    });
  },
  handleCurrentChange: function (val) {
    this.formInline.pageIndex = val;
    this.apiGet();
  },
  handleSCurrentChange: function (val) {
    this.formSInline.pageIndex = val;
    this.apiGetSelect();
  },
  btnSearchClick: function () {
    this.formInline.treeId = this.treeSelectId;
    this.formInline.pageIndex = 1;
    this.apiGet();
    this.apiGetSelect();
  },
  btnTmView: function (id) {
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('examTmLayerView', { id: id }),
      width: "58%",
      height: "88%"
    });
  },
  rowSelectClick(row, column, event) {
    this.$refs.tmSelectTable.toggleRowSelection(row);
  },
  rowClick(row, column, event) {
    this.$refs.tmTable.toggleRowSelection(row);
  },

  //tree
  apiGetTree: function () {
    var $this = this;
    $api.get($treeUrl).then(function (response) {
      var res = response.data;
      $this.treeItems = res.items;
      $this.txList = res.txList;
      $this.orderTypeList = res.orderTypeList;

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
      $this.$nextTick(() => {
        $this.treeDefaultExpandedKeys = $this.treeDefaultExpandedSetKeys;
        $this.apiGetTmTotal();
      })
    });
  },
  apiGetTmTotal: function () {
    var $this = this;
    setTimeout(function () {
      if ($this.treeItems && $this.treeItems.length > 0) {
        $this.treeItems.forEach(item => {
          $api.get($treeUrlTmTotal, { params: { id: item.id } }).then(function (response) {
            var res = response.data;
            item.total = res.total;
            item.selfTotal = res.count;
            if (item.children && item.children.length > 0) {
              $this.apiGetTmTotalChildren(item.children);
            }
          }).catch(function () {
          }).then(function () {
          });
        })
      }
    }, 200);
  },
  apiGetTmTotalChildren: function (childrenItems) {
    var $this = this;
    childrenItems.forEach(item => {
      $api.get($treeUrlTmTotal, { params: { id: item.id } }).then(function (response) {
        var res = response.data;
        item.total = res.total;
        item.selfTotal = res.count;
        if (item.children && item.children.length > 0) {
          $this.apiGetTmTotalChildren(item.children);
        }
      }).catch(function () {
      }).then(function () {
      });
    })
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
    this.apiGetSelect();
  }
});
