var $url = '/settings/usersRange';
var $urlRange = $url + "/range";

var $urlOtherData = $url + '/actions/otherData';

var data = utils.init({
  items: null,
  count: null,
  organs: null,
  form: {
    id: 0,
    rangeType: '',
    organId: 0,
    organType: '',
    keyword: '',
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  defaultProps: {
    children: 'children',
    label: 'name'
  },
  filterText: '',
  multipleSelection: []
});

var methods = {
  apiGetOtherData: function () {
    var $this = this;

    $api.get($urlOtherData).then(function (response) {
      var res = response.data;
      $this.organs = res.organs;
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
      $this.apiGet();
    });
  },
  apiGet: function () {
    var $this = this;

    $api.get($url, {
      params: this.form
    }).then(function (response) {
      var res = response.data;

      $this.items = res.users;
      $this.count = res.count;

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },

  handleSelectionChange(val) {
    this.multipleSelection = val;
  },
  btnRangeClick: function (rangeType, id) {
    var $this = this;

    var selectedUsers = $this.multipleSelection;
    if (selectedUsers.length > 0) {
      selectedUsers.forEach(user => {
        $this.formInline.rangeUserIds.push(user.id);
      })
    }

    if (selectedUsers.length > 0) {
      top.utils.alertSuccess({
        title: '安排考生',
        text: '确定安排安排吗？',
        showCancelButton: true,
        callback: function () {
          $this.apiRange(selectedUsers);
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
        utils.success("操作成功");
        $this.apiGet();
      }

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnViewClick: function (user) {
    utils.openUserView(user.id);
  },

  btnSearchClick() {
    this.form.pageIndex = 1;
    this.apiGet();
  },

  handleCurrentChange: function (val) {
    this.form.pageIndex = val;
    this.btnSearchClick();
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
  watch: {
    filterText(val) {
      this.$refs.tree.filter(val);
    }
  },
  created: function () {
    this.form.id = utils.getQueryInt("id");
    this.form.rangeType = utils.getQueryString("rangeType"),

      this.apiGetOtherData();
  }
});
