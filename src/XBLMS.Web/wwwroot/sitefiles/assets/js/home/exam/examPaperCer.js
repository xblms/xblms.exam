var $url = "/exam/examPaperCer";

var data = utils.init({
  form: {
    keyWords: '',
    dateFrom: '',
    dateTo: ''
  },
  list: []
});

var methods = {
  apiGet: function () {
    var $this = this;

    $api.get($url, { params: this.form }).then(function (response) {
      var res = response.data;
      $this.list = res.list;

      if ($this.list === null || $this.list.length === 0) {
        location.href = utils.getRootUrl("empty");
      }

    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnSearchClick: function () {
    this.list = [];
    this.apiGet();
  },
  btnViewCer: function (cer) {
    top.utils.openLayerPhoto({
      title: cer.name,
      id: cer.id,
      src: cer.cerImg + '?r=' + Math.random()
    })
  },
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  },
});
