var $url = 'settings/databaseQuery';

var data = utils.init({
  form: {
    query: ''
  },
  results: null,
  properties: null,
  count: 0
});


var methods = {
  apiQuery: function () {
    var $this = this;

    utils.loading($this, true);
    $api.post($url, this.form).then(function (response) {
      var res = response.data;

      $this.results = res.results;
      $this.properties = res.properties;
      $this.count = res.count;
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  getColumnAttributes: function (columnInfo) {
    var val = '';
    if (columnInfo.isIdentity) val += ", 自增长";
    if (columnInfo.isPrimaryKey) val += ", 主键";
    if (columnInfo.isExtend) val += ", 扩展字段";
    if (val) {
      val = val.substr(2);
    }
    return val;
  },

  btnSubmitClick: function () {
    var $this = this;
    this.$refs.form.validate(function (valid) {
      if (valid) {
        $this.apiQuery();
      }
    });
  }
};

var $vue = new Vue({
  el: '#main',
  data: data,
  watch: {
    table: function (val, oldVal) {
      this.apiPost();
    }
  },
  methods: methods,
  created: function () {
    utils.loading(this, false);
  }
});

