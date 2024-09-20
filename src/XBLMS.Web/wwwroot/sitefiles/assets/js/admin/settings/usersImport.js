var $url = '/settings/users';
var $urlUpload = $url + '/actions/import';
var $urlUploadCheck = $apiUrl + $url + '/actions/importCheck';

var data = utils.init({
  filePath: '',
  successCount: 0,
  failCount: 0,
  uploadForm: true,
  checkSuccess: false,
  checkError: false,
  importView: false,
  failList: null,
  uploadList: [],
});

var methods = {

  uploadBefore(file) {
    var isExcel = file.name.indexOf('.xlsx', file.name.length - '.xlsx'.length) !== -1;
    if (!isExcel) {
      utils.error('用户导入文件只能是 Excel 格式!', { layer: true });
    }
    return isExcel;
  },

  uploadProgress: function () {
    this.uploadForm = false;
    utils.loading(this, true, '正在验证导入数据...');
  },

  uploadSuccess: function (res, file) {

    var $this = this;
    $this.clear($this);

    $this.failCount = res.failure;
    $this.successCount = res.success;
    $this.filePath = res.filePath;

    if (res.value) {
      $this.checkSuccess = true;
    }
    else {
      $this.checkError = true;
      $this.failList = res.msgs;
    }
    utils.loading(this, false);
  },

  uploadError: function (err) {
    utils.loading(this, false);
    var error = JSON.parse(err.message);
    utils.error(error.message, { layer: true });
  },
  btnImport: function () {
    this.apiImport();
  },

  apiImport: function () {
    var $this = this;

    utils.loading(this, true, '正在导入，请稍等...');


    var rowNumber = [];
    if ($this.failList && $this.failList.length > 0) {
      $this.failList.forEach(fail => {
        rowNumber.push(fail.key);
      });
    }


    $api.post($urlUpload, {
      rowNumber: rowNumber,
      filePath: $this.filePath,
    }).then(function (response) {
      var res = response.data;

      $this.clear($this);
      $this.failCount = res.failure;
      $this.successCount = res.success;
      $this.importView = true;
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnClear: function () {
    this.clear(this);
  },
  clear: function ($this) {
    $this.checkSuccess = false;
    $this.checkError = false;
    $this.importView = false;
    $this.failList = null;
    $this.filePath = "";
  },
  btnCancelClick: function () {
    utils.closeLayer();
  }

};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    utils.loading(this, false);
  }
});
