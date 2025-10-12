var $url = "/exam/examPk";
var $urlRooms = $url + "/rooms";

var data = utils.init({
  roomList: [],
  roomTotal: 0,
  roomForm: {
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  roomLoadMoreLoading: false,
  form: {
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  list: [],
  total: 0,
  loadMoreLoading: false,
  currentPk: 0,
});

var methods = {
  btnSwichClick: function () {
    this.clearData();
    if (this.currentPk == 0) {
      this.currentPk = 1;
      this.apiGet();
    }
    else {
      this.currentPk = 0;
      this.apiGetRoom();
    }
  },
  clearData: function () {
    this.roomList = [];
    this.roomTotal = 0;
    this.roomForm.pageIndex = 1;

    this.form.pageIndex = 1;
    this.list = [];
    this.total = 0;
  },
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);

    $api.get($url, { params: this.form }).then(function (response) {
      var res = response.data;

      $this.total = res.total;

      if (res.list && res.list.length > 0) {
        res.list.forEach(pk => {
          $this.list.push(pk);
        });
      }

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      $this.loadMoreLoading = false;
      utils.loading($this, false);
    });
  },
  btnLoadMoreClick: function () {
    this.loadMoreLoading = true;
    this.form.pageIndex++;
    this.apiGet();
  },
  apiGetRoom: function () {
    var $this = this;

    utils.loading(this, true);

    $api.get($urlRooms, { params: this.roomForm }).then(function (response) {
      var res = response.data;

      $this.roomTotal = res.total;

      if (res.list && res.list.length > 0) {
        res.list.forEach(room => {
          $this.roomList.push(room);
        });
      }

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      $this.roomLoadMoreLoading = false;
      utils.loading($this, false);
    });
  },
  btnRoomLoadMoreClick: function () {
    this.roomLoadMoreLoading = true;
    this.roomForm.pageIndex++;
    this.apiGetRoom();
  },
  btnViewClick: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examPkRooms', { id: id }),
      width: "100%",
      height: "100%"
    });
  },
  btnViewRoomClick: function (room) {
    if (room.inRoom) {
      var $this = this;
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
        top.utils.openLayer({
          title: false,
          closebtn: 0,
          url: utils.getExamUrl('examPkRoom', { id: room.id }),
          width: "100%",
          height: "100%",
          end: function () {
            $this.roomList = [];
            $this.roomTotal = 0;
            $this.roomForm.pageIndex = 1;

            $this.apiGetRoom();
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
    top.document.title = "竞赛中心";
    this.apiGet();
    this.apiGetRoom();
  },
});
