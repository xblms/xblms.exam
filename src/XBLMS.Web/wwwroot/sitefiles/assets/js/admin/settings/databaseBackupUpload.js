var $url = 'settings/database/backup';

var data = utils.init({
  form: {
    securityKey: '',
    fileList:null
  },
  fileList:[],
  uploadBackupUrl: null,
  showFileList: true
});

var methods = {
  btnUploadClick: function () {
    var $this = this;
    this.$refs.form.validate(function (valid) {
      if (valid) {
        $this.$refs.upload.submit();
      }
    });
  },
  handleChange(file, fileList) {
    if (fileList && fileList.length > 0) {
      this.form.fileList = fileList;
    }
    else {
      this.form.fileList = null;
    }
  },
  handleExceed(files, fileList) {
    utils.error('仅支持单个文件上传', { layer: true });
  },
  beforeUpload(file) {
    if (!/\.(zip)$/.test(file.name)) {
      utils.error("请上传zip压缩包", { layer: true })
      return false;
    }
    else {
      this.showFileList = true;
      return true;
    }
  },
  customUpload(file) {
    this.fileUpload($url + '/upload', file, 0, utils.uuid());
  },
  fileUpload(uploadUrl, option, chunk, guid) {
    let vm = this;
    let file = option.file;
    let chunkSize = 1024 * 1024 * 2;
    let maxSize = 1024 * 1024 * 1024 * 100;
    let maxChunk = Math.ceil(file.size / chunkSize);
    let formData = new FormData();

    //将文件进行分段
    let fileSize = file.size;
    if (fileSize > maxSize) {
      utils.error("文件大小不能超过" + maxSize / 1024 / 100 / 1024 / 1024 + "M", { layer: true });
      return;
    }

    //当前上传进度
    let currentPercent = parseInt((chunk / maxChunk) * 100);
    option.onProgress({ percent: currentPercent }); //进度条
    formData.append(
      "file",
      file.slice(chunk * chunkSize, (chunk + 1) * chunkSize)
    );

    formData.append("securityKey", this.form.securityKey);
    formData.append("name", file.name);
    formData.append("chunk", chunk);
    formData.append("maxChunk", maxChunk);
    formData.append("guid", guid);

    var $this = this;
    $api.post(uploadUrl, formData).then(function (response) {
      var res = response.data;
      if (res.value) {
        top.utils.success("上传成功")
        utils.closeLayerSelf();
      }
      else {
        $this.fileUpload(uploadUrl, option, ++chunk, guid);
      }
    }).catch(function (error) {
      utils.error(error, { layer: true });
      $this.$refs.upload.clearFiles();
      $this.form.fileList = null;
      $this.form.securityKey = "";
      utils.loading($this, false);
    }).then(function () {
      utils.loading($this, false);
    });
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
