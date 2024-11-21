var $url = '/common/editorTmWordOpenLayer';
var $urlCheck = $url + '/check';
var $urlSubmit = $url + '/submit';

var data = utils.init({
  treeId: utils.getQueryInt("treeId"),
  editContent: '',
  attributeName: 'attributeName',
  checkDialogVisible: false,
  tmTotal: 0,
  tmErrorTotal: 0,
  tmSuccessTotal: 0
});

var methods = {
  apiGetExample: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url).then(function (response) {
      var res = response.data;

      utils.getEditor($this.attributeName).execCommand("clearDoc");
      utils.getEditor($this.attributeName).execCommand("insertHTML", res.example);

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnInsertExampleClick: function () {
    this.apiGetExample();
  },
  btnSaveClick: function () {
    if (this.tmSuccessTotal > 0 || this.editContent.length > 0) {
      var $this = this;

      utils.loading(this, true);
      $api.post($urlSubmit, { treeId: this.treeId, tmHtml: this.editContent }).then(function (response) {
        var res = response.data;

        if (res.value > 0) {
          utils.success("成功新增 " + res.value + " 道题目");
          utils.closeLayerSelf();
        }
        else {
          utils.error("新增 0 道题目");
          utils.closeLayerSelf();
        }

      }).catch(function (error) {
        utils.error(error, { layer: true });
      }).then(function () {
        utils.loading($this, false);
      });
    }
    else {
      utils.error("没有任何题目数据", { layer: true });
    }

  },
  btnCheckClick: function () {
    var $this = this;
    $this.showCheck = false;

    utils.loading(this, true, '正在检查，请稍等...');
    $api.post($urlCheck, { treeId: this.treeId, tmHtml: this.editContent }).then(function (response) {
      var res = response.data;

      $this.checkDialogVisible = true;
      $this.tmTotal = res.total;
      $this.tmErrorTotal = res.errorTotal;
      $this.tmSuccessTotal = res.successTotal;

      utils.getEditor($this.attributeName).execCommand("clearDoc");
      utils.getEditor($this.attributeName).execCommand("insertHTML", res.resultHtml);

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnImportWordClick: function () {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('editorWordOpenLayer', { pf: window.name }),
      width: "66%",
      height: "66%"
    });
  },
  btnViewDemo: function () {
    top.utils.openLayerPhoto({
      title: '题目格式',
      src: utils.getAssetsUrl("/images/tmFormat.png")
    })
  },
  setWordCallBack: function (wordContent) {
    utils.getEditor(this.attributeName).execCommand("clearDoc");
    utils.getEditor(this.attributeName).execCommand("insertHTML", wordContent);
  }
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    var $this = this;
    utils.loading(this, false);
    setTimeout(function () {
      var editor = utils.getEditor($this.attributeName, $(window).innerHeight() - 300);
      editor.ready(function () {
        this.addListener("contentChange", function () {
          $this.editContent = this.getContent();
        });
      });
    }, 100);
  }
});


