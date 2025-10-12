var $url = 'study/studyCourseFiles';

var data = utils.init({
  groupId: utils.getQueryInt('groupId'),

  uploadParams: { groupId: 0, duration: 0, cover: '' },
  uploadFormpanel: false,
  uploadBtnLoad: false,
  durationList: [],
  fileList: [],
  fileListNo: [],
  fileListYes: [],
  errorOffset: 0,
  drawer: false,
  activeName: 'select',
});

var methods = {

  uploadFiles: function () {
    if (this.fileList.length > 0) {
      this.$refs.uploadFiles.submit();
    }
    else {
      utils.error("请选择文件");
    }
  },
  uploadBefore(file) {

    this.uploadParams.groupId = this.groupId;
    this.uploadParams.duration = 1;

    if (this.durationList.length > 0) {
      for (var i = 0; i < this.durationList.length; i++) {
        var item = this.durationList[i];
        if (item.id === file.uid) {
          this.uploadParams.duration = item.duration;
          this.uploadParams.cover = item.cover;
          return true;
        }
      }
    }
    return true;
  },
  uploadChange: function (file, fileList) {

    var isLt10M = file.size / 1024 / 1024 / 1024 < 2;
    if (!isLt10M) {
      fileList.splice(fileList.indexOf(file), 1);

      var newFile = file;
      newFile.msg = '文件大小不能超过 2 G!';
      this.fileListNo.push(newFile);
      return false;
    }
    this.fileList = fileList;
    if (file.status !== 'ready' && file.raw.type != 'video/mp4') return;


    const coverBase64 = "";
    const getVideoInfo = new Promise((resolve) => {
      const videoElement = document.createElement("video");
      videoElement.src = URL.createObjectURL(file.raw);

      videoElement.onloadedmetadata = () => {
        const canvas = document.createElement("canvas");
        const { videoWidth, videoHeight } = videoElement;
        canvas.width = videoWidth;
        canvas.height = videoHeight;

        // 在视频播放到指定时间后截取画面
        const snapshotTime = 3; // 截取时间点（单位：秒）
        videoElement.currentTime = snapshotTime;

        videoElement.onseeked = () => {
          // 绘制视频当前时间点的画面到画布上
          canvas
            .getContext("2d")
            .drawImage(videoElement, 0, 0, videoWidth, videoHeight);

          resolve({
            duration: videoElement.duration,
            cover: canvas.toDataURL("image/jpeg")
          });
        };

      };


    });
    getVideoInfo.then((videoInfo) => {
      var duration = Math.floor(videoInfo.duration);
      duration = duration < 1 ? 1 : duration;
      this.durationList.push({ id: file.uid, duration: duration, cover: videoInfo.cover });
    })

  },
  uploadExceed: function (files, fileList) {
    utils.error("每次最多上传100个文件");
    return false;
  },
  uploadProgress: function (event, file, fileList) {
    this.uploadBtnLoad = true;
    if (file.percentage >= 99) {
      file.percentage = 99;
    }
  },
  uploadSuccess: function (response, file, fileList) {

    if (response.success) {
      this.fileListYes.push(file);
    }
    else {
      var newFile = file;
      newFile.msg = response.msg;
      this.fileListNo.push(newFile);
    }
    this.fileList.splice(fileList.indexOf(file), 1);
    if (this.fileList.length === 0) {
      this.uploadBtnLoad = false;
    }

  },

  uploadError: function (err, file, fileList) {
    var newFile = file;
    newFile.msg = JSON.parse(err.message).message;
    this.fileListNo.push(newFile);

    if (this.fileList.length === 0) {
      this.uploadBtnLoad = false;
    }
  },

  drawerClose: function () {
    this.fileList = [];
    this.fileListNo = [];
    this.fileListYes = [];
  },
  btnFileAbort: function (file) {
    this.$refs.uploadFiles.abort(file);
    this.fileList.splice(this.fileList.indexOf(file), 1);
    if (this.fileList.length <= 0) {
      this.uploadBtnLoad = false;
    }
  },
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    utils.loading(this, false);
    this.$urlUpload = $apiUrl + "/" + $url;
  }
});
