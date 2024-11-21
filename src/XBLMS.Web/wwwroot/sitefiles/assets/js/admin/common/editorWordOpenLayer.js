var $url = '/common/editorWordOpenLayer';

var data = utils.init({
  uploadList: [],
  uploadUrl: null,
  pf: utils.getQueryString("pf"),
});

var methods = {
  uploadBefore(file) {
    var re = /(\.docx)$/i;
    if (!re.exec(file.name)) {
      utils.error('文件只能是以.docx结尾的 Word 文件，请选择有效的文件上传!', { layer: true });
      return false;
    }
    return true;
  },
  uploadProgress: function () {
    utils.loading(this, true,"正在上传，请稍等...");
  },
  uploadSuccess: function (res) {
    utils.loading(this, false);
    utils.closeLayerSelf();

    var parentLayer = top.frames[this.pf];
    parentLayer.$vue.setWordCallBack(res.value);


  },
  uploadError: function (err) {
    utils.loading(this, false);
    var error = JSON.parse(err.message);
    utils.error(error.message, { layer: true });
  }
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.uploadUrl = $apiUrl + $url;
    utils.loading(this, false);
  }
});


