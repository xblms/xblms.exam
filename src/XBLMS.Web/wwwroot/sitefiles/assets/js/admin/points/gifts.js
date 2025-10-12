var $url = '/points/pointshop';
var $urlDelete = $url + '/del';

var data = utils.init({
  form: {
    keyWords: '',
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  list: null,
  total: 0
});

var methods = {
  apiGet: function (message) {
    var $this = this;

    utils.loading(this, true);
    $api.get($url, { params: this.form }).then(function (response) {
      var res = response.data;

      $this.list = res.list;
      $this.total = res.total;

    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
      if (message) {
        utils.success(message);
      }
    });
  },
  handleCurrentChange: function (val) {
    this.form.pageIndex = val;
    this.apiGet();
  },
  btnSearch: function () {
    this.apiGet();
  },
  btnEditClick: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getPointsUrl('giftsEdit', { id: id }),
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
      utils.success('操作成功！');
      $this.apiGet();
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  btnDeleteClick: function (gift) {
    var $this = this;
    top.utils.alertDelete({
      title: '删除商品',
      text: '此操作将删除商品 ' + gift.name + '，确定吗？',
      callback: function () {
        $this.apiDelete(gift.id);
      }
    });
  },
  btnListClick: function (row) {
    utils.openTopLeft(row.name + "-兑换记录", utils.getPointsUrl("giftUsers", { id: row.id }));
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
