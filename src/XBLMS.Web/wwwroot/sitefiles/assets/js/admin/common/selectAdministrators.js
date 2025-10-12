var $url = '/common/selectAdministrators';

var data = utils.init({
  administrators: null,
  count: null,
  formInline: {
    organId: 0,
    organType:'',
    role: '',
    order: '',
    lastActivityDate: 0,
    keyword: '',
    currentPage: 1,
    offset: 0,
    limit: PER_PAGE
  },
  selectRow:null
});

var methods = {
  apiGet: function () {
    var $this = this;

    $api.get($url, {
      params: this.formInline
    }).then(function (response) {
      var res = response.data;
      $this.count = res.count;
      $this.administrators = res.administrators;
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  btnViewClick: function(row) {
    utils.openAdminView(row.id);
  },

  btnSearchClick() {
    this.apiGet();
  },
  selectCurrentChange: function (row) {
    this.selectRow = row;
  },
  handleCurrentChange: function(val) {
    this.formInline.currentValue = val;
    this.formInline.offset = this.formInline.limit * (val - 1);

    this.btnSearchClick();
  },
  btnSelectConfirmClick: function () {

    if (this.selectRow && this.selectRow.id > 0) {
      var parentFrameName = utils.getQueryString("pf");
      var parentLayer = top.frames[parentFrameName];
      parentLayer.$vue.selectAdminCallback(this.selectRow.id, this.selectRow.displayName);
      utils.closeLayerSelf();
    }
    else {
      utils.error("单击行来选择", { layer: true });
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
