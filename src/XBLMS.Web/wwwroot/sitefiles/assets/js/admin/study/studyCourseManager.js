var $url = '/study/studyCourseManager';

var $urlUser = $url + '/user';
var $urlUserExport = $urlUser + '/export';
var $urlUserSet = $urlUser + '/set';
var $urlUserOver = $urlUser + '/over';


var $urlScore = $url + '/score';
var $urlScoreExport = $urlScore + '/export';

var $urlEvaluation = $url + '/evaluation';
var $urlEvaluationExport = $urlEvaluation + '/export';

var $urlExamQ = $url + '/examq';
var $urlExamQExport = $urlExamQ + '/export';

var $urlEvaluation = $url + '/evaluation';
var $urlEvaluationExport = $urlEvaluation + '/export';


var data = utils.init({
  id: 0,
  planId:0,
  course: null,
  form: {
    id: 0,
    planId:0,
    state: '',
    keyWords: '',
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  userList: null,
  userTotal: 0,
  userSelection: [],
  tabPosition: 'left',

  formCourseWare: {
    id: 0,
    planId: 0,
    keyWords: '',
  },
  courseList: null,

  formScore: {
    id: 0,
    planId: 0,
    keyWords: '',
    dateFrom: '',
    dateTo: '',
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  scoreList: null,
  scoreTotal: 0,

  formEvaluation: {
    id: 0,
    planId: 0,
    keyWords: '',
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  evaluationList: null,
  evaluationTotal: 0,
  evaluationItems:null,

  qTmTotal: 0,
  qAnswerTotal: 0,
  qList:null,

  passSeries: [100],
  passChartOptions: {
    chart: {
      type: 'radialBar',
      toolbar: {
        show: false
      }
    },
    plotOptions: {
      radialBar: {
        startAngle: -135,
        endAngle: 225,
        hollow: {
          margin: 0,
          size: '70%',
          background: '#fff',
          image: undefined,
          imageOffsetX: 0,
          imageOffsetY: 0,
          position: 'front',
          dropShadow: {
            enabled: true,
            top: 3,
            left: 0,
            blur: 4,
            opacity: 0.24
          }
        },
        track: {
          background: '#fff',
          strokeWidth: '88%',
          margin: 0, // margin is in pixels
          dropShadow: {
            enabled: true,
            top: -3,
            left: 0,
            blur: 4,
            opacity: 0.35
          }
        },

        dataLabels: {
          show: true,
          name: {
            offsetY: -20,
            show: true,
            color: '#ff6a00',
            fontSize: '16px'
          },
          value: {
            formatter: function (val) {
              return val + '%';
            },
            color: '#000',
            fontSize: '36px',
            show: true,
          }
        }
      }
    },
    fill: {
      colors: ['#67C23A']
    },
    stroke: {
      lineCap: 'round'
    },
    labels: ['完成率'],
  },
  pieChartColors: ['#67c23a', '#1989fa', '#5cb87a', '#e6a23c'],
});

var methods = {
  apiGet: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($url, { params: { id: this.id,planId:this.planId } }).then(function (response) {
      var res = response.data;
      $this.course = res.item;

      $this.passSeries = [utils.formatPercentFloat($this.course.totalPassUser, $this.course.totalUser)];

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false, { layer: true });
    });
  },
  apiGetUser: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($urlUser, { params: $this.form }).then(function (response) {
      var res = response.data;
      $this.userList = res.list;
      $this.userTotal = res.total;

    }).catch(function (error) {
      utils.loading($this, false);
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  userHandleSelectionChange(val) {
    this.userSelection = val;
  },
  btnOfflineSetClick: function () {

    var $this = this;

    var msg = "如果没有选择部分学员，则操作所有学员。确定批量设置为已上课状态吗？";

    top.utils.alertWarning({
      title: '批量设置已上课',
      text: msg,
      callback: function () {
        $this.apiSetOffLine();
      }
    });
  },
  apiSetOffLine: function () {
    var userIds = [];
    if (this.userSelection && this.userSelection.length > 0) {
      this.userSelection.forEach(u => {
        userIds.push(u.id);
      });
    }

    var $this = this;
    utils.loading(this, true);
    $api.post($urlUserSet, { keyWords: this.form.keyWords, courseId: this.id, planId: this.planId, courseUserIds: userIds }).then(function (response) {
      var res = response.data;
      $this.userSelection = [];
      utils.success("操作成功", { layer: true })
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
      $this.apiGetUser();
    });
  },
  btnOfflineOverClick: function () {
    var $this = this;

    var msg = "如果没有选择部分学员，则操作所有学员。确定批量设置为已完成状态吗？";

    top.utils.alertWarning({
      title: '批量设置完成课程',
      text: msg,
      callback: function () {
        $this.apiOverOffLine();
      }
    });
  },
  apiOverOffLine: function () {
    var userIds = [];
    if (this.userSelection && this.userSelection.length > 0) {
      this.userSelection.forEach(u => {
        userIds.push(u.id);
      });
    }

    var $this = this;
    utils.loading(this, true);
    $api.post($urlUserOver, { keyWords: this.form.keyWords, courseId: this.id, planId: this.planId, courseUserIds: userIds }).then(function (response) {
      var res = response.data;
      $this.userSelection = [];
      utils.success("操作成功", { layer: true })
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
      $this.apiGetUser();
    });
  },
  userHandleCurrentChange: function (val) {
    this.form.pageIndex = val;
    this.apiGetUser();
  },
  btnUserSearchClick: function () {
    this.form.pageIndex = 1;
    this.apiGetUser();
  },
  btnViewClick: function (id) {
    utils.openUserView(id);
  },
  btnAdminViewClick: function (id) {
    utils.openAdminView(id);
  },
  btnUserExportClick: function () {
    var $this = this;

    utils.loading(this, true);
    $api.post($urlUserExport, this.form).then(function (response) {
      var res = response.data;

      window.open(res.value);
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },


  apiGetCourse: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($urlCourse, { params: $this.formCourse }).then(function (response) {
      var res = response.data;
      $this.courseList = res.list;

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnCourseSearchClick: function () {
    this.apiGetCourse();
  },
  btnCourseExportClick: function () {
    var $this = this;

    utils.loading(this, true);
    $api.post($urlCourseExport, this.formCourse).then(function (response) {
      var res = response.data;

      window.open(res.value);
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  apiGetScore: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($urlScore, { params: $this.formScore }).then(function (response) {
      var res = response.data;
      $this.scoreList = res.list;
      $this.scoreTotal = res.total;

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnScoreSearchClick: function () {
    this.formScore.pageIndex = 1;
    this.apiGetScore();
  },
  btnScoreExportClick: function () {
    var $this = this;

    utils.loading(this, true);
    $api.post($urlScoreExport, this.formScore).then(function (response) {
      var res = response.data;

      window.open(res.value);
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  scoreHandleCurrentChange: function (val) {
    this.formScore.pageIndex = val;
    this.apiGetScore();
  },
  apiGetQ: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($urlExamQ, { params: { id: this.id, planId: this.planId } }).then(function (response) {
      var res = response.data;
      $this.qTmTotal = res.qTmTotal;
      $this.qAnswerTotal = res.qAnswerTotal;
      $this.qList = res.qList;

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false, { layer: true });
    });
  },
  apiGetEvaluation: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($urlEvaluation, { params: $this.formEvaluation }).then(function (response) {
      var res = response.data;
      $this.evaluationList = res.list;
      $this.evaluationTotal = res.total;
      $this.evaluationItems = res.items;

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnEvaluationSearchClick: function () {
    this.formEvaluation.pageIndex = 1;
    this.apiGetEvaluation();
  },
  btnEvaluationExportClick: function () {
    var $this = this;

    utils.loading(this, true);
    $api.post($urlEvaluationExport, this.formScore).then(function (response) {
      var res = response.data;

      window.open(res.value);
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  evaluationHandleCurrentChange: function (val) {
    this.formEvaluation.pageIndex = val;
    this.apiGetEvaluation();
  },
  btnPaperSocreView: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('examPaperUserLayerView', { id: id }),
      width: "99%",
      height: "99%"
    });
  },
  btnCerViewClick: function () {
    var $this = this;
    top.utils.openLayer({
      title: $this.course.name + '-获证人员列表',
      url: utils.getExamUrl('examCerUsers', { id: $this.course.cerId, planId: $this.planId,courseId:$this.id }),
      width: "88%",
      height: "98%"
    });
  }
};
Vue.component("apexchart", {
  extends: VueApexCharts
});
var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.id = this.form.id = this.formCourseWare.id = this.formScore.id = this.formEvaluation.id = utils.getQueryInt("id");
    this.planId = this.form.planId = this.formCourseWare.planId = this.formScore.planId = this.formEvaluation.planId = utils.getQueryInt("planId");

    this.apiGet();
    this.apiGetUser();
    this.apiGetScore();
    this.apiGetQ();
    this.apiGetEvaluation();
  }
});
