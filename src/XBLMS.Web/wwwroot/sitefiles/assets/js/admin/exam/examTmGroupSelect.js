var $url = '/exam/examTmGroup';
var $urlDelete = $url + '/delete';
var $urlTmTotal = $url + '/tmTotal';

var data = utils.init({
  groups: null,
  search: '',
  isGuding: utils.getQueryBoolean("isGuding")
});

var methods = {
  apiGet: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($url, { params: { search: this.search } }).then(function (response) {
      var res = response.data;
      if ($this.isGuding) {
        $this.groups = res.groups.filter(f => f.groupType === 'Fixed');
      }
      else {
        $this.groups = res.groups;
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      setTimeout(function () {
        $this.apiGetTmTotal();
      }, 100);
    });
  },
  apiGetTmTotal: function () {
    if (this.groups && this.groups.length > 0) {
      this.groups.forEach(item => {
        $api.get($urlTmTotal, { params: { id: item.id } }).then(function (response) {
          var res = response.data;
          item.tmTotal = res.tmTotal;
          item.useCount = res.useTotal;
          item.totalScore = res.scoreTotal;
        }).catch(function (error) {
        }).then(function () {
        });
      });
    }

  },
  btnSearch: function () {
    this.apiGet();
  },
  btnRangeClick: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examTmSelect', { id: id }),
      width: "99%",
      height: "99%",
      end: function () { $this.apiGet() }
    });
  },
  btnEditClick: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examTmGroupEdit', { id: id }),
      width: "60%",
      height: "88%",
      end: function () { $this.apiGet() }
    });
  },
  apiDelete: function (id) {
    var $this = this;
    utils.loading(this, true);
    $api.post($urlDelete, {
      id: id
    }).then(function (response) {
      var res = response.data;
      utils.success('操作成功！', { layer: true });
      $this.apiGet();
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnListClick: function (group) {
    top.utils.openLayer({
      title: '题目组：' + group.groupName,
      url: utils.getExamUrl('examTm', { groupId: group.id }),
      width: "90%",
      height: "90%",
    });
  },
  btnDeleteClick: function (group) {
    var $this = this;
    if (group.useCount > 0) {
      utils.error("不能删除被使用的组", { layer: true });
    }
    else {
      top.utils.alertDelete({
        title: '删除题目组',
        text: '此操作将删除题目组 ' + group.groupName + '，确定吗？',
        callback: function () {
          $this.apiDelete(group.id);
        }
      });
    }
  },
  selectable: function (row) {
    return row.tmTotal > 0;
  },
  btnSelectClick: function () {
    var nodes = this.$refs.tmGroupSelectTable.selection;
    if (nodes && nodes.length > 0) {
      var parentFrameName = utils.getQueryString("pf");
      var parentLayer = top.frames[parentFrameName];
      parentLayer.$vue.btnTmGroupSelectCallback(nodes);
      utils.closeLayerSelf();
    }
    else {
      utils.error("请至少选中一个题目组", { layer: true });
    }
  }
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  }
});
