var $url = '/exam/examTmStyle';

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
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url).then(function (response) {
      var res = response.data;

      $this.styles = res.styles;
      $this.tableName = res.tableName;
      $this.relatedIdentities = res.relatedIdentities;

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
  btnAddClick: function () {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('tableStyleLayerEditor', {
        tableName: this.tableName,
        relatedIdentities: this.relatedIdentities,
        excludes: 'Number,TextArea,TextEditor,CheckBox,Radio,SelectCascading,Date,DateTime,Image,Video,File,Customize,Hidden'
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
          excludes: 'Number,TextArea,TextEditor,CheckBox,Radio,SelectCascading,Date,DateTime,Image,Video,File,Customize,Hidden'
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
