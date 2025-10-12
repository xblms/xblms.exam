var $url = "/doTask";


var data = utils.init({
  total: 0,
  list: null
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url).then(function (response) {
      var res = response.data;

      $this.total = res.total;
      $this.list = res.list;

      if ($this.total === 0) {
        location.href = utils.getRootUrl("empty");
      }

    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnDoClick: function (task) {
    if (task.taskType === 'Exam') {
      this.btnViewPaperClick(task.taskId);
    }
    else if(task.taskType === 'ExamQ') {
      this.btnViewQClick(task.taskId);
    }
    else if(task.taskType === 'ExamAss') {
      this.btnViewAssClick(task.taskId);
    }
    else if (task.taskType === 'StudyPlan') {
      this.btnViewPlanClick(task.taskId);
    }
  },
  btnViewAssClick: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examAssessmenting', { id: id }),
      width: "68%",
      height: "100%",
      end: function () {
        $this.apiGet();
      }
    });
  },
  btnViewQClick: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examQuestionnairing', { id: id }),
      width: "68%",
      height: "100%",
      end: function () {
        $this.apiGet();
      }
    });
  },
  btnViewPaperClick: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examPaperInfo', { id: id }),
      width: "78%",
      height: "98%",
      end: function () {
        $this.apiGet();
      }
    });
  },
  btnViewPlanClick: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getStudyUrl('studyPlanInfo', { id: id }),
      width: "100%",
      height: "100%",
      end: function () {
        $this.apiGet(id);
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
