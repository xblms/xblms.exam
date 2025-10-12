var $url = "/exam/examPaperInfo";
var $urlCheck = $url + "/check";
var $urlClientExam = $url + "/clientExam";
var $urlClientExamStatus = $url + "/clientExamStatus";

var data = utils.init({
  id: utils.getQueryInt("id"),
  planId: utils.getQueryInt("planId"),
  courseId: utils.getQueryInt("courseId"),
  cjList: [],
  item: null,
  startLoading: false,
  isStart: true,
  isClientRead: false,
  isCientIn: utils.getQueryBoolean("isCientIn")
});

var methods = {
  apiGet: function () {
    var $this = this;

    if (this.total === 0) {
      utils.loading(this, true);
    }

    $api.get($url, { params: { id: $this.id, planId: $this.planId, courseId: $this.courseId } }).then(function (response) {
      var res = response.data;

      $this.item = res.item;
      top.utils.pointNotice(res.pointNotice);

      if ($this.item.examStartDateTimeLong > 0) {
        $this.isStart = false;
      }

      if ($this.item.examSubmiting) {
        setTimeout(function () {
          $this.apiGet();
        }, 3000)
      }

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  apiCheck: function () {
    var $this = this;
    utils.loading(this, true, '正在检测开考数据，请稍等...');
    $api.get($urlCheck, { params: { id: $this.id } }).then(function (response) {
      var res = response.data;
      if (res.success) {
        $this.goExaming();
      }
      else {
        utils.error(res.msg, { layer: true });
      }

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnStartExamClick: function () {
    this.apiCheck();
  },
  goExaming: function () {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examPaperExaming', { id: this.id, planId: this.planId, courseId: this.courseId }),
      width: "100%",
      height: "100%",
      end: function () {
        $this.apiGet();
      }
    });
  },
  btnOkClient: function () {
    this.isClientRead = true;
  },
  btnCheckClient: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($urlClientExamStatus).then(function (response) {
      var res = response.data;
      if (res.success) {
        $this.isClientRead = true;
        utils.success("已成功安装客户端程序，点击启动客户端进入考试", { layer: true });
      }
      else {
        utils.error(res.msg, { layer: true });
      }
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnStartClient: function () {
    var $this = this;
    utils.loading(this, true, '正在启动客户端考试工具，请稍等...');
    $api.post($urlClientExam, { token: $token, id: this.id }).then(function (response) {
      var res = response.data;
      location.href = 'KaoshiduanExamClient://' + res.value;
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnViewClick: function (id) {
    utils.closeLayerSelf();
    top.utils.openTopLeft(this.item.title, utils.getExamUrl("examPaperView", { id: id }));
  },
  timingFinish: function () {
    utils.success("倒计时结束，可以进入考试", { layer: true });
    this.isStart = true;
  },
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  },
});
