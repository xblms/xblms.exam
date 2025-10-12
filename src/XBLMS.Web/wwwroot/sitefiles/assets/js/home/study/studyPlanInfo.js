var $url = "/study/studyPlanInfo";
var $urlItem = $url + "/item";

var data = utils.init({
  id: utils.getQueryInt("id"),
  studyPlan:null
});

var methods = {
  apiGet: function () {
    var $this = this;

    $api.get($url, { params: {id:this.id} }).then(function (response) {
      var res = response.data;

      $this.studyPlan = res.item;
      top.utils.pointNotice(res.pointNotice);

    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnViewCourseClick: function (row) {

    var curl = utils.getStudyUrl('studyCourseInfo', { id: row.id, planId: this.studyPlan.plan.id });
    if (row.offLine) {
      curl = utils.getStudyUrl('studyCourseOfflineInfo', { id: row.id, planId: this.studyPlan.plan.id });
    }

    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: curl,
      width: "100%",
      height: "100%",
      end: function () {
        $this.apiGet();
      }
    });
  },
  btnViewExamClick: function () {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examPaperInfo', { id: this.studyPlan.plan.examId, planId: this.studyPlan.plan.id, courseId: 0 }),
      width: "78%",
      height: "98%",
      end: function () {
        $this.apiGet();
      }
    });
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
