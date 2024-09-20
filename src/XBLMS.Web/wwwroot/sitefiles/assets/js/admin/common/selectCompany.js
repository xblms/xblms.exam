var $url = '/common/selectCompany';

var data = utils.init({
  windowName: utils.getQueryString("windowName"),
  selectOrganIds: utils.getQueryIntList("selectOrganIds"),
  companys: null,
  search: '',
  multipleSelection: []
});

var methods = {
  apiGet: function (message) {
    var $this = this;

    utils.loading(this, true);
    $api.get($url, { params: { search: this.search } }).then(function (response) {
      var res = response.data;
      $this.companys = res.companys;

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);

      $this.$nextTick(() => {
        if ($this.companys != null && $this.companys.length > 0 && $this.selectOrganIds != null && $this.selectOrganIds.length>0) {
          $this.selectOrganIds.forEach(cid => {
            $this.companys.forEach(c => {
              if (cid === c.id) {
                $this.$refs.companysTable.toggleRowSelection(c);
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
    var selectCompanys = [];
    if (seletcs.length > 0) {
      seletcs.forEach(company => {
        selectCompanys.push({ id: company.id, name: company.name });
      })

      var parentLayer = top.frames[this.windowName];
      parentLayer.$vue.selectOrganCallBack(selectCompanys);
      utils.closeLayerSelf();

    }
    else {
      utils.error("请选择至少一个单位进行管理", { layer: true });
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
