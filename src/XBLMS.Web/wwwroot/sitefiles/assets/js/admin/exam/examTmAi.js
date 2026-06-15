var $url = '/exam/examTm/ai';
var $urlDelete = $url + '/del';
var $urlDeletes = $url + '/dels';
var $urlRuku = $url + '/ruku';
var $urlRukus = $url + '/rukus';

var $treeUrl = '/exam/examTmTree';
var $treeUrlTmTotal = $treeUrl + '/tmTotal';

var data = utils.init({
  formInline: {
    keyWords: '',
    status: 0,
    pageIndex: 1,
    pageSize: 50
  },
  drawerSelectTk: false,
  rukuId: 0,
  tmList: null,
  total: 0,
  //tree
  treeItems: null,
  treePopoverVisibles: [],
  treeDefaultExpandedKeys: [],
  treeDefaultExpandedSetKeys: [],
  treeName: '题库',
  treeTopAddPopover: false,
  treeFilterText: '',
  treeSelectId: 0,
  treeSelectName: ''
  //tree
});

var methods = {
  apiGet: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($url, { params: $this.formInline }).then(function (response) {
      var res = response.data;

      $this.tmList = res.list;
      $this.total = res.total;

    }).catch(function (error) {
      utils.loading($this, false);
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      if ($this.treeItems == null || $this.treeItems.length == 0) {
        $this.apiGetTree();
      }
    });
  },
  handleCurrentChange: function (val) {
    this.formInline.pageIndex = val;
    this.apiGet();
  },
  btnSearchClick: function () {
    this.formInline.pageIndex = 1;
    this.apiGet();
  },
  handleSizeChange: function (val) {
    this.formInline.pageIndex = 1;
    this.formInline.pageSize = val;
    this.apiGet();
  },
  handleCommand(type, row) {
    var $this = this;
    if (type === 'edit') {
      this.btnEditClick(row.id);
    }
    if (type === 'del') {
      top.utils.alertDelete({
        title: '删除题目',
        text: '确定删除吗？',
        callback: function () {
          $this.apiDelete(row.id);
        }
      });
    }
    if (type === 'ruku') {
      $this.rukuId = row.id;
      $this.drawerSelectTk = true;
    }
    if (type === 'info') {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getCommonUrl('examTmAiLayerView', { id: row.id }),
        width: "58%",
        height: "100%"
      });
    }
  },
  btnPublishClick: function () {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examTmAiPublish'),
      width: "100%",
      height: "100%",
      end: function () {
        $this.btnSearchClick();
      }
    });
  },
  btnEditClick: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examTmaIEdit', { id: id }),
      width: "78%",
      height: "100%",
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
        utils.success('操作成功！');
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.btnSearchClick();
    });
  },
  btnDeletesClick: function () {
    if (this.total > 0) {
      var $this = this;
      top.utils.alertDelete({
        title: '批量删除题目',
        text: '此操作将删除已入库的题目，确定删除吗？',
        callback: function () {
          $this.apiDeletes();
        }
      });
    }
    else {
      utils.error('没有数据可以操作！');
    }
  },
  apiDeletes: function () {
    var $this = this;
    utils.loading(this, true);
    $api.post($urlDeletes).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success('操作成功！');
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.btnSearchClick();
    });
  },
  btnRukusClick: function () {
    if (this.total > 0) {
      this.rukuId = 0;
      this.drawerSelectTk = true;
    }
    else {
      utils.error('没有数据可以操作！');
    }
  },
  btnSelectTkConfirmClick: function () {
    if (this.treeSelectId > 0) {
      var $this = this;
      if (this.rukuId > 0) {
        top.utils.alertWarning({
          title: '单题入库',
          text: '将该题目做入库操作，确定吗？',
          callback: function () {
            $this.apiRuku();
          }
        });
      }
      else {
        top.utils.alertWarning({
          title: '批量入库',
          text: '将所有未入库的题目做入库操作，确定吗？',
          callback: function () {
            $this.apiRukus();
          }
        });
      }
    }
    else {
      utils.error("请选择一个分类再做入库操作");
    }
  },
  apiRuku: function () {
    var $this = this;
    utils.loading(this, true);
    $api.post($urlRuku, { id: this.rukuId, treeId: this.treeSelectId }).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success('操作成功，已入库！');
        $this.drawerSelectTk = false;
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.rukuId = 0;
      $this.treeSelectId = 0;
      $this.treeSelectName = "";
      $this.btnSearchClick();
    });
  },
  apiRukus: function () {
    var $this = this;
    utils.loading(this, true);
    $api.post($urlRukus, { id: this.treeSelectId }).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success('操作成功，已全部入库！');
        $this.drawerSelectTk = false;
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.rukuId = 0;
      $this.treeSelectId = 0;
      $this.treeSelectName = "";
      $this.btnSearchClick();
    });
  },

  //tree
  apiGetTree: function () {
    var $this = this;
    $api.get($treeUrl, { params: { type: this.type } }).then(function (response) {
      var res = response.data;
      $this.treeItems = res.items;
    }).catch(function (error) {
      utils.error(error);
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
    }, 100);
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
    this.treeSelectName = node.label;
  },
  treeNodeContextmenu: function (event, data, node, self) {
    this.treePopoverVisibles[node.id] = true;
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
  created: function () {
    this.apiGet();
  }
});
