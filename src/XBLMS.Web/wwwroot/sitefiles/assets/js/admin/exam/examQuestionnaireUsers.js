var $url = '/exam/examQuestionnaireUsers';

var data = utils.init({
  id: 0,
  title: '',
  form: {
    id: 0,
    keywords: '',
    isSubmit: '',
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  list: null,
  total: 0,
});

var methods = {
  apiGet: function () {
    var $this = this;
    $api.get($url, { params: $this.form }).then(function (response) {
      var res = response.data;
      $this.list = res.list;
      $this.total = res.total;

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  userHandleCurrentChange: function (val) {
    this.form.pageIndex = val;
    this.apiGet();
  },
  btnUserSearchClick: function () {
    this.form.pageIndex = 1;
    this.apiGet();
  },
};
var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.id = this.form.id = utils.getQueryInt("id");
    this.apiGet();
  }
});
