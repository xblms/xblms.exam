var $url = '/exam/examQuestionnaire';
var $urlDelete = $url + '/del';
var $urlLock = $url + '/lock';
var $urlUnLock = $url + '/unLock';


var data = utils.init({
  formInline: {
    keyword: '',
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  paperList: null,
  paperTotal: 0,
});

var methods = {
  apiGet: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($url, { params: $this.formInline }).then(function (response) {
      var res = response.data;

      $this.paperList = res.items;
      $this.paperTotal = res.total;

    }).catch(function (error) {
      utils.loading($this, false);
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  handleCurrentChange: function (val) {
    this.formInline.pageIndex = val;
    this.apiGet();
  },
  btnSearchClick: function () {
    this.formInline.treeId = this.treeSelectId;
    this.formInline.pageIndex = 1;
    this.apiGet();
  },
  btnDeleteClick: function (id) {
    var $this = this;
    top.utils.alertDelete({
      title: '删除调查问卷',
      text: '确定删除吗？',
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
        utils.success("成功删除调查问卷")
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
      title: '解锁调查问卷',
      text: '确定解锁调查问卷吗？',
      callback: function () {
        $this.apiUnLock(id);
      }
    });
  },
  btnLockClick: function (id) {
    var $this = this;
    top.utils.alertWarning({
      title: '锁定调查问卷',
      text: '锁定后无法进行问卷填写，确定吗？',
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
        utils.success("成功锁定调查问卷")
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
        utils.success("成功解锁调查问卷")
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
      if (row.isStop) {
        utils.success("已停用");
      }
      else {
        utils.success("已启用");
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnViewClick: function (row) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examQuestionnaireAnalysis', { id: row.id }),
      width: "98%",
      height: "98%"
    });

  },
  btnViewUserClick: function (row) {
    var $this = this;
    top.utils.openLayer({
      title: row.title + '-用户列表',
      url: utils.getExamUrl('examQuestionnaireUsers', { id: row.id }),
      width: "88%",
      height: "98%"
    });

  },
  btnEditClick: function (id) {

    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examQuestionnaireEdit', { id: id, treeId: this.treeSelectId }),
      width: "98%",
      height: "98%",
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
      url: utils.getExamUrl('examQuestionnaireEdit', { id: id, copyId: id }),
      width: "98%",
      height: "98%",
      end: function () {
        $this.btnSearchClick();
      }
    });
  },
  btnQrcodeClick: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examQuestionnaireQrcode', { id: id }),
      width: "58%",
      height: "88%",
      end: function () {
        $this.btnSearchClick();
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
