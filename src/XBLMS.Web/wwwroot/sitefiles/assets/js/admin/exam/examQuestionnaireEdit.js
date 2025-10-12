var $url = 'exam/examQuestionnaireEdit';
var $urlUploadTm = $url + '/uploadTm';

var data = utils.init({
  id: utils.getQueryInt('id'),
  copyId: utils.getQueryInt('copyId'),
  isCourseUse: utils.getQueryBoolean('isCourseUse'),
  isSelect: false,
  form: null,
  tmList: [],
  submitDialogVisible: false,
  submitSubmitType: 'Save',
  tmImportDialogVisible: false,
  uploadShow: true,
  urlImport: $apiUrl + "/" + $urlUploadTm,
  uploadLoading: false,
  uploadResult: false,
  errorMsgList: [],
  uploadExcelTmList: [],
  userGroupList: null,
  successTotal: 0,
  systemCode: null
});

var methods = {
  apiGet: function () {
    var $this = this;

    this.isSelect = this.isCourseUse;

    utils.loading(this, true);
    $api.get($url, { params: { id: this.id } }).then(function (response) {
      var res = response.data;

      $this.userGroupList = res.userGroupList;
      $this.systemCode = res.systemCode;

      $this.form = _.assign({}, res.item);
      if ($this.id > 0) {
        $this.tmList = res.tmList;
        if ($this.copyId > 0) {
          $this.id = 0;
          $this.form.id = 0;
          $this.form.title = $this.form.title + "-复制";
          $this.form.submitType = "Save";
        }
      }
      else {
        $this.form.isCourseUse = $this.isCourseUse;
      }
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnSaveClick: function () {
    this.submitSubmitType = 'Save';
    var $this = this;
    this.$refs.form.validate(function (valid) {
      if (valid) {
        var valido = $this.submitValid();
        if (!valido) return;

        $this.apiSubmit();
      }
    });

  },
  btnSubmitClick: function () {
    this.submitSubmitType = 'Submit';
    var $this = this;
    this.$refs.form.validate(function (valid) {
      if (valid) {
        var valido = $this.submitValid();
        if (!valido) return;

        if ($this.id > 0 && $this.form.submitType === 'Submit') {
          $this.submitDialogVisible = true;
        }
        else {
          $this.apiSubmit();
        }
      }
    });
  },
  btnSubmit: function () {
    this.apiSubmit();
  },
  btnSubmitClear: function () {
    var $this = this;
    top.utils.alertWarning({
      title: '重新发布提醒',
      text: '重新发布将清空历史数据，确定重新发布吗？',
      confirmButtonText: '重新发布',
      showCancelButton: true,
      callback: function () {
        $this.apiSubmit();
      }
    });

  },
  apiSubmit: function () {

    var $this = this;
    utils.loading($this, true);
    $api.post($url, { submitType: this.submitSubmitType, item: $this.form, tmList: $this.tmList }).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success("操作成功");
        utils.closeLayerSelf();
      }

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  submitValid: function () {
    if (this.tmList && this.tmList.length > 0) {
      return true;
    }
    utils.error("请导入问卷内容", { layer: true });
    return false;
  },
  btnImportTmClick: function () {
    this.tmImportDialogVisible = true;
    this.uploadShow = true;
    this.uploadLoading = false;
    this.uploadResult = false;
  },
  uploadExcelTmBefore(file) {
    var isExcel = file.name.indexOf('.xlsx', file.name.length - '.xlsx'.length) !== -1;
    if (!isExcel) {
      utils.error('导入模板文件只能是 xlsx 格式!', { layer: true });
    }
    this.uploadLoading = true;
    this.uploadShow = false;
    return isExcel;
  },

  uploadExcelTmProgress: function () {

  },
  uploadExcelTmSuccess: function (res, file) {
    this.$refs.importTmUpload.clearFiles();

    this.uploadLoading = false;
    this.tmList = res.tmList;
    this.errorMsgList = res.errorMsgList;
    this.successTotal = res.successTotal;
    this.uploadResult = true;
    utils.success("导入完成，请查看导入结果", { layer: true });
  },

  uploadExcelTmError: function (err) {
    this.uploadLoading = false;
    var error = JSON.parse(err.message);
    utils.error(error.message, { layer: true });
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
