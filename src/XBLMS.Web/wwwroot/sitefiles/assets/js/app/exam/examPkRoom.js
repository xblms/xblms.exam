var $url = "/exam/examPkRoom";
var $urlNotice = $url + "/notice";

var data = utils.init({
  roomId: utils.getQueryInt("id"),
  userId: 0,
  isSafeMode: false,
  room: null,
  music1: false,
  music2: false,
  music3: true,
  layerMsgId: null,
  answer: null,
  tmList: null,
  tm: null,
  daojishiLoading: false,
  daojishiLimit: 3,
  answerJishi: false,
  answerDuraiont: 0,
  finishRoom: null
});

var methods = {
  apiGet: function () {
    var $this = this;

    $this.tm = null;

    $this.msgNoticeLoading("正在进入竞赛房间...");

    $api.get($url, { params: { id: this.roomId } }).then(function (response) {
      var res = response.data;
      $this.userId = res.userId;
      $this.tmList = res.tmList;

      $this.isSafeMode = res.isSafeMode;
      if ($this.isSafeMode) {
        $this.room = res.cacheRoom;
        $this.msgError("安全模式下，连接被禁用");
      }
      else {
        $this.joinSocket();
      }

    }).catch(function (error) {
      utils.error("连接错误，请重新进入", { layer: true });
      layer.close($this.layerMsgId);
    }).then(function () {
    });
  },
  answerStartJishi: function () {
    this.answerJishi = true;
    this.answerTmJishi();
  },
  answerTmJishi: function () {
    if (this.answerJishi) {
      if (this.userId === this.room.aUserId) {
        this.answerDuraiont++;

        setTimeout(this.answerTmJishi, 1000);
      }
      else if (this.userId === this.room.bUserId) {
        this.answerDuraiont++;
        setTimeout(this.answerTmJishi, 1000);
      }
    }
  },
  answerStopJishi: function () {
    if (this.tm.answer.length > 0) {
      this.answerJishi = false;

      if (this.userId === this.room.aUserId) {

        if (this.room.bUserState != 'AnswerLock') {
          utils.success("等待对方确认答案...", { layer: true });
        }

        this.room.aUserState = "AnswerLock";
        this.room.aUserAnswer = this.tm.answer;
        this.room.tmId = this.tm.id;
        this.apiNoticeSocket();
      }
      else if (this.userId === this.room.bUserId) {
        if (this.room.aUserState != 'AnswerLock') {
          utils.success("等待对方确认答案...", { layer: true });
        }

        this.room.bUserState = "AnswerLock";
        this.room.bUserAnswer = this.tm.answer;
        this.room.tmId = this.tm.id;
        this.apiNoticeSocket();
      }
    }
    else {
      utils.error("请答题", { layer: true });
    }

  },
  daojishi: function () {
    this.tm = null;
    this.daojishiLoading = true
    if (this.daojishiLimit <= 1) {
      this.daojishiLoading = false;
      this.tm = this.tmList[this.room.tmIndex];
      this.answerStartJishi();
      this.daojishiLimit = 3;
    }
    else {
      this.daojishiLimit--;
      setTimeout(this.daojishi, 1000);
    }
  },
  musicClick: function () {
    if (this.music1 || this.music2 || this.music3) {
      this.music1 = false;
      this.music2 = false;
      this.music3 = false;
    }
    else {
      this.music1 = true;
    }
  },
  music1Ended: function () {
    this.music1 = false;
    this.music2 = true;
    this.music3 = false;
  },
  music2Ended: function () {
    this.music1 = false;
    this.music2 = false;
    this.music3 = true;
  },
  music3Ended: function () {
    this.music1 = true;
    this.music2 = false;
    this.music3 = false;
  },
  btnReadyClick: function () {
    if (this.userId == this.room.aUserId) {
      if (this.room.aUserState == "Ready") {
        this.room.aUserState = "OnLine";
      }
      else if (this.room.aUserState == "OnLine") {
        this.room.aUserState = "Ready";
      }
      this.apiNoticeSocket();
    }
    else if (this.userId === this.room.bUserId) {
      if (this.room.bUserState == "Ready") {
        this.room.bUserState = "OnLine";
      }
      else if (this.room.bUserState == "OnLine") {
        this.room.bUserState = "Ready";
      }
      this.apiNoticeSocket();
    }
  },
  joinSocket: function () {
    var $this = this;
    this.conn = new signalR.HubConnectionBuilder()
      .withAutomaticReconnect()
      .withUrl('/xblmspkroomsocket', { accessTokenFactory: () => $token })
      .build();

    this.conn.on("pkroom" + $this.roomId, (obj) => {
      var socketRoom = JSON.parse(obj);

      if (socketRoom.noticeType === 'ReadyAnswer') {
        $this.daojishi();
      }
      else if (socketRoom.noticeType === 'Finished') {
        location.href = utils.getExamUrl("examPkResult", { id: $this.roomId });
      }

      $this.room = _.assign({}, socketRoom);
      layer.close($this.layerMsgId);

    });
    this.conn.start()
      .catch(err => $this.msgError("连接错误，请重新进入"));
  },
  apiNoticeSocket: function () {
    var $this = this;
    $api.post($urlNotice, $this.room).then(function (response) {
    }).catch(function (error) {
      $this.msgError(error);
    }).then(function () {
    });
  },
  msgNoticeLoading: function (noticeContent) {
    this.layerMsgId = layer.msg('<div class="py-3 text-center text-light"><i class="el-icon-loading fs-1"></i><div class="mt-3">' + noticeContent + '</div></div>', { time: 0 }, function () { });
  },
  msgNoticeShadowLoading: function (noticeContent) {
    this.layerMsgId = layer.msg('<div class="py-3 text-center text-light"><i class="el-icon-loading fs-1"></i><div class="mt-3">' + noticeContent + '</div></div>', { time: 0, shade: 0.3 }, function () { });
  },
  msgError: function (err) {
    layer.msg('<div class="p-3 text-center text-light"><i class="el-icon-error fs-1"></i><div class="mt-3">' + err + '</div></div>');
  },
  answerChange: function (val) {
    if (this.tm.baseTx === "Duoxuanti") {
      this.tm.answer = this.tm.optionsValues.join('');
    }

    if (this.userId === this.room.aUserId) {
      this.room.aUserAnswer = this.tm.answer;
      this.room.aUserDuration = this.answerDuraiont;
      this.apiNoticeSocket();
    }
    else if (this.userId === this.room.bUserId) {
      this.room.bUserAnswer = this.tm.answer;
      this.room.bUserDuration = this.answerDuraiont;
      this.apiNoticeSocket();
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
