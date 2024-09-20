var $url = 'settings/databaseTables';

var data = utils.init({
  tableColumns: null,
  tableNames: null,
  table: '',
  count: 0
});


var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url).then(function (response) {
      var res = response.data;

      $this.tableNames = res.tableNames;
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  apiPost: function () {
    var $this = this;

    utils.loading(true);
    $api.post($url, {
      tableName: this.table
    }).then(function (response) {
      var res = response.data;

      $this.tableColumns = res.tableColumns;
      $this.count = res.count;
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  getColumnAttributes: function (column) {
    var val = '';
    if (column.isIdentity) val += ", 自增长";
    if (column.isPrimaryKey) val += ", 主键";
    if (column.isExtend) val += ", 扩展字段";
    if (val) {
      val = val.substr(2);
    }
    return val;
  },

  getColumnLength: function (column) {
    if (column.dataType === 'VarChar') {
      return column.dataLength;
    }
    return '';
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
    this.apiGet();
  }
});

