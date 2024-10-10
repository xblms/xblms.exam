var $url = "/exam/examPractice";
var $urlSubmit = $url + "/submit";

var data = utils.init({
  list: [],
  total: 0,
  collectTotal: 0,
  wrongTotal:0
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);

    $api.get($url).then(function (response) {
      var res = response.data;

      $this.list = res.list;
      $this.total = res.total;
      $this.collectTotal = res.collectTotal;
      $this.wrongTotal = res.wrongTotal;

    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnCreateClick: function (practiceType, groupId,groupTmTotal) {
    if (practiceType === 'All') {
      if (this.total > 0) {
        this.apiCreatePractice(practiceType, groupId);
      }
      else {
        utils.error("没有题目可以练习");
      }
    }
    if (practiceType === 'Collect') {
      if (this.collectTotal > 0) {
        this.apiCreatePractice(practiceType, groupId);
      }
      else {
        utils.error("没有题目可以练习");
      }
    }
    if (practiceType === 'Wrong') {
      if (this.wrongTotal > 0) {
        this.apiCreatePractice(practiceType, groupId);
      }
      else {
        utils.error("没有题目可以练习");
      }
    }
    if (practiceType === 'Group') {
      if (groupTmTotal > 0) {
        this.apiCreatePractice(practiceType, groupId);
      }
      else {
        utils.error("没有题目可以练习");
      }
    }
  },
  apiCreatePractice: function (practiceType, groupId) {
    var $this = this;

    utils.loading(this, true, "正在创建练习...");

    $api.post($urlSubmit, { practiceType: practiceType, groupId: groupId }).then(function (response) {
      var res = response.data;

      if (res.success) {
        $this.goPractice(res.id);
      }
      else {
        utils.error(res.error);
      }

    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  goPractice: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examPracticing', { id: id }),
      width: "100%",
      height: "100%",
    });
  },
  btnLogClick: function () {
    location.href = utils.getExamUrl('examPracticeLog');
  }
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    document.title = "练习中心";
    this.apiGet();
  },
});
