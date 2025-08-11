var $url = "/exam/examQuestionnairing";
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

    utils.loading(this, true, "正在加载问卷内容...");
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
  answerChangeDuowei: function (setTm) {
    setTm.answer = setTm.optionsValues.join('');
    if (setTm.answer !== '' && setTm.answer.length > 0) {
      setTm.answerStatus = true;
    }
    else {
      setTm.answerStatus = false;
    }
    this.getAnswerTotal(setTm);
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

    this.getAnswerTotal(setTm);
  },
  getAnswerTotal: function (setTm) {
    this.answerTotal = 0;

    var smallTm = this.tmList.find(item => item.id === setTm.id);


    this.tmList.forEach(tm => {

      if (tm.parentId > 0 && tm.id === setTm.id) {
        tm.answerStatus = setTm.answerStatus;
        tm.answer = setTm.answer;
      }

      if (tm.answerStatus) {
        this.answerTotal++;
      }
      else {
        if (tm.id === setTm.parentId && !tm.answerStatus) {
          tm.answerStatus = true;
          this.answerTotal++;
        }
      }
    });
  },
  apiSubmitPaper: function () {
    var $this = this;
    utils.loading($this, true, "正在提交问卷...");
    $api.post($urlSubmitPaper, { id: this.id, tmList: this.tmList }).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success("已提交");
      }
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      if ($this.paper.published) {
        document.body.innerHTML = "已提交问卷，可以安全离开此页面";
        document.body.style.display = "block";

      }
      else {
        utils.closeLayerSelf();
        utils.loading($this, false);
      }
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
    var tmel = document.getElementById("tmid_" + id);
    if (tmel) {
      tmel.scrollIntoView({ behavior: "smooth", block: "center" });
    }
  },
  checkAnswer: function () {
    for (let i = 0; i < this.tmList.length; i++) {
      let letTm = this.tmList[i];
      if (!letTm.answerStatus && letTm.parentId === 0) {
        this.tmDidScroll(letTm.id);
        utils.error('请完善问卷', { layer: true });
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
    document.title = "调查问卷";
  },
});
