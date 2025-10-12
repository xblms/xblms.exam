var $url = "/exam/examPracticeResult/view";

var data = utils.init({
  id: utils.getQueryInt('id'),
  item: null,
  tmList: [],
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true, "正在加载答题记录...");

    $api.get($url, { params: { id: $this.id } }).then(function (response) {
      var res = response.data;

      $this.item = res.item;
      $this.tmList = JSON.parse(utils.AESDecrypt(res.tmList, res.salt));

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnGoTmClick: function (id) {
    var tmel = document.getElementById("tmid_" + id);
    if (tmel) {
      tmel.scrollIntoView({ behavior: "smooth", block: "center" });
    }
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
