var $url = 'exam/examPaperEdit';
var $urlGetConfig = $url + '/getConfig';
var $urlGetTmList = $url + '/getTms';

var data = utils.init({
  id: utils.getQueryInt('id'),
  treeId: utils.getQueryInt('treeId'),
  isCourseUse: utils.getQueryBoolean('isCourseUse'),
  isSelect: false,
  item: null,
  userGroupList: null,
  tmGroupList: null,
  tmAllGroupList: null,
  tmFixedGroupList: null,
  paperTree: null,
  cerList: null,
  txList: null,
  tmRandomConfig: [],
  selectTms: null,
  form: null,
  txTotalScore: 0,
  tmTotal: 0,
  tmTotalCount: 0,
  userTotal: 0,
  submitDialogVisible: false,
  submitSubmitType: 'Save',
  submitSubmitIsClear: false,
  tmConfigDialogVisible: false,
  isUpdateDateTime: false,
  isUpdateExamTimes: false,
  systemCode: null,
  active: 0,
  formBase: { title: null, subject: null, tmRandomType: "RandomNow", randomCount: 3, examBeginDateTime: null, examEndDateTime: null },
  formUser: { userGroupIds: null },
  tmGroupSelectedList: [],
  userGroupSelectedList: [],
  winHeight: $(window).height(),
  drawerHightConfig: false,
  publishSuccess: false,
  tmList: null
});

var methods = {
  apiGet: function () {
    var $this = this;
    this.isSelect = this.isCourseUse;
    utils.loading(this, true);
    $api.get($url, { params: { id: this.id } }).then(function (response) {
      var res = response.data;
      $this.tmAllGroupList = res.tmGroupList;
      $this.tmFixedGroupList = res.tmFixedGroupList;
      $this.txList = res.txList;
      $this.userGroupList = res.userGroupList;
      $this.paperTree = res.paperTree;
      $this.cerList = res.cerList;
      $this.systemCode = res.systemCode;
      $this.tmGroupList = $this.tmAllGroupList;
      $this.form = _.assign({}, res.item);
      $this.formBase.title = $this.form.title;
      $this.formBase.examBeginDateTime = $this.form.examBeginDateTime;
      $this.formBase.examEndDateTime = $this.form.examEndDateTime;

      $this.form.treeId = $this.treeId;
      $this.form.examTimes = 1;
      $this.form.isCourseUse = $this.isCourseUse;
      $this.form.cerId = null;
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  apiGetConfig: function () {
    var $this = this;
    utils.loading(this, true, "正在加载题目配置数据");
    $api.post($urlGetConfig, { txIds: $this.form.txIds, tmGroupIds: $this.form.tmGroupIds }).then(function (response) {
      var res = response.data;
      $this.tmRandomConfig = res.items;
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
      $this.$nextTick(() => {
        $this.sumTmTotal();
      })
    });
  },
  sumTmTotal: function () {
    this.tmTotal = 0;
    this.tmTotalCount = 0;
    this.tmRandomConfig.forEach(item => {
      this.tmTotal += (item.nandu1TmCount + item.nandu2TmCount + item.nandu3TmCount + item.nandu4TmCount + item.nandu5TmCount);
      this.tmTotalCount += (item.nandu1TmTotal + item.nandu2TmTotal + item.nandu3TmTotal + item.nandu4TmTotal + item.nandu5TmTotal);
    })
  },
  tmRandomTypeChange: function () {
    this.userTotal = 0;
    this.tmTotal = 0;
    this.tmTotalCount = 0;
    this.form.totalScore = 100;
    this.form.passScore = 60;
    this.form.tmScoreType = "ScoreTypeRate";
    this.form.txIds = [];
    this.form.tmGroupIds = [];
    this.form.userGroupIds = [];
    this.tmGroupSelectedList = [];
    this.userGroupSelectedList = [];
    this.tmGroupList = [];
    this.tmRandomConfig = [];
    this.tmList = [];
    if (this.formBase.tmRandomType === 'RandomNone') {
      this.form.tmScoreType = "ScoreTypeTm";
    }
  },
  btnOpenEditClick: function (ref, ptype) {
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('editorOpenLayer', { pf: window.name, ptype: ptype, ref: ref }),
      width: "58%",
      height: "78%"
    });
  },
  btnSaveClick: function () {
    this.submitSubmitType = 'Save';
    this.submitSubmitIsClear = false;
    var $this = this;
    this.$refs.form.validate(function (valid) {
      if (valid) {
        var valido = $this.submitValid();
        if (!valido) return;
        $this.apiSubmit();
      }
    });

  },
  btnSubmitClick: function () {
    this.submitSubmitType = 'Submit';
    this.submitSubmitIsClear = false;
    var valido = this.submitValid();
    if (!valido) return;
    this.apiSubmit();
  },
  btnSubmit: function () {
    this.submitSubmitIsClear = false;
    this.apiSubmit();
  },
  btnSubmitClear: function () {
    var $this = this;
    top.utils.alertWarning({
      title: '重新发布提醒',
      text: '确定清空后发布吗？',
      confirmButtonText: '重新发布',
      showCancelButton: true,
      callback: function () {
        $this.submitSubmitIsClear = true;
        $this.apiSubmit();
      }
    });
  },
  apiSubmit: function () {
    var $this = this;
    utils.loading(this, true, "正在发布试卷，请稍等...");
    $api.post($url, {
      isClear: this.submitSubmitIsClear,
      submitType: this.submitSubmitType,
      item: $this.form,
      configList: $this.tmRandomConfig,
      isUpdateDateTime: $this.isUpdateDateTime,
      isUpdateExamTimes: $this.isUpdateExamTimes
    }).then(function (response) {
      var res = response.data;
      if (res.value) {
        $this.publishSuccess = true;
      }
      else {
        utils.error("发布失败请重新尝试", { layer: true });
      }
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  submitValid: function () {
    if (this.form.examBeginDateTime >= this.form.examEndDateTime) {
      utils.error('请选择有效的考试时间', { layer: true });
      return false;
    }
    if (this.form.tmRandomType === 'RandomNone') {
      if (this.form.tmGroupIds === null || this.form.tmGroupIds.length === 0) {
        utils.error('请选择至少一个题目组', { layer: true });
        return false;
      }
    }
    else {
      if (this.tmTotal === 0 && this.submitSubmitType === 'Submit') {
        utils.error('没有配置任何题目', { layer: true });
        return false;
      }
    }
    return true;
  },
  scoreTypeChange: function (value) {
    if (value === 'ScoreTypeTx' && this.form.tmRandomType !== 'RandomNone') {
      this.form.totalScore = this.txTotalScore;
    }
    else {
      this.form.totalScore = 100;
    }
  },
  btnPreviousClick: function () {
    if (this.active > 0) this.active--;
  },
  btnFormBaseNextClick: function () {
    var $this = this;
    this.$refs.formBase.validate(function (valid) {
      if (valid) {
        $this.form.title = $this.formBase.title;
        $this.form.examBeginDateTime = $this.formBase.examBeginDateTime;
        $this.form.examEndDateTime = $this.formBase.examEndDateTime;
        $this.form.tmRandomType = $this.formBase.tmRandomType;
        $this.form.randomCount = $this.formBase.randomCount;
        $this.active++;
      }
    });
  },
  btnTmConfigNextClick: function () {
    if (this.tmTotal > 0) {
      this.active++;
    }
    else {
      utils.error("请配置题目", { layer: true });
    }
  },
  btnFormUserNextClick: function () {
    if (this.userTotal > 0) {
      this.form.userGroupIds = this.formUser.userGroupIds;
      if (this.form.passScore > this.form.totalScore) {
        this.form.passScore = this.form.totalScore;
      }
      this.active++;
    }
    else {
      utils.error("请选择用户组", { layer: true });
    }
  },
  btnFormDateNextClick: function () {
    var $this = this;
    this.$refs.formDate.validate(function (valid) {
      if (valid) {
        $this.active++;
      }
    });
  },
  btnCloseClick: function () {
    top.utils.alertWarning({
      title: '提醒',
      text: '中途退出不会保存数据，确定退出发布吗？',
      confirmButtonText: '确定',
      showCancelButton: true,
      callback: function () {
        utils.closeLayerSelf();
      }
    });
  },
  tmGroupSelectedClose: function (id) {
    this.tmGroupSelectedList = this.tmGroupSelectedList.filter(f => f.id !== id);
    this.apiGetConfig();
  },
  apiGetTmList: function () {
    var $this = this;
    utils.loading(this, true, "正在加载题目");
    $api.post($urlGetTmList, { txIds: $this.form.txIds, tmGroupIds: $this.form.tmGroupIds }).then(function (response) {
      var res = response.data;
      $this.tmList = res.items;
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
      $this.$nextTick(() => {
        $this.sumTmTotalByTmList();
      })
    });
  },
  sumTmTotalByTmList: function () {
    this.form.totalScore = 0;
    this.tmTotal = 0;
    this.tmTotalCount = 0;
    this.tmList.forEach(item => {
      this.tmTotal++;
      this.tmTotalCount++;
      this.form.totalScore = this.form.totalScore + item.score;
    })
    if (this.form.totalScore > 0) {
      this.form.totalScore = this.form.totalScore.toFixed(0);
    }
  },
  btnTmViewClick: function (val) {
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('examTmLayerView', { id: val.id }),
      width: "58%",
      height: "88%"
    });
  },
  btnTmGroupSelectClick: function () {
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examTmGroupSelect', { pf: window.name, isGuding: this.form.tmRandomType === 'RandomNone' }),
      width: "88%",
      height: "88%"
    });
  },
  btnTmGroupSelectCallback: function (groups) {
    this.tmGroupSelectedList = groups;
    this.getTmGroupIds();
    if (this.form.tmRandomType === 'RandomNone') {
      this.apiGetTmList();
    }
    else {
      this.apiGetConfig();
    }
  },
  getTmGroupIds: function () {
    var ids = _.map(this.tmGroupSelectedList, function (item) {
      return item.id;
    });
    this.form.tmGroupIds = ids;
  },
  btnUserGroupSelectClick: function () {
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getSettingsUrl('usersGroupSelect', { pf: window.name }),
      width: "88%",
      height: "88%"
    });
  },
  btnUserGroupSelectCallback: function (groups) {
    this.userGroupSelectedList = groups;
    this.getUserTotal();
    this.getUserGroupIds();
  },
  getUserTotal: function () {
    this.userTotal = 0;
    if (this.userGroupSelectedList && this.userGroupSelectedList.length > 0) {
      this.userGroupSelectedList.forEach(userGroup => {
        this.userTotal += userGroup.userTotal;
      });
    }
  },
  getUserGroupIds: function () {
    var ids = _.map(this.userGroupSelectedList, function (item) {
      return item.id;
    });
    this.formUser.userGroupIds = this.form.userGroupIds = ids;
  },
  btnFormPublishClick: function () {
    var $this = this;
    top.utils.alertWarning({
      title: '提醒',
      text: '确定发布考试吗？',
      confirmButtonText: '确定',
      showCancelButton: true,
      callback: function () {
        $this.btnSubmitClick();
      }
    });
  }
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  }
});
