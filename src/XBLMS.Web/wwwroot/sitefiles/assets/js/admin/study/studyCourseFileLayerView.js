var $url = '/study/studyCourseFileLayerView';

var data = utils.init({
  id: utils.getQueryInt('id'),
  fileName: '',
  fileUrl: '',
  fileType: '',
  isVideo: false
});

var methods = {
  apiGet: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($url, {
      params: {
        id: this.id
      }
    }).then(function (response) {
      var res = response.data;
      $this.fileName = res.fileName;
      $this.fileUrl = res.fileUrl;
      $this.fileType = res.fileType;
      $this.isVideo = res.isVideo;

    }).catch(function (error) {
      utils.loading($this, false);
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
      if ($this.fileType !== 'PDF') {
        $this.$nextTick(_ => {
          $this.videoPlayer = new Player({
            el: document.querySelector('#divPlayer'),
            url: $this.fileUrl,
            fluid: true,
            autoplay: true
          });
        });
      }
    });
  }
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  }
});
