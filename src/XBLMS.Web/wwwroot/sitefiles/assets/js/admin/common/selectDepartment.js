var $url = '/common/selectDepartment';

var data = utils.init({
  windowName: utils.getQueryString("windowName"),
  selectOrganIds: utils.getQueryIntList("selectOrganIds"),
  departments: null,
  search: '',
  multipleSelection: []
});

var methods = {
  apiGet: function (message) {
    var $this = this;

    utils.loading(this, true);
    $api.get($url, { params: { search: this.search } }).then(function (response) {
      var res = response.data;
      $this.departments = res.departments;

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);

      $this.$nextTick(() => {
        if ($this.departments != null && $this.departments.length > 0 && $this.selectOrganIds != null && $this.selectOrganIds.length>0) {
          $this.selectOrganIds.forEach(cid => {
            $this.departments.forEach(c => {
              if (cid === c.id) {
                $this.$refs.departmentsTable.toggleRowSelection(c);
              }
            })
          })
        }
      })
    });
  },
  btnSearch: function () {
    this.apiGet();
  },
  handleSelectionChange(val) {
    this.multipleSelection = val;
  },
  btnSubmitClick: function () {
    var seletcs = this.multipleSelection;
    var selectDepartments = [];
    if (seletcs.length > 0) {
      seletcs.forEach(department => {
        selectDepartments.push({ id: department.id, name: department.name });
      })

      var parentLayer = top.frames[this.windowName];
      parentLayer.$vue.selectOrganCallBack(selectDepartments);
      utils.closeLayerSelf();

    }
    else {
      utils.error("请选择至少一个部门进行管理", { layer: true });
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
