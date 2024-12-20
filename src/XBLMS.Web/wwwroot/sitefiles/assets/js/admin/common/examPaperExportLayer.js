var $url = 'common/examPaperExportLayer';

var data = utils.init({
  id: utils.getQueryInt('id'),
  paperId: utils.getQueryInt('paperId'),
  withAnswer: true,
  type: utils.getQueryString('type'),
  dateFrom: utils.getQueryString('dateFrom'),
  dateTo: utils.getQueryString('dateTo'),
  keywords: utils.getQueryString('keywords'),
  url: null
});

var methods = {
  apiGet: function () {
    var $this = this;
    if (this.type === 'PaperScoreOnlyOne' || this.type === 'PaperScoreRar') {
      $url = $url + "/score";
    }
    utils.loading(this, true, '正在导出，请稍等...');
    $api.get($url, {
      params: {
        id: this.id,
        paperId: this.paperId,
        withAnswer: this.withAnswer,
        type: this.type,
        dateFrom: this.dateFrom,
        dateTo: this.dateTo,
        keywords: this.keywords
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
