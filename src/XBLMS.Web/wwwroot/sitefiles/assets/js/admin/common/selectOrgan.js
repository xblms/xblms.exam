var $url = '/common/selectOrgan';
var $urlTreeLazy = '/settings/organs/lazy';

var data = utils.init({
  windowName: utils.getQueryString("windowName"),
  selectOne: utils.getQueryBoolean("selectOne"),
  organs: null,
  search: '',
  multipleSelection: [],
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
      parentId: this.treeParentId,
      organType: this.treeOrganType,
      showAdminTotal: false,
      showUserTotal: false
    };

    $api.get($urlTreeLazy, { params: organParams }).then(function (response) {
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
      parentId: this.treeParentId,
      organType: this.treeOrganType,
      showAdminTotal: false,
      showUserTotal: false
    };

    $api.get($urlTreeLazy, { params: organParams }).then(function (response) {
      var res = response.data;
      resolve(res.organs)
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
    });

  },
  btnSearchClick: function () {
    this.apiGet();
  },
  handleSelectionChange(val) {
    if (this.selectOne && val.length > 1) {
      this.multipleSelection = val[0];
      this.$refs.organsTable.clearSelection();
      this.$refs.organsTable.toggleRowSelection(val.pop());
    }
    else {
      this.multipleSelection = val;
    }
  },
  rowClick(row, column, event) {
    this.$refs.organsTable.toggleRowSelection(row);
    const expandBtn = event.currentTarget.querySelector('.el-table__expand-icon');
    if (expandBtn) {
      expandBtn.click();
    }
  },
  btnSubmitClick: function () {
    var seletcs = this.multipleSelection;
    var selectOrgans = [];
    if (seletcs.length > 0) {
      seletcs.forEach(organ => {
        selectOrgans.push({ id: organ.id, name: organ.name, type: organ.organType });
      })

      if (this.windowName.length > 0) {
        var parentLayer = top.frames[this.windowName];
        parentLayer.$vue.selectOrganCallback(selectOrgans);
        utils.closeLayerSelf();
      }
      else {
        parent.$vue.selectOrganCallback(selectOrgans);
        utils.closeLayerSelf();
      }

    }
    else {
      utils.error("请选择至少一个组织", { layer: true });
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
