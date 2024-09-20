var $url = "/exam/examQuestionnairing";
var $urlSubmitPaper = $url + "/submitPaper";
var data = utils.init({
  id: utils.getQueryInt('id'),
  paper: null,
  watermark:null,
  answerTotal: 0,
  tmTotal:0,
  tmList: []
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true, "正在加载问卷内容...");
    $api.get($url, { params: { id: $this.id } }).then(function (response) {
      var res = response.data;

      $this.watermark = res.watermark;
      $this.paper = res.item;
      $this.tmList = res.tmList;

      $this.tmTotal = $this.paper.tmTotal;

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  answerChange: function (setTm) {

    if (setTm.tx === "Duoxuanti") {
      setTm.answer = setTm.optionsValues.join('');
    }

    if (setTm.answer !== '' && setTm.answer.length > 0) {
      setTm.answerStatus = true;
    }
    else {
      setTm.answerStatus = false;
    }
    this.getAnswerTotal();
  },
  getAnswerTotal: function () {
    this.answerTotal = 0;
    this.tmList.forEach(tm => {
      if (tm.answerStatus) {
        this.answerTotal++;
      }
    });
  },
  apiSubmitPaper: function () {
    var $this = this;
    utils.loading($this, true,"正在提交问卷...");
    $api.post($urlSubmitPaper, { id: this.id, tmList: this.tmList }).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success("已提交");
      }
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.closeLayerSelf();
      utils.loading($this, false);
    });
  },
  btnPaperSubmit: function () {
    var $this = this;
    top.utils.alertWarning({
      title: '确定提交吗？',
      callback: function () {
        $this.apiSubmitPaper();
      }
    });
  },
  tmDidScroll: function (id) {
    this.$refs['answerScrollbar'].wrap.scrollTop = 0;
    var eid = '#tm_' + id;
    var scrollTop = ($(eid).offset().top) - 128;
    this.$refs['answerScrollbar'].wrap.scrollTop = scrollTop;
  },
  checkAnswer: function () {
    this.tmList.forEach(tm => {
      if (!tm.answerStatus) {
        this.tmDidScroll(tm.id);
        utils.error('请完善问卷', { layer: true });
      }
    });
  }
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  },
});
