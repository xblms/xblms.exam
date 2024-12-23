var $url = '/exam/examPaperManager';

var $urlScore = $url + '/score';
var $urlScoreExport = $urlScore + '/export';

var data = utils.init({
  id: 0,
  title: '',
  formScore: {
    id: 0,
    keywords: '',
    dateFrom: '',
    dateTo: '',
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  scoreList: null,
  scoreTotal: 0
});

var methods = {
  btnViewClick: function (id) {
    utils.openUserView(id);
  },
  btnAdminViewClick: function (id) {
    utils.openAdminView(id);
  },

  apiGet: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($urlScore, { params: $this.formScore }).then(function (response) {
      var res = response.data;
      $this.scoreList = res.list;
      $this.scoreTotal = res.total;

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnScoreSearchClick: function () {
    this.formScore.pageIndex = 1;
    this.apiGet();
  },
  btnScoreExportClick: function () {
    var $this = this;

    utils.loading(this, true);
    $api.post($urlScoreExport, this.formScore).then(function (response) {
      var res = response.data;

      window.open(res.value);
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  scoreHandleCurrentChange: function (val) {
    this.formScore.pageIndex = val;
    this.apiGet();
  },
  btnPaperSocreView: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('examPaperUserLayerView', { id: id }),
      width: "99%",
      height: "99%"
    });
  },

  btnMarkView: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('examPaperUserMarkLayerView', { id: id }),
      width: "99%",
      height: "99%"
    });
  },
  btnExportWordClick: function (id) {
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('examPaperExportLayer', { id: id, paperId: this.id, type: 'PaperScoreOnlyOne' }),
      width: "58%",
      height: "58%",
    });
  },
  btnExportWordZipClick: function () {
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('examPaperExportLayer', { paperId: this.id, type: 'PaperScoreRar', dateFrom: this.formScore.dateFrom, dateTo: this.formScore.dateTo, keywords: this.formScore.keywords }),
      width: "58%",
      height: "58%",
    });
  }
};
var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.id = this.formScore.id = utils.getQueryInt("id");
    this.apiGet();
  }
});
