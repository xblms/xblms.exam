var $url = "/exam/examPkResult";

var data = utils.init({
  id: utils.getQueryInt("id"),
  room: null,
  title:'',
  layerMsgId:null
});

var methods = {
  apiGet: function () {
    var $this = this;

    $this.msgNoticeLoading("正在加载竞赛结果...");

    $api.get($url, { params: { id: this.id } }).then(function (response) {
      var res = response.data;

      $this.title = res.title;
      $this.room = res.room;

    }).catch(function (error) {
      utils.error(error, { layer: true });
      
    }).then(function () {
      layer.close($this.layerMsgId);
    });
  },
  msgNoticeLoading: function (noticeContent) {
    this.layerMsgId = layer.msg('<div class="px-5 py-3 text-center text-light"><i class="el-icon-loading fs-1"></i><div class="mt-3">' + noticeContent + '</div></div>', { time: 0 }, function () { });
  },
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
