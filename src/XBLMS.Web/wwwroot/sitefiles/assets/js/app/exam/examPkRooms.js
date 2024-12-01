var $url = "/exam/examPkRooms";

var data = utils.init({
  id: utils.getQueryInt("id"),
  title: "",
  list: [],
  activeNames: [],
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);

    $api.get($url, { params: { id: this.id } }).then(function (response) {
      var res = response.data;

      $this.title = res.title;
      $this.list = res.list;

      if ($this.list.length > 0) {
        $this.activeNames.push($this.list[0].guid);
      }

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnViewRoomClick: function (room) {
    if (room.inRoom) {
      if (room.finished) {
        top.utils.openLayer({
          title: false,
          closebtn: 0,
          url: utils.getExamUrl('examPkResult', { id: room.id }),
          width: "100%",
          height: "100%"
        });
      }
      else {
        var $this = this;
        top.utils.openLayer({
          title: false,
          closebtn: 0,
          url: utils.getExamUrl('examPkRoom', { id: room.id }),
          width: "100%",
          height: "100%",
          end: function () {
            $this.list = [];
            $this.apiGet();
          }
        });
      }
    }
    else {
      utils.error(room.inRoomMsg, { layer: true });
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
