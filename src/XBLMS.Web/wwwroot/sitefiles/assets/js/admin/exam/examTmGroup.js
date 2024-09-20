var $url = '/exam/examTmGroup';
var $urlDelete = $url + '/delete';

var data = utils.init({
  groups: null,
  search: ''
});

var methods = {
  apiGet: function (message) {
    var $this = this;

    utils.loading(this, true);
    $api.get($url, { params: { search: this.search } }).then(function (response) {
      var res = response.data;

      $this.groups = res.groups;
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      if (message) {
        utils.success(message);
      }
    });
  },
  btnSearch: function () {
    this.apiGet();
  },
  btnRangeClick: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examTmSelect', { id: id }),
      width: "99%",
      height: "99%",
      end: function () { $this.apiGet() }
    });
  },
  btnEditClick: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examTmGroupEdit', { id: id }),
      width: "60%",
      height: "88%",
      end: function () { $this.apiGet() }
    });
  },
  apiDelete: function (id) {
    var $this = this;

    utils.loading(this, true);
    $api.post($urlDelete, {
      id: id
    }).then(function (response) {
      var res = response.data;
      utils.success('题目组删除成功！');
      $this.apiGet();
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnListClick: function (group) {
    top.utils.openLayer({
      title: '题目组：' + group.groupName,
      url: utils.getExamUrl('examTm', { groupId: group.id }),
      width: "90%",
      height: "90%",
    });
  },

  btnDeleteClick: function (group) {
    var $this = this;

    top.utils.alertDelete({
      title: '删除题目组',
      text: '此操作将删除题目组 ' + group.groupName + '，确定吗？',
      callback: function () {
        $this.apiDelete(group.id);
      }
    });
  },

  btnSubmitClick: function () {
    var $this = this;
    this.$refs.form.validate(function (valid) {
      if (valid) {
        $this.apiSubmit();
      }
    });
  },

  btnCancelClick: function () {
    this.panel = false;
  },

  btnCloseClick: function () {
    utils.removeTab();
  },
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  }
});
