var $url = '/common/selectOrganChange';

var data = utils.init({
  organs: null,
  search: '',
  treeParentId: 0,
  treeOrganType: 'company'
});

var methods = {
  apiGet: function () {
    var $this = this;
    utils.loading(this, true);

    this.treeParentId = 0;
    this.treeOrganType = 'company';

    var organParams = {
      keyWords: this.search,
      parentId: this.treeParentId
    };

    $api.get($url, { params: organParams }).then(function (response) {
      var res = response.data;
      $this.organs = res.organs;

    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  loadTree(tree, treeNode, resolve) {
    this.treeParentId = tree.id;
    this.treeOrganType = tree.organType;

    var organParams = {
      keyWords: this.search,
      parentId: this.treeParentId
    };

    $api.get($url, { params: organParams }).then(function (response) {
      var res = response.data;
      resolve(res.organs)
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
    });

  },
  rowClick(row, column, event) {
    const expandBtn = event.currentTarget.querySelector('.el-table__expand-icon');
    if (expandBtn) {
      expandBtn.click();
    }
  },
  btnSearchClick: function () {
    this.apiGet();
  },

  btnSubmitClick: function (row) {
    top.utils.alertWarning({
      title: '温馨提示',
      text: "确定切换管理组织为【" + row.name+"】吗？",
      button: '确定',
      callback: function () {
        var selectOrgans = [{ id: row.id, name: row.name, type: row.organType }];
        parent.$vue.selectOrganCallback(selectOrgans);
        utils.closeLayerSelf();
      }
    });

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
