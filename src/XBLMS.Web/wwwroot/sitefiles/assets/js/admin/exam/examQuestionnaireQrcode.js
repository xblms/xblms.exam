var $url = 'exam/examQuestionnaireQrcode';

var data = utils.init({
  id: utils.getQueryInt('id'),
  title: '',
  qrcodeUrl:''
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url, { params: { id: this.id }}).then(function (response) {
      var res = response.data;
      $this.title = res.title;
      $this.qrcodeUrl = res.qrcodeUrl;
    
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnCopyUrlClick: function () {
    let oInput = document.createElement('input');
    oInput.value = this.qrcodeUrl;
    document.body.appendChild(oInput);
    oInput.select();
    document.execCommand('Copy');
    utils.success("复制成功", { layer: true });
    oInput.remove();
  }
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  }
});
