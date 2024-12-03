var $url = "/exam/examAssessmenting";
var $urlSubmitPaper = $url + "/submitPaper";
var data = utils.init({
  id: utils.getQueryInt('id'),
  pr: utils.getQueryString('pr'),
  ps: utils.getQueryString('ps'),
  paper: null,
  watermark: null,
  answerTotal: 0,
  tmTotal: 0,
  tmList: []
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true, "正在加载测评内容...");
    $api.get($url, { params: { id: $this.id, ps: $this.ps } }).then(function (response) {
      var res = response.data;

      $this.watermark = res.watermark;
      $this.paper = res.item;

      if ($this.paper.published) {
        $this.watermark = DOCUMENTTITLE + "-" + res.watermark;
        $this.id = $this.paper.id;
      }

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
    utils.loading($this, true, "正在提交测评...");
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
        let check = $this.checkAnswer();
        if (check) {
          $this.apiSubmitPaper();
        }
      }
    });
  },
  tmDidScroll: function (id) {
    var eid = '#tm_' + id;
    var scrollTop = ($(eid).offset().top) - 128;
    document.documentElement.scrollTop = document.body.scrollTop = scrollTop;
  },
  checkAnswer: function () {
    for (let i = 0; i < this.tmList.length; i++) {
      let letTm = this.tmList[i];
      if (!letTm.answerStatus) {
        this.tmDidScroll(letTm.id);
        utils.error('请完善测评', { layer: true });
        return false;
      }
    }
    return true;
  }
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
    document.title = "测评中心";
  },
});
