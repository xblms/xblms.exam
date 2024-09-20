var $url = '/xbl/crudDemo';
var $urlDelete = $url + '/actions/delete';

var data = utils.init({
  form: {
    title:'',
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  total:0,
  list: null
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url, { params: this.form }).then(function (response) {
      var res = response.data;

      $this.total = res.total;
      $this.list = res.list;
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  apiDelete: function (item) {
    var $this = this;

    utils.loading(this, true);
    $api.post($urlDelete, {
      id: item.id
    }).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success("操作成功")
        $this.apiGet();
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  btnAdminViewClick: function (id) {
    utils.openAdminView(id);
  },
  btnEditClick: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getXblUrl('crudDemoEdit', { id: id }),
      width: "68%",
      height: "78%",
      end: function () {
        $this.apiGet();
      }
    });
  },

  btnDeleteClick: function (item) {
    var $this = this;

    top.utils.alertDelete({
      title: '删除数据',
      text: '确定删除吗？',
      callback: function () {
        $this.apiDelete(item);
      }
    });
  },
  btnSearchClick: function () {
    this.form.pageIndex = 1;
    this.apiGet();
  },
  handleCurrentChange: function (val) {
    this.form.pageIndex = val;
    this.apiGet();
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
