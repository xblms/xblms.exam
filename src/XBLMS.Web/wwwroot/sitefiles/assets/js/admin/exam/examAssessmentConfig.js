var $url = '/exam/examAssessmentConfig';
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
  total: 0,
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
  btnDeleteClick: function (config) {
    var $this = this;
    if (config.paperCount > 0) {
      utils.error("不能删除被使用的测评类别", { layer: true });
    }
    else {
      top.utils.alertDelete({
        title: '正在删除测评类别',
        text: '确定删除吗？',
        callback: function () {
          $this.apiDelete(config.id);
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
        utils.success("操作成功", { layer: true });
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
      title: '解锁类别',
      text: '确定解锁该测评类别吗？',
      callback: function () {
        $this.apiUnLock(id);
      }
    });
  },
  btnLockClick: function (id) {
    var $this = this;
    top.utils.alertWarning({
      title: '锁定类别',
      text: '确定锁定该测评类别吗？',
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
        utils.success("操作成功", { layer: true });
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
        utils.success("操作成功", { layer: true });
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.btnSearchClick();
    });
  },
  btnEditClick: function (id) {
    var $this = this;
    var url = utils.getExamUrl('examAssessmentConfigEdit', { id: id });

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
      url: utils.getCommonUrl('examAssessmentConfigLayerView', { id: id }),
      width: "50%",
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
