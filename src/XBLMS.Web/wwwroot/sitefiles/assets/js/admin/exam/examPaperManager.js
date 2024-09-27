var $url = '/exam/examPaperManager';

var $urlUser = $url + '/user';
var $urlUserDeleteOne = $urlUser + '/removeone';
var $urlUserDelete = $urlUser + '/remove';
var $urlUserExamTimes = $urlUser + '/examtimes';
var $urlUserDateTime = $urlUser + '/datetime';
var $urlUserExport = $urlUser + '/export';

var $urlScore = $url + '/score';
var $urlScoreExport = $urlScore + '/export';

var $urlMark = $url + '/mark';

var data = utils.init({
  id: 0,
  title: '',
  form: {
    id: 0,
    keywords: '',
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  userList: null,
  userTotal: 0,
  userSelection: [],
  userUpdateDateTimeDialogVisible: false,
  userUpdateDateTimeForm: {
    examBeginDateTime: '',
    examEndDateTime: ''
  },
  tabPosition: 'left',

  formScore: {
    id: 0,
    keywords: '',
    dateFrom: '',
    dateTo: '',
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  scoreList: null,
  scoreTotal: 0,
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
    labels: ['及格'],
  },
  nopassSeries: [0],
  nopassChartOptions: {
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
      colors: ['#F56C6C']
    },
    stroke: {
      lineCap: 'round'
    },
    labels: ['不及格'],
  },
  totalScore: 0,
  passScore: 0,
  totalUser: 0,
  maxScore: 0,
  minScore: 0,
  totalPass: 0,
  totalPassDistinct: 0,
  totalUserScore: 0,
  totalExamTimes: 0,
  totalExamTimesDistinct: 0,

  formMark: {
    id: 0,
    keywords: '',
    dateFrom: '',
    dateTo: '',
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  markList: null,
  markTotal: 0,
});

var methods = {
  apiGet: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($url, { params: { id: this.id } }).then(function (response) {
      var res = response.data;
      $this.title = res.title;
      $this.totalScore = res.totalScore;
      $this.passScore = res.passScore;
      $this.totalUser = res.totalUser;
      $this.maxScore = res.maxScore;
      $this.minScore = res.minScore;
      $this.totalPass = res.totalPass;
      $this.totalPassDistinct = res.totalPassDistinct;
      $this.totalUserScore = res.totalUserScore;
      $this.totalExamTimes = res.totalExamTimes;
      $this.totalExamTimesDistinct = res.totalExamTimesDistinct;

      setTimeout(function () {
        if ($this.totalExamTimes > 0) {
          var passS = utils.formatPercentFloat($this.totalPass, $this.totalExamTimes);
          $this.passSeries = [passS];
          $this.nopassSeries = [100 - passS];
        }
        else {
          $this.passSeries = [0];
          $this.nopassSeries = [0];
        }
      }, 1000);

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
  userClearSelection: function () {
    this.userSelection = [];
    this.userUpdateDateTimeDialogVisible = false;
  },
  btnUserUpdateExamTimes: function (inc) {
    var increment = inc == 1 ? true : false;
    var userIds = [];
    if (this.userSelection && this.userSelection.length > 0) {
      this.userSelection.forEach(u => {
        userIds.push(u.id);
      });
      var $this = this;

      var msg = increment ? "选中的考生全部增加1次考试机会，确定吗？" : "选中的考生全部减少1次考试机会，确定吗？";

      top.utils.alertWarning({
        title: '修改考试次数',
        text: msg,
        callback: function () {
          $this.apiUserUpdateExamTimes(increment, userIds);
        }
      });
    }
    else {
      utils.error("请选择考生", { layer: true });
    }
  },
  apiUserUpdateExamTimes: function (increment, ids) {
    var $this = this;
    utils.loading(this, true);
    $api.post($urlUserExamTimes, { increment: increment, ids: ids }).then(function (response) {
      var res = response.data;
      $this.userClearSelection();
      utils.success("操作成功", { layer: true })
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
      $this.apiGetUser();
    });
  },
  btnUserRemove: function () {
    var userIds = [];
    if (this.userSelection && this.userSelection.length > 0) {
      this.userSelection.forEach(u => {
        userIds.push(u.id);
      });
      var $this = this;
      top.utils.alertDelete({
        title: '移出考生',
        text: '将清空这些考生的考试数据，确定移出吗？',
        button: '确 定',
        callback: function () {
          $this.apiUserRemove(userIds);
        }
      });
    }
    else {
      utils.error("请选择考生", { layer: true });
    }
  },
  apiUserRemove: function (ids) {
    var $this = this;
    utils.loading(this, true);
    $api.post($urlUserDelete, { ids: ids }).then(function (response) {
      var res = response.data;
      $this.userClearSelection();
      utils.success("操作成功", { layer: true })
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
      $this.btnUserSearchClick();
    });
  },
  btnUserDelete: function (row) {
    var $this = this;
    top.utils.alertDelete({
      title: '移出考生-' + row.user.displayName,
      text: '将清空该考生的考试数据，确认移出吗？',
      button: '确 定',
      callback: function () {
        $this.apiUserDelete(row.id);
      }
    });
  },
  apiUserDelete: function (id) {
    var $this = this;
    utils.loading(this, true);
    $api.post($urlUserDeleteOne, { id: id }).then(function (response) {
      var res = response.data;
      $this.userClearSelection();
      utils.success("操作成功", { layer: true })
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
      $this.btnUserSearchClick();
    });
  },
  btnUserArrange: function () {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('usersRange', { id: this.form.id, rangeType: 'Exam' }),
      width: "88%",
      height: "88%",
      end: function () {
        $this.btnUserSearchClick();
      }
    });
  },
  btnUserUpdateDatetime: function () {
    if (this.userSelection && this.userSelection.length > 0) {
      this.userUpdateDateTimeDialogVisible = true;
    }
    else {
      utils.error("请选择考生", { layer: true });
    }
  },
  btnUserUpdateDatetimeSubmit: function () {
    var $this = this;
    this.$refs.userUpdateDateTimeForm.validate(function (valid) {
      if (valid) {

        $this.apiUserUpdateDateTime();
      }
    });
  },
  apiUserUpdateDateTime: function () {
    var $this = this;
    utils.loading(this, true);

    var userIds = [];
    this.userSelection.forEach(u => {
      userIds.push(u.id);
    });

    $api.post($urlUserDateTime, { ids: userIds, examBeginDateTime: this.userUpdateDateTimeForm.examBeginDateTime, examEndDateTime: this.userUpdateDateTimeForm.examEndDateTime }).then(function (response) {
      var res = response.data;
      $this.userClearSelection();
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

  apiGetMark: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($urlMark, { params: $this.formMark }).then(function (response) {
      var res = response.data;
      $this.markList = res.list;
      $this.markTotal = res.total;

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnMarkSearchClick: function () {
    this.formMark.pageIndex = 1;
    this.apiGetMark();
  },
  markHandleCurrentChange: function (val) {
    this.formMark.pageIndex = val;
    this.apiGetMark();
  },
};
Vue.component("apexchart", {
  extends: VueApexCharts
});
var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.id = this.form.id = this.formScore.id = this.formMark.id = utils.getQueryInt("id");
    this.apiGet();
    this.apiGetUser();
    this.apiGetScore();
    this.apiGetMark();
  }
});
