var $url = '/knowledges/edit';

var data = utils.init({
  id: utils.getQueryInt('id'),
  form: null,
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url, { params: { id: this.id } }).then(function (response) {
      var res = response.data;

      $this.form = _.assign({}, res.item);

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
        $this.apiSubmit();
      }
    });

  },
  uploadBefore(file) {
    var re = /(\.jpg|\.jpeg|\.bmp|\.gif|\.png|\.webp)$/i;
    if (!re.exec(file.name)) {
      utils.error('封面只能是图片格式，请选择有效的文件上传!', { layer: true });
      return false;
    }

    var isLt10M = file.size / 1024 / 1024 < 10;
    if (!isLt10M) {
      utils.error('封面大小不能超过 10MB!', { layer: true });
      return false;
    }
    return true;
  },
  uploadProgress: function () {
    utils.loading(this, true);
  },

  uploadSuccess: function (res, file, fileList) {
    this.form.coverImgUrl = res.value;
    utils.loading(this, false);
    if (fileList.length > 1) fileList.splice(0, 1);
  },
  uploadError: function (err) {
    utils.loading(this, false);
    var error = JSON.parse(err.message);
    utils.error(error.message, { layer: true });
  },

  uploadRemove(file) {
    this.form.coverImgUrl = null;
  },
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.uploadUrl = $apiUrl + $url + '/upload';
    this.apiGet();
  }
});
