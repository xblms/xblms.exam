var $url = '/settings/usersGroupUserList';

var data = utils.init({
  items: null,
  count: null,

  formInline: {
    state: '',
    groupId: utils.getQueryInt("groupId"),
    order: '',
    lastActivityDate: 0,
    keyword: '',
    currentPage: 1,
    organId: 0,
    organType: '',
    offset: 0,
    limit: 30
  }
});

var methods = {

  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url, {
      params: this.formInline
    }).then(function (response) {
      var res = response.data;

      $this.items = res.users;
      $this.count = res.count;

    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  btnSearchClick() {
    this.apiGet();
  },

  handleCurrentChange: function (val) {
    this.formInline.currentValue = val;
    this.formInline.offset = this.formInline.limit * (val - 1);

    this.btnSearchClick();
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
