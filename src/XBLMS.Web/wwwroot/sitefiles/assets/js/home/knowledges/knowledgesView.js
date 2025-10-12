var $url = "/knowledgesView";
var $urlLike = $url + "/like";
var $urlCollect = $url + "/collect";

var data = utils.init({
  id: utils.getQueryInt("id"),
  url: null,
  isLike: false,
  isCollect: false,
  likes: 0,
  collects: 0
});

var methods = {
  apiGet: function () {
    var $this = this;
    utils.loading($this, true);
    $api.get($url, { params: { id: this.id } }).then(function (response) {
      var res = response.data;
      $this.url = res.url;
      $this.isLike = res.isLike;
      $this.isCollect = res.isCollect;
      $this.likes = res.likes;
      $this.collects = res.collects;

      top.utils.pointNotice(res.pointNotice);
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnLikeClick: function () {
    if (this.isLike) {
      this.likes--;
    }
    else {
      this.likes++;
    }
    this.isLike = !this.isLike;
    this.apiLike();
  },
  btnCollectClick: function () {
    if (this.isCollect) {
      this.collects--;
    }
    else {
      this.collects++;
    }
    this.isCollect = !this.isCollect;
    this.apiCollect();
  },
  apiLike: function () {
    var $this = this;
    utils.loading($this, true);
    $api.post($urlLike, { id: this.id }).then(function (response) {
      var res = response.data;
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  apiCollect: function () {
    var $this = this;
    utils.loading($this, true);
    $api.post($urlCollect, { id: this.id }).then(function (response) {
      var res = response.data;
    }).catch(function (error) {
      utils.error(error);
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
