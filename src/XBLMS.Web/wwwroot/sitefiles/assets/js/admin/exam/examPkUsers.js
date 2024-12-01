var $url = '/exam/examPkUsers';

var data = utils.init({
  id: 0,
  title: '',
  form: {
    id: 0,
    keyWords: '',
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
      $this.title = res.title;

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnUserSearchClick: function () {
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
