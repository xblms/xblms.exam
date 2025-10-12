var $url = "/exam/examPractice";
var $urlSubmit = $url + "/submit";
var $urlDelete = $url + "/del";

var data = utils.init({
  list: [],
  total: 0,
  form: {
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  loadMoreLoading: false
});

var methods = {
  apiGet: function () {
    var $this = this;
    top.document.title = "刷题练习";
    if (this.total === 0) {
      utils.loading(this, true);
    }
    $api.get($url, { params: this.form }).then(function (response) {
      var res = response.data;

      if (res.list && res.list.length > 0) {
        res.list.forEach(paper => {
          $this.list.push(paper);
        });
      }
      $this.total = res.total;

    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.loadMoreLoading = false;
    });
  },
  btnCreateClick: function (practiceType, groupId, groupTmTotal) {
    var $this = this;
    if (practiceType === 'Create') {
      top.utils.openLayer({
        title: false,
        closebtn: 0,
        url: utils.getExamUrl('examPracticeReady'),
        width: "100%",
        height: "100%",
        end: function () {
          $this.btnSearchClick();
        }
      });
    }
    else {
      top.utils.alertWarning({
        title: '准备进入刷题模式',
        callback: function () {
          if (groupTmTotal > 0) {
            $this.apiCreatePractice(practiceType, groupId);
          }
          else {
            utils.error("没有题目可以练习");
          }
        }
      });
    }

  },
  apiCreatePractice: function (practiceType, groupId) {
    var $this = this;

    utils.loading(this, true, "正在创建练习...");

    $api.post($urlSubmit, { practiceType: practiceType, groupId: groupId }).then(function (response) {
      var res = response.data;

      if (res.success) {
        $this.goPractice(res.id);
      }
      else {
        utils.error(res.error);
      }

    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  goPractice: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examPracticing', { id: id }),
      width: "100%",
      height: "100%"
    });
  },
  btnSearchClick: function () {
    this.form.pageIndex = 1;
    this.list = [];
    this.apiGet();
  },
  btnLoadMoreClick: function () {
    this.loadMoreLoading = true;
    this.form.pageIndex++;
    this.apiGet();
  },
  btnDeleteClick: function (id) {
    var $this = this;
    top.utils.alertWarning({
      title: '删除自定义练习',
      text: '将清空该练习的所有练习记录，确定删除吗？',
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
      utils.success("操作成功")
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      $this.btnSearchClick();
    });
  }
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  },
});
