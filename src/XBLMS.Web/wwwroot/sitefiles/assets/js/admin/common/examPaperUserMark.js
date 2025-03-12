var $url = "/common/examPaperUserMark";
var $urlSve = $url + "/save";
var $urlSubmit = $url + "/submit";

var data = utils.init({
  id: utils.getQueryInt('id'),
  list: null,
  paper: null,
  watermark: null,
  answerTotal: 0,
  tmAnswerStatus: false,
  tmList: [],
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true, "正在加载答卷...");

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

        $this.markChange();
      }

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnGetTm(id) {
    this.$refs['answerScrollbar'].wrap.scrollTop = 0;
    var eid = '#tm_' + id;
    var scrollTop = ($(eid).offset().top) - 128;
    this.$refs['answerScrollbar'].wrap.scrollTop = scrollTop;
  },
  markChange: function () {
    var totalSScore = 0;
    this.tmList.forEach(tm => {
      totalSScore += tm.answerInfo.score;
    });
    this.paper.userSScore = totalSScore;
  },
  btnSaveClick: function () {
    var answerList = [];
    this.tmList.forEach(tm => {
      var answer = tm.answerInfo;
      answer.markState = tm.markState;
      answerList.push(answer);
    });

    var markForm = {
      startId: this.id,
      list: answerList
    };


    var $this = this;

    utils.loading(this, true, '正在保存阅卷...');
    $api.post($urlSve, markForm).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success("保存成功", { layer: true })
      }
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnSubmitClick: function () {
    var $this = this;

    var isAllMark = 0;

    this.tmList.forEach(tm => {
      if (!tm.markState) {
        isAllMark++;
      }
    });

    var msg = '确定提交阅卷吗？';
    if (isAllMark > 0) {
      msg = '还剩' + isAllMark + '道题没有判分，' + msg;
    }

    top.utils.alertWarning({
      title: '正在提交阅卷',
      text: msg,
      callback: function () {
        $this.apiSubmitMark();
      }
    });
  },
  apiSubmitMark: function () {
    var $this = this;

    var answerList = [];
    this.tmList.forEach(tm => {
      var answer = tm.answerInfo;
      answer.markState = tm.markState;
      answerList.push(answer);
    });

    var markForm = {
      startId: this.id,
      list: answerList
    };


    utils.loading(this, true, '正在提交阅卷...');
    $api.post($urlSubmit, markForm).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success("提交成功", { layer: true });
        utils.closeLayerSelf();
      }
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
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
