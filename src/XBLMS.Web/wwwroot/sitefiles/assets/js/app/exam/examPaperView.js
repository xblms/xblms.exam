var $url = "/exam/examPaperView";
var $urlItem = $url+ "/item";

var data = utils.init({
  id: utils.getQueryInt('id'),
  list: null,
  paper: null,
  tm: null,
  watermark:null,
  answerTotal: 0,
  tmAnswerStatus:false,
  tmList: [],
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true, "正在加载试卷...");

    $api.get($url, { params: { id: $this.id } }).then(function (response) {
      var res = response.data;

      $this.watermark = res.watermark;
      $this.paper = res.item;
      $this.list = res.txList;

      if ($this.list && $this.list.length > 0) {
        $this.list.forEach(item => {
          var cTmList = item.tmList;
          if (cTmList && cTmList.length > 0) {
            cTmList.forEach(ctm => {
              $this.tmList.push(ctm);
            })
          }

        });

        $this.btnGetTm($this.tmList[0].id);
      }

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnGetTm(id) {
    this.tm = null;
    var $this = this;
    $this.$nextTick(() => {
      var getCurTm = $this.tmList.find(item => item.id === id);
      $this.tm = getCurTm;
    })
  },
  getTmAnswerStatus: function (id) {
    var getCurTm = this.tmList.find(item => item.id === id);
    return getCurTm.isRight;
  },
  btnDownClick: function () {
    var curIndex = this.tm.tmIndex;
    let downTm = this.tmList.find(item => item.tmIndex === (curIndex + 1))
    this.btnGetTm(downTm.id);
  },
  btnUpClick: function () {
    var curIndex = this.tm.tmIndex;
    let upTm = this.tmList.find(item => item.tmIndex === (curIndex - 1))
    this.btnGetTm(upTm.id);
  }
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    utils.loading(this, false);
    this.apiGet();
  },
});
