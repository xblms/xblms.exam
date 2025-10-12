var $url = '/common/usersRange';
var $urlRange = $url + "/range";

var $urlOtherData = $url + '/actions/otherData';
var $urlTreeLazy = '/settings/organs/lazy';

var data = utils.init({
  title: '',
  list: null,
  total: null,
  organs: null,
  form: {
    id: 0,
    rangeType: '',
    organId: 0,
    organType: '',
    keyWords: '',
    pageIndex: 1,
    pageSize: PER_PAGE,
  },
  defaultProps: {
    children: 'children',
    label: 'name'
  },
  filterText: '',
  multipleSelection: [],
  treeParentId: 0,
  treeOrganType: 'company',
  treeLoading: false,
  topNode: null,
  topResolve: null
});

var methods = {
  apiGetOtherData: function () {
    var $this = this;

    $api.get($urlOtherData, {
      params: this.form
    }).then(function (response) {
      var res = response.data;

      $this.title = res.title;
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
      $this.apiGet();
    });
  },

  loadTree(node, resolve) {
    if (node.level !== 0) {
      let tree = node.data;
      this.treeParentId = tree.id;
      this.treeOrganType = tree.organType;
    }
    else {
      this.topNode = node;
      this.topResolve = resolve;
    }

    var organParams = {
      keyWords: this.filterText,
      parentId: this.treeParentId,
      organType: this.treeOrganType,
      showAdminTotal: false,
      showUserTotal: true
    };

    var $this = this;
    $this.treeLoading = true;
    $api.get($urlTreeLazy, { params: organParams }).then(function (response) {
      var res = response.data;
      resolve(res.organs)
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      $this.treeLoading = false;
    });

  },
  loadTreeSearch: function () {
    var $this = this;
    this.treeParentId = 0;
    this.treeOrganType = "company";
    $this.topNode.childNodes = [];
    $this.loadTree($this.topNode, $this.topResolve);
  },
  apiGet: function () {
    var $this = this;

    $api.get($url, {
      params: this.form
    }).then(function (response) {
      var res = response.data;

      $this.list = res.list;
      $this.total = res.total;

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  selectable: function (row) {
    return !row.isRange;
  },
  handleSelectionChange(val) {
    this.multipleSelection = val;
  },
  btnRangeClick: function () {
    var $this = this;
    var userIds = [];

    if (this.multipleSelection && this.multipleSelection.length > 0) {
      this.multipleSelection.forEach(user => {
        userIds.push(user.id);
      })

      top.utils.alertSuccess({
        title: '安排考生',
        text: '确定安排选中的考生吗？',
        showCancelButton: true,
        callback: function () {
          $this.apiRange(userIds);
        }
      });
    }
    else {
      utils.error("请选择要安排的考生", { layer: true });
    }
  },
  apiRange: function (ids) {
    var $this = this;

    $api.post($urlRange, { id: this.form.id, rangeType: this.form.rangeType, ids: ids }).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success("操作成功", { layer: true });
        $this.apiGet();
      }

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnViewClick: function (id) {
    utils.openUserView(id);
  },

  btnSearchClick() {
    this.form.pageIndex = 1;
    this.apiGet();
  },

  handleCurrentChange: function (val) {
    this.form.pageIndex = val;
    this.apiGet();
  },

  filterNode(value, data) {
    if (!value) return true;
    return data.name.indexOf(value) !== -1;
  },
  btnTreeClick: function (data, node, e) {
    this.form.organId = data.id;
    this.form.organType = data.organType;
    this.btnSearchClick();
  },
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.form.id = utils.getQueryInt("id");
    this.form.rangeType = utils.getQueryString("rangeType"),
      this.apiGetOtherData();
  }
});
