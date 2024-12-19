var $url = 'common/examPaperLayerView';

var data = utils.init({
  id: utils.getQueryInt('id'),
  list: null,
  randomIds: null,
  paper: null,
  curRandomId:0
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url, {
      params: {
        id: this.id,
        randomId: this.curRandomId
      }
    }).then(function (response) {
      var res = response.data;

      $this.paper = res.item;
      $this.list = res.txList;
      $this.randomIds = res.randomIds;
      $this.curRandomId = res.randomId;

    }).catch(function (error) {
      utils.error(error, { layer:true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  tmDidScroll: function (id) {
    this.$refs['answerScrollbar'].wrap.scrollTop = 0;
    var eid = '#tm_' + id;
    var scrollTop = ($(eid).offset().top) - 128;
    this.$refs['answerScrollbar'].wrap.scrollTop = scrollTop;
  },
  selectChangeRandomConfig: function (value) {
    this.curRandomId = value;
    this.apiGet();
  },
  btnExportWordClick: function () {
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('examPaperExportLayer', { id: this.curRandomId, paperId: this.id, type:'PaperOnlyOne' }),
      width: "58%",
      height: "58%",
    });
  },
  btnExportWordZipClick: function () {
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('examPaperExportLayer', { paperId: this.id, type: 'PaperRar' }),
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
    this.apiGet();
  }
});
