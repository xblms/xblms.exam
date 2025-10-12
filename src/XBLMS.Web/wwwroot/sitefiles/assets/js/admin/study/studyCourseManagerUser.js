var $url = '/study/studyCourseManager';

var $urlUser = $url + '/user';
var $urlUserExport = $urlUser + '/export';
var $urlUserSet = $urlUser + '/set';
var $urlUserOver = $urlUser + '/over';

var data = utils.init({
  id: 0,
  planId: 0,
  course:null,
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
});

var methods = {
  apiGet: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($urlUser, { params: $this.form }).then(function (response) {
      var res = response.data;
      $this.course = res.course;
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
      $this.apiGet();
    });
  },
  userHandleCurrentChange: function (val) {
    this.form.pageIndex = val;
    this.apiGet();
  },
  btnUserSearchClick: function () {
    this.form.pageIndex = 1;
    this.apiGet();
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
  }
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.id = this.form.id = utils.getQueryInt("id");
    this.planId = this.form.planId = utils.getQueryInt("planId");

    this.apiGet();
  }
});
