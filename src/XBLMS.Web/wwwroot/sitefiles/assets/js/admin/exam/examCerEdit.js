var $url = 'exam/examCerEdit';

var data = utils.init({
  id: utils.getQueryInt("id"),
  num: 0,
  lock: false,
  tabName: utils.getQueryString('tabName'),
  backgroundImg: "",
  typeNow: [],
  uploadUrl: '',
  uploadLoading: false,
  form: {
    id: 0,
    name: "",
    markList: [],
    prefix: "",
    organName: "",
    backgroundImg: "",
    fontSize: 18,
  },
  timer: null,
});

var methods = {
  apiGet: function () {
    if (utils.getQueryInt("id") > 0) {

      var $this = this;
      utils.loading(this, true);
      $api.get($url, { params: { id: this.id } }).then(function (response) {
        var res = response.data;
        var cerInfo = res.item;

        $this.form = _.assign({}, cerInfo);

        if ($this.id > 0)
        {
          $this.backgroundImg = cerInfo.backgroundImg;
          $this.typeNow = cerInfo.markList;
          $this.timer = setTimeout(function () {
            for (var i = 0; i < cerInfo.position.length; i++) {
              $this.createBox(cerInfo.position[i]);
            }
          }, 200);
        }

      }).catch(function (error) {
        utils.error(error,{ layer:true });
      }).then(function () {
        utils.loading($this, false);
      });
    }
    else {
      utils.loading(this, false);
    }

  },
  btnSubmitClick: function () {
    var $this = this;
    this.$refs.form.validate(function (valid) {
      if (valid) {
        if ($this.form.backgroundImg === '') {
          utils.error('请选择背景图', { layer: true });
          return;
        }
        $this.form.position = $this.getBoxData();
        $this.apiSubmit();
      }
    });
  },
  apiSubmit: function () {
    var $this = this;
    utils.loading($this, true,"正在生成模板文件...");
    $api.post($url, { item: $this.form }).then(function (response) {
      var res = response.data;
      if (res.value > 0) {
        $this.apiSubmitPosition(res.value);
      }
      else {
        utils.loading($this, false);
        utils.error('证书模板数据异常，请重新编辑', { layer: true });
      }

    }).catch(function (error) {
      utils.error(error, { layer:true });
    }).then(function () {
 
    });
  },
  apiSubmitPosition: function (id) {
    var $this = this;
    utils.loading($this, true, "正在生成模板文件...");
    $api.post($url + '/positionsSubmit', { id: id }).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success("操作成功");
        utils.closeLayerSelf();
      }
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  loadBox: function () {
    var $this = this;
    var loadData = [{ id: 1001, text: "C16\n16.5", color: "rgb(255, 0, 0)", height: 70, width: 77, pageX: 627, pageY: 364 },
    { id: 1004, text: "C19\n16.08", color: "rgb(0, 128, 0)", height: 70, width: 77, pageX: 870, pageY: 364 },
    { id: 1005, text: "C20\n16.5", color: "rgb(0, 0, 255)", height: 70, width: 77, pageX: 627, pageY: 439 },
    { id: 1006, text: "C21\n16.18", color: "rgb(255, 165, 0)", height: 70, width: 77, pageX: 709, pageY: 439 },
    { id: 1007, text: "C22\n16.08", color: "rgb(255, 165, 0)", height: 70, width: 77, pageX: 870, pageY: 439 },
    { id: 1008, text: "C23\n16.08", color: "rgb(255, 165, 0)", height: 70, width: 77, pageX: 789, pageY: 439 }];
    $.each(loadData, function (i, row) {
      $this.createBox(row);
    });
  },
  createBox: function (data) {
    var dataId = data.id || '';
    var value = data.text || '';
    var color = '#DCDFE6';
    var height = data.height || 0;
    var width = data.width || 0;
    var pageX = data.pageX || 0;
    var pageY = data.pageY || 0;

    var curNum = this.num++;
    var pos = $("#canvas").position();

    if (data.id == 1004) {
      var box = $('<div class="box" rel="' + curNum + '" id="' + dataId + '" dataId="' + dataId + '">' + value + '<div class="coor transparent"></div></div>').css({
        width: width,
        height: height,
        top: pageY > 0 ? pageY : (pos.top > 0 ? 0 : pos.top * -1),
        left: pageX > 0 ? pageX : (pos.left > 0 ? 0 : pos.left * -1)
      }).appendTo("#canvas");
    }
    else {
      var box = $('<div class="box" rel="' + curNum + '" id="' + dataId + '" dataId="' + dataId + '">' + value + '</div>').css({
        top: pageY > 0 ? pageY : (pos.top > 0 ? 0 : pos.top * -1),
        left: pageX > 0 ? pageX : (pos.left > 0 ? 0 : pos.left * -1)
      }).appendTo("#canvas");
    }
  },
  getBoxData: function () {
    var data = [];
    $('.box').each(function () {
      var box = {};
      box['id'] = $(this).attr('dataId');
      box['text'] = $(this).text();
      box['height'] = parseInt($(this).height());
      box['width'] = parseInt($(this).width());
      box['pageX'] = parseInt($(this).position().left);
      box['pageY'] = parseInt($(this).position().top);
      data.push(box);
    });
    return data;
  },
  mousedown: function () {
    var lock = this.lock;
    $("#canvas").mousedown(function (e) {
      var canvas = $(this);
      e.preventDefault();
      var pos = $(this).position();
      this.posix = { 'x': e.pageX - pos.left, 'y': e.pageY - pos.top };
      $.extend(document, {
        'move': true, 'move_target': this, 'call_down': function (e, posix) {
          canvas.css({
            'cursor': 'move',
            'top': e.pageY - posix.y,
            'left': e.pageX - posix.x
          });
        }, 'call_up': function () {
          canvas.css('cursor', 'default');
        }
      });
    });
    $("#canvas").on('mousedown', '.box', function (e) {
      if (lock) return;
      var pos = $(this).position();
      this.posix = { 'x': e.pageX - pos.left, 'y': e.pageY - pos.top };
      $.extend(document, { 'move': true, 'move_target': this });
      e.stopPropagation();
    });
    $("#canvas").on('mousedown', '.box .coor', function (e) {
      var $box = $(this).parent();
      var posix = {
        'w': $box.width(),
        'h': $box.height(),
        'x': e.pageX,
        'y': e.pageY
      };
      $.extend(document, {
        'move': true, 'call_down': function (e) {
          $box.css({
            'width': Math.max(30, e.pageX - posix.x + posix.w),
            'height': Math.max(30, e.pageY - posix.y + posix.h)
          });
        }
      });
      e.stopPropagation();
    });
  },
  boxChange: function (value, id, name, text, height, width, x, y) {
    if (utils.contains(this.typeNow, name)) {
      if (!utils.contains(value, name)) {
        this.typeNow.splice(this.typeNow.indexOf(name), 1);
        $("#" + id + "").remove();
      }
    }
    else {
      if (utils.contains(value, name)) {
        this.typeNow.push(name);
        var data = { id: id, text: text, color: "rgb(255, 0, 0)", height: height, width: width, pageX: x, pageY: y };
        this.createBox(data);
      }

    }
  },
  typeChange: function (value) {
    this.boxChange(value, 1001, "name", "姓名", 30, 100, 10, 10);
    this.boxChange(value, 1004, "picture", "照片", 120, 100, 10, 131);
    this.boxChange(value, 1005, "number", "证书编号", 30, 100, 10, 265);
    this.boxChange(value, 1006, "date", "认证日期", 30, 100, 10, 304);
    this.boxChange(value, 1007, "organName", "颁发单位", 30, 100, 10, 352);
    this.boxChange(value, 1010, "examName", "试卷名称", 30, 100, 10, 400);
    this.boxChange(value, 1011, "examScore", "考试成绩", 30, 100, 120, 400);
  },
  uploadBefore(file) {
    var re = /(\.jpg|\.jpeg|\.bmp|\.gif|\.png|\.webp)$/i;
    if (!re.exec(file.name)) {
      utils.error('请选择有效的图片!', { layer: true });
      return false;
    }

    var isLt10M = file.size / 1024 / 1024 < 5;
    if (!isLt10M) {
      utils.error('大小不能超过 5MB!', { layer: true });
      return false;
    }
    return true;
  },

  uploadProgress: function () {
    this.uploadLoading = true;
  },

  uploadSuccess: function (res, file, fileList) {
    this.$refs.upload.clearFiles();
    this.form.backgroundImg = this.backgroundImg = res.value;
    this.uploadLoading = false;
  },

  uploadError: function (err) {
    this.uploadLoading = false;
    var error = JSON.parse(err.message);
    utils.error(error.message, { layer: true });
  },

  uploadRemove(file) {
    this.backgroundImg = null;
  }
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    var $this = this;
    this.uploadUrl = $apiUrl + "/" + $url + '/upload';
    this.apiGet();
    $this.$nextTick(() => {
      $this.mousedown();
    })
  },
});
