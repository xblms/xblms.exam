var $url = "/study/studyCourseOfflineInfo";

var $urlCollection = $url + "/collection";
var $urlEvaluation = $url + "/evaluation";

var data = utils.init({
  id: utils.getQueryInt("id"),
  planId: utils.getQueryInt("planId"),
  courseInfo: null,
  courseUser: null,
  ePageIndex: 1,
  ePageSize: 10,
  eTotal: 0,
  eList: [],
  eLoadMoreLoading: false,
  eAvg: '',
  eUser: 0,
  eStarList: null,
  bgImgUrl:'/sitefiles/assets/images/cover/bg.png'
});

var methods = {
  apiGet: function () {
    var $this = this;
    $api.get($url, { params: { planId: this.planId, courseId: this.id } }).then(function (response) {
      var res = response.data;

      $this.courseInfo = res.courseInfo;
      $this.courseUser = res.courseInfo.courseUserInfo;

      top.utils.pointNotice(res.pointNotice);

    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnCollectionClick: function () {
    var $this = this;
    utils.loading(this, true);
    $api.post($urlCollection, { id: this.courseUser.id }).then(function (response) {
      var res = response.data;
      utils.success($this.courseUser.collection ? "已取消收藏" : "已收藏", { layer: true });
      $this.courseUser.collection = !$this.courseUser.collection;
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnViewExamQClick: function () {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examQuestionnairing', { id: this.courseInfo.examQuestionnaireId, planId: this.planId, courseId: this.id }),
      width: "68%",
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
      url: utils.getExamUrl('examPaperInfo', { id: this.courseInfo.examId, planId: this.planId, courseId: this.id }),
      width: "78%",
      height: "98%",
      end: function () {
        $this.apiGet();
      }
    });
  },
  btnViewEvaluationClick: function () {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getStudyUrl('studyCourseEvaluation', { courseId: this.id, planId: this.planId, eId: this.courseInfo.studyCourseEvaluationId }),
      width: "58%",
      height: "98%",
      end: function () {
        $this.apiGet();
        $this.ePageIndex = 1;
        $this.apiGetEvaluation();
      }
    });
  },
  apiGetEvaluation: function () {
    var $this = this;
    $api.get($urlEvaluation, { params: { planId: this.planId, courseId: this.id, pageIndex: this.ePageIndex, pageSize: this.ePageSize } }).then(function (response) {
      var res = response.data;
      if ($this.ePageIndex <= 1) {
        $this.eAvg = res.starAvg;
        $this.eUser = res.starUser;
        $this.eStarList = res.starList;
      }
      if (res.list && res.list.length > 0) {
        res.list.forEach(item => {
          $this.eList.push(item);
        });
      }
      $this.eTotal = res.total;

    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      $this.eLoadMoreLoading = false;
    });
  },
  btnLoadMoreEvaluationClick: function () {
    this.eLoadMoreLoading = true;
    this.ePageIndex++;
    this.apiGetEvaluation();
  },
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
    this.apiGetEvaluation();
  },
});
