var $url = '/points/pointshop/users';

var data = utils.init({
  form: {
    id: utils.getQueryInt('id'),
    keyWords: '',
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  list: null,
  total: 0,
  stateOptionsOnline: [
    {
      value: 'Daifahuo',
      label: '待发货'
    },
    {
      value: 'Yifahuo',
      label: '已发货'
    },
    {
      value: 'Daiqianshou',
      label: '待签收'
    },
    {
      value: 'Yiqianshou',
      label: '已签收'
    }
  ],
  stateOptionsOffline: [
    {
      value: 'Dailing',
      label: '待领取'
    },
    {
      value: 'Yiling',
      label: '已领取'
    }
  ]
});

var methods = {
  apiGet: function () {
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
    });
  },
  handleCurrentChange: function (val) {
    this.form.pageIndex = val;
    this.apiGet();
  },
  btnSearch: function () {
    this.pageIndex = 1;
    this.apiGet();
  },
  shopStateChange: function (row) {
    var $this = this;

    utils.loading(this, true);
    $api.post($url, { state: row.adminState, id: row.id }).then(function (response) {
      var res = response.data;
      utils.success("操作成功");
      $this.apiGet();
    }).catch(function (error) {
      utils.error(error);
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
    this.apiGet();
  }
});
