var $url = '/study/studyPlan';
var $urlDelete = $url + '/del';
var $urlLock = $url + '/lock';
var $urlUnLock = $url + '/unLock';


var data = utils.init({
  form: {
    keyword: '',
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  list: null,
  total: 0
});

var methods = {
  apiGet: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($url, { params: $this.form }).then(function (response) {
      var res = response.data;

      $this.list = res.list;
      $this.total = res.total;

    }).catch(function (error) {
      utils.loading($this, false);
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  handleCurrentChange: function (val) {
    this.form.pageIndex = val;
    this.apiGet();
  },
  btnSearchClick: function () {
    this.form.pageIndex = 1;
    this.apiGet();
  },
  btnDeleteClick: function (id) {
    var $this = this;
    top.utils.alertDelete({
      title: '删除计划提醒',
      text: '确认删除计划吗？',
      callback: function () {
        $this.apiDelete(id);
      }
    });
  },
  apiDelete: function (id) {
    var $this = this;
    utils.loading(this, true);
    $api.post($urlDelete, { id: id }).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success("操作成功")
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.btnSearchClick();
    });
  },
  btnUnLockClick: function (id) {
    var $this = this;
    top.utils.alertWarning({
      title: '解锁计划',
      text: '确定解锁计划吗？',
      callback: function () {
        $this.apiUnLock(id);
      }
    });
  },
  btnLockClick: function (id) {
    var $this = this;
    top.utils.alertWarning({
      title: '关闭计划',
      text: '确定锁定计划吗？',
      callback: function () {
        $this.apiLock(id);
      }
    });
  },
  apiLock: function (id) {
    var $this = this;
    utils.loading(this, true);
    $api.post($urlLock, { id: id }).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success("操作成功")
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.btnSearchClick();
    });
  },
  apiUnLock: function (id) {
    var $this = this;
    utils.loading(this, true);
    $api.post($urlUnLock, { id: id }).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success("操作成功")
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.btnSearchClick();
    });
  },
  switchLocked(row) {
    var $this = this;
    utils.loading(this, true);
    $api.post($urlLock, { id: row.id, locked: row.isStop }).then(function (response) {
      var res = response.data;
      utils.success("操作成功");
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  btnEditClick: function (id) {
    var $this = this;

    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getStudyUrl('studyPlanEdit', { id: id }),
      width: "99%",
      height: "99%",
      end: function () {
        $this.btnSearchClick();
      }
    });

  },
  btnCopyClick: function (id) {
    var $this = this;

    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getStudyUrl('studyPlanEdit', { id: id, copyId: id }),
      width: "99%",
      height: "99%",
      end: function () {
        $this.btnSearchClick();
      }
    });
  },

  btnManagerAnalysisClick: function (row) {
    utils.openTopLeft(row.planName + '-综合统计', utils.getStudyUrl("studyPlanManagerAnalysis", { id: row.id }));
  },
  btnManagerUserClick: function (row) {
    utils.openTopLeft(row.planName + '-学习情况', utils.getStudyUrl("studyPlanManagerUser", { id: row.id }));
  },
  btnManagerScoreClick: function (row) {
    utils.openTopLeft(row.planName + '-考试成绩', utils.getExamUrl("examPaperManagerScore", { id: row.examId,planId:row.id }));
  },
  btnManagerCourseClick: function (id) {

    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getStudyUrl('studyPlanManagerCourse', { id: id }),
      width: "100%",
      height: "100%"
    });

  },
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  }
});
