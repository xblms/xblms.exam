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

    utils.loading(this, true);

    $api.get($url, { params: { id: this.id } }).then(function (response) {
      var res = response.data;

      $this.title = res.title;
      $this.room = res.room;

    }).catch(function (error) {
      utils.error(error, { layer: true });
      
    }).then(function () {
      utils.loading($this,false);
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
