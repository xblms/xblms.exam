var $url = '/exam/examPracticing';
var $urlTm = $url + '/tm';
var $urlAnswer = $url + '/answer';
var $urlCollection = $url + '/collection';
var $urlCollectionRemove = $url + '/collectionRemove';
var $urlWrongRemove = $url + '/wrongRemove';

var data = utils.init({
  id: utils.getQueryInt("id"),
  tmIds: [],
  tmList: [],
  total: 0,
  answerTotal: 0,
  rightTotal: 0,
  wrongTotal: 0,
  title: '',
  tm: null,
  tmIndex: 0,
  watermark: null,
  answerResult: null
});

var methods = {
  apiGet: function () {
    var $this = this;
    utils.loading(this, false);
    utils.loading(this, true, "正在加载题目...");

    $api.get($url, { params: { id: this.id } }).then(function (response) {
      var res = response.data;
      $this.tmIds = res.tmIds;
      $this.total = res.total;
      $this.title = res.title;

      top.utils.pointNotice(res.pointNotice);

      $this.watermark = res.watermark;
      $this.apiGetTmInfo($this.tmIds[0])

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  apiGetTmInfo: function (tmid) {
    var $this = this;
    utils.loading(this, true);

    $api.get($urlTm, { params: { id: tmid } }).then(function (response) {
      var res = response.data;
      $this.tm = JSON.parse(utils.AESDecrypt(res.tm, res.salt));
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  answerChange: function () {

    let setTm = this.tm;
    let answer = setTm.answer;

    if (setTm.baseTx === "Duoxuanti") {
      answer = setTm.optionsValues.join('');
    }
    if (setTm.baseTx === "Tiankongti") {
      answer = setTm.optionsValues.join(',');
    }

    this.tm.answer = answer;
  },
  answerChangeSmall: function () {
    var smallList = this.tm.smallList;
    var allSmallAnswer = true;
    smallList.forEach(small => {
      let answer = small.myAnswer;
      if (small.baseTx === "Duoxuanti") {
        answer = small.optionsValues.join('');
      }
      if (small.baseTx === "Tiankongti") {
        answer = small.optionsValues.join(',');
      }
      small.myAnswer = answer;
      if (answer.length === 0) {
        allSmallAnswer = false;
      }
    })
    if (allSmallAnswer) {
      this.tm.answer = "ok";
    }
  },
  apiSubmitAnswer: function () {
    var $this = this;
    utils.loading(this, true);

    var smallList = [];
    if (this.tm.baseTx === 'Zuheti' && this.tm.smallList !== null && this.tm.smallList.length > 0) {
      this.tm.smallList.forEach(small => {
        smallList.push({ id: small.id, answer: small.myAnswer, answerValues: small.optionsValues });
      });
    }

    $api.post($urlAnswer, { id: this.tm.id, answer: this.tm.answer, practiceId: this.id, answerValues: this.tm.optionsValues, smallList: smallList }).then(function (response) {
      var res = response.data;
      $this.answerResult = res;
      if ($this.answerResult.isRight) {
        $this.rightTotal++;
      }
      else {
        $this.wrongTotal++;
      }
      $this.answerTotal = $this.rightTotal + $this.wrongTotal;

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnSubmitAnswerClick: function () {
    if (this.tm.answer && this.tm.answer.length > 0) {
      this.apiSubmitAnswer();
    }
    else {
      utils.error("请答题", { layer: true });
    }
  },
  btnDownClick: function () {
    if (this.tmIndex === this.total - 1) {
      this.btnResultClick();
    }
    else {
      this.tm = null;
      this.answerResult = null;
      this.tmIndex++;
      this.apiGetTmInfo(this.tmIds[this.tmIndex]);
    }
  },
  btnCollectionClick: function () {
    this.apiCollection();
  },
  apiCollection: function () {
    var $this = this;
    utils.loading(this, true);

    $api.post($urlCollection, { id: this.tm.id }).then(function (response) {
      var res = response.data;
      if (res.value) {
        $this.tm.isCollection = true;
        utils.success("收藏成功", { layer: true })
      }
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnCollectionRemoveClick: function () {
    this.apiCollectionRemove();
  },
  apiCollectionRemove: function () {
    var $this = this;
    utils.loading(this, true);

    $api.post($urlCollectionRemove, { id: this.tm.id }).then(function (response) {
      var res = response.data;
      if (res.value) {
        $this.tm.isCollection = false;
        utils.success("已取消收藏", { layer: true })
      }
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnWrongRemoveClick: function(){
    this.apiWrongRemove();
  },
  apiWrongRemove: function () {
    var $this = this;
    utils.loading(this, true);

    $api.post($urlWrongRemove, { id: this.tm.id }).then(function (response) {
      var res = response.data;
      if (res.value) {
        $this.tm.isWrong = false;
        utils.success("已移出错题库", { layer: true })
      }
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnResultClick: function () {
    this.goResult();
  },
  goResult: function () {
    utils.loading(this, true,"正在统计练习...");
    location.href = utils.getExamUrl("examPracticeResult", { id: this.id });
  },
  btnCorrection: function () {
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examTmCorrection', { tmId: this.tm.id }),
      width: "100%",
      height: "100%"
    });
  }
};
var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  }
});
