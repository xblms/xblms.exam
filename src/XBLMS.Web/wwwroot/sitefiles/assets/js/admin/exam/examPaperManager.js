var $url = '/exam/examPaperManager';

var $urlUser = '/exam/examPaperManager/user';
var $urlUserDeleteOne = $urlUser + '/removeone';
var $urlUserDelete = $urlUser + '/remove';
var $urlUserExamTimes = $urlUser + '/examtimes';
var $urlUserDateTime = $urlUser + '/datetime';

var data = utils.init({
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
    examEndDateTime:''
  },
  tabPosition: 'left'
});

var methods = {
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
          $this.apiUserUpdateExamTimes(increment,userIds);
        }
      });
    }
    else {
      utils.error("请选择考生", { layer: true });
    }
  },
  apiUserUpdateExamTimes: function (increment,ids) {
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
        text: '将清空这些考生的考试数据，确认移出吗？',
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
    $api.post($urlUserDelete, { ids:ids }).then(function (response) {
      var res = response.data;
      $this.userClearSelection();
      utils.success("操作成功", { layer: true })
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
      $this.btnSearchClick();
    });
  },
  btnUserDelete: function (row) {
    var $this = this;
    top.utils.alertDelete({
      title: '移出考生-' + row.user.displayName,
      text: '将清空该考生的考试数据，确认移出吗？',
      callback: function () {
        $this.apiUserDelete(id);
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
      $this.btnSearchClick();
    });
  },
  btnUserArrange: function () {
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('usersRange', { id: this.form.id, rangeType: 'Exam' }),
      width: "88%",
      height: "88%",
      end: function () {
        $this.btnSearchClick();
      }
    });
  },
  btnUserUpdateDatetime: function () {
    if (this.userSelection && this.userSelection.length > 0)
    {
      this.userUpdateDateTimeDialogVisible = true;
    }
    else
    {
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

    $api.post($urlUserDateTime, { ids: userIds, examBeginDateTime: userUpdateDateTimeForm.examBeginDateTime, examEndDateTime: userUpdateDateTimeForm.examEndDateTime }).then(function (response) {
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
  apiGet: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($url, { params: $this.form }).then(function (response) {
      var res = response.data;

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false, { layer: true });
    });
  },
  handleCurrentChange: function (val) {
    this.form.pageIndex = val;
    this.apiGet();
  },
  btnSearchClick: function () {
    this.form.pageIndex = 1;
    this.apiGet();
  }
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.form.id = utils.getQueryInt("id");
    this.apiGetUser();
  }
});
