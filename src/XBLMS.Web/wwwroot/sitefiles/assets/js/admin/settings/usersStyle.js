var $url = '/settings/usersStyle';

var data = utils.init({
  returnUrl: decodeURIComponent(utils.getQueryString('returnUrl')),
  urlUpload: null,
  styles: null,
  tableName: null,
  relatedIdentities: null,

  uploadPanel: false,
  uploadLoading: false,
  uploadList: []
});

var methods = {
  runTableStyleLayerAddMultiple: function() {
    this.apiGet();
  },

  runTableStyleLayerEditor: function() {
    this.apiGet();
  },

  runTableStyleLayerValidate: function() {
    this.apiGet();
  },

  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url).then(function (response) {
      var res = response.data;

      $this.styles = res.styles;
      $this.tableName = res.tableName;
      $this.relatedIdentities = res.relatedIdentities;

      $this.urlUpload = $apiUrl + $url + '/actions/import';
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  apiDelete: function (attributeName) {
    var $this = this;

    utils.loading(this, true);
    $api.post($url + '/del', {
      attributeName: attributeName
    }).then(function (response) {
      var res = response.data;
      utils.success("操作成功");
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      $this.apiGet();
      utils.loading($this, false);
    });
  },

  apiReset: function () {
    var $this = this;

    utils.loading(this, true);
    $api.post($url + '/actions/reset').then(function (response) {
      var res = response.data;

      $this.styles = res.value;
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  getRules: function(rules) {
    if (!rules || rules.length === 0) return '无验证';
    return _.map(rules, function (rule) {
      return rule.message;
    }).join(',');
  },

  btnValidateClick: function (attributeName) {
    utils.openLayer({
      title: '设置验证规则',
      url: utils.getCommonUrl('tableStyleLayerValidate', {
        tableName: this.tableName,
        relatedIdentities: this.relatedIdentities,
        attributeName: attributeName
      })
    });
  },

  btnAddMultipleClick: function () {
    utils.openLayer({
      title: '批量新增字段',
      url: utils.getCommonUrl('tableStyleLayerAddMultiple', {
        tableName: this.tableName,
        relatedIdentities: this.relatedIdentities,
        excludes: 'TextEditor,SelectCascading,Customize'
      })
    });
  },

  btnImportClick: function() {
    this.uploadPanel = true;
  },

  btnResetClick: function() {
    var $this = this;

    utils.alertDelete({
      title: '重置字段',
      text: '此操作将清空自定义字段并将用户字段恢复为系统默认值，确认重置吗？',
      callback: function () {
        $this.apiReset();
      }
    });
  },

  uploadBefore(file) {
    var isZip = file.name.indexOf('.zip', file.name.length - '.zip'.length) !== -1;
    if (!isZip) {
      utils.error('样式导入文件只能是 Zip 格式!');
    }
    return isZip;
  },

  uploadProgress: function() {
    utils.loading(this, true);
  },

  uploadSuccess: function(res, file) {
    this.uploadList = [];
    this.uploadPanel = false;
    utils.success('字段导入成功！');
    this.apiGet();
  },

  uploadError: function(err) {
    this.uploadList = [];
    utils.loading(this, false);
    var error = JSON.parse(err.message);
    utils.error(error.message);
  },

  btnExportClick: function() {
    window.open($apiUrl + $url + '/actions/export?access_token=' + $token);
  },
  btnAddClick: function () {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('tableStyleLayerEditor', {
        tableName: this.tableName,
        relatedIdentities: this.relatedIdentities,
        excludes: 'TextEditor,SelectCascading,Customize,Image,Video,Hidden,Number,CheckBox,Radio,File'
      }),
      width: "48%",
      height: "88%",
      end: function () {
        $this.apiGet();
      }
    });
  },
  handleCommand: function (type, row) {

    var $this = this;
    if (type === 'edit') {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getCommonUrl('tableStyleLayerEditor', {
          tableName: this.tableName,
          relatedIdentities: this.relatedIdentities,
          attributeName: row.attributeName,
          excludes: 'TextEditor,SelectCascading,Customize,Image,Video,Hidden,Number,CheckBox,Radio,File'
        }),
        width: "48%",
        height: "88%",
        end: function () {
          $this.apiGet();
        }
      });
    }
    if (type === 'delete') {

      top.utils.alertDelete({
        title: '删除字段',
        text: '此操作将删除字段 ' + row.attributeName + '，确定删除吗？',
        callback: function () {
          $this.apiDelete(row.attributeName);
        }
      });
    }
  },
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  }
});
