var $url = "/exam/examPaperExaming";
var $urlItem = $url+ "/item";
var $urlSubmitAnswer = $url +"/submitAnswer";
var $urlSubmitPaper = $url + "/submitPaper";
var $urlSubmitTiming = $url + "/submitTiming";

var data = utils.init({
  id: utils.getQueryInt('id'),
  startId:0,
  list: null,
  paper: null,
  tm: null,
  watermark:null,
  answerTotal: 0,
  tmAnswerStatus:false,
  tmList: [],
  surplusSecond: 0,
  curTimingSecond: 1,
  datikaDialogVisible: false,
  loadCounts: utils.getQueryInt('loadCounts'),
});

var methods = {
  apiGet: function () {
    var $this = this;

    if (this.loadCounts >= 5) {
      utils.loading($this, false);
      utils.alertExamWarning({
        title: '温馨提示',
        text: "多次尝试加载试卷，结果还是一无所获。非常抱歉，请退出后重新进入考试。",
        button: '好的',
        callback: function () {
          $this.startId = utils.getQueryInt('loadStartId');
          $this.apiSubmitPaper();
          utils.closeLayerSelf();
        }
      });
    }
    else {
      if (this.loadCounts > 0) {
        utils.loading(this, true, "(" + this.loadCounts + ")正在加载试卷...");
      }
      else {
        utils.loading(this, true, "正在加载试卷...");
      }

      $api.get($url, { params: { id: $this.id, loadCounts: this.loadCounts } }).then(function (response) {
        var res = response.data;

        $this.startId = res.item.startId;

        if (res.item.tmTotal > 0) {
          $this.watermark = res.watermark;
          $this.paper = res.item;
          $this.list = res.txList;

          $this.startId = $this.paper.startId;

          $this.loadUseTime();
        }
        else {
          location.href = utils.getExamUrl('examPaperExaming', { id: $this.id, loadCounts: $this.loadCounts + 1, loadStartId: $this.startId })
        }

      }).catch(function (error) {
        utils.error(error, { layer: true });
      }).then(function () {
        utils.loading($this, false);
      });
    }

  },
  loadUseTime: function () {
    var $this = this;
    if ($this.paper.isTiming) {
      if ($this.paper.useTimeSecond > -1) {
        $this.surplusSecond = Date.now() + $this.paper.timingMinute * 60 * 1000 - $this.paper.useTimeSecond * 1000;
        if ($this.surplusSecond > 0) {
          $this.timingChange();
          $this.loadTm();
        }
        else {
          $this.btnSubmitPaperClick();
        }
      }
      else {
        $this.btnSubmitPaperClick();
      }
    }
    else {
      $this.loadTm();
    }

  },
  loadTm: function () {
    var $this = this;

    if ($this.list && $this.list.length > 0) {
      $this.list.forEach(item => {
        var cTmList = item.tmList;
        if (cTmList && cTmList.length > 0) {
          cTmList.forEach(ctm => {
            $this.tmList.push(ctm);
            if (ctm.answerStatus) {
              $this.answerTotal++;
            }
          })
        }

      });
    }
  },
  getTmAnswerStatus: function (id) {
    var getCurTm = this.tmList.find(item => item.id === id);
    return getCurTm.answerStatus;
  },
  answerChange: function (tm) {

    tm.answerStatus = false;

    if (tm.baseTx === "Duoxuanti") {
      tm.answerInfo.answer = tm.answerInfo.optionsValues.join('');
    }
    var completionStatus = true;
    if (tm.baseTx === "Tiankongti") {
      for (var i = 0; i < tm.answerInfo.optionsValues.length; i++) {
        if (tm.answerInfo.optionsValues[i] === '' || tm.answerInfo.optionsValues[i] === null) {
          completionStatus = false;
        }
      }
      tm.answerInfo.answer = tm.answerInfo.optionsValues.join(',');
    }

    if (tm.answerInfo.answer !== '' && tm.answerInfo.answer.length > 0) {
      if (completionStatus) {
        tm.answerStatus = true;
      }
      else {
        tm.answerStatus = false;
      }
    }


    var answerTotals = this.tmList.filter(f => f.answerStatus);
    this.answerTotal = answerTotals.length;

    this.apiSubmitAnswer(tm.answerInfo);
  },
  btnGoTm: function (id) {
    var tmel = document.getElementById("tmid_" + id);
    if (tmel) {
      tmel.scrollIntoView({ behavior: "smooth", block: "center" });
    }
    this.datikaDialogVisible = false;
  },
  apiSubmitAnswer: function (setTm) {
    $api.post($urlSubmitAnswer, { answer: setTm }).then(function (response) { });
  },
  apiSubmitPaper: function () {
    $api.post($urlSubmitPaper, { id: this.startId }).then(function (response) { });
  },
  apiSubmitTiming: function () {
    $api.post($urlSubmitTiming, { id: this.startId }).then(function (response) { });
  },
  btnSubmitPaperClick: function () {
    this.apiSubmitPaper();
    location.href = utils.getExamUrl("examPaperSubmitResult", { id: this.startId });
  },
  btnPaperSubmit: function () {
    var $this = this;

    if (this.answerTotal < this.paper.tmTotal) {
      utils.alertWarning({
        title: '温馨提示',
        text: '还剩（' + (this.paper.tmTotal - this.answerTotal) + '）道题没有答完，确定交卷么？',
        callback: function () {
          $this.btnSubmitPaperClick();
        }
      });
    }
    else {
      utils.alertWarning({
        title: '交卷提醒',
        text: '立即交卷，确定吗？',
        callback: function () {
          $this.btnSubmitPaperClick();
        }
      });
    }

  },
  timingFinish: function () {
    this.btnSubmitPaperClick();
  },
  timingChange: function () {
    if (this.curTimingSecond % 5 === 0) {
      this.apiSubmitTiming();
    }
    var $this = this;
    setTimeout(function () {
      $this.curTimingSecond++;
      $this.timingChange();
    },1000)
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
