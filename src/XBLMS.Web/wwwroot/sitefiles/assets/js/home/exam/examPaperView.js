var $url = "/exam/examPaperView";
var $urlItem = $url + "/item";

var data = utils.init({
  id: utils.getQueryInt('id'),
  list: null,
  paper: null,
  watermark: null,
  tmList: []
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true, "正在加载答卷...");

    $api.get($url, { params: { id: $this.id } }).then(function (response) {
      var res = response.data;

      $this.watermark = res.watermark;
      $this.paper = res.item;
      $this.list = JSON.parse(utils.AESDecrypt(res.txList, res.salt));

      if ($this.list && $this.list.length > 0) {
        $this.list.forEach(item => {
          var cTmList = item.tmList;
          if (cTmList && cTmList.length > 0) {
            cTmList.forEach(ctm => {
              $this.tmList.push(ctm);
            })
          }

        });
      }

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnGetTm(id) {
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
    utils.loading(this, false);
    this.apiGet();
  },
});
