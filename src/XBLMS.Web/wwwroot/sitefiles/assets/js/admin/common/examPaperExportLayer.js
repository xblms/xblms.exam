var $url = 'common/examPaperExportLayer';

var data = utils.init({
  id: utils.getQueryInt('id'),
  paperId: utils.getQueryInt('paperId'),
  withAnswer: true,
  type: utils.getQueryString('type'),
  url: null
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true, '正在导出，请稍等...');
    $api.get($url, {
      params: {
        id: this.id,
        paperId: this.paperId,
        withAnswer: this.withAnswer,
        type: this.type
      }
    }).then(function (response) {
      var res = response.data;
      $this.url = res.value;

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnDownloadClick: function () {
    window.open(this.url);
  },
  btnExportClick: function () {
    this.apiGet();
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
