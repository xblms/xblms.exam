var $url = "/exam/examPaperSubmitResult";

var data = utils.init({
  id: utils.getQueryInt("id"),
  item: {
    queue: 0,
    success: false,
    isPass: false,
    score: 0,
    isShowScore: false,
    title: '',
    isMark:true
  }
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);

    $api.get($url, { params: { id: $this.id } }).then(function (response) {
      var res = response.data;

      $this.item.success = res.success;
      $this.item.queue = res.queue;
      $this.item.isPass = res.isPass;
      $this.item.score = res.score;
      $this.item.isShowScore = res.isShowScore;
      $this.item.title = res.title;
      $this.item.isMark = res.isMark;

      if (!$this.item.success) {
        setTimeout(function () {
          $this.apiGet();
        }, 1000);
      }

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
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
