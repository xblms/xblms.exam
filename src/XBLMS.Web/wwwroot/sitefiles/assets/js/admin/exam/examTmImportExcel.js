var $url = '/exam/examTm/importExcel';
var $urlImportCache = $url + '/getCache';

var data = utils.init({
  uploadExcelTmLoading: false,
  uploadExcelTmList: [],
  uploadExcelTmimportMessageShow: false,
  uploadExcelTmImportResult: {
    success: 0,
    failure: 0,
    errorMessageList: []
  },
  importTmCache: null,
  uploadShow: true,
  urlImport: $apiUrl + $url + "/?treeId=" + utils.getQueryInt("treeId")
});

var methods = {
  uploadExcelTmBefore(file) {
    var isExcel = file.name.indexOf('.xlsx', file.name.length - '.xlsx'.length) !== -1;
    if (!isExcel) {
      utils.error('导入模板文件只能是 xlsx 格式!', { layer: true });
    }
    return isExcel;
  },

  uploadExcelTmProgress: function () {
    this.uploadShow = false;
    this.uploadExcelTmProgressCache();
  },
  uploadExcelTmProgressCache: function () {
    var $this = this;
    $api.get($urlImportCache).then(function (response) {
      var res = response.data;
      $this.importTmCache = res;
      if (!res.isStop) {
        setTimeout($this.uploadExcelTmProgressCache, 200);
      }
      else {
        $this.importTmCache.tmTotal = $this.importTmCache.tmCurrent = $this.importTmCache.tmTotal;
      }
    }).catch(function (error) {
      $this.importTmCache.tmTotal = $this.importTmCache.tmCurrent = $this.importTmCache.tmTotal;
    }).then(function () {
    });
  },
  uploadExcelTmSuccess: function (res, file) {
    var success = res.success;
    var failure = res.failure;
    var errorMessage = res.errorMessage;
    this.uploadExcelTmImportResult.success = res.success;
    this.uploadExcelTmImportResult.failure = res.failure;
    this.uploadExcelTmImportResult.errorMessageList = res.errorMessageList;
    this.uploadExcelTmimportMessageShow = true;
    this.$refs.importTmUpload.clearFiles();

    this.importTmCache.tmTotal = this.importTmCache.tmCurrent = this.importTmCache.tmTotal;

    utils.success("导入完成，请查看导入结果", { layer: true });
  },

  uploadExcelTmError: function (err) {
    utils.loading(this, false);
    var error = JSON.parse(err.message);
    utils.error(error.message, { layer: true });
    this.uploadExcelTmimportMessageShow = true;
  },
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    utils.loading(this, false);
  }
});
