var $url = '/study/studyCourseEvaluation';
var $urlDelete = $url + '/del';
var $urlLock = $url + '/lock';
var $urlUnLock = $url + '/unLock';


var data = utils.init({
  formInline: {
    keyword: '',
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  list: null,
  total: 0,
});

var methods = {
  apiGet: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($url, { params: $this.formInline }).then(function (response) {
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
    this.formInline.pageIndex = val;
    this.apiGet();
  },
  btnSearchClick: function () {
    this.formInline.pageIndex = 1;
    this.apiGet();
  },
  btnDeleteClick: function (row) {
    var $this = this;
    if (row.useCount > 0) {
      utils.error("不能删除被使用的评价");
    }
    else {
      top.utils.alertDelete({
        title: '删除课程评价',
        text: '确定删除吗？',
        callback: function () {
          $this.apiDelete(row.id);
        }
      });
    }
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
      title: '解锁课程评价',
      text: '确定吗？',
      callback: function () {
        $this.apiUnLock(id);
      }
    });
  },
  btnLockClick: function (id) {
    var $this = this;
    top.utils.alertWarning({
      title: '锁定课程评价',
      text: '确定吗？',
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
    var url = utils.getStudyUrl('studyCourseEvaluationEdit', { id: id });

    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: url,
      width: "68%",
      height: "98%",
      end: function () {
        $this.btnSearchClick();
      }
    });

  },
  btnCopyClick: function (id) {
    var $this = this;
 
    var url = utils.getStudyUrl('studyCourseEvaluationEdit', { id: id, copyId: id });

    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: url,
      width: "68%",
      height: "98%",
      end: function () {
        $this.btnSearchClick();
      }
    });
  },
  btnViewClick: function (id) {
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getStudyUrl('studyCourseEvaluationLayerView', { id: id }),
      width: "58%",
      height: "88%"
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
