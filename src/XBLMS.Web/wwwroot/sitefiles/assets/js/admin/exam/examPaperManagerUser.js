var $url = '/exam/examPaperManager';

var $urlUser = $url + '/user';
var $urlUserDeleteOne = $urlUser + '/removeone';
var $urlUserDelete = $urlUser + '/remove';
var $urlUserExamTimes = $urlUser + '/examtimes';
var $urlUserDateTime = $urlUser + '/datetime';
var $urlUserExport = $urlUser + '/export';

var data = utils.init({
  id: 0,
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
  }
});

var methods = {
  apiGet: function () {
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
      $this.apiGet();
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
    this.apiGet();
  }
});
