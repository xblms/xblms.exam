var $url = '/settings/usersCerAvatarUpload';

var data = utils.init({
  uploadParams: {},
  uploadFormpanel: false,
  uploadBtnLoad: false,
  fileList: [],
  fileListNo: [],
  fileListYes: [],
  errorOffset: 0,
  activeName: 'select',
});

var methods = {

  uploadFiles: function () {
    if (this.fileList.length > 0) {
      this.$refs.uploadFiles.submit();
    }
    else {
      utils.error("请选择文件");
    }
  },
  uploadBefore(file) {
    return true;
  },
  uploadChange: function (file, fileList) {

    var re = /(\.jpg|\.jpeg|\.png)$/i;
    if (!re.exec(file.name)) {
      fileList.splice(fileList.indexOf(file), 1);

      var newFile = file;
      newFile.msg = '仅支持图片格式文件(jpg,jpeg,png)';
      this.fileListNo.push(newFile);
      return false;
    }


    var isLt10M = file.size / 1024 / 1024 / 1024 < 2;
    if (!isLt10M) {
      fileList.splice(fileList.indexOf(file), 1);

      var newFile = file;
      newFile.msg = '文件大小不能超过 2 G!';
      this.fileListNo.push(newFile);
      return false;
    }


    this.fileList = fileList;
    return true;

  },
  uploadExceed: function (files, fileList) {
    return false;
  },
  uploadProgress: function (event, file, fileList) {
    this.uploadBtnLoad = true;
    if (file.percentage >= 99) {
      file.percentage = 99;
    }
  },
  uploadSuccess: function (response, file, fileList) {

    if (response.success) {
      this.fileListYes.push(file);
    }
    else {
      var newFile = file;
      newFile.msg = response.msg;
      this.fileListNo.push(newFile);
    }
    this.fileList.splice(fileList.indexOf(file), 1);
    if (this.fileList.length === 0) {
      this.uploadBtnLoad = false;
    }

  },

  uploadError: function (err, file, fileList) {
    var newFile = file;
    newFile.msg = JSON.parse(err.message).message;
    this.fileListNo.push(newFile);

    if (this.fileList.length === 0) {
      this.uploadBtnLoad = false;
    }
  },

  btnFileAbort: function (file) {
    this.$refs.uploadFiles.abort(file);
    this.fileList.splice(this.fileList.indexOf(file), 1);
    if (this.fileList.length <= 0) {
      this.uploadBtnLoad = false;
    }
  },
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    utils.loading(this, false);
    this.$urlUpload = $apiUrl + $url;
  }
});
