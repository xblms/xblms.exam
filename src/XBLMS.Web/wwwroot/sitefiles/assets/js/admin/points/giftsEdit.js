var $url = '/points/pointshop/item';
var $urlUpload = $url + "/upload";

var data = utils.init({
  id: utils.getQueryInt('id'),
  uploadUrl: null,
  form: null,
  fileList: [],
  uploadFileList: []
});

var methods = {
  apiGet: function () {
    var $this = this;

    $api.get($url, {
      params: {
        id: this.id
      }
    }).then(function (response) {
      var res = response.data;

      $this.form = _.assign({}, res.item);
      if ($this.id > 0) {
        $this.fileList = $this.uploadFileList = res.fileList;
      }

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },

  apiSubmit: function () {
    var $this = this;

    utils.loading(this, true);
    $api.post($url, { item: this.form }).then(function (response) {
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



  btnSubmitClick: function () {
    var $this = this;

    this.$refs.form.validate(function (valid) {
      if (valid) {
        if ($this.uploadFileList.length > 0) {
          $this.form.coverImg = _.map($this.uploadFileList, function (item) { return item.url; });
          $this.apiSubmit();
        }
        else {
          utils.error("请上传商品图片，要不难看的很", { layer: true });
        }
      }
    });
  },

  uploadBefore(file) {
    var re = /(\.jpg|\.jpeg|\.bmp|\.gif|\.png|\.webp)$/i;
    if (!re.exec(file.name)) {
      utils.error('只能是图片格式，请选择有效的文件上传!', { layer: true });
      return false;
    }

    var isLt10M = file.size / 1024 / 1024 < 10;
    if (!isLt10M) {
      utils.error('图片大小不能超过 10MB!', { layer: true });
      return false;
    }

    const formData = new FormData();
    formData.append('file', file);
    return formData;

  },
  uploadProgress: function (event, file, fileList) {
    utils.loading(this, true);
  },

  uploadSuccess: function (res, file, fileList) {
    this.fileList = fileList;
    this.uploadFileList.push(res);
    utils.loading(this, false);
  },

  uploadError: function (err) {
    utils.loading(this, false);
    var error = JSON.parse(err.message);
    utils.error(error.message, { layer: true });
  },

  uploadRemove(file) {
    this.fileList = this.fileList.filter(f => f.name !== file.name);
    this.uploadFileList = this.uploadFileList.filter(f => f.name !== file.name);
  },
  uploadPreview: function (file) {
    utils.openLayerPhoto({ src: file.url });
  },
  btnOpenEditClick: function (ref, ptype) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('editorOpenLayer', { pf: window.name, ptype: ptype, ref: ref }),
      width: "58%",
      height: "78%"
    });
  }
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.uploadUrl = $apiUrl + $urlUpload;
    this.apiGet();
  }
});
