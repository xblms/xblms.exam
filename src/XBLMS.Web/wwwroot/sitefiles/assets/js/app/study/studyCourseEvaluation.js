var $url = "/study/studyCourseEvaluation";

var data = utils.init({
  title: null,
  courseId: utils.getQueryInt("courseId"),
  planId: utils.getQueryInt("planId"),
  eId: utils.getQueryInt("eId"),
  list: null,
  maxStar:0
});

var methods = {
  apiGet: function () {
    var $this = this;

    $api.get($url, { params: { courseId: this.courseId, planId: this.planId, eId: this.eId } }).then(function (response) {
      var res = response.data;

      $this.title = res.title;
      $this.list = res.list;
      $this.maxStar = res.maxStar;

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  apiSubmit: function (id) {
    var $this = this;
    utils.loading(this, true);
    $api.post($url, { list: this.list, courseId: this.courseId, planId: this.planId, eId: this.eId }).then(function (response) {
      utils.success('操作成功！', { layer: true });

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
      utils.closeLayerSelf();
    });
  },
  btnSubmitClick: function () {
    var allStar = true;
    this.list.forEach(item => {
      if (!item.textContent) {
        if (item.starValue === 0) {
          allStar = false;
        }
      }
    });
    if (allStar) {
      this.apiSubmit();
    }
    else {
      utils.error("请完成所有带星评价", { layer: true });
    }

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
