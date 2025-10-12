var $url = '/common/userDocLayerView';

var data = utils.init({
  id: utils.getQueryInt('id'),
  avatarUrl: null,
  displayName: null,
  systemCode: null,
  curTab: "index",
  userFrameUrl: utils.getCommonUrl('userDocIndex', { id: utils.getQueryInt('id') }),
  userTabList: [
    { name: "综合统计", value: "index", url: utils.getCommonUrl('userDocIndex', { id: utils.getQueryInt('id') }) },
    { name: "学习任务", value: "studyPlan", url: utils.getCommonUrl('userDocStudyPlan', { id: utils.getQueryInt('id') }) },
    { name: "课程学习", value: "studyCourse", url: utils.getCommonUrl('userDocStudyCourse', { id: utils.getQueryInt('id') }) },
    { name: "参加考试", value: "exam", url: utils.getCommonUrl('userDocExam', { id: utils.getQueryInt('id') }) },
    { name: "模拟考试", value: "examMoni", url: utils.getCommonUrl('userDocExamMoni', { id: utils.getQueryInt('id') }) },
    { name: "获得证书", value: "examCer", url: utils.getCommonUrl('userDocExamCer', { id: utils.getQueryInt('id') }) },
    { name: "问卷调查", value: "examQ", url: utils.getCommonUrl('userDocQ', { id: utils.getQueryInt('id') }) },
    { name: "参加测评", value: "examAss", url: utils.getCommonUrl('userDocAss', { id: utils.getQueryInt('id') }) },
    { name: "刷题记录", value: "examPractice", url: utils.getCommonUrl('userDocPractice', { id: utils.getQueryInt('id') }) },
    { name: "积分记录", value: "pointLog", url: utils.getCommonUrl('userDocPointLog', { id: utils.getQueryInt('id') }) },
    { name: "兑换记录", value: "pointShopLog", url: utils.getCommonUrl('userDocPointShopLog', { id: utils.getQueryInt('id') }) },
    { name: "登录日志", value: "loginLog", url: utils.getCommonUrl('userDocLoginLog', { id: utils.getQueryInt('id') }) },
  ],
  leftWidth: 299,
  winHeight: $(window).height(),
  winWidth: $(window).width(),
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url, {
      params: {
        id: this.id
      }
    }).then(function (response) {
      var res = response.data;

      $this.avatarUrl = res.avatarUrl;
      $this.displayName = res.displayName;
      $this.systemCode = res.systemCode;
      if ($this.systemCode === 'Exam') {
        $this.userTabList = $this.userTabList.filter(f => f.value !== 'studyPlan' && f.value !== 'studyCourse');
      }

      setTimeout($this.ready, 100);

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
    });
  },
  btnTab: function (tabInfo) {
    this.curTab = tabInfo.value;
    this.userFrameUrl = tabInfo.url;
  },
  ready: function () {
    window.onresize = this.winResize;
    window.onresize();
    utils.loading(this, false);
  },
  winResize: function () {
    this.winHeight = $(window).height();
    this.winWidth = $(window).width();
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

