var $url = "/exam/examPaperExaming";
var $urlItem = $url + "/item";
var $urlSubmitAnswer = $url + "/submitAnswer";
var $urlSubmitPaper = $url + "/submitPaper";
var $urlSubmitTiming = $url + "/submitTiming";

var data = utils.init({
  id: utils.getQueryInt('id'),
  startId: 0,
  list: null,
  paper: null,
  tm: null,
  watermark: null,
  answerTotal: 0,
  tmAnswerStatus: false,
  tmList: [],
  surplusSecond: 0,
  curTimingSecond: 1,
  isLoad: false,
  isScreen: false
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

      $this.startId = $this.paper.startId;

      $this.loadFullScree();

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  loadFullScree: function () {
    var $this = this;
    if ($this.paper.fullScreen) {
      utils.alertExamWarning({
        title: '温馨提示',
        text: "本场考试要求启用浏览器全屏。",
        button: '知道了',
        callback: function () {
          $this.fullScreen();
          setTimeout($this.loadExist, 200);
        }
      });
    }
    else {
      $this.loadExist();
    }
  },
  loadExist: function () {
    var $this = this;
    if ($this.paper.existCount > 0) {
      if ($this.paper.existUserCount >= $this.paper.existCount) {
        $this.btnSubmitPaperClick();
      }
      else {
        utils.alertExamWarning({
          title: '温馨提示',
          text: "请勿随意退出考试页面，退出次数达到" + $this.paper.existCount + "（" + $this.paper.existUserCount + "）次后将强制交卷。",
          button: '知道了',
          callback: function () {
            $this.loadUseTime();
          }
        });
      }
    }
    else {
      $this.loadUseTime();
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

      $this.isLoad = true;

      $this.btnGetTm($this.tmList[0].id);
    }
  },
  btnGetTm(id) {
    this.tm = null;
    var $this = this;
    $this.$nextTick(() => {
      setTimeout(function () {
        var getCurTm = $this.tmList.find(item => item.id === id);
        $this.tm = getCurTm;
      }, 200);
    })
  },
  getTmAnswerStatus: function (id) {
    var getCurTm = this.tmList.find(item => item.id === id);
    return getCurTm.answerStatus;
  },
  answerChange: function () {

    let setTm = this.tm;

    this.tmList = this.tmList.filter(f => f.id !== setTm.id);
    setTm.answerStatus = false;

    if (setTm.baseTx === "Duoxuanti") {
      setTm.answerInfo.answer = setTm.answerInfo.optionsValues.join('');
    }
    var completionStatus = true;
    if (setTm.baseTx === "Tiankongti") {
      for (var i = 0; i < setTm.answerInfo.optionsValues.length; i++) {
        if (setTm.answerInfo.optionsValues[i] === '' || setTm.answerInfo.optionsValues[i] === null) {
          completionStatus = false;
        }
      }
      setTm.answerInfo.answer = setTm.answerInfo.optionsValues.join(',');
    }

    if (setTm.answerInfo.answer !== '' && setTm.answerInfo.answer.length > 0) {
      if (completionStatus) {
        setTm.answerStatus = true;
      }
      else {
        setTm.answerStatus = false;
      }
    }

    this.tmList.push(setTm);


    var answerTotals = this.tmList.filter(f => f.answerStatus);
    this.answerTotal = answerTotals.length;

    this.apiSubmitAnswer(setTm.answerInfo);
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
    }, 1000)
  },
  fullScreen: function () {
    let element = document.documentElement;
    if (element.requestFullscreen) {
      element.requestFullscreen();
    } else if (element.webkitRequestFullScreen) {
      element.webkitRequestFullScreen();
    } else if (element.mozRequestFullScreen) {
      element.mozRequestFullScreen();
    } else if (element.msRequestFullscreen) {
      element.msRequestFullscreen();
    }
  },
  isFullscreenFun: function () {
    if (document.fullscreenElement ||
      document.msFullscreenElement ||
      document.mozFullScreenElement ||
      document.webkitFullscreenElement) {
      return true;
    }
    return false;
  },
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    utils.loading(this, false);
    this.apiGet();
  },
  mounted: function () {
    let exitHandler = () => {
      var $this = this;

      if (!$this.isFullscreenFun()) {
        utils.alertExamWarning({
          title: '温馨提示',
          text: "本场考试要求启用浏览器全屏。",
          button: '知道了',
          callback: function () {
            $this.fullScreen();
          }
        });
      }

    };
    let visibilityHandler = (e) => {
      var $this = this;
      switch (e.target.visibilityState) {
        case 'prerender':
          break;
        case 'hidden':
          break;
        case 'visible':
          if ($this.paper.existCount > 0) {
            $this.paper.existUserCount++;
            if ($this.paper.existUserCount >= $this.paper.existCount) {
              $this.btnSubmitPaperClick();
            }
            else {
              utils.alertExamWarning({
                title: '温馨提示',
                text: "你刚刚离开了考试页面，退出次数已达" + $this.paper.existUserCount + "（" + $this.paper.existCount + "）次。",
                button: '继续考试',
                callback: function () {
                  if ($this.paper.fullScreen) {
                    $this.fullScreen();
                  }
                }
              });
            }
            break;
          }
      }
    };
    document.addEventListener('visibilitychange', visibilityHandler, false);
    document.addEventListener('webkitfullscreenchange', exitHandler, false);
    document.addEventListener('mozfullscreenchange', exitHandler, false);
    document.addEventListener('fullscreenchange', exitHandler, false);
    document.addEventListener('MSFullscreenChange', exitHandler, false);
    document.addEventListener('msfullscreenchange', exitHandler, false);
  }
});
